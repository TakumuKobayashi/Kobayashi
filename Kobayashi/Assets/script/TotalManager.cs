using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TotalManager : MonoBehaviour
{
    [Header("スクリプト関係")]
    public ScoreManager SM;
    public GameManager GM;
    [Header("必要な数値")]
    public float[] ScoreT;
    public float TotalScore;
    public int[] SyousaiCount = new int[6];
    public TextMeshProUGUI[] SyousaiCountUI = new TextMeshProUGUI[6];
    [Header("オブジェクト関連")]
    public TextMeshProUGUI[] ScoreHyoukiUI;
    public GameObject[] ScoreUPUI;
    public TextMeshProUGUI TotalScoreDummyUI;
    public TextMeshProUGUI TotalScoreUI;
    public GameObject AnatahaUI;
    public GameObject HyoukaUI;
    public GameObject HankoUI;
    public GameObject BackButton;
    public GameObject RetryButton;
    public GameObject SyousaiButton;
    
    void GetScoreResult()
    {
        ScoreT[0] = SM.Score;
        ScoreT[1] = SM.MaxConbo;
        ScoreT[2] = SM.PaperCount;
        
        SyousaiCount[0] = SM.HankoCount;
        SyousaiCount[0] = SM.MissCount;
        SyousaiCount[1] = GM.MS.PeCount;
        SyousaiCount[2] = GM.MS.GrCount;
        SyousaiCount[3] = GM.MS.GoCount;
        SyousaiCount[4] = GM.MS.BaCount;

        var MathTotal = ScoreT[0] + (ScoreT[1] * 500f) + (ScoreT[2] * 1000f);
        TotalScore = MathTotal;
    }


    public IEnumerator TotalScoreOpen()
    {
        GetScoreResult();
        for (int i = 0; i < 1000; i++)
        {
            yield return null;
        }

        GM.ResultMove();
        yield return null;
        GM.StartCoroutine(GM.PaperMove());

        for (int i = 0; i < 500; i++)
        {
            yield return null;
        }

        for (int i = 0;i <3;i++)
        {
            if(i == 0)
            ScoreHyoukiUI[i].text = $"{ScoreT[i]:F0}pt";
            else
                ScoreHyoukiUI[i].text = $"{ScoreT[i]:F0}";
            ScoreHyoukiUI[i].gameObject.SetActive(true);
            SoundManager.SoundM.ScoreOpensound();

            for(int j = 0; j < 500; j++)
            {
            yield return null;
            }
        }

          for (int i = 0; i < 2; i++)
        {
            ScoreUPUI[i].gameObject.SetActive(true);
            SoundManager.SoundM.ScoreUPsound();

            for (int j = 0; j < 200; j++)
            {
                yield return null;
            }
        }
          
        for(int j = 0; j < 300; j++)
        {
            yield return null;
        }

        StartCoroutine(TotalAnim());

    }

    IEnumerator TotalAnim()
    {
       int[] R = new int[6];
       TotalScoreDummyUI.gameObject.SetActive(true);
       for (int i = 0; i < 30; i++)
       {
           yield return null;
       }

       SoundManager.SoundM.TotalWaitsound();
       
       for (int i = 0; i < 300; i++)
       {
           TotalScoreDummyUI.text = $"";
           for (int j = 0; j < 6; j++)
           {
               R[j] = Random.Range(0, 9);
               TotalScoreDummyUI.text += $"{R[j]}";
           }
           TotalScoreDummyUI.text += $"pt";
           
           yield return null;
       }
       
       TotalScoreDummyUI.gameObject.SetActive(false);
       TotalScoreUI.text = $"{TotalScore:F0}pt";
       TotalScoreUI.gameObject.SetActive(true);
       SoundManager.SoundM.TotalOpensound();
       for (int i = 0; i < 600; i++)
       {
           yield return null;
       }
       AnatahaUI.SetActive(true);
       for (int i = 0; i < 600; i++)
       {
           yield return null;
       }
       HyoukaUI.SetActive(true);
       
       for (int i = 0; i < 1000; i++)
       {
           yield return null;
       }
       HankoUI.SetActive(true);
       for (int i = 0; i < 200; i++)
       {
           yield return null;
       }
       RetryButton.SetActive(true);
       BackButton.SetActive(true);
       GM.AntiStanpUI.SetActive(false);
       SyousaiButton.SetActive(true);
     
    }
}
