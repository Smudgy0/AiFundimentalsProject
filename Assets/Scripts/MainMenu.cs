using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadScene_Button(int whichScene)
    {
        SceneManager.LoadScene(whichScene);
    }
}
