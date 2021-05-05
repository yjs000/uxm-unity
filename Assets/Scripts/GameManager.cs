using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Make Players

    //public Transform[] spawnPositions;
    public GameObject playerPrefab;
    //public GameObject networkVoiceManager;
    public GameObject spawnPoint;

    public static GameObject player;
    public static bool isSpawned = false;

    //voice
    public Recorder recorder;
    public Toggle MuteToggle;

    //alarm
    public AudioClip JoinClip;
    public AudioClip LeaveClip;
    public AudioSource source;

    ////setting
    //[SerializeField]
    //private GameObject basicSettingPanel;
    //[SerializeField]
    //private GameObject PopupSettingPanel;

    [SerializeField]
    int playerNumInRoom = 0;
    [SerializeField]
    List<GameObject> spawnPointList;

    //public Camera playerCameraPrefab;
    void Start()
    {
        //PhotonNetwork.MasterClient.
        playerPrefab = LauncherTest.selectedAvatarPref;
        playerNumInRoom = PhotonNetwork.CurrentRoom.PlayerCount;
        Vector3 position = spawnPointList[playerNumInRoom -1].transform.position;
        player = SpawnPlayer(position);


        ////voice
        PhotonVoiceView photonVoiceView = player.GetComponent<PhotonVoiceView>();
        recorder = photonVoiceView.RecorderInUse;
        //transmitEnalbed = true이면 Mutetoggle은 false
        //transmitEnabled = false이면 Mutetoggle은 true
        recorder.TransmitEnabled = !LauncherTest.boolMute;
        MuteToggle.isOn = !recorder.TransmitEnabled; // init

        ////SettingCanvas 초기 안 보이게 -> SettingButton 활성화 시 보이게
        //basicSettingPanel.SetActive(true);
        //PopupSettingPanel.SetActive(false);
    }


    private GameObject SpawnPlayer(Vector3 position)
    {
        Debug.Log("!");
        //Vector3 position = spawnPoint.transform.position;
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
        isSpawned = true;
        return player;
    }




    #endregion

    #region Setting Canvas

    //public void OnSettingButton()
    //{
    //    basicSettingPanel.SetActive(false);
    //    PopupSettingPanel.SetActive(true);
    //}

    //public void OnCloseButton()
    //{
    //    PopupSettingPanel.SetActive(false);
    //    basicSettingPanel.SetActive(true);
    //}

    #endregion


    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Home_Kit_Assembled_copy");
    }


    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        //OnLeftRoom()호출 -> Launcher.Start()호출 -> control화면
        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.Destroy(networkVoiceManager);
    }

    public void Mute()
    {
        //MuteToggle이 true이면 transmitEnabled = false
        //MuteToggle이 false이면 transmitEnabled = true
        recorder.TransmitEnabled = !MuteToggle.isOn;
    }

    #endregion

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        //상대플레이어? remote player가 방에 들어오면 호출됨
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }

        ////alarm
        //if (LauncherTest.boolAlarm)
        //{
        //    if (this.JoinClip != null)
        //    {
        //        if (this.source == null) this.source = FindObjectOfType<AudioSource>();
        //        this.source.PlayOneShot(this.JoinClip);
        //    }
        //}
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }

        //if (LauncherTest.boolAlarm)
        //{
        //    if (this.LeaveClip != null)
        //    {
        //        if (this.source == null) this.source = FindObjectOfType<AudioSource>();
        //        this.source.PlayOneShot(this.LeaveClip);
        //    }
        //}
    }

    #endregion
}