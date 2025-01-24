using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private TurretSpawner turretSpawner;
    [SerializeField] private TurretDataViewer turretDataViewer;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
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
    }
}
