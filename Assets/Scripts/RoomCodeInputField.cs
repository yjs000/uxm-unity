using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCodeInputField : MonoBehaviour
{
    const string roomCodePrefKey = "RoomCode";

    public void Start()
    {
        //입력했던 roomCode를 저장해놓고 defaultCode로 입력해줌
        string defaultCode = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null) //inputField가 있으면
        {
            if (PlayerPrefs.HasKey(roomCodePrefKey))
            {
                defaultCode = PlayerPrefs.GetString(roomCodePrefKey);
                _inputField.text = defaultCode;
            }
        }
    }
    public void SetRoomCode(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Room Code is Null or Empty");
            return;
        }
        PlayerPrefs.SetString(roomCodePrefKey, value);
        Debug.Log(PlayerPrefs.GetString(roomCodePrefKey));
    }
}
