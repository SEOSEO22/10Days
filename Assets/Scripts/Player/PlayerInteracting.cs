using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracting : MonoBehaviour
{
    [SerializeField] float gatheringForce = 10f;
    [SerializeField] float attackForce = 10f;
    [SerializeField] float delayTime = 1f;

    private BoxCollider2D toolCollider;
    private bool isInteracing = false;

    private void Start()
    {
        toolCollider = GetComponent<BoxCollider2D>();
        toolCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toolCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Harvest harvestObject = collision.GetComponent<Harvest>();
        int collisionLayer = collision.gameObject.layer;

        if (harvestObject != null)
        {
            if (collisionLayer == LayerMask.NameToLayer("Enemy") || (collisionLayer == LayerMask.NameToLayer("Animal") && collision is BoxCollider2D))
            {
                StartCoroutine(Attack(collision.gameObject));
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Resource"))
            {
                StartCoroutine(HarvestObject(collision.gameObject));
            }
        }

        toolCollider.enabled = false;
    }

    public IEnumerator Attack(GameObject attackTarget)
    {
        if (!isInteracing)
        {
            isInteracing = true;
            // anim.SetTrigger("IsAttacking");

            // 타겟 오브젝트의 체력을 깎는 메소드
            ObjectHPBar targetHP = attackTarget.gameObject.GetComponentInChildren<ObjectHPBar>();
            targetHP.Damaged(attackForce);

            // anim.ResetTrigger("IsAttacking");

            yield return new WaitForSeconds(delayTime);
            isInteracing = false;
        }
    }

    public IEnumerator HarvestObject(GameObject harvestTarget)
    {
        if (!isInteracing)
        {
            isInteracing = true;
            // anim.SetTrigger("IsAttacking");

            // 타겟 오브젝트의 체력을 깎는 메소드
            ObjectHPBar targetHP = harvestTarget.gameObject.GetComponentInChildren<ObjectHPBar>();
            targetHP.Damaged(attackForce);
            harvestTarget.GetComponent<Harvest>().SetHarvestParticle();

            // anim.ResetTrigger("IsAttacking");

            yield return new WaitForSeconds(delayTime);
            isInteracing = false;
        }
    }
}
