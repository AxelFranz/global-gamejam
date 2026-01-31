using UnityEngine;
using UnityEngine.UI;
public class MainMenuListener : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button m_startButton;
    [SerializeField] private Button m_creditsButton;
    [SerializeField] private Button m_backButton;
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_credits;


    void Start()
    {
        m_creditsButton.onClick.AddListener(switchToCredits);
        m_backButton.onClick.AddListener(backToMenu);
        m_startButton.onClick.AddListener(startGame);
        GameManager.Instance.ChangeState(GameState.Menu);
    }

    private void switchToCredits()
    {
        m_mainMenu.SetActive(false);
        m_credits.SetActive(true);
    }

    private void startGame()
    {
        m_mainMenu.SetActive(false);
        GameManager.Instance.ChangeState(GameState.Playing);
    }

    private void backToMenu()
    {
        m_credits.SetActive(false);
        m_mainMenu.SetActive(true);
    }

}
