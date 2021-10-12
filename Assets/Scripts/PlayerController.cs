using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, CreatureBehaviour.ICreatureListener
{
    private const string IS_RUNNING = "isRunning";
    private const string IS_JUMPING = "isJumping";

    public float speed = 5f;
    public float jumpPower = 5f;

    private Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D collider2D;
    public Transform groundCheckPos;

    private bool isGrounded;
    private bool jumped = false;

    public float freezeTime;
    private float freezeTimer;

    public LayerMask groundLayer;

    public CreatureBehaviour creatureBehaviour;
    public float pushMultiplier;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        creatureBehaviour.SetListener(this);
    }

    private void Freeze()
    {
        freezeTimer = Time.time + freezeTime;
    }

    void Start()
    {
        
    }

    void Update()
    {
        CheckIfGorunded();
        HandleJump();
        HandleAttack();
        HandleCast();
    }

    private void FixedUpdate()
    {
        PlayerWalk();
    }

    void PlayerWalk()
    {
        if (Time.time > freezeTimer)
        {
            float h = Input.GetAxisRaw("Horizontal");

            if (h > 0)
            {
                body.velocity = new Vector2(speed, body.velocity.y);
                anim.SetBool(IS_RUNNING, true);
                FlipDirection(1);
            }
            else if (h < 0)
            {
                body.velocity = new Vector2(-speed, body.velocity.y);
                anim.SetBool(IS_RUNNING, true);
                FlipDirection(-1);
            }
            else
            {
                body.velocity = new Vector2(0f, body.velocity.y);
                anim.SetBool(IS_RUNNING, false);
            }
        }
    }

    void FlipDirection(int direction)
    {
        Vector3 tempDirection = transform.localScale;
        tempDirection.x = direction;
        transform.localScale = tempDirection;
    }

    void CheckIfGorunded()
    {
        isGrounded = Physics2D.Raycast(groundCheckPos.position, Vector2.down, 0.1f, groundLayer);
        
        if (isGrounded && jumped)
        {
            jumped = false;
            anim.SetBool(IS_JUMPING, false);
        }
    }

    void HandleJump()
    {
        if(Time.time > freezeTimer)
        {
            if (isGrounded && Input.GetKey(KeyCode.Space))
            {
                jumped = true;

                body.velocity = new Vector2(body.velocity.x, jumpPower);
                anim.SetBool(IS_JUMPING, true);
            }
        }
       
    }

    void HandleAttack()
    {
        if (Time.time > freezeTimer)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Freeze();
                anim.SetTrigger("attack");
            }
        }
    }

    void HandleCast()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Freeze();
            anim.SetTrigger("cast");
        }
    }

    public void TookDamage(int damage)
    {
        anim.SetTrigger("hurt");

        Freeze();

        if (transform.localScale.x == 1)
        {
            body.velocity = new Vector2(pushMultiplier * damage, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(pushMultiplier * damage, body.velocity.y);
        }
    }

    public void Dead()
    {
        anim.SetTrigger("die");
        this.enabled = false;
        body.velocity = Vector2.zero;
        body.isKinematic = true;
        collider2D.enabled = false;

    }
}

