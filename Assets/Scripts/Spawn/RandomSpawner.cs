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
            // 최대 반지름으로 원 내부의 랜덤한 점 생성
            randomPosition = Random.insideUnitCircle * maxRadius;
        }
        // 최소 반지름 조건을 만족하지 않으면 다시 생성
        while (randomPosition.magnitude < minRadius);

        return randomPosition;
    }

    // Gizmos로 반지름 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // minRadius 영역 색상
        Gizmos.DrawWireSphere(transform.position, minRadius);

        Gizmos.color = Color.red; // maxRadius 영역 색상
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
