using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float distance = 1.5f;

    // private Animator anim;
    private Vector3 moveDirection;
    private ObstacleAttacking attacking;
    private GameObject target = null;
    private FindTarget findTarget;
    private Animator anim;

    private void Start()
    {
        findTarget = GetComponent<FindTarget>();
        attacking = GetComponent<ObstacleAttacking>();
        anim = GetComponent<Animator>();
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
    }

    // �ִϸ��̼� ���� ����
    public void SetAnimationDirection(float xValue, float yValue)
    {
        if (Mathf.Abs(xValue) >= Mathf.Abs(yValue))
        {
            if (anim.GetInteger("xAxis") != Mathf.Sign(xValue))
            {
                anim.SetBool("isChange", true);
                anim.SetInteger("xAxis", (int)Mathf.Sign(xValue));
                anim.SetInteger("yAxis", 0);
            }
            else anim.SetBool("isChange", false);
        }
        else
        {
            if (anim.GetInteger("yAxis") != Mathf.Sign(yValue))
            {
                anim.SetBool("isChange", true);
                anim.SetInteger("yAxis", (int)Mathf.Sign(yValue));
                anim.SetInteger("xAxis", 0);
            }
            else anim.SetBool("isChange", false);
        }
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

            return;
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        SetAnimationDirection(moveDirection.x, moveDirection.y);
    }
}
