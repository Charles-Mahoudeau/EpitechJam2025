using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;
   private void OnTriggerEnter(Collider other)
   {
       if (other.CompareTag("Player"))
       {
           animator.SetBool("IsOpen", false);
       } 
   }
}
