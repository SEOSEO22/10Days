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

    // 적 오브젝트의 타겟 설정
    private void SetClosetTarget()
    {
        target = findTarget.FindClosetTarget(GameObject.FindGameObjectsWithTag("Build Item"));

        if (target == null) target = GameObject.FindWithTag("Player");
    }

    // 적 오브젝트가 바라보는 방향 설정
    private void SetDirection()
    {
        moveDirection = (target.transform.position - transform.position).normalized;

        // anim.SetFloat("Horizontal", moveDirection.x);
        // anim.SetFloat("Vertical", moveDirection.y);
    }

    private void EnemyMove()
    {
        // 타겟과 적 오브젝트간 거리가 설정 거리보다 작을 경우 적 오브젝트는 제자리에 존재하며 타겟을 공격.
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
