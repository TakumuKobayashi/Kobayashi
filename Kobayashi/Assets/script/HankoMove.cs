using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HankoMove : MonoBehaviour
{
    public bool MoveR;
    public bool Hanko;
    public float distance;
    public int distanceSpeed;
    public int distanceMaxSpeed;
    public GameObject HankoMoveR;
    public GameObject HankoMoveL;
    public GameObject HankoSpawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!Hanko)
        {
                var Pos= transform.position;
            for (int i = 1;i<=15; i++)
            {
                
                if (MoveR)
                {
                    GameObject A = Instantiate(HankoMoveR,this.transform);
                    A.transform.position = new Vector3(Pos.x - distance * i, Pos.y, Pos.z);
                    HankoMove K = A.GetComponent<HankoMove>();
                    K.HankoSpawnPos = HankoSpawnPos;
                }
                else
                {
                    GameObject A = Instantiate(HankoMoveL, this.transform);
                    A.transform.position = new Vector3(Pos.x + distance * i, Pos.y, Pos.z);
                    HankoMove K = A.GetComponent<HankoMove>();
                    K.HankoSpawnPos = HankoSpawnPos;
                }
                   
            }
          
        }
      
    }

    public void NewHanko(bool T,GameObject H)
    {
        Destroy(H);
        var Pos = transform.position;
        if (T)
        {
            GameObject A = Instantiate(HankoMoveR, this.transform);
            A.transform.position = new Vector3(Pos.x - distance * 15, Pos.y, Pos.z);
            HankoMove K = A.GetComponent<HankoMove>();
            K.HankoSpawnPos = HankoSpawnPos;
        }
        else
        {
            GameObject A = Instantiate(HankoMoveL, this.transform);
            A.transform.position = new Vector3(Pos.x + distance * 15, Pos.y, Pos.z);
            HankoMove K = A.GetComponent<HankoMove>();
            K.HankoSpawnPos = HankoSpawnPos;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Hanko)
        {
            var H = this.transform.position;
            distanceSpeed++;
            if (distanceSpeed >= distanceMaxSpeed)
            {
                if (MoveR)
                {
                    this.transform.position = new Vector3(H.x + 1, H.y, H.z);
                    if (this.transform.position.x >= HankoSpawnPos.transform.position.x)
                    {
                        HankoMove Hankoscr = HankoSpawnPos.GetComponent<HankoMove>();
                        Hankoscr.NewHanko(MoveR, this.gameObject);
                    }
                }
                else
                {
                    this.transform.position = new Vector3(H.x - 1, H.y, H.z);
                    if (this.transform.position.x <= HankoSpawnPos.transform.position.x)
                    {
                        HankoMove Hankoscr = HankoSpawnPos.GetComponent<HankoMove>();
                        Hankoscr.NewHanko(MoveR, this.gameObject);
                    }
                }
                distanceSpeed = 0;
            }


        }
    }
}
