using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public void Escape(InputAction.CallbackContext context) // if the escape key is clicked it will revert back to the first scene which is the main menu.
    {
        if (context.performed)
        {
            SceneManager.LoadScene(0);
        }
    }
}
