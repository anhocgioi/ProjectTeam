using UnityEngine;
using TMPro; // 🔥 PHẢI CÓ DÒNG NÀY

public class BattleManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText; // 🔥 ĐỔI THÀNH TextMeshProUGUI

    public void FinishGame(int loserID)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (winnerText != null)
        {
            // Gán nội dung chữ tại đây
            winnerText.text = (loserID == 2) ? "PLAYER 1 WIN!" : "PLAYER 2 WIN!";
        }

        // Tạm thời bỏ dòng Time.timeScale = 0 để kiểm tra cho dễ
    }
}