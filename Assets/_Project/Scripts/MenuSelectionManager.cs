using UnityEngine;
using UnityEngine.UI;

public class MenuSelectionManager : MonoBehaviour
{
    public Button dayButton, nightButton;
    public Button easyButton, mediumButton, hardButton;

    public Material daySkybox;
    public Material nightSkybox;

    private Button selectedDayButton;
    private Button selectedDifficultyButton;

    void Start()
    {
        selectedDayButton = dayButton;
        selectedDifficultyButton = easyButton;

        UpdateButtonColors();
    }

    public void SelectDayButton(Button button)
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

    public void SelectDifficultyButton(Button button)
    {
        selectedDifficultyButton = button;
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