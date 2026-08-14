using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Startmanager : MonoBehaviour
{
    public GameObject[] buttonUI = new GameObject[4];
    public Vector3[] InScaleUI = new Vector3[4];
    public Vector3[] OutScaleUI = new Vector3[4];
    public GameObject BlackIn;

    public GameObject InfoUI;
    public GameObject[] InfoPaper;
    public int InfoPaperCount = 0;

    public StartMouseSistem SMS;
    public GameObject CheckUI;
    public GameObject SettingUI;
    public GameObject[] TypeCheckUI;

    public Image ScoreButtonUI;
    public TextMeshProUGUI[] ScoreTextUI;
    public int[] HyoukaCount;
    public Sprite[] HyoukaImage;
    public Image HyoukaImageUI;

    public bool ActiveEsc;
    public bool ActiveInfo;
    public bool ActiveSetting;
    public bool ActiveEndCheck;
    public bool ActiveScoreOn;


    //主にボタンの中に入る関数たち

    public void ButtonIn(int Num)
    {
        buttonUI[Num].transform.localScale = InScaleUI[Num];
        SoundManager.SoundM.Kasolsound();
    }
    public void ButtonOut(int Num)
    {
        buttonUI[Num].transform.localScale = OutScaleUI[Num];
    }

    public void PushButton(int Num)
    {
        StartCoroutine(ChangeScene(Num));
    }

    public IEnumerator ChangeScene(int Num)
    {
        ActiveEsc = false;
        BlackIn.SetActive(true);
        Debug.Log("1");
        if (Num == 0)
        {
            SoundManager.SoundM.Changesound();
           
        }
        else if (Num == 1)
        {
            SoundManager.SoundM.TotalOpensound();
            Debug.Log("2");
        }
          
        for (int i = 0; i < 300; i++)
        {
            yield return null;
        }

        if (Num == 0)
        {
            SceneManager.LoadScene(0);
        }
        else if (Num == 1)
        {
            Debug.Log("3");
            SceneManager.LoadScene(1);
        }
    }

    public void OpenInfo()
    {
        InfoUI.SetActive(true);
        InfoPaperCount = 0;
        InfoPaper[0].SetActive(true);
        SoundManager.SoundM.PauseInsound();
        ActiveInfo = true;
    }

    public void InfoButton(int Num)
    {
        SMS.StartCoroutine(SMS.HandMove());
        if (Num == 0)
        {
            InfoUI.SetActive(false);
            SoundManager.SoundM.PauseOutsound();
            ActiveInfo = false;
        }
        else if (Num == 1)
        {
            InfoPaper[InfoPaperCount].SetActive(false);
            InfoPaperCount++;
            InfoPaper[InfoPaperCount].SetActive(true);
            SoundManager.SoundM.Hankosound();
        }
        else if (Num == 2)
        {
            InfoPaper[InfoPaperCount].SetActive(false);
            InfoPaperCount--;
            InfoPaper[InfoPaperCount].SetActive(true);
            SoundManager.SoundM.Hankosound();
        }
    }

    public void CheckUIOpen(int Num)
    {
        CheckUI.SetActive(true);
        TypeCheckUI[Num].SetActive(true);
        SoundManager.SoundM.Warningsound();
        ActiveEndCheck = true;
        ActiveEsc = false;
    }
    public void CheckUIBuck()
    {
        CheckUI.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            TypeCheckUI[i].SetActive(false);
        }
        SoundManager.SoundM.Cancelsound();
        ActiveEndCheck = false;
        ActiveEsc = true;
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        TypeCheckUI[2].SetActive(true);
        SoundManager.SoundM.Hankosound();
    }

    public void QuitGame()
    {
        // ビルドしたゲームを終了
        Application.Quit();

        // Unityエディタ上で動かしている場合（デバッグ用）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SettingOpen()
    {
        SettingUI.SetActive(true);
        SoundManager.SoundM.Settingsound();
        SMS.Handpos.SetActive(false);
        ActiveSetting = true;
    }

    public void SettingOut()
    {
        SettingUI.SetActive(false);
        SoundManager.SoundM.Cancelsound();
        SMS.Handpos.SetActive(true);
        ActiveSetting = false;
    }

    private void Start()
    {
        var High = PlayerPrefs.GetFloat("TotalScoreR", 0);
        if (High > 0)
        {
            ScoreButtonUI.color = Color.white;
            ScoreTextUI[0].text = $"{PlayerPrefs.GetFloat("TotalScoreR")}";
            ScoreTextUI[1].text = $"{PlayerPrefs.GetFloat("ScoreR")}";
            ScoreTextUI[2].text = $"{PlayerPrefs.GetFloat("ConboR")}";
            ScoreTextUI[3].text = $"{PlayerPrefs.GetFloat("PaperR")}";
            // 詳細データも保存
            ScoreTextUI[4].text = $"{PlayerPrefs.GetFloat("HankoR")}";
            ScoreTextUI[5].text = $"{PlayerPrefs.GetFloat("PerfectR")}";
            ScoreTextUI[6].text = $"{PlayerPrefs.GetFloat("GreatR")}";
            ScoreTextUI[7].text = $"{PlayerPrefs.GetFloat("GoodR")}";
            ScoreTextUI[8].text = $"{PlayerPrefs.GetFloat("BadR")}";
            ScoreTextUI[9].text = $"{PlayerPrefs.GetFloat("MissR")}";

            for (int i = 6; i >= 0; i--)
            {
                //Debug.Log("トータル："+TotalScore+"基準 : "+HyoukaCount[i]);
                if (High >= HyoukaCount[i])
                {
                    HyoukaImageUI.sprite = HyoukaImage[i];
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if(ActiveEsc)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(ActiveInfo)
                {
                    InfoUI.SetActive(false);
                    SoundManager.SoundM.PauseOutsound();
                    ActiveInfo = false;
                }
                else if(ActiveSetting)
                {
                    SettingOut();
                }
                else if(ActiveEndCheck)
                {

                }
                else
                {
                    CheckUIOpen(0);
                }
            }
        }
    }
}
