using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMoving : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float distance = 10f;
    [SerializeField] float attackDistance = 1f;

    // private Animator anim;
    private ObstacleAttacking attacking; // 공격 스크립트
    private Collider2D trigger; // 트리거 컴포넌트
    private Vector3 moveDirection;
    private float currentDistance = 0f; // 타겟과의 현재 거리
    private bool isMoving; // 현재 오브젝트 움직임 상태
    private bool isTargeting;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        attacking = GetComponent<ObstacleAttacking>();
        trigger = GetComponent<Collider2D>();
        // anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isMoving = false;
    }

    private void Update()
    {
        SetDirection();
        AnimalMove();
    }

    private void AnimalMove()
    {
        currentDistance = Vector2.Distance(transform.position, target.transform.position);

        if (!isTargeting && !isMoving)
        {
            isMoving = true;
            StartCoroutine(NormalMoveing());
        }

        if (currentDistance > distance)
        {
            trigger.enabled = false;
        }
        else
        {
            trigger.enabled = true;
        }
    }

    // 적 오브젝트가 바라보는 방향 설정
    private void SetDirection()
    {
        moveDirection = (target.transform.position - transform.position).normalized;

        // anim.SetFloat("Horizontal", moveDirection.x);
        // anim.SetFloat("Vertical", moveDirection.y);
    }

    // 일반적인 행동을 취하는 함수(타깃과 거리가 멀 경우)
    private IEnumerator NormalMoveing()
    {
        int randomMove = Random.Range(0, 3); // 0 : Standing, 1~2 : Moving
        float movingTime = 0f; // 이동 시간
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;

        while (movingTime < 2f)
        {
            if (isTargeting) yield break;

            movingTime += Time.deltaTime;

            if (randomMove > 0) // 랜덤한 방향으로 이동
            {
                transform.Translate(randomDirection * moveSpeed * Time.deltaTime);
            }

            yield return null; // 다음 프레임까지 대기
        }

        isMoving = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMoving = false;
            isTargeting = true;
            TargetingMoving();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTargeting = false;
        }
    }

    // 타깃을 쫒아가는 함수(타깃과 거리가 가까울 경우)
    private void TargetingMoving()
    {
        // 타겟과 적 오브젝트간 거리가 설정 거리보다 작을 경우 적 오브젝트는 제자리에 존재하며 타겟을 공격.
        if ((target.transform.position - transform.position).magnitude < attackDistance)
        {
            StartCoroutine(attacking.AttackPlayer(target));
            // anim.SetBool("IsWalking", false);
            return;
        }

        // anim.SetBool("IsWalking", true);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
