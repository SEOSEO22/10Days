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
    private Animator anim;
    private bool isAttacking = false;

    private void Start()
    {
        playerState = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isAttacking = false;
    }

    public IEnumerator AttackTarget(GameObject attackTarget)
    {
        if (attackTarget == null) yield break;

        anim.SetTrigger("isAttack");
        targetHP = attackTarget.gameObject.GetComponentInChildren<ObjectHPBar>();

        if (targetHP == null) yield break;
        if (isAttacking) yield break; // 이미 공격 중이면 중단

        isAttacking = true;
        // anim.SetTrigger("IsAttacking");

        // 타겟 오브젝트의 체력을 깎는 메소드
        targetHP.Damaged(attackForce);

        yield return new WaitForSeconds(attackDelayTime);

        // anim.ResetTrigger("IsAttacking");
        isAttacking = false;

    }

    public IEnumerator AttackPlayer(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("isAttack");

            // 타겟 오브젝트의 체력을 깎는 메소드
            playerState.DecreaseHealthStat(attackForce);
            // anim.ResetTrigger("IsAttacking");

            yield return new WaitForSeconds(attackDelayTime);
            isAttacking = false;
        }
    }
}
