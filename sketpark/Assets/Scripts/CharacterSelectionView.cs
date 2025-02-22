using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionView : View
{
    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackButton()
    {
        ViewManager.ShowLast();
    }
}
