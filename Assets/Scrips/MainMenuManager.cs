using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OpenCharacterSelect()
    {
        // Nó sẽ mở Scene có số thứ tự là 1 trong Build Settings
        SceneManager.LoadScene(1);
    }
}