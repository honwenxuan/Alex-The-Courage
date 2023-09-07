using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public BoxCollider mainCollider;
    public GameObject EnRig;
    public Animator EnAnimate;
    public Behaviour Controller;

    void Start()
    {
        GetRagdollBits();
        RagdollModeOff();
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "KnockDownCube")
        {
            StartCoroutine(RagdollAndWakeUp());
        }
    }

    Collider[] ragDollColliders;
    Rigidbody[] limbsRigidbodies;

    void GetRagdollBits()
    {
        ragDollColliders = EnRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = EnRig.GetComponentsInChildren<Rigidbody>();
    }

    void RagdollModeOn()
    {
        EnAnimate.enabled = false;
        Controller.enabled = false;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
        }

        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void RagdollModeOff()
    {
        EnAnimate.enabled = true;
        EnAnimate.SetTrigger("GetUp");  // Assuming "GetUp" is the name of your trigger

        // Disable ragdoll physics
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;
        }

        // We'll enable these after the animation is done
        mainCollider.enabled = true;
        Controller.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    IEnumerator RagdollAndWakeUp()
    {
        // Turn on Ragdoll mode
        RagdollModeOn();

        // Wait for 3 seconds while in Ragdoll mode
        yield return new WaitForSeconds(3);

        // Play 'Get Up' animation and wait for it to finish
        float animationDuration = 1.0f;  // Set this to the length of your "GetUp" animation
        RagdollModeOff();
        yield return new WaitForSeconds(animationDuration);

        // Fully reactivate enemy here if needed
    }
}