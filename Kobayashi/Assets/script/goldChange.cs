using TMPro;
using UnityEngine;

public class goldChange : MonoBehaviour
{
    public GameManager GM;
    public ScoreManager SM;

    public GameObject ConboTimerUI;
    public GameObject ConboTimerDummyUI;
    public TextMeshProUGUI TextTimerDummyUI;
    public TextMeshProUGUI TextTimerUI;
    public Color redColorsan;

    public void ConboChange()
    {
        // 3コンボごとに倍率の段階（ScoreUp）を計算
        var ScoreUp = SM.ConboCount / 3;

        SoundManager.SoundM.Zangyousound(2);

        // 15コンボ以下の場合、コンボ数に応じて倍率を上げる（最大5段階、1.0f + 0.2f*5 = 2.0倍）
        if (SM.ConboCount <= 15)
        {
            for (int i = 0; i < 6; i++)
            {
                if (ScoreUp >= i && ScoreUp < i + 1)
                {
                    SM.Conbo = 1.0f + 2.0f * i; // 倍率を設定（1.0倍、2.0倍...）
                    if (i > 0)
                        SM.Conbo -= 1.0f;

                    GM.ConboScoreUI.text = ("×" + SM.Conbo); // UIに倍率を表示
                    TextTimerDummyUI.text = ("×" + SM.Conbo);
                    if (SM.Conbo >= 2 && SM.Conbo < 10)
                    {
                        GM.ConboScoreUI.text = ("×" + SM.Conbo + ".0"); // UIに倍率を表示
                        TextTimerDummyUI.text = ("×" + SM.Conbo + ".0");
                    }


                    break;
                }
            }
        }
        else
        {
            SM.Conbo = 10.0f; 

            GM.ConboScoreUI.text = ("×" + SM.Conbo); // UIに倍率を表示
            TextTimerDummyUI.text = ("×" + SM.Conbo);
        }
    }

     
    public void ActiveUI() => ConboTimerUI.SetActive(true);

}
