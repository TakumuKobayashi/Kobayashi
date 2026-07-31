using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//AIにコメントを書いてもらいました

// 書類ごとの個別情報を管理するクラス
[System.Serializable]
public class PaperInfo
{
    public GameObject Paper;       // 書類のプレハブ（見た目やオブジェクトの実体）
    public int HankoPoint;         // その書類に押さなければいけないハンコの数
}

public class GameManager : MonoBehaviour
{
    [Header("時間関係")]
    public float Timer;            // ゲームの制限時間など（現時点では未使用）
    public float BonusTimer;       // コンボ等で獲得したボーナス時間
    public bool TimerActive;
    public bool TimerRed;

   
    [Header("残り押さなければいけない数")]
    public int NokoriHankoCount;   // 現在の書類にあと何回ハンコを押す必要があるか
    public int NextHankoPoint;     // 次に出現する書類の必要ハンコ数
    
    [Header("ゲーム中に出現する書類情報")]
    public PaperInfo[] Syorui;     // ゲームに登場する書類の種類（インスペクターで設定）
    public GameObject BonusSyorui;
    public GameObject Resultpaper;

    [Header("ハンコを押す書類の判別")]
    public GameObject NextPaper;   // 次に出現予定の待機中書類
    public GameObject NowPaper;    // 現在画面中央にある処理中の書類
    public GameObject EndPaper;    // 処理が終わり、画面外へ退場中の書類
    public GameObject NowKakusi;   // 現在の書類の判定隠し用オブジェクト
    public GameObject NextKakusi;  // 次の書類の判定隠し用オブジェクト
    
    public GameObject HanteiKakusi; // 判定を隠すためのオブジェクトのプレハブ
    
    [Header("書類の出現地点")]
    public GameObject BackSpawn;   // 次の書類が裏で待機する場所の親オブジェクト
    public GameObject FrontSpawn;  // 現在の書類が配置される正面の親オブジェクト
    public GameObject EndSpawn;    // 終わった書類が移動する退場用の親オブジェクト

    [Header("ボーナスタイム関係")]
    public GameObject BonusUI;
    public TextMeshProUGUI BonusText;
    public int BonusPaper;
    public bool BonusActive;

    [Header("クリア関係")] public int ActiveEnd;
    
    [Header("UI関係")]
    public GameObject ConboUI;          // コンボ表示全体のUI親オブジェクト

    public TextMeshProUGUI ScoreUI;     // スコアを表示するテキスト

    public TextMeshProUGUI ConboCountUI;// コンボ数を表示するテキスト

    public TextMeshProUGUI ConboScoreUI;// コンボ倍率（×1.2など）を表示するテキスト

    public Image ConboTimerUI;          // コンボゲージの画像（Fill Amount用）

    public TextMeshProUGUI TimerUI;
    public GameObject AntiStanpUI;

    public GameObject StartUI;
    public GameObject EndUI;
    public GameObject FeedInUI;

    [Header("スクリプト関係")]
    public MouseSistem MS;         // マウス操作を管理するスクリプトへの参照
    public ScoreManager SM;
    public TotalManager TM;

    // ーーー ここから関数（メソッド） ーーー

