using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public Vector3 OutSize;
    public Vector3 InSize;

    public GameObject PauseUI;
    public GameObject[] buttonUI;
    public TextMeshProUGUI SetumeiText;
    public string[] Setumei;

    public static PauseManager PM;

    public bool ActiveESC;
    public bool ActivePause;

    private void Awake()
    {
        PM = this;
    }

    public void InScale(int Num)
    {
        buttonUI[Num].transform.localScale = InSize;
        SoundManager.SoundM.Kasolsound();
        SetumeiText.text = $"{Setumei[Num]}";
    }

    public void OutScale(int Num)
    {
        buttonUI[Num].transform.localScale = OutSize;
        SetumeiText.text = $"";
    }

    public void PressESC()
    {
        if (ActiveESC)
        {
            if (!ActivePause)
            {
                for (int i = 0; i < 4; i++)
                { buttonUI[i].transform.localScale = OutSize;
                SetumeiText.text = $"";
                }
               
                SoundManager.SoundM.PauseInsound();
                SoundManager.SoundM.BGM.Pause();
                Time.timeScale = 0;
                PauseUI.SetActive(true);
                ActivePause = true;
            }
            else
            {
                SoundManager.SoundM.PauseOutsound();
                SoundManager.SoundM.BGM.UnPause();
                Time.timeScale = 1;
                PauseUI.SetActive(false);
                ActivePause = false;
            }
        }

     }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressESC();
            Debug.Log("a");
        }
    }
}
