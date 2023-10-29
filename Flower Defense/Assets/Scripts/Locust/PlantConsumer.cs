using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantConsumer : MonoBehaviour
{
    private List<Upgradeable> _food = new List<Upgradeable>();
    [SerializeField] private int _eatingRate;
    private CircleCollider2D collider;
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
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Crops"))
    //    {
    //        _food = collision.GetComponent<PlantDestroyable>();
    //        StartCoroutine(EatCoroutine());
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (_food != null && collision.gameObject == _food.gameObject)
    //    {
    //        _food = null;
    //    }
    //}
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
        //while (_food != null && _food.Hp > 0)
        //{
        //    _food.Hp -= _eatingRate;
        //    yield return new WaitForSeconds(1);
        //}
    }
}
