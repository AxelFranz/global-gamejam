using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private Vector3 m_spawnPosition;
    [SerializeField] private MaskState m_startingEquippedMask;
    [SerializeField] private MaskState m_startingBackMask;
    [SerializeField] private string m_nextSceneName;
    [SerializeField] private GameObject m_WaterDroppedMaskPrefab;
    [SerializeField] private GameObject m_FireDroppedMaskPrefab;
    [SerializeField] private GameObject m_PlantDroppedMaskPrefab;
    private List<PickUp> m_pickUps;
    private void Awake()
    {
        // Singleton checks
        if (Instance == null) { // If there is no instance of LevelManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a LevelManager instance already exists, destroy the new one
            Debug.LogWarning("Player Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }

        m_pickUps = new List<PickUp>();

        Events.Win += onWin;
        Events.Die += onDie;
    }

    private void Start()
    {
        savePickUps();
        spawnPlayer();
    }

    void onWin()
    {
        SceneManager.LoadScene(m_nextSceneName);
    }

    void onDie()
    {
        spawnPlayer();
        spawnPickUps();
    }

    void spawnPlayer()
    {
        Player.Instance.transform.position = m_spawnPosition;
        Player.Instance.setStartingMasks(m_startingEquippedMask, m_startingBackMask);
    }
	private void savePickUps() {
        m_pickUps.Clear();
        DroppedMask[] pickUps = FindObjectsByType<DroppedMask>(FindObjectsSortMode.InstanceID);
        foreach (DroppedMask pickup in pickUps) {
            PickUp newPickUp = new(pickup.transform.position, pickup.transform.rotation, pickup.state);
            m_pickUps.Add(newPickUp);
            Debug.Log("Save: " + pickup.transform.position);
        }
	}

    private void clearPickUps()
    {
        DroppedMask[] pickUps = FindObjectsByType<DroppedMask>(FindObjectsSortMode.InstanceID);
        foreach (DroppedMask pickup in pickUps) {
            Debug.Log("Clear: " + pickup.transform.position);
            Destroy(pickup);
            Destroy(pickup.gameObject);
        }
    }
    private void spawnPickUps()
    {
        clearPickUps();
        foreach(PickUp mask in m_pickUps) {
            //if (mask.instance != null) continue;
            GameObject variant = m_FireDroppedMaskPrefab;
            switch (mask.maskState) {
                case MaskState.Fire:
                    variant = m_FireDroppedMaskPrefab;
                    break;
                case MaskState.Water:
                    variant = m_WaterDroppedMaskPrefab;
                    break;
                case MaskState.Plant:
                    variant = m_PlantDroppedMaskPrefab;
                    break;
            }
			DroppedMask newDroppedMask = Instantiate(variant, mask.position, mask.rotation).GetComponent<DroppedMask>();
        }
    }
}
public class PickUp 
{
    public Vector3 position;
    public MaskState maskState;
    public Quaternion rotation;

	public PickUp(Vector3 position, Quaternion rotation, MaskState maskState) {
        this.position = position;
        this.rotation = rotation;
        this.maskState = maskState;
	}
}
