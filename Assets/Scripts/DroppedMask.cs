using UnityEngine;

public class DroppedMask : MonoBehaviour
{
    public MaskState state;

    private void OnTriggerEnter(Collider other)
    {
        Player.Instance.addMask(state);
        Destroy(gameObject);
 
    }


}
