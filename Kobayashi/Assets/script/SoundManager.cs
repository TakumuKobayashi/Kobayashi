using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SE;
    public AudioSource ClapSE;
    public AudioSource HankoSE;
    [Header("BGM")]
    public AudioClip InGameSound;
    public AudioClip GameEndSound;
    public AudioClip ZangyouBGM;
    public AudioClip TitleBGMFull;
    public AudioClip TitleBGMLoop;

    [Header("汎用音")]
    public AudioClip SyousaiSound;
    public AudioClip CancelSound;
    public AudioClip KasolSound;
    public AudioClip SettingSound;

    [Header("ハンコ音")]
    public AudioClip HankoSound;
    [Header("判定音")]
    public AudioClip[] HanteiSound;
  
    [Header("称賛音")]
    public AudioClip HakusyuSound;
    [Header("開始音")]
    public AudioClip BlueActiveSound;
    public AudioClip PeraSound;
    public AudioClip StartSound;
    public AudioClip HajimeSound;
    public AudioClip HankoFallSound;
    [Header("終了音")]
    public AudioClip WarningSound;
    public AudioClip ChangeSound;
    public AudioClip EndSound;
    [Header("発表時音")]
    public AudioClip ScoreOpenSound;
    public AudioClip ScoreUPSound;
    public AudioClip TotalWaitSound;
    public AudioClip TotalOpenSound;
    public AudioClip HyoukaOpenSound;

    [Header("休憩音")]
    public AudioClip PauseInSound;
    public AudioClip PauseOutSound;

    [Header("残業音")]
    public AudioClip[] ZangyouSound = new AudioClip[7];

    public static SoundManager SoundM;

    private void Awake()
    {
        SoundM = this;
    }
    //BGM
    public void GameBGMsound() => BGM.PlayOneShot(InGameSound);
    public void GameEndsound() => BGM.PlayOneShot(GameEndSound);
    public void Zangyoubgm() => BGM.PlayOneShot(ZangyouBGM);
    public void TitleBGMfull() => BGM.PlayOneShot(TitleBGMFull);
    public void TitleBGMloop()
    {
        BGM.loop = true;
        BGM.resource = TitleBGMLoop;
        BGM.Play();
    }

    //SE
    public void Hankosound() => HankoSE.PlayOneShot(HankoSound);
    public void Hanteisound(int Num) => SE.PlayOneShot(HanteiSound[Num]);
    public void Perasound() => SE.PlayOneShot(PeraSound);
    public void Syousaisound() => SE.PlayOneShot(SyousaiSound);
    public void Cancelsound() => SE.PlayOneShot(CancelSound); 
    public void Kasolsound() => SE.PlayOneShot(KasolSound); 
    public void Hakusyusound() => ClapSE.PlayOneShot(HakusyuSound);
    public void BlueActivesound() => SE.PlayOneShot(BlueActiveSound);
    public void Endsound() => SE.PlayOneShot(EndSound);
    public void ScoreOpensound() => SE.PlayOneShot(ScoreOpenSound);
    public void ScoreUPsound() => SE.PlayOneShot(ScoreUPSound);
    public void TotalWaitsound() => SE.PlayOneShot(TotalWaitSound);
    public void TotalOpensound() => SE.PlayOneShot(TotalOpenSound);
    public void HyoukaOpensound() => SE.PlayOneShot(HyoukaOpenSound);
    public void StopSE() => SE.Stop();
    public void Startsound() => SE.PlayOneShot(StartSound);
    public void Hajimesound() => SE.PlayOneShot(HajimeSound);
    public void HankoFallsound() => SE.PlayOneShot(HankoFallSound);
    public void Warningsound() => SE.PlayOneShot(WarningSound);
    public void Changesound() => SE.PlayOneShot(ChangeSound);
    public void PauseInsound() => SE.PlayOneShot(PauseInSound);
    public void PauseOutsound() => SE.PlayOneShot(PauseOutSound);
    public void Settingsound() => SE.PlayOneShot(SettingSound);
    public void Zangyousound(int Num) => SE.PlayOneShot(ZangyouSound[Num]);
}
