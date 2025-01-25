using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private TurretSpawner turretSpawner;
    [SerializeField] private BarrierSpawner barrierSpawner;
    [SerializeField] private StructureDataViewer structureDataViewer;

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
                    if (turretSpawner.isOnBuildButton) turretSpawner.SpawnTurret(hit.transform);
                    else if (barrierSpawner.isOnBuildButton) barrierSpawner.SpawnBarrier(hit.transform);
                }
                else if (hit.transform.CompareTag("Build Item"))
                {
                    structureDataViewer.InitCurrentStructure();
                    structureDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (hitTranform == null || hitTranform.CompareTag("Build Item") == false)
            {
                if (turretSpawner.isOnBuildButton == false) structureDataViewer.OffPanel();
                else if (barrierSpawner.isOnBuildButton == false) structureDataViewer.OffPanel();
            }

            hitTranform = null;
        }
    }
}
