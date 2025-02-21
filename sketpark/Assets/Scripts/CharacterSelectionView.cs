using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionView : View
{
    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        SceneManager.LoadScene("GameScene"); // Change this to your actual game scene name
    }

    public void OnBackButton()
    {
        ViewManager.ShowLast();
    }
}
