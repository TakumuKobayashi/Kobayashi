using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingManager : MonoBehaviour
{
    public MouseSistem MS;
    public StartMouseSistem SMS;
  public AudioMixer Mix;

    public string bgmName = "BGM";
    public string seName = "SE";
    public Slider BGMSlider;
    public Slider SESlider;
    public Slider MouseSlider;

    public Vector3 INScale;
    public Vector3 OutScale;

    public TextMeshProUGUI BGMTextUI;
    public TextMeshProUGUI SETextUI;
    public TextMeshProUGUI MouseTextUI;

    public GameObject SettingUI;
    public GameObject BuckButtonUI;

    private void Start()
    {
        var BGMT = PlayerPrefs.GetFloat("BGMVolText",50);
        BGMTextUI.text = $"{BGMT:F0}";
        BGMSlider.value = BGMT / 100;
        Mix.SetFloat(bgmName, PlayerPrefs.GetFloat("BGMVol",  Mathf.Log10(Mathf.Max(0.5f, 0.0001f)) * 20f));

        var SET = PlayerPrefs.GetFloat("SEVolText",50);
        SETextUI.text = $"{SET:F0}";
        SESlider.value = SET / 100;
        Mix.SetFloat(seName, PlayerPrefs.GetFloat("SEVol", Mathf.Log10(Mathf.Max(0.5f, 0.0001f)) * 20f));

        MouseSlider.value = PlayerPrefs.GetFloat("MouseSen",1);
        MouseTextUI.text = $"{PlayerPrefs.GetFloat("MouseSen",1):F2}";
        if (MS != null)
        {
            MS.Sen = PlayerPrefs.GetFloat("MouseSen",1);
        }
        else
        {
            SMS.Sen = PlayerPrefs.GetFloat("MouseSen", 1);
        }
    }

    public void SetVolumeBGM(float Value)
    {
        // 0以下の値を防止しつつ対数変換（-80dB 〜 0dB）
        float dB = Mathf.Log10(Mathf.Max(Value, 0.0001f)) * 20f;

        // AudioMixerの音量を更新
        Mix.SetFloat(bgmName, dB);

        var wariai = (Value / 1)*100;

        BGMTextUI.text = $"{wariai:F0}";

        PlayerPrefs.SetFloat("BGMVol", dB);
        PlayerPrefs.SetFloat("BGMVolText", wariai);
    }

    public void SetVolumeSE(float Value)
    {
        // 0以下の値を防止しつつ対数変換（-80dB 〜 0dB）
        float dB = Mathf.Log10(Mathf.Max(Value, 0.0001f)) * 20f;

        // AudioMixerの音量を更新
        Mix.SetFloat(seName, dB);

        var wariai = (Value / 1) * 100;

        SETextUI.text = $"{wariai:F0}";
        PlayerPrefs.SetFloat("SEVol",dB);
        PlayerPrefs.SetFloat("SEVolText",wariai);
    }

    public void SetMouseSen(float Value)
    {
        if (MS != null)
        {
            MS.Sen = Value;
        }
        else
        {
            SMS.Sen = Value;
        }
        MouseTextUI.text = $"{Value:F2}";
        if (MS != null)
        {
            PlayerPrefs.SetFloat("MouseSen", MS.Sen);
        }
        else
        {
            PlayerPrefs.SetFloat("MouseSen", SMS.Sen);
        }
        
    }

    public void UPSen(float Value)
    {
        if (MS != null)
        {
            MS.Sen += Value;
        }
        else
        {
            SMS.Sen += Value;
        }
        MouseSlider.value += Value;
        SoundManager.SoundM.Kasolsound();
        if (MS != null)
        {
            PlayerPrefs.SetFloat("MouseSen", MS.Sen);
        }
        else
        {
            PlayerPrefs.SetFloat("MouseSen", SMS.Sen);
        }
    }

    public void DownSen(float Value)
    {
        if (MS != null)
        {
            MS.Sen -= Value;
        }
        else
        {
            SMS.Sen -= Value;
        }
        MouseSlider.value -= Value;
        SoundManager.SoundM.Kasolsound();
        if (MS != null)
        {
            PlayerPrefs.SetFloat("MouseSen", MS.Sen);
        }
        else
        {
            PlayerPrefs.SetFloat("MouseSen", SMS.Sen);
        }
    }

    public void TestSound()
    {
        SoundManager.SoundM.Hankosound();
    }

    public void ScaleIn()
    {
        BuckButtonUI.transform.localScale = INScale;
        SoundManager.SoundM.Kasolsound();
    }

    public void ScaleOut() => BuckButtonUI.transform.localScale = OutScale;

    public void BuckSetting()
    {
        BuckButtonUI.transform.localScale = OutScale;
        SettingUI.gameObject.SetActive(false);
        SoundManager.SoundM.Cancelsound();
        PauseManager.PM.ActiveSetting = false;
    }
}
