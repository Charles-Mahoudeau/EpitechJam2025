using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FloorDieLvl2")
        {
            Die();
        }
    }

    private void Die()
    {
        // Logique pour tuer le joueur, par exemple :
        Debug.Log("Player has died.");
        // Vous pouvez ajouter ici la logique pour réinitialiser le niveau, afficher un écran de game over, etc.
    }
}