    // ゲーム開始時に最初の書類を生成する処理
    void Awake()
    {
        // ターゲットフレームレートを350FPSに固定
       Application.targetFrameRate = 350;
        
        // （補足）V-Sync（垂直同期）を無効化しないとtargetFrameRateが効かない場合があります
        QualitySettings.vSyncCount = 0;
        
    }
    public IEnumerator FirstPaper()
    {
        // 登録されている書類情報（Syorui配列）からランダムに1つ選ぶ
        var syoruiNum = Random.Range(0, Syorui.Length);

        // 正面スプレース（FrontSpawn）の子として書類を生成
        GameObject spawnedPaper = Instantiate(Syorui[syoruiNum].Paper, FrontSpawn.transform);

        NowPaper = spawnedPaper; // 現在の書類として設定

        // 判定隠しオブジェクトを書類の子として生成
        NowKakusi = Instantiate(HanteiKakusi, spawnedPaper.transform);

        // その書類に必要なハンコ数をセット
        NokoriHankoCount = Syorui[syoruiNum].HankoPoint;
        
        NowPaper.SetActive(false);

        // マウスシステム側へ、現在の書類のTransformを伝える
        MS.NowPaper = NowPaper.transform;

        for (int i = 0; i < 200; i++)
        {
            yield return null;
        }

        TimerUI.gameObject.SetActive(true);

        for (int i = 0; i < 500; i++)
        {
            yield return null;
        }
        
        StartUI.SetActive(true);
        
        for (int i = 0; i < 500; i++)
        {
            yield return null;
        }
        NowPaper.SetActive(true);
        StartCoroutine(PaperMove());
        SoundManager.SoundM.GameBGMsound();
        TimerActive = true;

       // 次の書類を裏で先読み生成しておく
              PaperSpwan();
    }

    // 次の書類を裏（BackSpawn）で生成・待機させておく処理
    public void PaperSpwan()
    {
        // ランダムに書類を選ぶ
        var syoruiNum = Random.Range(0, Syorui.Length);
        GameObject spawnedPaper = null;
        
        if ((SM.PaperCount+1) % 10 == 0 && SM.PaperCount > 0)
        {
             spawnedPaper =  Instantiate(BonusSyorui,BackSpawn.transform);
            BonusPaper BP = spawnedPaper.GetComponent<BonusPaper>();
            BP.GM = MS.GM;
            NextHankoPoint = 777;
            SM.RendaCount = 0;
        }
        else
        {
            // 待機場所（BackSpawn）の子として書類を生成
            spawnedPaper = Instantiate(Syorui[syoruiNum].Paper, BackSpawn.transform);
            // 次の書類の必要ハンコ数をキープしておく
            NextHankoPoint = Syorui[syoruiNum].HankoPoint;
            SoundManager.SoundM.BlueActivesound();

        }
        

        NextPaper = spawnedPaper; // 次の書類として設定
        
        // 次の書類用の判定隠しオブジェクトを生成
        NextKakusi = Instantiate(HanteiKakusi, spawnedPaper.transform);
        
        // まだ出番ではないので非アクティブ（非表示）にしておく
        NextPaper.SetActive(false);
    }

    public void BonusTimeUP()
    {
        BonusUI.SetActive(false);
        BonusText.text = $"+ {BonusTimer:F1}s";
        SoundManager.SoundM.Hakusyusound();
        BonusUI.SetActive(true);
    }

    // 書類のノルマ達成時、古い書類を片付けて次の書類を前面に出す処理
    public void PaperChange()
    {
        BonusActive = false;
        // 今まで使っていた書類の親を退場場所（EndSpawn）に変更
        NowPaper.transform.SetParent(EndSpawn.transform);

        NowKakusi.SetActive(true); // 隠しオブジェクトを有効化

        // 役目を終えた書類をEndPaperとしてキープ
        EndPaper = NowPaper;

        // この書類で一度もミスをしていなければ、コンボ数に応じてボーナス時間を獲得
        if(!SM.PaperMiss)
        {
            BonusPaper++;
            
            var s = BonusPaper/ 2;
            //s = (int)s;
            
                    switch (s)//現在のミスしなかった書類の数に合わせてボーナスタイムを獲得する
                    {
                        case 0:
                        {
                            break;
                        }
                        case 1:
                        {
                            BonusTimer += 0.2f;
                            break;
                        }
                        case 2:
                        {
                            BonusTimer += 0.4f;
                            break;
                        }
                        case 3:
                        {
                            BonusTimer += 0.6f;
                            break;
                        }
                        case 4:
                        {
                            BonusTimer += 0.8f;
                            break;
                        }
                        default:
                        {
                            BonusTimer += 1.2f;
                            break;
                        }
                    }

                    if (s >= 1)
                    {
                BonusTimeUP();
                        
                    }
                 
        }
        else
        {
            // ミスがあった場合はフラグを元に戻す
            SM.PaperMiss = false;
        }
        
        // ーーー ここから次の書類を現在の書類に昇格させる処理 ーーー
        NextPaper.transform.SetParent(FrontSpawn.transform); // 正面へ移動
        NowKakusi = NextKakusi;  // 隠しオブジェクトの参照を現在用に更新
        NowPaper = NextPaper;    // 次の書類を「現在の書類」に設定
        NowPaper.SetActive(true);// 画面に表示する
        NokoriHankoCount = NextHankoPoint; // 残りハンコ数を次の書類のものに更新
        MS.NowPaper = NowPaper.transform;  // マウスシステムへ新しい書類を通知

        SM.PaperCount++; // 処理した書類の数をカウントアップ
        if ((SM.PaperCount) % 10 == 0 && SM.PaperCount > 0)
        {
            BonusActive = true;
        }
        // さらにその次の書類を裏で新しく生成
        PaperSpwan();

        // 終わった書類（EndPaper）を下にスライドさせて消去するアニメーション（コルーチン）を開始
        StartCoroutine(PaperMove());
    }

