using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class LauncherTest : MonoBehaviourPunCallbacks
{
    //panels
    [SerializeField]
    private GameObject loadingPanel;

    [SerializeField]
    private GameObject controlPanel;

    [SerializeField]
    private GameObject createRoomPanel;

    [SerializeField]
    private GameObject settingPanel;

    [SerializeField]
    private GameObject popupPanel;

    [SerializeField]
    private GameObject progressLabel;



    // loadingPanel
    [SerializeField]
    private InputField playerNameInputField;

    [SerializeField]
    private Text connectionInfoText;


    //controlPanel
    [SerializeField]
    private InputField roomCodeInputField;


    //createRoomPanel
    [SerializeField]
    Slider maxPlayerSlider;
    [SerializeField]
    Text sliderNum;

    //popupWindow
    /// <summary> Meeting Room->0+code , Lecture Room->1+code </summary>
    [SerializeField]
    private Text roomCode;


    //settingPanel
    [SerializeField]
    private Toggle muteToggle;
    public static bool boolMute;
    [SerializeField]
    private Toggle alarmToggle;
    public static bool boolAlarm;


    [SerializeField]
    List<GameObject> avatarList = new List<GameObject>();

    [SerializeField]
    GameObject spawnPoint;

    [SerializeField]
    Sprite avatar1;
    [SerializeField]
    Sprite avatar2;
    [SerializeField]
    Sprite avatar3;

    // private
    string gameVersion = "1";
    bool isCreatingRoom;
    string selectedRoomOption;
    int maxPlayer = 4;

    //avatar
    int avatarListNum;
    GameObject image;
    /// <summary>
    /// 현재 선택된 아바타 프리팹
    /// </summary>
    public static GameObject selectedAvatarPref;

    // Start is called before the first frame update
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        image = GameObject.Find("Image");
    }

    void Start()
    {
        //최초 시작 시에는 네트워크와 연결되지 않아 연결하는 화면으로 이동(else)
        //방 입장 후 leave room 실행 시에는 바로 로비화면으로 이동(if)
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("바로 로비로 갑니다.");
            SetScene("control");
        }
        else
        {
            SetScene("loading");
            connectionInfoText.text = "offline";
        }
    }


    #region loadingPanel
    public void Connect()
    {
        Debug.Log("connect()호출");
        if (PhotonNetwork.IsConnected)
        {
            SetScene("control");
        }
        else
        {
            if (playerNameInputField.text == "")
            {
                Debug.Log("입력값이 없습니다.");
                return;
            }
            connectionInfoText.text = "연결중...";
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void Close() //App종료
    {
        Debug.Log("App을 종료합니다.");
        Application.Quit();
    }


    #endregion

    #region controlPanel

    public void OfflineButton()
    {
        PhotonNetwork.Disconnect();
    }

    public void PlusButton() // 방만들기 팝업창
    {
        SetScene("createroom");
    }

    public void PlayButton()
    {
        Play();
    }

    public void SettingButton()
    {
        SetScene("setting");
    }

    #endregion


    #region createroomPanel


    public void LectureRoomToggle()
    {
        selectedRoomOption = "Lecture Room";
    }

    public void MeetingRoomToggle()
    {
        //회의실 선택
        selectedRoomOption = "Meeting Room";
    }

    public void setMaxPlayer()
    {
        maxPlayer = (int)maxPlayerSlider.value;
        sliderNum.text = maxPlayer.ToString();
        Debug.Log("maxPlayer : " + maxPlayer);
    }

    public void CreateRoomButton()
    {
        popupPanel.SetActive(true);
        CreateCode();
    }

    public void CreateRoom_closeButton()
    {
        SetScene("control");
    }
    #endregion

    #region popupPanel

    public void CopyButton()
    {
        GUIUtility.systemCopyBuffer = roomCode.text;
    }

    public void YesButton() //입장버튼
    {
        SetScene("progress");
        isCreatingRoom = true;
        Play(); //방만들고 입장
    }

    public void NoButton()
    {
        SetScene("createroom");
    }
    #endregion

    #region settingPanel

    public void Setting_closeButton()
    {
        SetScene("control");
    }

    public void Setting_OKButton()
    {
        boolMute = muteToggle.isOn;
        boolAlarm = alarmToggle.isOn;
        SetScene("control");
    }
    #endregion

    #region Method
    public void SetScene(int sceneNumber)
    {
        bool[] sceneToBool = new bool[6] { false, false, false, false, false, false };
        sceneToBool[sceneNumber] = true;

        loadingPanel.SetActive(sceneToBool[0]);
        controlPanel.SetActive(sceneToBool[1]);
        createRoomPanel.SetActive(sceneToBool[2]);
        popupPanel.SetActive(sceneToBool[3]);
        progressLabel.SetActive(sceneToBool[4]);
        settingPanel.SetActive(sceneToBool[5]);
    }



    public void SetScene(string sceneName)
    {
        Dictionary<string, int> sceneNameToNum = new Dictionary<string, int>
        {
            ["loading"] = 0,
            ["control"] = 1,
            ["createroom"] = 2,
            ["popupwindow"] = 3,
            ["progress"] = 4,
            ["setting"] = 5

        };
        int sceneNum = sceneNameToNum[sceneName.ToLower()];
        SetScene(sceneNum);

        if (sceneName == "control")
        {
            selectedAvatarPref = avatarList[0];
            //spawned = Instantiate(selectedAvatarPref, spawnPoint.transform.position, Quaternion.identity);
            image.GetComponent<Image>().sprite = avatar1;
            //isSpawned = true;
        }
    }

    public void Play()
    {
        SetScene("progress");

        if (isCreatingRoom)
        {
            Debug.Log("creating Room : " + isCreatingRoom);
            //creatingroomPanel에서 방만들고 입장
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)maxPlayer;
            string _roomName = roomCode.text;
            Debug.Log(_roomName);

            PhotonNetwork.CreateRoom(_roomName, roomOptions);
        }
        else
        {
            Debug.Log("creating Room : " + isCreatingRoom);
            //방을 만들지 않고 코드로 입장하는 경우
            if (roomCodeInputField.text == "")
            {
                Debug.Log("입력값이 없습니다.");
                SetScene("control");
                return;
            }
            Debug.Log("조인룸호출");
            PhotonNetwork.JoinRoom(roomCodeInputField.text);
        }
    }

    /// <summary>
    /// roomCode에 맞는 방을 로드해줌
    /// </summary>
    /// <param name="roomCode"></param>
    public void LoadRoom(string roomCode)
    {
        Debug.Log("roomCode : " + roomCode);
        if (roomCode[0] == '0')
        {
            PhotonNetwork.LoadLevel("Meeting Room");
            Debug.Log($"{roomCode}방에 입장했습니다.");
            Debug.Log("현재 플레이어 수 " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        else if (roomCode[0] == '1')
        {
            PhotonNetwork.LoadLevel("library");
            Debug.Log($"{roomCode}방에 입장했습니다.");
            Debug.Log("현재 플레이어 수 " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    public void CreateCode()
    {   //선택된 roomOption에 따라서 roomcode생성해주는 버튼 //makecode가 여기에
        if (selectedRoomOption == "Meeting Room")
        {
            roomCode.text = "0" + MakeCode(6);
        }
        else if (selectedRoomOption == "Lecture Room")
        {
            roomCode.text = "1" + MakeCode(6);
        }
    }

    public string MakeCode(int Length)
    {
        const string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; //문자 생성 풀 
        char[] chRandom = new char[Length];
        for (int i = 0; i < Length; i++)
        {
            chRandom[i] = strPool[UnityEngine.Random.Range(0, strPool.Length)];
        }
        string code = new string(chRandom);
        return code;
    }


    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() 호출됨. connect완료.");
        SetScene("control");
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        SetScene("loading");
        isCreatingRoom = false;

        connectionInfoText.text = "offline";
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {
        //loadbalacing Client가 방에 들어오면 호출됨.
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (isCreatingRoom)
        {
            LoadRoom(roomCode.text);
        }
        else
        {
            LoadRoom(roomCodeInputField.text);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRoomFailed() was called by PUN. No room available. Please make Room.");
        SetScene("control");
    }

    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    roomList = m_roomList;
    //}
    #endregion

    #region Avatars

    public void setImage(int avatarListNum)
    {
        if (avatarListNum == 0)
        {
            image.GetComponent<Image>().sprite = avatar1;
        }
        else if (avatarListNum == 1)
        {
            image.GetComponent<Image>().sprite = avatar2;
        }
        else if (avatarListNum == 2)
        {
            image.GetComponent<Image>().sprite = avatar3;
        }
    }

    public void LeftButton()
    {
        //bTurnLeft = true;
        //bTurnRight = false;
        avatarListNum--;
        if (avatarListNum == -1)
        {
            avatarListNum = avatarList.Count - 1;
        }
        selectedAvatarPref = avatarList[avatarListNum];
        Debug.Log(selectedAvatarPref.name);
        Debug.Log("avatarListNum : " + avatarListNum);

        setImage(avatarListNum);
    }


    public void RightButton()
    {

        avatarListNum++;
        if (avatarListNum == avatarList.Count)
        {
            avatarListNum = 0;
        }
        selectedAvatarPref = avatarList[avatarListNum];
        Debug.Log(selectedAvatarPref.name);
        Debug.Log("avatarListNum : " + avatarListNum);


        setImage(avatarListNum);
    }


    #endregion
}