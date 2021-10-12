using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformerFollow : MonoBehaviour
{
    public Transform destiny;

    private IFollowListener listener;
    public float speed;
    public float limitDistance;

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
            if (destiny.position.x > transform.position.x)
            {
                listener.Move(Vector2.right * speed);
            }
            else if (destiny.position.x < transform.position.x)
            {
                listener.Move(Vector2.left * speed);
            }

            listener.IsWalking(true);
        }
        else
        {
            listener.Move(Vector2.zero);
            listener.IsWalking(false);
        }
        
    }

}
