using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float returnToNormalScale = 2f;

    private float horizontal;
    public float speed = 8f;
    private float jumpingPower = 20f;
    private bool isFacingRight = true;

    private bool canDash = true;
    public bool isDashing;
    private float dashingPower = 26f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.75f;
    private int jumpsLeft = 1;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private void CheckCollision(Collision2D collision)
{
    if (collision.gameObject.layer == LayerMask.NameToLayer("Lava"))
    {
        // Il player è entrato in contatto con la lava
        Destroy(gameObject); // distruggi il player
        RestartLevel(); // avvia il restart del livello
    }
    else if (collision.gameObject.CompareTag("Enemy"))
    {
        // Il player è entrato in contatto con un nemico
        // Aggiungi qui la logica per gestire la collisione con un nemico
    }
}
    
    private void Start()
    {
        coyoteTimeCounter = coyoteTime;
    }


    private void Update()
    {

        coyoteTimeCounter -=Time.deltaTime;

        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpsLeft--;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
        

        if (IsGrounded())
        {
            jumpsLeft = 1;
            sr.color = Color.green;
        }
        if (jumpsLeft == 0 && canDash)
        {
            sr.color = Color.yellow;
        }
        if (!canDash)
        {
            sr.color = Color.yellow;
        }
        if (jumpsLeft == 0 && !canDash)
        {
            sr.color = Color.red;
        }
    }
    
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
        public void RestartLevel()
    {
        Debug.Log("Restarting level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        transform.localScale = new Vector3(transform.localScale.x, 0.5f * transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        transform.localScale = new Vector3(transform.localScale.x, returnToNormalScale * transform.localScale.y, transform.localScale.z);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}