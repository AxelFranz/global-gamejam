using UnityEngine;
using UnityEngine.SceneManagement;

public class Trophy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Events.Win();
    }
}
