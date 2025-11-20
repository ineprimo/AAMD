using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExtraLife : MonoBehaviour
{
    public GameObject smallExtraLife;
    public Transform[] smallExtraLifeSpawners;
    public int numExtraLives;


    public void GetExtraLife(int num = 0)
    {
        HashSet<int> selected = new HashSet<int>();
        int i = 0;
        while ( i < numExtraLives + num)
        {
            int p = -1;
            Transform spawnPoint = Utils.SelectRandom<Transform>(smallExtraLifeSpawners, out p);
            if (!selected.Contains(p)) 
            {
                i++;
                selected.Add(p);
                GameMgr.Instance.GetSpawnerMgr().CreateNewGameObject(smallExtraLife, spawnPoint.position, Quaternion.identity, LogicLayerRoot.Get.Root);
            }
        }
    }
}
