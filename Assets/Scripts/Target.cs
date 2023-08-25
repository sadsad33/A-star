using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Vector3 moveDir;
    private float moveSpeed = 3f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        transform.LookAt(transform.position + moveDir);
        moveSpeed = Input.GetButton("Run") ? moveSpeed * 2 : moveSpeed;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
