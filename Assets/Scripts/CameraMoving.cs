using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject player;

    private void Start()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.transform.position, .8f * Time.deltaTime);
    }
}
