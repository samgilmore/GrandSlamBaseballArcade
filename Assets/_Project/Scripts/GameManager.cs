using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

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

        dayButton.onClick.AddListener(() => SetDayNight(true));
        nightButton.onClick.AddListener(() => SetDayNight(false));
        easyButton.onClick.AddListener(() => SetDifficulty("easy"));
        mediumButton.onClick.AddListener(() => SetDifficulty("medium"));
        hardButton.onClick.AddListener(() => SetDifficulty("hard"));
    }

    public void StartGame()
    {
        // Ensure the player is in a valid teleportable area
        if (!IsPlayerInTeleportableArea())
        {
            Debug.LogError("Player is not in a valid teleportable area to start the game.");
            return;
        }

        currentScore = 0;
        homeRuns = 0;
        consecutiveHomeRuns = 0;
        pitchesRemaining = pitchesPerGame;
        isGameActive = true;

        UpdateScoreboard();
        startMenu.SetActive(false);
    }

    public void EndGame()
    {
        isGameActive = false;

        // Show final score and home runs
        finalScoreText.text = "Final Score: " + currentScore;
        finalHomeRunsText.text = "Home Runs: " + homeRuns;

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
        homeRunsText.text = "Home Runs: " + homeRuns;
        pitchesRemainingText.text = "Pitches Remaining: " + pitchesRemaining;
    }

    public void SetDayNight(bool isDay)
    {
        RenderSettings.skybox = isDay ? daySkybox : nightSkybox;
        selectedDayButton = isDay ? dayButton : nightButton;
        UpdateButtonColors();
    }

    public void SetDifficulty(string difficulty)
    {
        switch (difficulty.ToLower())
        {
            case "easy":
                pitchesPerGame = 15;
                selectedDifficultyButton = easyButton;
                break;
            case "medium":
                pitchesPerGame = 10;
                selectedDifficultyButton = mediumButton;
                break;
            case "hard":
                pitchesPerGame = 7;
                selectedDifficultyButton = hardButton;
                break;
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

    private bool IsPlayerInTeleportableArea()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (teleportableAreaLefty.GetComponent<Collider>().bounds.Contains(player.transform.position) ||
            teleportableAreaRighty.GetComponent<Collider>().bounds.Contains(player.transform.position))
        {
            return true;
        }

        return false;
    }
}