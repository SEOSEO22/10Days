using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToHome : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void OnHomeButtonClicked()
    {
        player.transform.position = Vector2.zero;
    }
}
