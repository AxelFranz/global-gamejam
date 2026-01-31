using UnityEngine;
using UnityEngine.UI;
public class MainMenuListener : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button m_startButton;
    [SerializeField] private Button m_creditsButton;
    [SerializeField] private Button m_controlsButton;
    [SerializeField] private Button m_backCreditsButton;
    [SerializeField] private Button m_backCommButton;
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_credits;
    [SerializeField] private GameObject m_commandes;


    void Start()
    {
        m_creditsButton.onClick.AddListener(switchToCredits);
        m_controlsButton.onClick.AddListener(switchToControls);
        m_backCreditsButton.onClick.AddListener(backToMenu);
        m_backCommButton.onClick.AddListener(backToMenu);
        m_startButton.onClick.AddListener(startGame);
        GameManager.Instance.ChangeState(GameState.Menu);
    }

    private void switchToCredits()
    {
        m_mainMenu.SetActive(false);
        m_credits.SetActive(true);
    }

    private void switchToControls()
    {
        m_mainMenu.SetActive(false);
        m_commandes.SetActive(true);
    }


    private void startGame()
    {
        m_mainMenu.SetActive(false);
        GameManager.Instance.ChangeState(GameState.Playing);
    }

    private void backToMenu()
    {
        m_credits.SetActive(false);
        m_commandes.SetActive(false);
        m_mainMenu.SetActive(true);
    }

}
