using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDestroyable : Destroyable
{
    private ProjectileTower _towerComponent;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Color _dryColor = new Color(0.47f, 0.38f, 0.11f);
    public override int Hp
    {
        get { return base.Hp; }
        set
        {
            if(Hp==0 && value>0 && _towerComponent!=null)
            {
                _towerComponent.SetActive(true);
            }
            _sprite.color = Color.Lerp(_dryColor, Color.white,(Hp/(float)MaxHp));
            base.Hp = value;
        }
    }
    protected override void Start()
    {
        _sprite=GetComponentInChildren<SpriteRenderer>();
        _towerComponent= GetComponentInParent<ProjectileTower>();
        base.Start();
    }
    protected override void OnHpDown()
    {
        if(_towerComponent!=null)
        {
            _towerComponent.SetActive(false);
        }
    }
    public void ChangeLevel(int dmg, float cd, float speed)
    {
        if(_towerComponent!=null)
        {
            _towerComponent.ProjectileDamage = dmg;
            _towerComponent.ProjectileSpeed= speed;
            _towerComponent.CoolDown= cd;
        }
    }
}
