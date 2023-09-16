using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebration : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Your movement or gameplay code here
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            animator.SetTrigger("Victory");
            // Other code when player reaches finish
        }
    }
}
