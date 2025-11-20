using System.Threading;
using UnityEngine;

public delegate void ThreadEndCallback(ThreadJob job);

public class WorkerIsBussy : GameException
{
    public WorkerIsBussy(string msg) : base(msg) { }
}

public class Worker {

    private volatile bool _bussy;
    private volatile bool _killWorkerWhenFinish;
    private volatile bool _workDeath;
    private Thread _thread;
    private IThreadJobWorkerView _job;
    private IJobsFinished _jobsFinished;
    private int _id;
    private ManualResetEvent _semaphoreWaitingForJob;
    private ManualResetEvent _semaphoreWaitForEnd;


    /// <summary>
    /// Crea un worker y se le asigna el lugar donde dejará los trabajos terminados.
    /// </summary>
    /// <param name="jobF"></param>
    public Worker(IJobsFinished jobF, int ID)
    {
        _killWorkerWhenFinish = false;
        _workDeath = false;
        _id = ID;
        _thread = new Thread(new ThreadStart (this.Run));
        _jobsFinished = jobF;
        _semaphoreWaitingForJob = new ManualResetEvent(false);
        _semaphoreWaitForEnd = new ManualResetEvent(false);
        _thread.Start();
    }

    /// <summary>
    /// Comprueba si el trabajador está realizando alguna tarea.
    /// </summary>
    public bool IsBussy
    {
        get{return _bussy;}
        protected set{_bussy = value;}
    }

    public bool IsDeath
    {
        get { return _workDeath; }
    }

    public bool EndWorkWhenFinish
    {
        get { return _killWorkerWhenFinish; }
        set { _killWorkerWhenFinish = value; }
    }

    /// <summary>
    /// identificador dle trabajador para depurar.
    /// </summary>
    public int ID
    {
        get { return _id; }
    }

    /// <summary>
    /// Inicia al trabajador. Se comprueba que el trabajador no esté ocupado, si lo está se devuelve una excepción.
    /// Tambien se comprueba si el trabajo ya ha sido asignado a algun otro worker.
    /// </summary>
    /// <param name="job"></param>
    public void Start(IThreadJobWorkerView job)
    {
        if(IsBussy)
            throw new AlreadyWorkerAssignedException("The job " + job.ID + " was previously asigned to Worker " + job.WorkerID);
        if (job.WorkerID > 0)
            throw new AlreadyWorkerAssignedException("The job "+ job.ID + " was previously asigned to Worker "+ job.WorkerID);

        job.WorkerID = ID;
        _job = job;
        if (!_bussy)
        {
            _bussy = true;
            SemaphoreSignal();
        }
    }

    public bool WaitForThread()
    {
        bool b = _semaphoreWaitForEnd.WaitOne();
        b |= _semaphoreWaitForEnd.Reset();
        return b;
    }

    public bool WaitForThread(int ms)
    {
        if (_semaphoreWaitForEnd.WaitOne(ms))
        {
            _semaphoreWaitForEnd.Reset();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Podemos dormir un worker.
    /// </summary>
    /// <param name="time"></param>
    public void Sleep(int time)
    {
        Thread.Sleep(time);
    }


    public void ThreadAbort()
    {
        _thread.Abort();
    }


    /// <summary>
    /// Cuando un trabajo es finalizado, se almacena en la lista de trabajos realizados.
    /// </summary>
    protected void Finish()
    {
        _jobsFinished.AddJobToFinish(_job);
    }


    /// <summary>
    /// Metodo que se ejecuta al hilo para realizar un trabajo. 
    /// El metodo job.Run debería estar protegido por un mutex por seguridad.
    /// </summary>
    protected virtual void Run()
    {
        while(!_killWorkerWhenFinish)
        {
            SemaphoreWaitForSignal();
            _job.Run();
            Finish();
            _bussy = false;
            _semaphoreWaitForEnd.Set();
            ResetSemaphore();
        }
        _workDeath = true;
    }

    protected void SemaphoreWaitForSignal()
    {
        _semaphoreWaitingForJob.WaitOne();
    }

    protected void ResetSemaphore()
    {
        _semaphoreWaitingForJob.Reset();
    }

    protected void SemaphoreSignal()
    {
        _semaphoreWaitingForJob.Set();
    }

}
