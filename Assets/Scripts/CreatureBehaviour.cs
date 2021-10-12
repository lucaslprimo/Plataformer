using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    public int hpMax;
    private int hpActual;

    private ICreatureListener listener;    

    public void SetListener(ICreatureListener listener)
    {
        this.listener = listener;
    }

    void Awake()
    {
        hpActual = hpMax;
    }

    public interface ICreatureListener
    {
        void TookDamage(int damage);
        void Dead();
    }

    public void TakeDamage(int damage)
    {
        hpActual = hpActual - damage;

        listener.TookDamage(damage);
        if (hpActual <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        listener.Dead();
    }
}
