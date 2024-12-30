using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [Header("Enemy Moving")]
    [SerializeField] GameObject target;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float distance = 1f;

    [Header("Enemy Attacking")]
    [SerializeField] float attackForce = 10f;
    [SerializeField] float attackDelayTime = 2f;

    // private Animator anim;
    private PlayerState playerState;
    private Vector3 moveDirection;
    private bool isAttacking = false;

    private void Start()
    {
        playerState = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
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
        // Ÿ�ٰ� �� ������Ʈ�� �Ÿ��� Epsilon ������ ���� ��� �� ������Ʈ�� ���ڸ��� �����ϸ� Ÿ���� ����.
        if ((target.transform.position - transform.position).magnitude < distance)
        {
            StartCoroutine(EnemyAttack(target));
            // anim.SetBool("IsWalking", false);
            return;
        }

        // anim.SetBool("IsWalking", true);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    IEnumerator EnemyAttack(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            // anim.SetTrigger("IsAttacking");

            yield return new WaitForSeconds(attackDelayTime);

            // Ÿ�� ������Ʈ�� ü���� ��� �޼ҵ�
            playerState.DecreaseHealthStat(attackForce);
            // anim.ResetTrigger("IsAttacking");
            isAttacking = false;
        }
    }
}
