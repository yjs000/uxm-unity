using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine.UI;

public class VoiceUI : MonoBehaviour
{
    public Recorder recorder;
    public Toggle MuteToggle;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (GameManager.isSpawned)
        {
            PhotonVoiceView photonVoiceView = GameManager.player.GetComponent<PhotonVoiceView>();
            recorder = photonVoiceView.RecorderInUse;
            MuteToggle.isOn = !recorder.TransmitEnabled; // init
        }
    }

    public void Mute()
    {

        recorder.TransmitEnabled = MuteToggle.isOn;
    }
}
