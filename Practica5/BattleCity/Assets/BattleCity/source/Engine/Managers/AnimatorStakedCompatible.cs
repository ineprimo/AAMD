using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStakedCompatible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.keepAnimatorStateOnDisable = true;
        }
        else
            Debug.LogError("Error, AnimatorStakedCompatible no está en el gameobject del componente Animator");
    }
}
