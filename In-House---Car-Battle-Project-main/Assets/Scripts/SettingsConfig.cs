using UnityEngine;
using UnityEngine.UI;

public class SettingsConfig : MonoBehaviour
{
    public enum MusicButtonsStatus
    {
        M_On,
        M_Off
    }
    public enum SoundButtonsStatus
    {
        S_On,
        S_Off
    }
    public enum Controls
    {
        Touch,
        Buttons
    }
    public enum GraphicsSettings
    {
        G_Low,
        G_Medium,
        G_High
    }
    [SerializeField] MusicButtonsStatus M_status;
    [SerializeField] SoundButtonsStatus S_status;
    [SerializeField] Controls controls;
    [SerializeField] GraphicsSettings graphicSettings;

    [Header ("MUSIC BUTTONS INFO")]
    [SerializeField] int musicValue;
    [SerializeField] int soundValue;
    [SerializeField] int controlsValue;
    [SerializeField] int graphicInt;

    [Header ("Player pref")]
    [SerializeField] string musicPref;
    [SerializeField] string soundPref;
    [SerializeField] string controlsPref;
    [SerializeField] string graphicPref;

    [Header("Audio source")]
    [SerializeField] GameObject musicSource;
    [SerializeField] GameObject soundSource;

    [Header("Image")]
    [SerializeField] Image musicButtonImage;
    [SerializeField] Image soundButtonImage;
    [Space]
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;

    [Space]
    [Header("RATIO BUTTON INFO")]
    [SerializeField] Image touchRatioButtons;
    [SerializeField] Image buttonsRatioButtons;

    [Header("Controls radio buttons sprites")]
    [SerializeField] Sprite ratioOnSprite;
    [SerializeField] Sprite ratioOffSprite;

    // rules, 0 = touch controls; 1 = buttons controls
    

    [Header("Controls settings button")]
    // rules, 0 = Low, 1 = Medium, 2 = High
    
    [Space]
    [SerializeField] Image I_Low;
    [SerializeField] Image I_Medium;
    [SerializeField] Image I_High;
    [Space]
    [SerializeField] Sprite BG_Normal;
    [SerializeField] Sprite BG_Selected;



    private void Awake()
    {
        soundValue = PlayerPrefs.GetInt(soundPref, 0);
        musicValue = PlayerPrefs.GetInt(musicPref, 0);

        UpdateMusicUI();
        UpdateSoundUI();

        UpdateCotrolsInUI();
        UpdateGraphicUI();
    }

    // 0 = on; 1 = off
    #region Music and sound control
    public void B_MusicButtonFunction ()
    {
        musicValue = PlayerPrefs.GetInt(musicPref, 0);

        // turn the object off
        if (M_status == MusicButtonsStatus.M_On)
        {
            M_status = MusicButtonsStatus.M_Off;
            musicValue = 1;
            PlayerPrefs.SetInt(musicPref, musicValue);
            musicSource.SetActive(false);
            musicButtonImage.sprite = offSprite;

        }
        // turn the object on
        else if (M_status == MusicButtonsStatus.M_Off)
        {
            M_status = MusicButtonsStatus.M_On;
            musicValue = 0;
            PlayerPrefs.SetInt(musicPref, musicValue);
            musicSource.SetActive(true);
            musicButtonImage.sprite = onSprite;
        }
    }

    #region Update music button UI
    void UpdateMusicUI ()
    {
        // button is on
        if (musicValue == 0)
        {
            M_status = MusicButtonsStatus.M_On;
            musicSource.SetActive(true);
            musicButtonImage.sprite = onSprite;
        }
        // button is off 
        else if (musicValue == 1)
        {
            M_status = MusicButtonsStatus.M_Off;
            musicSource.SetActive(false);
            musicButtonImage.sprite = offSprite;
        }
    }

    void UpdateSoundUI()
    {
        // button is on
        if (soundValue == 0)
        {
            soundSource.SetActive(true);
            soundButtonImage.sprite = onSprite;

        }
        // button is off
        else if (soundValue == 1)
        {
            soundSource.SetActive(false);
            soundButtonImage.sprite = offSprite;
        }
    }
    #endregion

    public void B_SoundButtonFunction()
    {
        soundValue = PlayerPrefs.GetInt(soundPref, 0);

        // turn the object off
        if (S_status == SoundButtonsStatus.S_On)
        {
            S_status = SoundButtonsStatus.S_Off;
            soundValue = 1;
            PlayerPrefs.SetInt(soundPref, soundValue);
            soundSource.SetActive(false);
            soundButtonImage.sprite = offSprite;

        }
        // turn the object on
        else if (S_status == SoundButtonsStatus.S_Off)
        {
            S_status = SoundButtonsStatus.S_On;
            soundValue = 0;
            PlayerPrefs.SetInt(soundPref, soundValue);
            soundSource.SetActive(true);
            soundButtonImage.sprite = onSprite;
        }
    }
    #endregion

    #region Controls settings
    
    

    public void B_ChangeToButtonsControl()
    {
        PlayerPrefs.SetInt(controlsPref, 1);
        controls = Controls.Buttons;
        buttonsRatioButtons.sprite = ratioOnSprite;
        touchRatioButtons.sprite = ratioOffSprite;
    }

    public void B_ChangeToTouchControl ()
    {
        PlayerPrefs.SetInt(controlsPref, 0);
        controls = Controls.Touch;
        touchRatioButtons.sprite = ratioOnSprite;
        buttonsRatioButtons.sprite = ratioOffSprite;
    }

    #region Upate interface on start
    void UpdateCotrolsInUI ()
    {
        controlsValue = PlayerPrefs.GetInt(controlsPref, 0);
        // touch controls 
        if (controlsValue == 0)
        {
            controls = Controls.Touch;
            touchRatioButtons.sprite = ratioOnSprite;
            buttonsRatioButtons.sprite = ratioOffSprite;
        }
        else if (controlsValue == 1)
        {
            controls = Controls.Buttons;
            buttonsRatioButtons.sprite = ratioOnSprite;
            touchRatioButtons.sprite = ratioOffSprite;
        }
        #endregion
    }

    #endregion

    #region Graphic settings

    void SetAllImageNormal ()
    {
        I_Low.sprite = BG_Normal;
        I_Medium.sprite = BG_Normal;
        I_High.sprite = BG_Normal;
    }

    // change to low setting
    public void ChangeToLowSettings ()
    {
        SetAllImageNormal();
        I_Low.sprite = BG_Selected;
        PlayerPrefs.SetInt(graphicPref, 0);
    }

    public void ChangeToMediumSettings()
    {
        SetAllImageNormal();
        I_Medium.sprite = BG_Selected;
        PlayerPrefs.SetInt(graphicPref, 1);
    }

    public void ChangeToHighSettigs ()
    {
        SetAllImageNormal();
        I_High.sprite = BG_Selected;
        PlayerPrefs.SetInt(graphicPref, 2);
    }

    private void UpdateGraphicUI()
    {
        graphicInt = PlayerPrefs.GetInt(graphicPref, 1);

        // graphics settings at start
        if (graphicInt == 0) ChangeToLowSettings();
        else if (graphicInt == 1) ChangeToMediumSettings();
        else if (graphicInt == 2) ChangeToHighSettigs();
    }

    #endregion
}
