using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int MaxHp;
    private int _hp;
    public virtual int Hp
    {
        get { return _hp; }
        set
        {
            if (value > 0)
            {
                _hp = Math.Clamp(value, 0, MaxHp);
            }
            else
            {
                OnHpDown();
            }
        }
    }
    protected virtual void Start()
    {
        Hp = MaxHp;
    }
    protected virtual void OnHpDown()
    {
        Die();
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
