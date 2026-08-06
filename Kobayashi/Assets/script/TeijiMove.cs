using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeijiMove : MonoBehaviour
{
    public GameManager GM;
    public ScoreManager SM;

    public TextMeshProUGUI TimeUI;
    public TextMeshProUGUI ZangyouTimeUI;
    public TextMeshProUGUI BonusUI;
    public TextMeshProUGUI BairituUI;
    public TextMeshProUGUI GoUI;

    public TextMeshProUGUI TimeDummyUI;
    public TextMeshProUGUI BonusDummyUI;
    public TextMeshProUGUI BairituDummyUI;

    public GameObject ZangyouUI;
    public GameObject HanteiAntiUI;
        
    public GameObject BonusTextUI;
    public Image ConboTimerUI;
    public Image ConboTimerDummyUI;
    public GameObject TimerUI;
    public Image TimerWUI;
    public GameObject TimerUIDummy;
    public Sprite GoldRound;

    public GameObject Teiji;
    public GameObject Rainbo;
    public bool ActiveTeijiMove = false;
    public Color RedSanple;

    public void DummySet01()
    {
        TimeUI.gameObject.SetActive(false);
        BonusUI.gameObject.SetActive(false);
        BonusTextUI.gameObject.SetActive(false);
        TimeDummyUI.text = TimeUI.text;
        BonusDummyUI.text = BonusUI.text;
        TimeDummyUI.gameObject.SetActive(true);
        BonusDummyUI.gameObject.SetActive(true);
        ZangyouManager.ZM.ActiveZangyou = true;
        SoundManager.SoundM.GameEndsound();
    }

    public void TimeSetUp()
    {
        TimeDummyUI.text = $"Time : {GM.BonusTimer:F1}s";
        GM.Timer += GM.BonusTimer;
        GM.BonusTimer = 0;
        SoundManager.SoundM.TotalOpensound();
    }

    public void DummySet02()
    {
        SoundManager.SoundM.Zangyousound(3);
        BairituUI.color =  RedSanple;
        TimerWUI.sprite = GoldRound;
        if (SM.ConboCount > 0)
        {
            TimerUI.gameObject.SetActive(false);
            ConboTimerDummyUI.fillAmount = ConboTimerUI.fillAmount;
            BairituDummyUI.text = BairituUI.text;
            TimerUIDummy.gameObject.SetActive(true);
           
        }
    }
    public void ZaqngyouStart()
    {
        SoundManager.SoundM.Zangyousound(4);
        GoUI.text = $"Go!";
    }

    public void ActiveWave() => ActiveTeijiMove = true;

    public void DummySet03()
    {
        ZangyouTimeUI.text = TimeDummyUI.text;
        ZangyouTimeUI.gameObject.SetActive(true);
        ZangyouUI.SetActive(true) ;
        HanteiAntiUI.SetActive(false) ;
        GM.ActiveEnd = 3;
        GM.TimerActive = true;
        GM.TimerRed = false;

        Destroy(GM.NextPaper.gameObject);
        Destroy(GM.NextKakusi.gameObject);

        var syoruiNum = Random.Range(0, GM.Syorui.Length);
        GameObject spawnedPaper = null;

        // 待機場所（BackSpawn）の子として書類を生成
        spawnedPaper = Instantiate(GM.Syorui[syoruiNum].Paper, GM.BackSpawn.transform);
        // 次の書類の必要ハンコ数をキープしておく
        GM.NextHankoPoint = GM.Syorui[syoruiNum].HankoPoint;


        GM.NextPaper = spawnedPaper; // 次の書類として設定

        // 次の書類用の判定隠しオブジェクトを生成
        GM.NextKakusi = Instantiate(GM.HanteiKakusi, spawnedPaper.transform);

        // まだ出番ではないので非アクティブ（非表示）にしておく
        GM.NextPaper.SetActive(false);
    }


    public void sistemFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    public void SoundStart() => SoundManager.SoundM.Changesound();

    public IEnumerator FadeOut()
    {
     
        var BGMC =SoundManager.SoundM.BGM.GetComponent<AudioSource>();
        var Vol = BGMC.volume;
        var VolC = Vol / 1000;
        
        SM.RendaUI.SetActive(false);

        for (int i = 0;i< 1000;i++)
        {
            Vol -= VolC;
            BGMC.volume = Vol;
            yield return null;
        }
    }

    public void ZangyouS(int Num) => SoundManager.SoundM.Zangyousound(Num);
    public void ActiveRainbo() => Rainbo.SetActive(true);

    public void ZangyouBGM()
    {
        var BGMC = SoundManager.SoundM.BGM.GetComponent<AudioSource>();
        SoundManager.SoundM.BGM.Stop();
        BGMC.volume = 0.3f;
        SoundManager.SoundM.Zangyoubgm();
    }
    
    private void Update()
    {
        if (ActiveTeijiMove)
        {
            var one = Random.Range(-1.0f, 1.0f);
            var two = Random.Range(-1.0f, 1.0f);
            var Pos = Teiji.transform.position;
            Teiji.transform.position = new Vector3(Pos.x + one, Pos.y + two, Pos.z);
        }
    }
}
