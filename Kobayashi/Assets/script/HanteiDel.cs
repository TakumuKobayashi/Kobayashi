using UnityEngine;

public class HanteiDel : MonoBehaviour
{

    public void DELETE()
    {
        Destroy(gameObject);
    }

    public void HyoukaIn() => SoundManager.SoundM.HyoukaOpensound();
    public void HyoukaOut() => SoundManager.SoundM.StopSE();
    public void HankoIn() => SoundManager.SoundM.Hankosound();
    public void StartSound() => SoundManager.SoundM.Startsound();

    public void Loopsound()
    { 
        SoundManager.SoundM.StopSE();
        SoundManager.SoundM.TotalWaitsound();
    }
    

}
