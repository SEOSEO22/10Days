using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float distance = 3f;

    // private Animator anim;
    private Vector3 moveDirection;
    private ObstacleAttacking attacking;
    private GameObject target = null;
    private FindTarget findTarget;

    private void Start()
    {
        findTarget = GetComponent<FindTarget>();
        attacking = GetComponent<ObstacleAttacking>();
        // anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetClosetTarget();
        SetDirection();
        EnemyMove();
    }

    // �� ������Ʈ�� Ÿ�� ����
    private void SetClosetTarget()
    {
        target = findTarget.FindClosetTarget(GameObject.FindGameObjectsWithTag("Build Item"));

        if (target == null) target = GameObject.FindWithTag("Player");
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
            if (target.CompareTag("Build Item"))
            {
                StartCoroutine(attacking.AttackTarget(target));
            }
            else if (target.CompareTag("Player"))
            {
                StartCoroutine(attacking.AttackPlayer(target));
            }
            // anim.SetBool("IsWalking", false);

            return;
        }

        // anim.SetBool("IsWalking", true);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
