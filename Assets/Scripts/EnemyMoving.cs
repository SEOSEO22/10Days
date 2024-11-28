using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float distance = 1f;
    [SerializeField] float attackDelayTime = 2f;

    private Animator anim;
    private Vector3 moveDirection;
    private bool isAttacking = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetDirection();
        EnemyMove();
    }

    // 적 오브젝트가 바라보는 방향 설정
    private void SetDirection()
    {
        moveDirection = (target.transform.position - transform.position).normalized;

        anim.SetFloat("Horizontal", moveDirection.x);
        anim.SetFloat("Vertical", moveDirection.y);
    }

    private void EnemyMove()
    {
        // 타겟과 적 오브젝트간 거리가 Epsilon 값보다 작을 경우 적 오브젝트는 제자리에 존재하며 타겟을 공격.
        if ((target.transform.position - transform.position).magnitude < distance)
        {
            StartCoroutine(EnemyAttack(target));
            anim.SetBool("IsWalking", false);
            return;
        }

        anim.SetBool("IsWalking", true);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    IEnumerator EnemyAttack(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("IsAttacking");
            // 타겟 오브젝트의 체력을 깎는 메소드 삽입 예정
            yield return new WaitForSeconds(attackDelayTime);
            anim.ResetTrigger("IsAttacking");
            isAttacking = false;
        }
    }
}
