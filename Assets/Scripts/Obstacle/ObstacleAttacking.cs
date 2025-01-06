using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObstacleAttacking : MonoBehaviour
{
    [SerializeField] float attackForce = 10f;
    [SerializeField] float attackDelayTime = 2f;

    private PlayerState playerState;
    private bool isAttacking = false;

    private void Start()
    {
        playerState = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
    }

    public IEnumerator AttackPlayer(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            yield return new WaitForSeconds(attackDelayTime);

            isAttacking = true;
            // anim.SetTrigger("IsAttacking");

            // 타겟 오브젝트의 체력을 깎는 메소드
            playerState.DecreaseHealthStat(attackForce);
            // anim.ResetTrigger("IsAttacking");
            isAttacking = false;
        }
    }
}
