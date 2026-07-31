using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TotalManager : MonoBehaviour
{
    //AIにコメントを付けてもらいました。

    [Header("スクリプト参照")]
    public ScoreManager SM; // スコア計算系（プレイ中の結果データ）
    public GameManager GM;  // ゲーム進行や演出を管理する

    [Header("スコア関連データ")]
    public float[] ScoreT; // 各スコア要素（0:基本スコア, 1:最大コンボ, 2:枚数）
    public float TotalScore; // 最終的に算出される合計スコア

    public int[] SyousaiCount = new int[6];
    // 詳細表示用のカウント
    // 0:ハンコ数 / 1:Perfect / 2:Great / 3:Good / 4:Bad / 5:Miss

    public TextMeshProUGUI[] SyousaiCountUI = new TextMeshProUGUI[6];
    // 上記カウントを表示するUI

    public int[] HyoukaCount = new int[7];
    // 評価ランクのしきい値（スコア条件）

    public Sprite[] HyoukaImage = new Sprite[7];
    // 評価ごとの画像

    public bool ActiveSyousai = false;
    // 詳細UIが開いているかどうか

    [Header("UI関連")]
    public TextMeshProUGUI[] ScoreHyoukiUI; // スコア・コンボ・枚数の表示テキスト
    public GameObject[] ScoreUPUI;          // 「+○○」みたいなボーナス演出
    public TextMeshProUGUI TotalScoreDummyUI; // スロット演出用のダミー数字
    public TextMeshProUGUI TotalScoreUI;      // 最終確定スコア表示

    public GameObject AnatahaUI;  // 「あなたは〜」表示
    public GameObject HyoukaUI;   // 評価表示枠
    public Image HyoukaImageUI;   // 評価画像表示

    public GameObject HankoUI;    // ハンコ演出
    public GameObject BackButton; // 戻る
    public GameObject RetryButton; // リトライ
    public GameObject SyousaiButton; // 詳細ボタン
    public GameObject SyousaiUI;    // 詳細パネル

    /// <summary>
    /// プレイ結果を取得してトータルスコアを計算＆ハイスコア保存
    /// </summary>
    void GetScoreResult()
    {
        // --- スコア取得 ---
        ScoreT[0] = SM.Score;       // 基本スコア
        ScoreT[1] = SM.MaxConbo;    // 最大コンボ
        ScoreT[2] = SM.PaperCount;  // 枚数

        // --- 詳細カウント取得 ---
        SyousaiCount[0] = SM.HankoCount;
        SyousaiCount[5] = SM.MissCount;
        SyousaiCount[1] = GM.MS.PeCount;
        SyousaiCount[2] = GM.MS.GrCount;
        SyousaiCount[3] = GM.MS.GoCount;
        SyousaiCount[4] = GM.MS.BaCount;

        // --- 合計スコア計算 ---
        // 基本 + コンボボーナス + 枚数ボーナス
        var MathTotal = ScoreT[0] + (ScoreT[1] * 500f) + (ScoreT[2] * 1000f);
        TotalScore = MathTotal;

        // --- ハイスコア取得 ---
        var High = PlayerPrefs.GetFloat("TotalScoreR", 0);

        // --- ハイスコア更新 ---
        if (High < TotalScore)
        {
            PlayerPrefs.SetFloat("TotalScoreR", TotalScore);
            PlayerPrefs.SetFloat("ScoreR", ScoreT[0]);
            PlayerPrefs.SetFloat("ConboR", ScoreT[1]);
            PlayerPrefs.SetFloat("PaperR", ScoreT[2]);

            // 詳細データも保存
            PlayerPrefs.SetFloat("HankoR", SyousaiCount[0]);
            PlayerPrefs.SetFloat("PerfectR", SyousaiCount[1]);
            PlayerPrefs.SetFloat("GreatR", SyousaiCount[2]);
            PlayerPrefs.SetFloat("GoodR", SyousaiCount[3]);
            PlayerPrefs.SetFloat("BadR", SyousaiCount[4]);
            PlayerPrefs.SetFloat("MissR", SyousaiCount[5]);

            PlayerPrefs.Save(); // 即時保存
        }
    }

    public IEnumerator SistemFadeout()
    {
        var TimeC = GM.TimerUI.color;
        var ScoreC = GM.ScoreUI.color;
        var HankoC = GM.MS.HankoCol.color; 
        Color TimeCP = TimeC;
        Color ScoreCP = ScoreC;
        Color HankoCP = HankoC;
        var BGMC =SoundManager.SoundM.BGM.GetComponent<AudioSource>();
        var Vol = BGMC.volume;
        var VolC = Vol / 1000;
        
        SM.RendaUI.SetActive(false);
        Debug.Log("sistem実行");

        for (int i = 0;i< 1000;i++)
        {
            TimeCP.a -= 0.001f;
            ScoreCP.a -= 0.001f;
            HankoCP.a -= 0.001f;
            Vol -= VolC;
            GM.TimerUI.color = TimeCP;
            GM.ScoreUI.color = ScoreCP;
            GM.MS.HankoCol.color = HankoCP;
            BGMC.volume = Vol;
            yield return null;
        }
    }


    /// <summary>
    /// リザルト画面の演出フロー（メイン）
    /// </summary>
    public IEnumerator TotalScoreOpen()
    {
        GetScoreResult(); // スコア確定
        StartCoroutine(SistemFadeout());
        for (int i = 0; i < 200; i++)
            yield return null;
        GM.EndUI.SetActive(true);

        // --- 最初の待機 ---
        for (int i = 0; i < 1000; i++)
            yield return null;

        // --- カメラや演出開始 ---
        GM.ResultMove();
        yield return null;
        GM.StartCoroutine(GM.PaperMove());
        var BGMC = SoundManager.SoundM.BGM.GetComponent<AudioSource>();
        SoundManager.SoundM.BGM.Stop();
        BGMC.volume = 0.3f;
        SoundManager.SoundM.GameEndsound();

        // --- 演出待機 ---
        for (int i = 0; i < 500; i++)
            yield return null;

        // --- 各スコア項目を順番表示 ---
        for (int i = 0; i < 3; i++)
        {
            // 表示形式を分ける（スコアだけpt付き）
            if (i == 0)
                ScoreHyoukiUI[i].text = $"{ScoreT[i]:F0}pt";
            else
                ScoreHyoukiUI[i].text = $"{ScoreT[i]:F0}";

            ScoreHyoukiUI[i].gameObject.SetActive(true);
            SoundManager.SoundM.ScoreOpensound();

            for (int j = 0; j < 500; j++)
                yield return null;
        }

        // --- ボーナス演出 ---
        for (int i = 0; i < 2; i++)
        {
            ScoreUPUI[i].gameObject.SetActive(true);
            SoundManager.SoundM.ScoreUPsound();

            for (int j = 0; j < 200; j++)
                yield return null;
        }

        for (int j = 0; j < 300; j++)
            yield return null;

        // --- 合計スコア演出へ ---
        StartCoroutine(TotalAnim());
    }

    /// <summary>
    /// トータルスコアのスロット演出＆評価表示
    /// </summary>
    IEnumerator TotalAnim()
    {
        int[] R = new int[6]; // ダミー数字用
        TotalScoreDummyUI.gameObject.SetActive(true);

        // 少し溜める
        for (int i = 0; i < 30; i++)
            yield return null;

        SoundManager.SoundM.TotalWaitsound();

        // --- スロット演出（ランダム数字） ---
        for (int i = 0; i < 300; i++)
        {
            TotalScoreDummyUI.text = "";

            for (int j = 0; j < 6; j++)
            {
                R[j] = Random.Range(0, 9);
                TotalScoreDummyUI.text += $"{R[j]}";
            }

            TotalScoreDummyUI.text += $"pt";
            yield return null;
        }

        // --- 本当のスコア表示 ---
        TotalScoreDummyUI.gameObject.SetActive(false);
        TotalScoreUI.text = $"{TotalScore:F0}pt";
        TotalScoreUI.gameObject.SetActive(true);
        SoundManager.SoundM.TotalOpensound();

        // --- 評価決定 ---
        // 高いランクから順にチェックして一致したものをセット
        for (int i = 6; i >= 0; i--)
        {
            if (TotalScore >= HyoukaCount[i])
            {
                HyoukaImageUI.sprite = HyoukaImage[i];
            }
        }

        // --- UI演出の順番表示 ---
        for (int i = 0; i < 600; i++) yield return null;
        AnatahaUI.SetActive(true);

        for (int i = 0; i < 600; i++) yield return null;
        HyoukaUI.SetActive(true);

        for (int i = 0; i < 1000; i++) yield return null;
        HankoUI.SetActive(true);

        for (int i = 0; i < 200; i++) yield return null;

        // --- 詳細UIの数値反映 ---
        for (int i = 0; i < 6; i++)
        {
            SyousaiCountUI[i].text = $"{SyousaiCount[i]}";
        }

        // --- ボタン解放 ---
        RetryButton.SetActive(true);
        BackButton.SetActive(true);
        GM.AntiStanpUI.SetActive(false);
        SyousaiButton.SetActive(true);
        var a =GM.MS.HankoCol.color;
        a.a = 1;
        GM.MS.HankoCol.color = a;
    }

    /// <summary>
    /// 詳細ボタン：ホバー時の拡大
    /// </summary>
    public void SyousaiIn(int num)
    {
        if (num == 0)
        {
            SyousaiButton.transform.localScale = new Vector3(0.2852955f, 0.2852955f, 0.2852955f);
        }
        else if (num == 1)
        {
            RetryButton.transform.localScale = new Vector3(2.790183f, 2.790183f, 2.790183f);
        }
        else if (num == 2)
        {
            BackButton.transform.localScale = new Vector3(2.790183f, 2.130363f, 2.790183f);
        }
        
        SoundManager.SoundM.Kasolsound();
    }

    /// <summary>
    /// 詳細ボタン：ホバー解除で元サイズ
    /// </summary>
    public void SyousaiOut(int Num)
    {
        if (Num == 0)
        {
            SyousaiButton.transform.localScale = new Vector3(0.2270918f, 0.2270918f, 0.2270918f);
            
        }
        else if (Num == 1)
        {
            RetryButton.transform.localScale = new Vector3(2.407197f, 2.347018f, 2.347018f);
        }
        else if (Num == 2)
        {
            BackButton.transform.localScale = new Vector3(2.407197f, 1.777603f, 2.347018f);
        }
    }
       

    /// <summary>
    /// 詳細パネルの開閉トグル
    /// </summary>
    public void SyousaiOpen()
    {
        if (!ActiveSyousai)
        {
            SyousaiUI.SetActive(true);
            SoundManager.SoundM.Syousaisound();
            ActiveSyousai = true;
        }
        else
        {
            SyousaiUI.SetActive(false);
            SoundManager.SoundM.Cancelsound();
            ActiveSyousai = false;
        }
    }

    public void ButtonSetumei(int Num)
    {
        if (Num == 0)
        {
            
        }
    }

    public void ButtonScene(int Num)
    {
      StartCoroutine(ChangeScene(Num));
    }

    public IEnumerator ChangeScene(int Num)
    {
        if(Num == 0)
        SoundManager.SoundM.Changesound();
        else if (Num == 1)
            SoundManager.SoundM.TotalOpensound();
        GM.FeedInUI.SetActive(true);
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

        }
    }
}