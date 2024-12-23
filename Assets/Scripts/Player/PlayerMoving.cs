using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (Vector3.up * yAxis) + (Vector3.right * xAxis);

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
}
