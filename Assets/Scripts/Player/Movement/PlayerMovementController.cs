using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D rb;
    Transform sprite;

    public Animator animator;

    public float normalGravity;

    //movement speed
    public int jumpheight;
    public float speed;

    public float distance;

    //ground checking
    public Transform groundCheck;
    public bool isGrounded;
    public bool isOnGround;

    public LayerMask groundMask;

    public bool jumpCheck;

    //wall jumping
    public Transform[] wallCheck;
    bool onWallRight;
    bool onWallLeft;
    public bool onWall;

    public bool wallJump;

    public LayerMask wallMask;
    public float wallGravity;

    public float climbSeconds;

    //ceiling climbing
    public Transform ceilingCheck;
    public bool OnCeiling;

    public LayerMask ceilingMask;

    // Start is called before the first frame update
    void Start()
    {
        //get the players rigidbody2D
        rb = this.GetComponent<Rigidbody2D>();
        sprite = this.transform.GetChild(6);
        animator = sprite.gameObject.GetComponent<Animator>();
    }

    //on collision with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = collision.transform.parent;
        }

        //if it collides with a wall reset velocity
        if (collision.gameObject.tag == "Wall")
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Check if on ground
        if (!jumpCheck)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);
        }
        if (isGrounded && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Fall")
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Falling", false);
        }
        if (!isGrounded)
        {
            animator.SetBool("Falling", true);
        }

        //check if on a wall
        CheckWall();

        //check if on ceiling
        OnCeiling = Physics2D.OverlapCircle(ceilingCheck.position, 0.1f, ceilingMask);


        //Movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("Running", false);
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpCheck();
        }

        SetGravity();
        
        //ceiling climbing
        if (OnCeiling)
        {
            rb.gravityScale = -1;
            StartCoroutine(Check());
        }
    }

    void SetGravity()
    {
        if (!OnCeiling || !onWall)
        {
            rb.gravityScale = normalGravity;
        }
        if (onWall)
        {
            rb.gravityScale = wallGravity;
        }
    }

    void CheckWall()
    {
        onWallRight = Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, wallMask);
        onWallLeft = Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, wallMask);
        if (onWallRight || onWallLeft)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }
        if (onWallLeft)
        {
            sprite.localScale = new Vector3(-1, 1, 1);
        }
        if (onWallRight)
        {
            sprite.localScale = new Vector3(1, 1, 1);
        }

        if (isGrounded)
        {
            onWall = false;
            wallJump = false;
        }

        if (OnCeiling)
        {
            wallJump = false;
        }

        if (onWall)
        {
            animator.SetBool("Wall", true);
        }
        else
        {
            animator.SetBool("Wall", false);
        }
    }

    void MoveRight()
    {
        float dirX = Input.GetAxis("Horizontal");
        if (!onWallRight && !OnCeiling)
        {
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            animator.SetBool("Running", true);
            sprite.localScale = new Vector3(1, 1, 1);
        }
        if (OnCeiling && !wallJump)
        {
            rb.velocity = new Vector2(dirX * (speed / 2.5f), rb.velocity.y);
            sprite.localScale = new Vector3(1, 1, 1);
        }
    }
    void MoveLeft()
    {
        float dirX = Input.GetAxis("Horizontal");
        if (!onWallLeft && !OnCeiling)
        {
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            animator.SetBool("Running", true);
            sprite.localScale = new Vector3(-1, 1, 1);
        }
        if (OnCeiling && !wallJump)
        {
            rb.velocity = new Vector2(dirX * (speed / 2.5f), rb.velocity.y);
            sprite.localScale = new Vector3(-1, 1, 1);
        }
    }

    void JumpCheck()
    {
        if (isGrounded)
        {
            isGrounded = false;
            animator.SetBool("Jump", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpheight);
            StartCoroutine(JumpWallCheck());
        }

        //walljump
        if (onWall)
        {
            if (onWallRight)
            {
                onWall = false;
                wallJump = true;
                rb.velocity = new Vector2(-distance * 1.25f, distance);
                StartCoroutine(WaitForLeftWall());
            }
            if (onWallLeft)
            {
                onWall = false;
                wallJump = true;
                rb.velocity = new Vector2(distance * 1.25f, distance);
                StartCoroutine(WaitForRightWall());
            }
        }
        if (OnCeiling)
        {
            rb.velocity = new Vector2(0, -jumpheight / 2);
        }
        else
        {
            rb.gravityScale = normalGravity;
        }

        if (OnCeiling)
        {
            wallJump = false;
        }
    }

    //check if hanging on a ceiling for too long
    IEnumerator Check()
    {
        Vector3 lastPos = this.transform.position;
        float seconds = climbSeconds;
        while (seconds > 0)
        {
            seconds -= Time.deltaTime;
            if (lastPos != this.transform.position)
            {
                yield break;
            }
            lastPos = this.transform.position;
            yield return new WaitForEndOfFrame();
        }
        rb.velocity = new Vector2(0, -jumpheight / 2);
    }

    // check if you do or do not hit the right wall
    IEnumerator WaitForLeftWall()
    {
        while (true)
        {
            if (onWallLeft)
            {
                wallJump = false;
                break;
            }
            if (isGrounded)
            {
                break;
            }
            if (OnCeiling)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    // check if you do or do not hit the right wall
    IEnumerator WaitForRightWall()
    {
        while (true)
        {
            if (onWallRight)
            {
                wallJump = false;
                break;
            }
            if (isGrounded)
            {
                break;
            }
            if (OnCeiling)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    // jump normally if next to a wall
    IEnumerator JumpWallCheck()
    {
        jumpCheck = true;
        float seconds = 0.25f;
        while (true)
        {
            seconds -= Time.deltaTime;
            if (!isGrounded)
            {
                rb.gravityScale = normalGravity;
                
            }
            if (onWall && (Input.GetKey(KeyCode.A) || onWall && Input.GetKey(KeyCode.D) || rb.velocity.x < 0 || rb.velocity.x > 1))
            {
                rb.velocity = new Vector2(0, 0);
                jumpCheck = false;
                break;
            }
            if (isGrounded)
            {
                jumpCheck = false;
                break;
            }
            if (OnCeiling)
            {
                jumpCheck = false;
                break;
            }
            if (seconds <= 0)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
