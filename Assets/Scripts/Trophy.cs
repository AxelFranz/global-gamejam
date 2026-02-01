using UnityEngine;
using UnityEngine.SceneManagement;

public class Trophy : MonoBehaviour
{
    [SerializeField] private GameObject bibelot;
    private void OnTriggerEnter(Collider other)
    {
        bibelot.GetComponent<MeshRenderer>().enabled = false;
        Events.Win();
    }
}
