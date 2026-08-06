using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MouseSistem : MonoBehaviour
{
    [Header("UI関係")]
    public Transform NowPaper;
    public GameObject Hanko;
    public GameObject HankoPos;
    public Image HankoCol;

    private GameObject HankoYosou;
    [Header("判定数確認")] 
    public int PeCount;
    public int GrCount;
    public int GoCount;
    public int BaCount;
    
    [Header("スクリプト関係")]
    public GameManager GM;
    public ScoreManager SM;
    public HanteiHyouki HanHyou;
    
    [Header("座標関係")]
    public Transform MousePos;

    [Header("終了関係")]
    public bool ActiveEnd;
  

    // UIの検知に必要なコンポーネント
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private EventSystem eventSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       HankoYosou= Instantiate(HankoPos, MousePos);
       HankoCol = HankoYosou.GetComponent<Image>();
        Cursor.visible = false;
    }

    public void HanteiCheck(Vector2 MP,GameObject HanteiBOX)
    {
        Vector2 PosA = MP;
        Vector2 PosB = HanteiBOX.transform.position;

        float distance = Vector2.Distance(PosA, PosB);

        Debug.Log(distance);

        var GetScore = 0.0f;

        if(distance <= 6)
        {
            Debug.Log("Perfect!!");
            PeCount++;
            GetScore = 100;
            HanHyou.Hyouki(0);
            SoundManager.SoundM.Hanteisound(0);
        }
        else if (distance > 6 && distance <= 16)
        {
            Debug.Log("Great!");
            GrCount++;
            GetScore = 50;
            HanHyou.Hyouki(1);
            SoundManager.SoundM.Hanteisound(1);
        }
        else if (distance > 16 && distance <= 31)
        {
            Debug.Log("Good");
            GoCount++;
            GetScore = 25;
            HanHyou.Hyouki(2);
            SoundManager.SoundM.Hanteisound(2);
        }
        else if (distance > 31)
        {
            Debug.Log("Bad");
            BaCount++;
            GetScore = 10f;
            HanHyou.Hyouki(3);
            SoundManager.SoundM.Hanteisound(3);
        }

        SM.ScoreUp(GetScore);

        if (distance <= 31)
        {
            SM.ConboSistem();
        }
        else
        {
            SM.ConboEnd(true);
        }

       
        Destroy(HanteiBOX.gameObject);
    }

    public void EndCheck()
    {
        ActiveEnd = true;
        NowPaper = GM.Resultpaper.transform;
    }


    // Update is called once per frame
    void Update()
    {
         /// 1. マウスの画面座標を取得
            Vector3 mousePos = Input.mousePosition;

        HankoYosou.transform.position = mousePos;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = mousePos;

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerData, results);

            GameObject targetPaper = null;
            GameObject targetHantei = null;
            var CR = false;

            // 1. まずクリック位置にあるUIをすべてチェックして、該当するものを探す
            foreach (RaycastResult result in results)
            {
                GameObject clickedUI = result.gameObject;


                if (clickedUI.CompareTag("Kasol")) continue;
                if (clickedUI.CompareTag("Kakusi")) break;

                if (clickedUI.CompareTag("Hantei") && targetHantei == null)
                {
                    targetHantei = clickedUI; // Hanteiを見つけた
                    CR = false;
                }
                else if (clickedUI.CompareTag("Renda") && targetHantei == null)
                {
                    targetHantei = clickedUI; // Rendaを見つけた
                    CR = true;
                }
                else if (clickedUI.CompareTag("Paper") && targetPaper == null)
                {
                    targetPaper = clickedUI; // Paperを見つけた
                }
            }

         
            // 2. 【最優先】Hantei が見つかっていれば、Hanteiの処理を行う（Paperは無視）
            if (targetHantei != null)
            {
                GameObject spawnedUI = Instantiate(Hanko, NowPaper);
                spawnedUI.transform.position = mousePos;

                SoundManager.SoundM.Hankosound();
                if(CR)
                {
                    Destroy(HanHyou.HyoukiPast);
                    SM.RendaCheak();
                }
                else
                {
                    SM.RendaUI.SetActive(false);
                    HanteiCheck(mousePos, targetHantei);
                }

                    
            }
            // 3. Hantei がなくて Paper だけが見つかっていれば、Paperの処理を行う
            else if (targetPaper != null)
            {
                if (!ActiveEnd)
                {
                    GameObject spawnedUI = Instantiate(Hanko, NowPaper); 
                    spawnedUI.transform.position = mousePos; 
                    SM.RendaUI.SetActive(false);
                    
                    SoundManager.SoundM.Hankosound();
                    
                    Debug.Log("?"); 
                    SM.Score -= 10; 
                    SM.GetScoreUIUp(-10,new Color(0.83f, 0.35f, 0.35f)); 
                    if (SM.Score < 0) SM.Score = 0;
                    
                    SM.HankoCount++; 
                    GM.ScoreUI.text = ("Score : " + SM.Score + "pt"); 
                    SM.ConboEnd(true); 
                    HanHyou.Hyouki(5);
                    
                }
                else
                {
                    GameObject spawnedUI = Instantiate(Hanko, NowPaper); 
                    spawnedUI.transform.position = mousePos;
                    SoundManager.SoundM.Hankosound();
                }
               
            }
        }
        // --------------------------------------
        //// 2. UIをCanvasの子要素として生成
        //GameObject spawnedUI = Instantiate(Hanko, NowPaper);

        //// 3. 生成したUIの画面位置にマウス座標をそのままセット
        //spawnedUI.transform.position = mousePos;

        //SE.PlayOneShot(PONSound);
         
    }
}
