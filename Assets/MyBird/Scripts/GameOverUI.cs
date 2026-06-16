using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyBird
{
    public class GameOverUI : MonoBehaviour
    {
        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}