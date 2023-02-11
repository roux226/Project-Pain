using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D playerRigidbody;
    public float cameraHeight = 1.25f;
    public float minimumCameraHeight = 0.4f;
    public float cameraLerpSpeed = 4.0f;
    public float cameraFollowThreshold = 13.0f;

    void Update()
    {
        float newCameraHeight = cameraHeight;
        if (playerTransform.position.y > cameraHeight)
        {
            if (playerRigidbody.velocity.y >= cameraFollowThreshold)
            {
                newCameraHeight = playerTransform.position.y - (playerRigidbody.velocity.y - cameraFollowThreshold) * Time.deltaTime;
            }
            else
            {
                newCameraHeight = playerTransform.position.y;
            }
        }
        if (newCameraHeight < minimumCameraHeight)
        {
            newCameraHeight = minimumCameraHeight;
        }

        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, newCameraHeight, Time.deltaTime * cameraLerpSpeed), transform.position.z);
    }
}