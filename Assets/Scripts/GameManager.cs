using UnityEngine;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MaskState MaskState { get; private set; }
    public GameState GameState { get; private set; }


    private void Awake()
    {
        this.GameState = GameState.Menu;
        this.MaskState = MaskState.Unmasked;
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
        if(MaskState != newState)
        {
            AudioManager.Instance.StopAll();

            AudioManager.Instance.changeMask((newState == MaskState.Unmasked) ? "menu" : newState.ToString() );

        }
        Events.MaskChanged?.Invoke(newState);
        MaskState = newState;
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch(newState)
        {
            case GameState.Menu:
                AudioManager.Instance.StopAll();
                AudioManager.Instance.Play("menu");
                Events.MenuOpened(); 
                Camera.Instance.AddComponent<SpinCamera>();
                Camera.Instance.GetComponent<SmoothCameraFollow>().enabled = false;
                break;
            case GameState.Playing:
                Destroy(Camera.Instance.GetComponent<SpinCamera>());
                Camera.Instance.GetComponent<SmoothCameraFollow>().enabled = true;
                Events.PlayerLoaded();
                Events.MenuClosed();
                break;
        }
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
