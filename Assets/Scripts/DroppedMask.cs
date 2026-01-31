using UnityEngine;

public class DroppedMask : MonoBehaviour
{
    public MaskState state;

    private void OnCollisionEnter(Collision collision)
    {
        Player.Instance.addMask(state);
        Destroy(gameObject);
    }


}
