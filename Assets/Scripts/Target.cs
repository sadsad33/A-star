using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    float moveSpeed = 8f;
    float speedX, speedY;
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = 16f;
        } else {
            moveSpeed = 8f;
        }

        speedX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        speedY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(speedX, 0, speedY);
    }
}
