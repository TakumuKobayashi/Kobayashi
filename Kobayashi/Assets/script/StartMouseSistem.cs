using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartMouseSistem : MonoBehaviour
{
    [Header("UI関係")]
    public Transform NowPaper;
    public GameObject Hanko;
    public GameObject Hand;
    public GameObject HankoPos;
    public GameObject Handpos;
    public Image HankoCol;
    public Image HandCol;
    public GameObject HandObj;
   
    [Header("感度関連")]
    public float Sen = 1.00f;
    public Vector3 Mousepos;
    public bool ActiveCasol;

    [Header("座標関係")]
    public Transform MousePos;

    // UIの検知に必要なコンポーネント
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private EventSystem eventSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HandObj = Instantiate(Hand, MousePos);
        var handScale = HandObj.transform.localScale;
        handScale.x = 1.47f;
        handScale.y = 1.47f;
        handScale.z = 1.47f;
        HandObj.transform.localScale = handScale;
        Handanim han = HandObj.GetComponent<Handanim>();
        Handpos = han.HandPos;
        HankoPos = han.HankoPos;
        HankoCol = HankoPos.GetComponent<Image>();
        HandCol = han.HandCol;
        Cursor.visible = false;
        Mousepos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }


    public IEnumerator HandMove()
    {
        Handpos.SetActive(false);
        yield return null;
        Handpos.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ActiveCasol = true;
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            ActiveCasol = false;
        }

        // ----------------------------------------------------
        // 1. マウスの移動量を計算し、仮想カーソル位置を更新
        // ----------------------------------------------------
        // Input.GetAxis でフレームごとのマウス移動量を取得し、感度（sensitivity）を適用
        float mouseX = Input.GetAxis("Mouse X") * Sen * 20f;
        float mouseY = Input.GetAxis("Mouse Y") * Sen * 20f;

        Mousepos.x += mouseX;
        Mousepos.y += mouseY;

        // 仮想カーソルが画面外に出ないよう、画面サイズ内（0 ～ Screen幅/高さ）に制限
        Mousepos.x = Mathf.Clamp(Mousepos.x, 0, Screen.width);
        Mousepos.y = Mathf.Clamp(Mousepos.y, 0, Screen.height);

        // 追従表示するハンコの見た目（カーソル代わり）の位置を更新
        HandObj.transform.position = Mousepos;
        if (!ActiveCasol)
            Mouse.current.WarpCursorPosition(HandObj.transform.position);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Mousepos;

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerData, results);

            GameObject targetPaper = null;

            // 1. まずクリック位置にあるUIをすべてチェックして、該当するものを探す
            foreach (RaycastResult result in results)
            {
                GameObject clickedUI = result.gameObject;


                if (clickedUI.CompareTag("Kasol")) continue;
                if (clickedUI.CompareTag("Kakusi")) break;

                if (clickedUI.CompareTag("Paper") && targetPaper == null)
                {
                    targetPaper = clickedUI; // Paperを見つけた
                    NowPaper = clickedUI.transform;
                }
            }
            
            if (targetPaper != null)
            {
                    StartCoroutine(HandMove());
                    GameObject spawnedUI = Instantiate(Hanko, NowPaper);
                    spawnedUI.transform.position = Mousepos;
                var sca = spawnedUI.transform.localScale;
                sca.x = 0.2206296f;
                sca.y = 0.2206296f;
                sca.z = 0.2206296f;
                spawnedUI.transform.localScale = sca;
                    SoundManager.SoundM.Hankosound(); 

            }
        }


    }
}
