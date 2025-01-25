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
    private Transform hitTranform = null; // ���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // ���콺�� UI�� �ӹ��� �ִٸ� �Ʒ� �ڵ带 �������� ���� (��ư�� ��ȣ�ۿ� �� ������ ������ ���� ����)
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
