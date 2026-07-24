using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class HanteiHyouki : MonoBehaviour
{
    public GameObject Hyouki01;
    public GameObject Hyouki02;
    public GameObject Hyouki03;

    public Sprite[] hantei;
    public GameObject HyoukiPast;


    public void Hyouki(int num)
    {
        if(num <= 1 )
        {
            if(HyoukiPast  != null)
            {
                Destroy(HyoukiPast);
            }

            GameObject per =  Instantiate(Hyouki01, gameObject.transform);

            Image ha = per.GetComponent<Image>();

            ha.sprite = hantei[num];

            HyoukiPast = per;
        }
        else if (num <= 3 && num >= 2)
        {
            if (HyoukiPast != null)
            {
                Destroy(HyoukiPast);
            }

            GameObject per = Instantiate(Hyouki02, gameObject.transform);

            Image ha = per.GetComponent<Image>();

            ha.sprite = hantei[num];
            HyoukiPast = per;
        }
        else if(num == 5)
        {
            if (HyoukiPast != null)
            {
                Destroy(HyoukiPast);
            }

           GameObject har = Instantiate(Hyouki03, gameObject.transform);
            HyoukiPast = har;
        }
    }


}
