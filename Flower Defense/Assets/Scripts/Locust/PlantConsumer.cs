using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantConsumer : MonoBehaviour
{
    private PlantDestroyable _food; 
    [SerializeField] private int _eatingRate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Crops"))
        {
            _food= collision.GetComponent<PlantDestroyable>();
            StartCoroutine(EatCoroutine());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_food!=null && collision.gameObject==_food.gameObject)
        {
            _food = null;
        }
    }
    private IEnumerator EatCoroutine()
    {
        while(_food!=null && _food.Hp>0)
        {
            _food.Hp -= _eatingRate;
            yield return new WaitForSeconds(1);
        }
    }
}
