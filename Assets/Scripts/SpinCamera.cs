using UnityEngine;

public class SpinCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        transform.RotateAround(Player.Instance.transform.position, Vector3.up, -40 * Time.deltaTime);
        transform.LookAt(Player.Instance.transform);
    }
}
