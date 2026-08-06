using UnityEngine;

public class RainboTurn : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var Circle = this.transform.rotation;
        this.transform.eulerAngles -= new Vector3(Circle.x,Circle.y, Circle.z-1.5f);   
    }
}
