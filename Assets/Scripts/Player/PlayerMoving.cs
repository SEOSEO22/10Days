using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private BoxCollider2D toolCollider;

    private PlayerState playerStat;
    private Coroutine footstepCoroutine = null; // 현재 실행 중인 코루틴 저장 변수
    private SpriteRenderer renderer;
    private Animator anim;
    private bool isHorizonMove;

    private void Start()
    {
        playerStat = GetComponent<PlayerState>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        if (hDown)
        {
            isHorizonMove = true;
            toolCollider.offset = new Vector2((int)xAxis, 0) + Vector2.down;
        }
        else if (vDown)
        {
            isHorizonMove = false;
            toolCollider.offset = new Vector2(0, (int)yAxis) + Vector2.down;
        }
        else if (hUp || vUp)
        {
            isHorizonMove = xAxis != 0;
        }

        // 애니메이션
        if (anim.GetInteger("hAxisRaw") != xAxis)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)xAxis);
        }
        else if (anim.GetInteger("vAxisRaw") != yAxis)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)yAxis);
        }
        else
        {
            anim.SetBool("isChange", false);
        }

        if (xAxis < 0)
        {
            renderer.flipX = true;
        }
        else if (xAxis > 0)
        {
            renderer.flipX = false;
        }

        Vector2 moveDir = isHorizonMove ? new Vector2(xAxis, 0) : new Vector2(0, yAxis);

        if (moveDir != Vector2.zero)
        {
            if (footstepCoroutine == null) // 코루틴이 실행 중이 아닐 때만 실행
            {
                footstepCoroutine = StartCoroutine(PlayFootstepSound());
            }

            // 플레이어가 움직인다면 허기 감소
            if (GameManager.Instance.GetTimeInfo() == TimeInfo.Day)
            {
                playerStat.DecreaseHungerStat(Time.deltaTime / 6);
            }
        }
        else
        {
            if (footstepCoroutine != null) // 이동을 멈추면 기존 코루틴 종료
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }


        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private IEnumerator PlayFootstepSound()
    {
        while (true)
        {
            SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_PLAYER); // 발소리 재생
            yield return new WaitForSeconds(0.6f); // 대기 초마다 반복
        }
    }
}
