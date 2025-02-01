using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private PlayerState playerStat;
    private Coroutine footstepCoroutine = null; // 현재 실행 중인 코루틴 저장 변수

    private void Start()
    {
        playerStat = GetComponent<PlayerState>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = (Vector3.up * yAxis) + (Vector3.right * xAxis);

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
