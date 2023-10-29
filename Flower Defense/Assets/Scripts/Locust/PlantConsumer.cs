using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantConsumer : MonoBehaviour
{
    private List<Upgradeable> _food = new List<Upgradeable>();
    [SerializeField] private int _eatingRate;
    private CircleCollider2D collider;
    private CastleDestroyable _castle;
    private void Start()
    {
        collider= GetComponent<CircleCollider2D>();
        StartCoroutine(EatCoroutine());
    }
    private void Update()
    {
        _food.Clear();
        var colliders = Physics2D.OverlapCircleAll(transform.position, collider.radius);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Crops"))
            {
                var dest = col.GetComponent<Upgradeable>();
                if (dest != null)
                {
                    _food.Add(dest);
                }
            }
            else if(col.CompareTag("Castle") && _castle==null)
            {
                _castle=col.GetComponent<CastleDestroyable>();
                StartCoroutine(EatCastleCoroutine());
            }
        }
    }
    private IEnumerator EatCastleCoroutine()
    {
        while(true)
        {
            _castle.Hp -= _eatingRate;
            yield return new WaitForSeconds(1);
        }
    }
  
    private IEnumerator EatCoroutine()
    {
        while(true)
        {
            foreach(var item in _food)
            {
                item.CurrentProgress -= _eatingRate;
            }
            yield return new WaitForSeconds(1);
        }
       
    }
}
