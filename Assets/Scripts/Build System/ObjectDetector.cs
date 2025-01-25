using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private TurretSpawner turretSpawner;
    [SerializeField] private TurretDataViewer turretDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTranform = null; // 마우스 픽킹으로 선택한 오브젝트 임시 저장

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 마우스가 UI에 머물러 있다면 아래 코드를 실행하지 않음 (버튼과 상호작용 시 정보가 꺼지는 상태 방지)
        if (EventSystem.current.IsPointerOverGameObject() == true) return;

        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTranform = hit.transform;

                if (hit.transform.CompareTag("Tile"))
                {
                    turretSpawner.SpawnTurret(hit.transform);
                }
                else if (hit.transform.CompareTag("Build Item"))
                {
                    turretDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (hitTranform == null || hitTranform.CompareTag("Build Item") == false)
            {
                turretDataViewer.OffPanel();
            }

            hitTranform = null;
        }
    }
}
