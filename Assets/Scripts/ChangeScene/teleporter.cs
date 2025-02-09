using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChangeScene
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private string newScene;
        [SerializeField] private Vector3 teleportDestination = new Vector3(0, 0, 0);
        [SerializeField] private bool movePlayer = true;
        public GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            if (movePlayer)
            {
                DontDestroyOnLoad(player);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!movePlayer)
                    Destroy(other);
                Debug.Log("Player entered in tp zone.");
                SceneManager.LoadScene(newScene);
                if (movePlayer)
                    other.transform.position = teleportDestination;
            }
        }
    }
}