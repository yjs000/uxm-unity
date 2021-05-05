using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SettingCanvas : MonoBehaviour
{
    public Camera myCamera;
    public GameObject myCanvas;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.isSpawned)
        {
            myCamera = GameManager.player.GetComponentInChildren<Camera>();
            myCanvas = GameObject.Find("SettingCanvas");
            myCanvas.GetComponent<Canvas>().worldCamera = myCamera;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
