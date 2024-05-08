using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;
    private GameObject player;
    public Transform target;
    public bool ismoving = false;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
   
    void Start()
    {
        //player = GameObject.Find("Player");
    }


    private void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        rig.MovePosition(pos);
        transform.LookAt(pos);
        ismoving = true;
    }
}
