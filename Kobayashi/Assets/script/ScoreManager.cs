using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("獲得スコア")]
    public float Score;            // 現在のトータルスコア
    
    public int PaperCount;         // 処理した書類の総数

    public int HankoCount;         // 押したハンコの総数
    

    [Header("コンボ関係")]
    public float Conbo;            // スコアにかかるコンボ倍率（1.0〜2.0倍など）
    public float MaxConbo;      //最大コンボ数

    public int ConboCount;         // 現在の連続コンボ数

    public float ConboTimer;       // コンボが持続する残り時間

    public int MaxConboTime = 4;
    
    [Header("ミス関係")]
    public int MissCount;          // ミスした回数
    public bool PaperMiss;         // 現在の書類でミスが発生したかどうかのフラグ

    public MouseSistem MS;         // マウス操作を管理するスクリプトへの参照
    public GameManager GM;

    [Header("オブジェクト関係")] 
    public GameObject GetScoreUI;
    public GameObject GetScoreUISpawn;

    [Header("連打関係")]
    public int RendaCount;
    public GameObject RendaUI;
    public TextMeshProUGUI RendaTextUI;
    
    // ーーー ここから関数（メソッド） ーーー
    
    // 正しくハンコを押した時に呼ばれるコンボ上昇処理
    public void ConboSistem()
    {
        GM.ConboUI.SetActive(true); // コンボUIを表示

        ConboCount++;    // コンボ数をカウントアップ
        if(MaxConbo < ConboCount)
        {
            MaxConbo = ConboCount;
        }
        ConboTimer = MaxConboTime;  // コンボ受付時間を5秒にリセット

        // コンボ数が1桁の場合は「09」のように脳内補正してテキスト更新
        if(ConboCount < 10)
        {
            GM.ConboCountUI.text = ("0" + ConboCount);
        }
        else
        {
            GM.ConboCountUI.text = ("" + ConboCount);
        }

        // 3コンボごとに倍率の段階（ScoreUp）を計算
        var ScoreUp = ConboCount / 3;

        // 15コンボ以下の場合、コンボ数に応じて倍率を上げる（最大5段階、1.0f + 0.2f*5 = 2.0倍）
        if(ConboCount <= 15)
        {
            for (int i = 1; i < 6; i++)
            {
                if (ScoreUp >= i && ScoreUp < i + 1)
                {
                    Conbo = 1.0f + 0.2f * i; // 倍率を設定（1.2倍、1.4倍...）
                    GM.ConboScoreUI.text = ("×" + Conbo); // UIに倍率を表示
                    if(Conbo >= 2)
                        GM.ConboScoreUI.text = ("×" + Conbo + ".0"); // UIに倍率を表示
                    break;
                }
            }
        }
    }
    
    // コンボが途切れた時、またはミスした時に呼ばれる処理
    public void ConboEnd(bool Miss)
    {
        GM.ConboUI.SetActive (false); // コンボUIを非表示にする

        // コンボ情報を初期値にリセット
        ConboTimer = 0.0f;
        ConboCount = 0;
        Conbo = 1.0f;
        GM.ConboScoreUI.text = ("×" + Conbo+ ".0");

        // ミスによる終了だった場合のペナルティ処理
        if (Miss)
        { 
            MissCount++;        // 総ミス回数を加算
            PaperMiss = true;   // この書類でミスしたというフラグを立てる
            GM.BonusPaper = 0;
        }
    }
    
    // ハンコが成功した時にスコアを加算する処理
    public void ScoreUp(float Getscore) 
    {
        // 基礎点にコンボ倍率をかけたものをスコアに加算
        Score += Getscore * Conbo;
        HankoCount++;       // 総ハンコ数を加算
        GM.NokoriHankoCount--; // この書類の残り必要ハンコ数を減らす
        Score = (int)Score;
        GM.ScoreUI.text = ("Score : " + Score + "pt"); // スコアUIを更新
        var SanpleScore = Getscore * Conbo;
        SanpleScore = (int)SanpleScore;
        GetScoreUIUp(SanpleScore,new Color(0.91f, 0.86f, 0.18f));

        // この書類に押すべきハンコがなくなったら次の書類へチェンジ
        if (GM.NokoriHankoCount <= 0)
        {
            GM.PaperChange();
        }
    }

    public void GetScoreUIUp(float Score, Color color)
    {
        GameObject Get = Instantiate(GetScoreUI, GetScoreUISpawn.transform);
        Get.transform.localPosition = new Vector3(0, 0, 0);
        TextMeshProUGUI TMP = Get.GetComponentInChildren<TextMeshProUGUI>();
        TMP.color = color;
        if(Score >= 0)
            TMP.text = $"+{Score}pt";
        else
            TMP.text = $"{Score}pt";
    }

    public void RendaCheak()
    {
        RendaCount++;
        RendaUI.SetActive(true);
        RendaTextUI.gameObject.SetActive(false);
        RendaTextUI.text = $"{RendaCount}";
        RendaTextUI.gameObject.SetActive(true);
        GM.BonusTimer += 0.5f;
        GM.BonusTimeUP();
        ConboTimer = MaxConboTime;
    }

    
    void Update()
    {
        // コンボタイマー作動中の場合
        if(ConboTimer > 0 && GM.ActiveEnd != 2)
        {
            ConboTimer -= Time.deltaTime; // 時間を減らす

            var wariai = ConboTimer / MaxConboTime;  // 4秒を基準とした残り時間の割合（0.0〜1.0）を計算

            GM.ConboTimerUI.fillAmount = wariai; // UIのゲージ量を更新
        }
        
        // コンボタイマーが0以下（タイムアップ）になったらコンボ終了
        if(ConboTimer < 0)
        {
            ConboEnd(false); // ミスではないのでfalseを渡す
        }
    }
}
