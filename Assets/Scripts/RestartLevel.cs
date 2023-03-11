using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public float delay = 3f; // il tempo di attesa prima del riavvio

    private void OnDestroy()
    {
        Invoke("RestartScene", delay);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}