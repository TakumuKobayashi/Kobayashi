using UnityEngine;

public class RainboTurn : MonoBehaviour
{
    public bool stopR;
    // Update is called once per frame
    void Update()
    {
        if(!stopR)
        {
            var Circle = this.transform.rotation;
        this.transform.eulerAngles -= new Vector3(Circle.x,Circle.y, Circle.z-1.5f);   
        }
        
    }
}