    // 終わった書類を下にスクロールさせて画面外で削除するコルーチン
    public IEnumerator PaperMove()
    {
        SoundManager.SoundM.Perasound();

        // 200フレームかけて少しずつ下に移動させる
        for (int i = 0; i < 200; i++)
        {
            if(EndPaper != null)
            EndPaper.transform.position -= new Vector3(0, 5, 0); // Y座標をマイナス8ずつ下げる
            yield return null; // 1フレーム待機
        }
        
        // 画面外に落ちたらオブジェクトを完全に削除してメモリを解放
        Destroy(EndPaper);
    }
    
    // ゲーム起動時に1度だけ実行される初期化処理
    void Start()
    {
        SM.Conbo = 1.0f; // コンボ倍率を等倍(1.0)に
        ConboUI.SetActive(false); // コンボUIを隠す
        ScoreUI.text = ("Score : " + SM.Score + "pt"); // スコア表示初期化
        StartCoroutine(FirstPaper()); // 最初の書類をセットアップ
    }

    public void ResultMove()
    {
        // 今まで使っていた書類の親を退場場所（EndSpawn）に変更
        NowPaper.transform.SetParent(EndSpawn.transform);

        NowKakusi.SetActive(true); // 隠しオブジェクトを有効化

        // 役目を終えた書類をEndPaperとしてキープ
        EndPaper = NowPaper;

        //Instantiate(Resultpaper, FrontSpawn.transform);
        NowPaper = Resultpaper;
    }

    public void GameEnd()
    {
        AntiStanpUI.SetActive(true);
        /*if (BonusTimer > 0)
        {
            
        }*/
        //else
        {
            ConboCountUI.gameObject.SetActive(false);
            MS.EndCheck();

            TM.StartCoroutine(TM.TotalScoreOpen());
        }
    }


    void Update()
    {
        if(TimerActive)
        {
            Timer -= Time.deltaTime;
            TimerUI.text = $"Time : {Timer:F1}s"; 

            if(Timer > 0 && Timer <= 5)
                TimerUI.color = new Color(1f, 0.32f, 0.34f);
            if(Timer <= 5 && !TimerRed)
            {
                SoundManager.SoundM.Warningsound();
                TimerRed = true;
            }

            if (Timer <= 0)
            {
                Timer = 0;
                TimerUI.text = $"Time : {Timer:F1}s";
                TimerActive = false;
                ActiveEnd = 1;
            }
        }
        else if (!TimerActive && ActiveEnd == 1 && !BonusActive)
        {
            Debug.Log("定時");
            SoundManager.SoundM.StopSE();
            SoundManager.SoundM.Endsound();
            GameEnd();
            ActiveEnd = 2;
        }
 
    }
}