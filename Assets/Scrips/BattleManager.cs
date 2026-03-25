using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    // Tạo Singleton để Player dễ gọi
    public static BattleManager instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;

    private void Awake()
    {
        instance = this;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void FinishGame(int loserID)
    {
        // Gọi Camera lướt tới người thắng
        CameraControl cam = FindObjectOfType<CameraControl>();
        if (cam != null)
        {
            int winnerID = (loserID == 1) ? 2 : 1;
            cam.SetVictoryZoom(winnerID);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (winnerText != null)
            {
                winnerText.text = (loserID == 1) ? "PLAYER 2 WIN!" : "PLAYER 1 WIN!";
            }

            // Nếu muốn game dừng hẳn sau khi zoom xong, hãy dùng Invoke sau 2 giây
            // Invoke("PauseGame", 2f);
        }
    }

    void PauseGame() { Time.timeScale = 0; }
    public void PlayAgain() // Phải có chữ public này
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void BackToMenu() // Phải có chữ public này
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    }
    
