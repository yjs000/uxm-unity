using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UMA.PoseTools;

public class Mood : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;
    private ExpressionPlayer expression;
    private bool connected;

    public enum Moods { Basic, Happy, Sad, Angry }
    public Moods mood = Moods.Basic;
    private Moods lastMood = Moods.Basic;

    private void OnEnable()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
        avatar.CharacterCreated.AddListener(OnCreated);
    }
    private void OnDisable()
    {
        avatar.CharacterCreated.RemoveListener(OnCreated);
    }
    public void OnCreated(UMAData data)
    {
        expression = GetComponent<ExpressionPlayer>();
        expression.enableBlinking = true;
        expression.enableSaccades = true;
        connected = true;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (connected && lastMood != mood)
        {
            lastMood = mood;
            switch (mood)
            {
                case Moods.Basic:
                    expression.leftMouthSmile_Frown = 0f;
                    expression.rightMouthSmile_Frown = 0f;
                    expression.leftEyeOpen_Close = 0f;
                    expression.rightEyeOpen_Close = 0f;
                    expression.midBrowUp_Down = 0f;
                    break;
                case Moods.Happy:
                    expression.leftMouthSmile_Frown = 1.8f;
                    expression.rightMouthSmile_Frown = 1.8f;
                    expression.leftEyeOpen_Close = -1.0f;
                    expression.rightEyeOpen_Close = -1.0f;
                    expression.midBrowUp_Down = 1.0f;
                    break;
                case Moods.Sad:
                    expression.leftMouthSmile_Frown = -0.8f;
                    expression.rightMouthSmile_Frown = -0.8f;
                    expression.leftEyeOpen_Close = 0.4f;
                    expression.rightEyeOpen_Close = 0.4f;
                    expression.leftBrowUp_Down = -0.5f;
                    expression.rightBrowUp_Down = -0.5f;
                    expression.midBrowUp_Down = 0.5f;
                    break;
                    case Moods.Angry:
                    expression.leftMouthSmile_Frown = -0.8f;
                    expression.rightMouthSmile_Frown = -0.8f;
                    expression.leftEyeOpen_Close = 0.4f;
                    expression.rightEyeOpen_Close = 0.4f;
                    expression.leftBrowUp_Down = 0.5f;
                    expression.rightBrowUp_Down = 0.5f;
                    expression.midBrowUp_Down = -0.5f;
                    break;
                default:
                    break;
            }
        }
    }
}
