using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private Label winLabel;
    private Label scoreLabel;
    private int currentUiVersion = 0;

    void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        if (panelRenderer != null)
        {
            panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameWon += ShowWinScreen;
            GameManager.Instance.OnScoreChanged += UpdateScoreUI; 
        }
    }

    void OnDestroy()
    {
        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameWon -= ShowWinScreen;
            GameManager.Instance.OnScoreChanged -= UpdateScoreUI;
        }
    }

    void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
    {
        if (currentUiVersion == version) return;
        currentUiVersion = version;

        winLabel = rootElement.Q<Label>("WinLabel");
        if (winLabel != null) winLabel.style.display = DisplayStyle.None;

        scoreLabel = rootElement.Q<Label>("ScoreLabel");
        if (scoreLabel != null && GameManager.Instance != null)
        {
            scoreLabel.text = "Cans Left: " + GameManager.Instance.cansRemaining;
        }
    }

    public void ShowWinScreen()
    {
        if (winLabel != null) winLabel.style.display = DisplayStyle.Flex;
    }

    private void UpdateScoreUI(int remainingCans)
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = "Cans Left: " + remainingCans;
        }
    }
}