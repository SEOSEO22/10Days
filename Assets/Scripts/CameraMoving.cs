using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        miniMapCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
