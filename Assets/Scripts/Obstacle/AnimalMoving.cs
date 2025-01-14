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
    private ObstacleAttacking attacking; // ���� ��ũ��Ʈ
    private Collider2D trigger; // Ʈ���� ������Ʈ
    private Vector3 moveDirection;
    private float currentDistance = 0f; // Ÿ�ٰ��� ���� �Ÿ�
    private bool isMoving; // ���� ������Ʈ ������ ����
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

    // �� ������Ʈ�� �ٶ󺸴� ���� ����
    private void SetDirection()
    {
        moveDirection = (target.transform.position - transform.position).normalized;

        // anim.SetFloat("Horizontal", moveDirection.x);
        // anim.SetFloat("Vertical", moveDirection.y);
    }

    // �Ϲ����� �ൿ�� ���ϴ� �Լ�(Ÿ��� �Ÿ��� �� ���)
    private IEnumerator NormalMoveing()
    {
        int randomMove = Random.Range(0, 3); // 0 : Standing, 1~2 : Moving
        float movingTime = 0f; // �̵� �ð�
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;

        while (movingTime < 2f)
        {
            if (isTargeting) yield break;

            movingTime += Time.deltaTime;

            if (randomMove > 0) // ������ �������� �̵�
            {
                transform.Translate(randomDirection * moveSpeed * Time.deltaTime);
            }

            yield return null; // ���� �����ӱ��� ���
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

    // Ÿ���� �i�ư��� �Լ�(Ÿ��� �Ÿ��� ����� ���)
    private void TargetingMoving()
    {
        // Ÿ�ٰ� �� ������Ʈ�� �Ÿ��� ���� �Ÿ����� ���� ��� �� ������Ʈ�� ���ڸ��� �����ϸ� Ÿ���� ����.
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
