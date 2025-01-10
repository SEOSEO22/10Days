using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private float minRadius = 20f;
    [SerializeField] private float maxRadius = 100f;

    public Vector2 GetRandomSpawnPosition()
    {
        return GetRandomPosition(minRadius, maxRadius);
    }

    private Vector2 GetRandomPosition(float minRadius, float maxRadius) {
        Vector2 randomPosition;

        do
        {
            // �ִ� ���������� �� ������ ������ �� ����
            randomPosition = Random.insideUnitCircle * maxRadius;
        }
        // �ּ� ������ ������ �������� ������ �ٽ� ����
        while (randomPosition.magnitude < minRadius);

        return randomPosition;
    }

    // Gizmos�� ������ �ð�ȭ
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // minRadius ���� ����
        Gizmos.DrawWireSphere(transform.position, minRadius);

        Gizmos.color = Color.red; // maxRadius ���� ����
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
