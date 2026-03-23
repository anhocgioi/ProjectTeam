using UnityEngine;
using TMPro;

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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (winnerText != null)
            {
                // Nếu Player 1 thua (loserID = 1) thì Player 2 thắng
                winnerText.text = (loserID == 1) ? "PLAYER 2 WIN!" : "PLAYER 1 WIN!";
            }

            // Dừng thời gian nếu muốn
             Time.timeScale = 0; 
        }
        else
        {
            Debug.LogError("Chưa kéo thả GameOverPanel vào BattleManager trên Hierarchy!");
        }
    }
}