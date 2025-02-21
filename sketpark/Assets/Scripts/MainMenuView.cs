using UnityEngine;

public class MainMenuView : View
{
    public void OnStartButton()
    {
        ViewManager.Show<CharacterSelectionView>();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
