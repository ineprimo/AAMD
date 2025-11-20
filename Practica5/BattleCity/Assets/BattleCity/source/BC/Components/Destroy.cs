using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject explossion;
    public string sound;

    public virtual void InitDestroy(GameObject attacker)
    {
        GameMgr.Instance.GetServer<SoundMgr>().PlaySound(sound);
        GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(explossion, this.transform.position, Quaternion.identity, null);
        Destroy(this.gameObject);
    }
}
