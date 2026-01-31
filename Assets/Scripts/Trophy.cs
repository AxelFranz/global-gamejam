using UnityEngine;
using UnityEngine.SceneManagement;

public class Trophy : MonoBehaviour
{
    [SerializeField] private string m_sceneName;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("NON");
        if (other.gameObject == Player.Instance.gameObject) {
            Debug.Log("OUI");
            SceneManager.LoadScene(m_sceneName);
        }
    }
}
