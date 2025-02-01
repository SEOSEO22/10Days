using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private PlayerState playerStat;
    private Coroutine footstepCoroutine = null; // ���� ���� ���� �ڷ�ƾ ���� ����

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
            if (footstepCoroutine == null) // �ڷ�ƾ�� ���� ���� �ƴ� ���� ����
            {
                footstepCoroutine = StartCoroutine(PlayFootstepSound());
            }

            // �÷��̾ �����δٸ� ��� ����
            if (GameManager.Instance.GetTimeInfo() == TimeInfo.Day)
            {
                playerStat.DecreaseHungerStat(Time.deltaTime / 6);
            }
        }
        else
        {
            if (footstepCoroutine != null) // �̵��� ���߸� ���� �ڷ�ƾ ����
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
            SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_PLAYER); // �߼Ҹ� ���
            yield return new WaitForSeconds(0.6f); // ��� �ʸ��� �ݺ�
        }
    }
}
