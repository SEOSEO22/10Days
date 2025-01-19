using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private PlayerState playerStat;

    private void Start()
    {
        playerStat = GetComponent<PlayerState>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = (Vector3.up * yAxis) + (Vector3.right * xAxis);

        // 플레이어가 움직인다면 허기 감소
        if (moveDir != Vector2.zero && GameManager.Instance.GetTimeInfo() == TimeInfo.Day) playerStat.DecreaseHungerStat(Time.deltaTime / 6);

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
}
