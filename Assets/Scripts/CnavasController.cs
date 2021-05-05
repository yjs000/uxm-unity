using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CnavasController : MonoBehaviour
{
    public Camera myCamera;
    public GameObject myCanvas;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.isSpawned)
        {
            myCamera = GameManager.player.GetComponentInChildren<Camera>();
            myCanvas = GameObject.Find("Canvas");
            myCanvas.GetComponent<Canvas>().worldCamera = myCamera;
        }
    }
}
