using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Level1");
    }

    /*
    public void Continue()
    {
        int level = PlayerPrefs.GetInt("lastLevel", 1);
        SceneManager.LoadScene("Level1" + level);
    }
    */

    public void SelectLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
