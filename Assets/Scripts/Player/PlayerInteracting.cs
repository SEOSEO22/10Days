using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracting : MonoBehaviour
{
    [SerializeField] float gatheringForce = 10f;
    [SerializeField] float attackForce = 10f;
    [SerializeField] float attackDelayTime = 2f;

    private ObjectHPBar objectHP;
    private bool isInteracing = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public IEnumerator Attack(GameObject attackTarget)
    {
        if (!isInteracing)
        {
            yield return new WaitForSeconds(attackDelayTime);

            isInteracing = true;
            // anim.SetTrigger("IsAttacking");

            // 타겟 오브젝트의 체력을 깎는 메소드
            playerState.DecreaseHealthStat(attackForce);
            // anim.ResetTrigger("IsAttacking");
            isInteracing = false;
        }
    }
}
