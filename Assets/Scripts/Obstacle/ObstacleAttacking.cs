using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObstacleAttacking : MonoBehaviour
{
    [SerializeField] float attackForce = 10f;
    [SerializeField] float attackDelayTime = 2f;

    private PlayerState playerState;
    private ObjectHPBar targetHP;
    private bool isAttacking = false;

    private void Start()
    {
        playerState = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
    }

    public IEnumerator AttackTarget(GameObject attackTarget)
    {
        if (attackTarget == null) yield break;

        targetHP = attackTarget.gameObject.GetComponentInChildren<ObjectHPBar>();

        if (targetHP == null) yield break;
        if (isAttacking) yield break; // �̹� ���� ���̸� �ߴ�

        isAttacking = true;
        // anim.SetTrigger("IsAttacking");

        // Ÿ�� ������Ʈ�� ü���� ��� �޼ҵ�
        Debug.Log("����");
        targetHP.Damaged(attackForce);

        yield return new WaitForSeconds(attackDelayTime);

        // anim.ResetTrigger("IsAttacking");
        isAttacking = false;

    }

    public IEnumerator AttackPlayer(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            yield return new WaitForSeconds(attackDelayTime);

            isAttacking = true;
            // anim.SetTrigger("IsAttacking");

            // Ÿ�� ������Ʈ�� ü���� ��� �޼ҵ�
            playerState.DecreaseHealthStat(attackForce);
            // anim.ResetTrigger("IsAttacking");
            isAttacking = false;
        }
    }
}
