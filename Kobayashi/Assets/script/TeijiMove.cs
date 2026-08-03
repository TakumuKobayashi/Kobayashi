using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeijiMove : MonoBehaviour
{
    public GameManager GM;
    public ScoreManager SM;

    public TextMeshProUGUI ScoreUI;
    public TextMeshProUGUI TimeUI;
    public TextMeshProUGUI BonusUI;
    public TextMeshProUGUI BairituUI;

    public TextMeshProUGUI ScoreDummyUI;
    public TextMeshProUGUI TimeDummyUI;
    public TextMeshProUGUI BonusDummyUI;
    public TextMeshProUGUI BairituDummyUI;

    public GameObject BonusTextUI;
    public Image ConboTimerUI;
    public Image ConboTimerDummyUI;
    public GameObject TimerUI;
    public GameObject TimerUIDummy;
    public Sprite GoldRound;

    public GameObject Teiji;
    public bool ActiveTeijiMove = false;

    public void DummySet01()
    {
        ScoreUI.gameObject.SetActive(false);
        TimeUI.gameObject.SetActive(false);
        BonusUI.gameObject.SetActive(false);
        BonusTextUI.gameObject.SetActive(false);
        ScoreDummyUI.text = ScoreUI.text;
        TimeDummyUI.text = TimeUI.text;
        BonusDummyUI.text = BonusUI.text;
        ScoreDummyUI.gameObject.SetActive(true);
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
        ActiveTeijiMove = true;
        if (SM.ConboCount > 0)
        {
            TimerUI.gameObject.SetActive(false);
            ConboTimerDummyUI.fillAmount = ConboTimerUI.fillAmount;
            BairituDummyUI.text = BairituUI.text;
            ConboTimerUI.sprite = GoldRound;
            TimerUIDummy.gameObject.SetActive(true);
           
        }
    }

    public void sistemFadeOut()
    {
        StartCoroutine(FadeOut());
    }

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
