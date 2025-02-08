using System;
using UnityEngine;

public class StartElevatorMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Component elevator;
    public GameObject player;
    public Vector3 speed;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerStay(Collider other)
    {
        if (animator.GetBool("IsOpen") == false && other.CompareTag("Player"))
        {
            elevator.transform.position += speed * Time.deltaTime;
            player.transform.position += speed * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsOpen", true);
        }
    }
}