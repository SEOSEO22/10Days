using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed = 4f;

    private Animator anim;
    private Vector3 moveDirection;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetDirection();
        EnemyMove();
    }

    private void SetDirection()
    {
        moveDirection = (player.transform.position - transform.position).normalized;

        anim.SetFloat("Horizontal", moveDirection.x);
        anim.SetFloat("Vertical", moveDirection.y);
    }

    private void EnemyMove()
    {
        //Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
