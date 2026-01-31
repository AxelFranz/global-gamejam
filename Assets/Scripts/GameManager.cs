using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    public static GameManager Instance { get; private set; }

    public MaskState MaskState { get; private set; }


    private void Awake()
    {
        // Keep the GameManager when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("GameManager Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
    }


    public void ChangeMask(MaskState newState)
    {
        Events.MaskChanged?.Invoke(newState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum MaskState
{
    Fire,
    Water,
    Plant,
    Unmasked
}
public enum GameState
{
    Playing,
    Menu,
    GameOver,
    Credits
}
