using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRadius;
    public int damage;
    public LayerMask layerMask;

    public void CheckIfHits()
    {
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, layerMask);

        foreach(Collider2D collider in colliders2D)
        {
            collider.GetComponent<CreatureBehaviour>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
