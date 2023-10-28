using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDestroyable : Destroyable
{
    private ProjectileTower _towerComponent;
    public override int Hp
    {
        get { return base.Hp; }
        set
        {
            if(Hp==0 && value>0)
            {
                _towerComponent.SetActive(true);
            }
            base.Hp = value;
        }
    }
    protected override void Start()
    {
        _towerComponent= GetComponentInParent<ProjectileTower>();
        base.Start();
    }
    protected override void OnHpDown()
    {
        _towerComponent.SetActive(false);
    }
}
