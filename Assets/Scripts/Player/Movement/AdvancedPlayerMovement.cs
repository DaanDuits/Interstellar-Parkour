using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedPlayerMovement : MonoBehaviour
{
    //Rigidbody and animator
    Rigidbody2D rb;
    GameObject animatorHolder;
    Animator anim;

    //movement variables
    [SerializeField]
    float runningSpeed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float maxClimbingSpeed;

    [SerializeField]
    float climbSeconds;

    [SerializeField]
    float wallSlidingSpeed;
    [SerializeField]
    Vector2 wallJumpingSpeed;

    bool jumping;

    //orientation
    bool right;

    //groundCheck
    [SerializeField]
    Transform groundCheck;
    bool isGrounded;
    [SerializeField]
    LayerMask groundLayers;

    //wallCheck
    [SerializeField]
    Transform wallCheck;
    bool isOnWall;
    [SerializeField]
    LayerMask wallLayers;

    //ceiling check
    [SerializeField]
    Transform ceilingCheck;
    bool isOnCeiling;
    [SerializeField]
    LayerMask ceilingLayers;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = this.GetComponent<Rigidbody2D>();
        animatorHolder = this.transform.GetChild(3).gameObject;
        anim = animatorHolder.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
            this.transform.parent = collision.transform;

        if (collision.gameObject.tag == "Ceiling" && Physics2D.OverlapCircle(ceilingCheck.position, 0.1f, ceilingLayers))
            isOnCeiling = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
            this.transform.parent = null;
    }

    private void Update()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayers);

        isOnWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayers);

        if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.1f, ceilingLayers))
            isOnCeiling = false;

        if (isGrounded || isOnWall)
        CheckGroundedOnWall();

        if (isOnCeiling)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxClimbingSpeed, maxClimbingSpeed), Mathf.Clamp(rb.velocity.y, 1, 1));
            StartCoroutine(CheckForFall());
            rb.velocity = new Vector2(rb.velocity.x, 1);

            if (Input.GetButton("Jump"))
                isOnCeiling = false;
        }

        FallingAndJumping();
    }
    void CheckGroundedOnWall()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumping = true;
        }
        else if (Input.GetButton("Jump") && isOnWall)
        {
            rb.AddForce(new Vector2(wallJumpingSpeed.x * -this.transform.localScale.x, wallJumpingSpeed.y), ForceMode2D.Impulse);

            if (this.transform.localScale.x > 0 && !right)
                Flip();

            else if (this.transform.localScale.x < 0 && right)
                Flip();
        }
        else if (isOnWall && !isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            anim.SetBool("Wall", true);
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.AddForce(new Vector2(h * runningSpeed, rb.velocity.y) * Time.deltaTime, ForceMode2D.Impulse);
        if (!isOnCeiling)
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);

        if (h != 0)
            anim.SetBool("Running", true);

        else
            anim.SetBool("Running", false);

        if (h > 0 && right)
            Flip();

        else if (h < 0 && !right)
            Flip();

        anim.SetBool("Wall", false);
    }

    void FallingAndJumping()
    {
        if (!isGrounded && jumping && !anim.GetBool("Falling"))
        {
            anim.SetBool("Jump", true);
            jumping = false;
        }
        else if(isGrounded)
            anim.SetBool("Jump", false);

        if (!isGrounded && !anim.GetBool("Jump"))
            anim.SetBool("Falling", true);

        else
            anim.SetBool("Falling", false);
    }

    void Flip()
    {
        Vector3 scale = this.transform.localScale;
        this.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        right = !right;
    }

    IEnumerator CheckForFall()
    {
        Vector3 lastPos = this.transform.position;
        float seconds = climbSeconds;
        while (seconds > 0)
        {
            seconds -= Time.deltaTime;
            if (lastPos != this.transform.position)
                yield break;

            lastPos = this.transform.position;
            yield return new WaitForEndOfFrame();
        }
        isOnCeiling = false;
    }
}