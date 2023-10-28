using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSimply : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Target!=null && Vector3.Distance(transform.position,Target.transform.position)>0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
        }

    }
}
