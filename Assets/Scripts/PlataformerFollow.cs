using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformerFollow : MonoBehaviour
{
    public Transform destiny;

    private IFollowListener listener;
    [SerializeField] float speed;
    [SerializeField] float limitDistance;
    private bool walking = false;
    private Vector2 movement;

    private void Start()
    {
        walking = false;
    }

    public void SetListener(IFollowListener listener)
    {
        this.listener = listener;
    }

    public interface IFollowListener
    {
        void IsWalking(bool walking);
        void Move(Vector2 velocity);
    }
   
    void Update()
    {
        HandleFollowing();
    }

    private void HandleFollowing()
    {
        if(Vector2.Distance(destiny.position,transform.position) > limitDistance){
            walking = true;
            if (destiny.position.x > transform.position.x)
            {
                movement = Vector2.right * speed;
            }
            else if (destiny.position.x < transform.position.x)
            {
                movement = Vector2.left * speed;
            }
        }
        else
        {
            walking = false;
            movement = Vector2.zero;
            
        }
    }

    private void FixedUpdate()
    {
        listener.Move(movement);
    }

    private void LateUpdate()
    {
        listener.IsWalking(walking);
    }
}
