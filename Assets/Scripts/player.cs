using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class player : MonoBehaviourPun
{

    //[SerializeField]
    //float speed = 100;  

    [SerializeField]
    private TextMesh playerName;

    GameObject testText;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            Debug.Log("is mine: " + photonView.Owner.NickName);
            GetComponentInChildren<Camera>().depth = 1;
            GetComponentInChildren<AudioListener>().enabled = true;
            Debug.Log("is my camera: " + GetComponentInChildren<Camera>().depth);
            Debug.Log("is my audiolistener : " + GetComponentInChildren<AudioListener>().enabled);
        }
        else
        {
            Debug.Log("not mine: " + photonView.Owner.NickName);
            Transform TrackingSpace = this.transform.FindDeepChild("TrackingSpace");
            Destroy(TrackingSpace.Find("LeftEyeAnchor").gameObject);
            Destroy(TrackingSpace.Find("CenterEyeAnchor").gameObject);
            Destroy(TrackingSpace.Find("RightEyeAnchor").gameObject);
            //Debug.Log("not my camera : " + GetComponentInChildren<Camera>().depth);
            //Debug.Log("not my audiolistener : " + GetComponentInChildren<AudioListener>().enabled);
        }
        playerName.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
		if (photonView.IsMine)
		{
            //GameObject UMADynamicCharacterAvatar = GameObject.Find("UMADynamicCharacterAvatar");
            //UMADynamicCharacterAvatar.GetComponent<Rigidbody>().useGravity = false;
            //UMADynamicCharacterAvatar.GetComponent<CapsuleCollider>().enabled = false;

            //fix Y axis
            float yAxis = GetComponent<Transform>().position.y;
            if(yAxis < 0f || yAxis > 0f)
			{
                GetComponent<Transform>().position = new Vector3(transform.position.x, 0, transform.position.z);

            }
		}

		//if (!photonView.IsMine)
  //      {
  //          GetComponentInChildren<AudioListener>().enabled = false;
  //          //Debug.Log("not mine. audioListner false."); 
  //      }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject UMADynamicCharacterAvatar = GameObject.Find("UMADynamicCharacterAvatar");
            UMADynamicCharacterAvatar.GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("avatar collision!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject UMADynamicCharacterAvatar = GameObject.Find("UMADynamicCharacterAvatar");
        UMADynamicCharacterAvatar.GetComponent<Rigidbody>().isKinematic = false;
    }

}
