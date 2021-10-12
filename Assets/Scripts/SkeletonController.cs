using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour, CreatureBehaviour.ICreatureListener, PlataformerFollow.IFollowListener
{
    private const string PLAYER_TAG = "Player";

    private Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private GameObject player;
    private int direction = 1;


    public CreatureBehaviour creatureBehaviour;
    public PlataformerFollow plataformerFollow;
    public float freezeTime = 0.5f;
    public float pushMultiplier = 100f;
    private float freezeTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        plataformerFollow.destiny = player.transform;
        plataformerFollow.SetListener(this);
        creatureBehaviour.SetListener(this);
    }

    public void Dead()
    {
        anim.SetTrigger("die");
        body.isKinematic = true;
        body.velocity = Vector2.zero;
        boxCollider.enabled = false;
        plataformerFollow.enabled = false;
        enabled = false;
    }

    public void TookDamage(int damage)
    {
        Freeze();
        anim.SetTrigger("hurt");
        if (direction == 1) {
            body.velocity = new Vector2(pushMultiplier * damage, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(pushMultiplier * damage, body.velocity.y);
        }
    }

    private void Freeze()
    {
        freezeTimer = Time.time + freezeTime;
    }

    private void Update()
    {
        HandleDirection();
    }

    private void HandleDirection()
    {
        if (Time.time > freezeTimer)
        {
            if (player.transform.position.x > transform.position.x && direction != 1)
            {
                Vector3 tempDirection = transform.localScale;
                tempDirection.x = 1;
                transform.localScale = tempDirection;

                direction = 1;
            }
            else if (player.transform.position.x < transform.position.x && direction != -1)
            {
                Vector3 tempDirection = transform.localScale;
                tempDirection.x = -1;
                transform.localScale = tempDirection;

                direction = -1;
            }
        }
    }

    public void IsWalking(bool walking)
    {
        if (Time.time > freezeTimer)
        {
            anim.SetBool("isWalking", walking);

            if (!walking)
            {
                anim.SetTrigger("attack");
                Freeze();
            }
        }        
    }

    public void Move(Vector2 velocity)
    {
        if(Time.time > freezeTimer)
        {
            body.velocity = velocity;
        }         
    }
}
