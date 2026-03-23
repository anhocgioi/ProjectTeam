using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [Header("Giao diện hiển thị Map")]
    // Mảng chứa các GameObject (ảnh hoặc bối cảnh) của các Map để ẩn/hiện
    public GameObject[] maps;
    private int index = 0;

    [Header("Cấu hình Scene")]
    // Tên các Scene tương ứng với từng Map (phải khớp 100% trong Build Settings)
    public string[] sceneNames = { "Map1", "Map2", "Map3" };

    void Start()
    {
        // Khi bắt đầu, hiển thị map đầu tiên
        UpdateMapDisplay();
    }

    public void NextMap()
    {
        index++;
        if (index >= maps.Length) index = 0;
        UpdateMapDisplay();
    }

    public void PreviousMap()
    {
        index--;
        if (index < 0) index = maps.Length - 1;
        UpdateMapDisplay();
    }

    private void UpdateMapDisplay()
    {
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i] != null)
                maps[i].SetActive(i == index);
        }
    }

    public void StartGame()
    {
        // 1. Lưu thông tin map đã chọn vào GameDataManager (nếu có)
        if (GameDataManager.instance != null)
        {
            GameDataManager.instance.selectedMapName = sceneNames[index];
            Debug.Log("Đã chọn Map: " + sceneNames[index]);
        }

        // 2. Chuyển đến Scene tương ứng
        if (index < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[index]);
        }
        else
        {
            Debug.LogError("Chỉ số Index Map vượt quá số lượng Scene thiết lập!");
        }
    }
}