using UnityEngine;
using UnityEngine.UI;

public class BonusPaper : MonoBehaviour
{
    public GameManager GM;
    public float BPTimer;
    public Image BPGage;
    private bool End;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BPTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        BPTimer -= Time.deltaTime; // 時間を減らす

        var wariai = BPTimer / 3;  // 3秒を基準とした残り時間の割合（0.0〜1.0）を計算

       BPGage.fillAmount = wariai; // UIのゲージ量を更新

       if (wariai <= 0 && !End)
       {
           GM.PaperChange();
            End = true;
       }
    }
}
