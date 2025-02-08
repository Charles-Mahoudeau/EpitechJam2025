using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChangeScene
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private string newScene;
        [SerializeField] private Vector3 teleportDestination = new Vector3(0, 0, 0);
        public Component player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered in tp zone.");
                DontDestroyOnLoad(player);
                SceneManager.LoadScene(newScene);
                other.transform.position = teleportDestination;
            }
        }
    }
}