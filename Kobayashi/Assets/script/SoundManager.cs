using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SE;

    [Header("判定音")]
    public AudioClip[] HanteiSound;
    public AudioClip PeraSound;
    public AudioClip HakusyuSound;
    public AudioClip BlueActiveSound;
    public AudioClip EndSound;

    public static SoundManager SoundM;

    private void Awake()
    {
        SoundM = this;
    }

    public void Hanteisound(int Num) => SE.PlayOneShot(HanteiSound[Num]);
    public void Perasound() => SE.PlayOneShot(PeraSound);
    public void Hakusyusound() => SE.PlayOneShot(HakusyuSound);
    public void BlueActivesound() => SE.PlayOneShot(BlueActiveSound);
    public void Endsound() => SE.PlayOneShot(EndSound);
}
