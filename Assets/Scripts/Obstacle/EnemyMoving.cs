using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float distance = 3f;

    // private Animator anim;
    private Vector3 moveDirection;
    private ObstacleAttacking attacking;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        attacking = GetComponent<ObstacleAttacking>();
        // anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetDirection();
        EnemyMove();
    }

    // �� ������Ʈ�� �ٶ󺸴� ���� ����
    private void SetDirection()
    {
        moveDirection = (target.transform.position - transform.position).normalized;

        // anim.SetFloat("Horizontal", moveDirection.x);
        // anim.SetFloat("Vertical", moveDirection.y);
    }

    private void EnemyMove()
    {
        // Ÿ�ٰ� �� ������Ʈ�� �Ÿ��� ���� �Ÿ����� ���� ��� �� ������Ʈ�� ���ڸ��� �����ϸ� Ÿ���� ����.
        if ((target.transform.position - transform.position).magnitude < distance)
        {
            StartCoroutine(attacking.AttackPlayer(target));
            // anim.SetBool("IsWalking", false);
            return;
        }

        // anim.SetBool("IsWalking", true);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
