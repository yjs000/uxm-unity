using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNameInputField : MonoBehaviour
{
    #region Private Constants


    // Store the PlayerPref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";

    #endregion

    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        //이름을 쓰면 PlayerPrefs에 저장해놓고 defaultName으로 설정해줌
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null) //inputField가 있으면
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }
    #endregion


    #region Public Methods


    /// <summary>
    /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
    /// </summary>
    /// <param name="value">The name of the Player</param>
    public void SetPlayerName(string value)
    {
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);
        Debug.Log(PlayerPrefs.GetString(playerNamePrefKey));
    }

    
    #endregion
}
