using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform target;
    private float moveSpeed = 1.5f;
    void Start()
    {
        target = GameObject.Find("Target").GetComponent<Transform>();
    }
    void Update()
    {
        Trace();
    }

    void Trace()
    {
        Debug.Log("ÃßÀû Áß");
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
