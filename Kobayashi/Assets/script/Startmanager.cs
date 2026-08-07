using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Startmanager : MonoBehaviour
{
    public GameObject[] buttonUI = new GameObject[4];
    public Vector3[] InScaleUI = new Vector3[4];
    public Vector3[] OutScaleUI = new Vector3[4];
    public GameObject BlackIn;

    public void ButtonIn(int Num)
    {
        buttonUI[Num].transform.localScale = InScaleUI[Num];
        SoundManager.SoundM.Kasolsound();
    }
    public void ButtonOut(int Num)
    {
        buttonUI[Num].transform.localScale = OutScaleUI[Num];
     }

    public void PushButton(int Num)
    {
                StartCoroutine(ChangeScene(Num));
    }

    public IEnumerator ChangeScene(int Num)
    {
        if (Num == 0)
        {
            SoundManager.SoundM.Changesound();
            BlackIn.SetActive(true);
        }
        else if (Num == 1)
            SoundManager.SoundM.TotalOpensound();
        for (int i = 0; i < 300; i++)
        {
            yield return null;
        }
        if (Num == 0)
        {
            SceneManager.LoadScene(0);
        }
        else if (Num == 1)
        {
            SceneManager.LoadScene(1);
        }
    }
}
