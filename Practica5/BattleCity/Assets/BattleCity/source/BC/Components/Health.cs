using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public Destroy destroy;
    public Slider slider;
    private int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHealth(int amount)
    {
        health += amount;
        health = Mathf.Min(maxHealth,health);
        UpdateSlider();
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        health -= damage;
        UpdateSlider();
        if (health <= 0)
        {
            destroy.InitDestroy(attacker);
        }
    }

    protected void UpdateSlider()
    {
        if (slider != null)
            slider.value = health;
    }
}
