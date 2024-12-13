using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int pitchesPerGame = 10;
    public int homeRunMultiplierThreshold = 3;

    [Header("Scoreboard Display")]
    public TextMeshProUGUI homeRunsText;
    public TextMeshProUGUI pitchesRemainingText;

    [Header("Final Summary Display")]
    public GameObject resetMenu;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalHomeRunsText;

    [Header("Menus")]
    public GameObject startMenu;

    [Header("Day/Night Settings")]
    public Button dayButton;
    public Button nightButton;
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Difficulty Settings")]
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    [Header("Meta Teleport")]
    public GameObject teleportableAreaLefty;
    public GameObject teleportableAreaRighty;

    [Header("Managers")]
    public BallManager ballManager;

    private Button selectedDayButton;
    private Button selectedDifficultyButton;

    private int currentScore = 0;
    private int homeRuns = 0;
    private int consecutiveHomeRuns = 0;
    private int pitchesRemaining;
    private bool isGameActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeStartMenu();
    }

    private void InitializeStartMenu()
    {
        selectedDayButton = dayButton;
        selectedDifficultyButton = easyButton;
        UpdateButtonColors();

        startMenu.SetActive(true);
        resetMenu.SetActive(false);

        dayButton.onClick.AddListener(() => SelectDayNightButton(dayButton));
        nightButton.onClick.AddListener(() => SelectDayNightButton(nightButton));
        easyButton.onClick.AddListener(() => SetDifficulty("easy"));
        mediumButton.onClick.AddListener(() => SetDifficulty("medium"));
        hardButton.onClick.AddListener(() => SetDifficulty("hard"));
    }

    public void PlayerEnteredTeleportArea()
    {
        if (isGameActive) return;

        StartGame();
    }

    private void StartGame()
    {
        currentScore = 0;
        homeRuns = 0;
        consecutiveHomeRuns = 0;
        pitchesRemaining = pitchesPerGame;
        isGameActive = true;

        UpdateScoreboard();
        startMenu.SetActive(false);

        // Start the pitching process in BallManager
        ballManager.StartPitching(GetDifficultyLevel(), pitchesPerGame);
    }

    public void EndGame()
    {
        isGameActive = false;

        // Stop the pitching process in BallManager
        ballManager.StopPitching();

        // Show final score and home runs
        finalScoreText.text = currentScore.ToString();
        finalHomeRunsText.text = homeRuns.ToString();

        resetMenu.SetActive(true);
    }

    public void RestartGame()
    {
        StartGame();
        resetMenu.SetActive(false);
    }

    public void HandlePitchOutcome(bool isHomeRun)
    {
        if (!isGameActive) return;

        pitchesRemaining--;

        if (isHomeRun)
        {
            homeRuns++;
            consecutiveHomeRuns++;
            int multiplier = (consecutiveHomeRuns >= homeRunMultiplierThreshold) ? consecutiveHomeRuns : 1;
            currentScore += 10 * multiplier;
        }
        else
        {
            consecutiveHomeRuns = 0;
        }

        UpdateScoreboard();

        if (pitchesRemaining <= 0)
        {
            EndGame();
        }
    }

    private void UpdateScoreboard()
    {
        homeRunsText.text = homeRuns.ToString();
        pitchesRemainingText.text = pitchesRemaining.ToString();
    }

    private int GetDifficultyLevel()
    {
        if (selectedDifficultyButton == easyButton) return 0;
        if (selectedDifficultyButton == mediumButton) return 1;
        return 2;
    }

    public void SetDifficulty(string difficulty)
    {
        Debug.Log("Setting difficulty: " + difficulty);

        switch (difficulty.ToLower())
        {
            case "easy":
                pitchesPerGame = 3;
                selectedDifficultyButton = easyButton;
                break;
            case "medium":
                pitchesPerGame = 5;
                selectedDifficultyButton = mediumButton;
                break;
            case "hard":
                pitchesPerGame = 10;
                selectedDifficultyButton = hardButton;
                break;
        }
        UpdateButtonColors();
    }

    public void SelectDayNightButton(Button button)
    {
        selectedDayButton = button;

        if (button == dayButton)
        {
            RenderSettings.skybox = daySkybox;
        }
        else if (button == nightButton)
        {
            RenderSettings.skybox = nightSkybox;
        }

        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        // Parse hex colors
        Color dayColor, nightColor, easyColor, mediumColor, hardColor;
        ColorUtility.TryParseHtmlString("#A2FBFF", out dayColor);
        ColorUtility.TryParseHtmlString("#2860FF", out nightColor);
        ColorUtility.TryParseHtmlString("#8AFF73", out easyColor);
        ColorUtility.TryParseHtmlString("#FFC147", out mediumColor);
        ColorUtility.TryParseHtmlString("#FF6060", out hardColor);

        // Day/Night button color updates
        dayButton.GetComponent<Image>().color = (selectedDayButton == dayButton) ? dayColor : Color.white;
        nightButton.GetComponent<Image>().color = (selectedDayButton == nightButton) ? nightColor : Color.white;

        // Difficulty button color updates
        easyButton.GetComponent<Image>().color = (selectedDifficultyButton == easyButton) ? easyColor : Color.white;
        mediumButton.GetComponent<Image>().color = (selectedDifficultyButton == mediumButton) ? mediumColor : Color.white;
        hardButton.GetComponent<Image>().color = (selectedDifficultyButton == hardButton) ? hardColor : Color.white;
    }
}