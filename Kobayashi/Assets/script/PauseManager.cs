using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    public Vector3 OutSize;
    public Vector3 InSize;

    public GameObject PauseUI;
    public GameObject[] buttonUI;
    public GameObject SettingUI;
    public TextMeshProUGUI SetumeiText;
    public string[] Setumei;

    public HanteiHyouki KasolPos;
    public static PauseManager PM;

    public bool ActiveESC;
    public bool ActivePause;
    public bool ActiveSetting;

    private void Awake()
    {
        PM = this;
    }

    public void InScale(int Num)
    {
        buttonUI[Num].transform.localScale = InSize;
        SoundManager.SoundM.Kasolsound();
        SetumeiText.text = $"{Setumei[Num]}";
    }

    public void OutScale(int Num)
    {
        buttonUI[Num].transform.localScale = OutSize;
        SetumeiText.text = $"";
    }

    public void SettingOpen()
    {
        SettingUI.gameObject.SetActive(true);
        SoundManager.SoundM.Settingsound();
        ActiveSetting = true;
    }

    public void PressESC()
    {
        if (ActiveESC)
        {
            if(ActiveSetting)
            {
                SettingUI.SetActive(false);
                ActiveSetting = false;
            }
            else
            {
                if (!ActivePause)
            {
                for (int i = 0; i < 4; i++)
                {
                    buttonUI[i].transform.localScale = OutSize;
                    SetumeiText.text = $"";
                }

                SoundManager.SoundM.PauseInsound();
                SoundManager.SoundM.BGM.Pause();
                if (KasolPos.HyoukiPast != null)
                {
                    KasolPos.HyoukiPast.SetActive(false);
                }
                PauseUI.SetActive(true);
                ActivePause = true;
                Time.timeScale = 0;
            }
            else
            {
                SoundManager.SoundM.PauseOutsound();
                SoundManager.SoundM.BGM.UnPause();

                if (KasolPos.HyoukiPast != null)
                {
                    KasolPos.HyoukiPast.SetActive(true);
                }

                PauseUI.SetActive(false);
                ActivePause = false;
                Time.timeScale = 1;
            }

            }

        }

     }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressESC();
        }
    }
}
