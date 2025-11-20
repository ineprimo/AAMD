using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJobsFinished
{
    void AddJobToFinish(IThreadJobWorkerView thread);
}
