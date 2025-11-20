using System.Collections;
using System.Threading;

public class AlreadyWorkerAssignedException : GameException
{
    public AlreadyWorkerAssignedException(string msg) : base(msg)
    {

    }
}

public abstract class ThreadJob : IThreadJobWorkerView{

    private Mutex _runMutex;
    private Mutex _workerIDMutex;
    private ThreadEndCallback _callback;
    private volatile int _workerID;
    private volatile int _id;
    private volatile bool _isWorkerWorking;

    private static int _ThreadJobID = 0;

    /// <summary>
    /// Crea un trabajo para ser procesado por un hilo.
    /// Los datos que se procesen en la clase se asume que son exclusivos de esta clase
    /// Es decir que nadie de cualquier otro hilo de ejecución tiene acceso a ellos.
    /// Se recomienda copiar todos los datos a una zona segura para evitar accessos concurrentes.
    /// </summary>
    /// <param name="callback"></param>
    public ThreadJob(ThreadEndCallback callback)
    {
        _runMutex = new Mutex();
        _workerIDMutex = new Mutex();
        ResetWork(callback);
    }


    public bool IsWorkerWorking
    {
        get { return _isWorkerWorking; }
    }

    /// <summary>
    /// Método para resetear el trabajo sin necesidad de crear uno nuevo. Útil si no quieremos crear 
    /// nuevamente todos los datos.
    /// </summary>
    /// <param name="callback"></param>
    public virtual void ResetWork(ThreadEndCallback callback)
    {
        _callback = callback;
        _workerID = -1;
        _id = _ThreadJobID++;
    }

    /// <summary>
    /// Devuelve el identificador del worker asignado o -1 si no ha isod asignado ninguno.
    /// </summary>
    public int WorkerID
    {
        get {
            int res = 0;
            _workerIDMutex.WaitOne();
            res = _workerID;
            _workerIDMutex.ReleaseMutex();
            return res;
        }
        set {
            _workerIDMutex.WaitOne();
            _workerID = value;
            _workerIDMutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// identificador unico del trabajo.
    /// </summary>
    public int ID
    {
        get { return _id; }
    }

    /// <summary>
    /// Este método protege con un mutex a Process para evitar accesos concurrentes.
    /// Process es quien realmente implemente el algoritmo o proceso a calcular sobre los datos.
    /// </summary>
    public void Run()
    {
        _runMutex.WaitOne();
        _isWorkerWorking = true;
        Process();
        _isWorkerWorking = false;
        _runMutex.ReleaseMutex();
    }

    /// <summary>
    /// Notifica el resultado a los interesados (clientes).
    /// Esto se llama desde el hilo principal, no llamar desde otro hilo para evitar accesoss concurrentes al código dle cliente.
    /// </summary>
    public void Notify()
    {
        _callback(this);
    }


    /// <summary>
    /// Cuando se sobrescriba esta clase, los usuario debe definir en el método Process el algoritmo o proceso
    /// que queremos calcular sobre los datos.
    /// </summary>
    protected abstract void Process();
}
