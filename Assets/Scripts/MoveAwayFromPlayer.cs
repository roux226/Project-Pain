using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAwayFromPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 7f;
    public float fovRadius = 5f;
    public float viewAngle = 45f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // Check if player is within FOV radius
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= fovRadius)
        {
            // Check if player is within view angle
            float angleToPlayer = Vector2.Angle(transform.right, direction);
            if (angleToPlayer <= viewAngle / 2f)
            {
                // Move away from player
                rb.velocity = -direction * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

        void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Draw FOV radius
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, fovRadius);

        // Draw FOV angle
        Gizmos.color = new Color(1f, 1f, 0f, 0.4f);
        Vector3 fovLine1 = Quaternion.AngleAxis(viewAngle / 2f, Vector3.forward) * transform.right * fovRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-viewAngle / 2f, Vector3.forward) * transform.right * fovRadius;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);
    }
}