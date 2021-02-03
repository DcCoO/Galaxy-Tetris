using System;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{

    public static ModeController instance;
    Mode mode
    {
        get => IntToMode(PlayerPrefs.GetInt("Mode", 0));
        set => PlayerPrefs.SetInt("Mode", ModeToInt(value));
    }
    
    [Header("Normal")] 
    public Text normalText;
    public Color normalColor;
    
    [Header("Hard")] 
    public Text hardText;
    public Color hardColor;

    [Header("Impossible")] 
    public Text impossibleText;
    public Color impossibleColor;

    [Header("Settings")] 
    public TimeSettings settingsNormal;
    public TimeSettings settingsHard;
    public TimeSettings settingsImpossible;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mode = IntToMode(PlayerPrefs.GetInt("Mode", 0));
        ConfigMode();
        TimeController.instance.settings = mode == Mode.NORMAL ? settingsNormal : mode == Mode.HARD ? settingsHard : settingsImpossible;
    }


    private static int ModeToInt(Mode mode)
    {
        return (int) mode;
    }

    private static Mode IntToMode(int value)
    {
        return (Mode) value;
    }

    public void ChooseMode(int modeValue)
    {
        mode = IntToMode(modeValue);
        ConfigMode();
    }
    
    private void ConfigMode()
    {
        if (mode == Mode.NORMAL)
        {
            normalText.color = normalColor;
            hardText.color = impossibleText.color = Color.white;
            TimeController.instance.settings = settingsNormal;
        }
        else if (mode == Mode.HARD)
        {
            hardText.color = hardColor;
            normalText.color = impossibleText.color = Color.white;
            TimeController.instance.settings = settingsHard;
        }
        else
        {
            impossibleText.color = impossibleColor;
            normalText.color = hardText.color = Color.white;
            TimeController.instance.settings = settingsImpossible;
        }
    }
    
    public int GetPointMultiplier()
    {
        if (mode == Mode.NORMAL) return 1;
        if (mode == Mode.HARD) return 2;
        return 3;
    }
}

[Serializable]
public enum Mode
{
    NORMAL = 0, HARD = 1, IMPOSSIBLE = 2
}
