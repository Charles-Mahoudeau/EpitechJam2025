using System;
using UnityEngine;

public class Animations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator.SetBool("IsOpen", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsOpen", false);
        }
    }
}