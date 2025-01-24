using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    private Movement movement2D;
    private Transform target;

    public void Setup(Transform target, float damage)
    {
        movement2D = GetComponent<Movement>();
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if (target != null && target.gameObject.activeSelf)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.transform != target) return;

        if (target.gameObject.activeSelf)
        {
            collision.GetComponentInChildren<ObjectHPBar>().Damaged(damage);
        }

        gameObject.SetActive(false);
    }
}
