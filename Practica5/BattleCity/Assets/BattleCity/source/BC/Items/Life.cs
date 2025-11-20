using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int amount;
    public List<string> tags;
    public string sound;
    private bool adquited = false;
    // Start is called before the first frame update
    void Start()
    {
        adquited = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (adquited)
            return;
        if(tags == null || tags.Contains(other.tag))
        {
            Health health = other.GetComponent<Health>();
            if(health!= null)
            {
                health.AddHealth(amount);
                adquited = true;
                GameMgr.Instance.SoundMgr.PlaySound(sound);
                GameLogic.Get().DeleteLife();
                Destroy(this.gameObject);
            }
        }
    }
}
