using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int MaxHp;
    private int _hp;
    [HideInInspector] public StateManager stateManager;

    public virtual int Hp
    {
        get { return _hp; }
        set
        {
            _hp = Math.Clamp(value, 0, MaxHp);
            if (_hp == 0)
            {
                OnHpDown();
            }

            if (stateManager != null)
            {
                stateManager.HpChanged(this);
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