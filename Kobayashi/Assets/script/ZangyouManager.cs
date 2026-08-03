using UnityEngine;

public class ZangyouManager : MonoBehaviour
{
    public static ZangyouManager ZM;
    public bool ActiveZangyou = false;
    
    void Awake()
    {
        ZM = this;
    }
    
}
