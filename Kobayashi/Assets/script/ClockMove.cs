using JetBrains.Annotations;
using UnityEngine;

public class ClockMove : MonoBehaviour
{
    public bool ActiveClock;
    public static ClockMove CM;
    public void Awake()
    {
        CM = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(ActiveClock)
        {
            transform.Rotate(0, 0, -9f * Time.deltaTime);
        }
        
    }
}
