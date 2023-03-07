using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;

    void LateUpdate()
    {
        if (playerTransform != null) // added null check
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
    }
}