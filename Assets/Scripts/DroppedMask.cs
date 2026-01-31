using System;
using UnityEngine;

public class DroppedMask : MonoBehaviour
{
    public MaskState state;
    public Boolean activeMode;
    

    private void OnTriggerEnter(Collider other)
    {
        if(activeMode)
        {
            Player.Instance.addMask(state);
            Destroy(gameObject);

        }
 
    }

    private void OnTriggerExit(Collider other)
    {
        activeMode = true;
    }


}
