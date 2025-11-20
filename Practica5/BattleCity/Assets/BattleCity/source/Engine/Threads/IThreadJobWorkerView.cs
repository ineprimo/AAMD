using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThreadJobWorkerView
{
    int WorkerID { get; set; }
    void Run();
    int ID { get; }
}
