using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Destroyable Target;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, _speed * Time.deltaTime);
        }
        else Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject==Target.gameObject)
        {
            Target.Hp -= _damage;
            Destroy(gameObject);
        }
    }

}
