using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class SettingManager : MonoBehaviour
{

  public AudioMixer Mix;

    public string bgmName = "BGM";
    public string seName = "SE";

    public TextMeshProUGUI BGMTextUI;
    public TextMeshProUGUI SETextUI;

    public void SetVolumeBGM(float Value)
    {
        // 0以下の値を防止しつつ対数変換（-80dB 〜 0dB）
        float dB = Mathf.Log10(Mathf.Max(Value, 0.0001f)) * 20f;

        // AudioMixerの音量を更新
        Mix.SetFloat(bgmName, dB);

        var wariai = (Value / 1)*100;

        BGMTextUI.text = $"{wariai:F0}";
    }

    public void SetVolumeSE(float Value)
    {
        // 0以下の値を防止しつつ対数変換（-80dB 〜 0dB）
        float dB = Mathf.Log10(Mathf.Max(Value, 0.0001f)) * 20f;

        // AudioMixerの音量を更新
        Mix.SetFloat(seName, dB);

        var wariai = (Value / 1) * 100;

        SETextUI.text = $"{wariai:F0}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
