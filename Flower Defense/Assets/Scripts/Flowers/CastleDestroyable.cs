using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleDestroyable : Destroyable
{
    [SerializeField] private Slider _hpSlider;
    private GameManager _gameManager;
    private void Awake()
    {
        _hpSlider = GetComponentInChildren<Slider>();
        _gameManager= FindObjectOfType<GameManager>();
    }
    public override int Hp
    {
        get { return base.Hp; }
        set
        {
            base.Hp = value;
            _hpSlider.value = Hp / (float)MaxHp;
        }
    }
    protected override void OnHpDown()
    {
        _gameManager.GameOver(false);
    }
}
