using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            Destroy(gameObject); // distruggi il player
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}