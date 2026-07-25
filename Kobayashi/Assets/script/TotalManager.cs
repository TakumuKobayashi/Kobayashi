using System.Collections;
using TMPro;
using UnityEngine;

public class TotalManager : MonoBehaviour
{
    [Header("スクリプト関係")]
    public ScoreManager SM;
    public GameManager GM;
    [Header("必要な数値")]
    public float[] ScoreT;
  

    public float TotalScore;
    [Header("オブジェクト関連")]
    public TextMeshProUGUI[] ScoreHyoukiUI;
    public GameObject[] ScoreUPUI;
    public TextMeshProUGUI TotalScoreDummyUI;
    public TextMeshProUGUI TotalScoreUI;
    public GameObject AnatahaUI;
    public TextMeshProUGUI HyoukaUI;
    public GameObject HankoUI;

    void GetScoreResult()
    {
        ScoreT[0] = SM.Score;
        ScoreT[1] = SM.MaxConbo;
        ScoreT[2] = SM.PaperCount;

        var MathTotal = ScoreT[0] + (ScoreT[1] * 500f) + (ScoreT[2] * 1000f);
        TotalScore = MathTotal;
    }


    public IEnumerator TotalScoreOpen()
    {
        GetScoreResult();
        for (int i = 0; i < 2000; i++)
        {
            yield return null;
        }

        GM.StartCoroutine(GM.PaperMove());

        for (int i = 0; i < 1000; i++)
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

            for(int j = 0; j < 600; j++)
            {
            yield return null;
            }
        }

        for (int j = 0; j < 600; j++)
        {
            yield return null;
        }
        for (int i = 0; i < 2; i++)
        {
            ScoreUPUI[i].gameObject.SetActive(true);
            SoundManager.SoundM.ScoreUPsound();

            for (int j = 0; j < 400; j++)
            {
                yield return null;
            }
        }

    }
}
