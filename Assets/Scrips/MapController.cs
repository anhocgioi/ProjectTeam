using UnityEngine;
using UnityEngine.SceneManagement;
public class MapController : MonoBehaviour
{
    // Mảng chứa các GameObject của các Map
    public GameObject[] maps;
    private int index = 0;

    void Start()
    {
        // Khi bắt đầu, chỉ hiển thị map đầu tiên
        UpdateMapDisplay();
    }

    public void NextMap()
    {
        index++;
        if (index >= maps.Length) index = 0; // Quay lại đầu
        UpdateMapDisplay();
    }

    public void PreviousMap()
    {
        index--;
        if (index < 0) index = maps.Length - 1; // Nhảy tới cuối
        UpdateMapDisplay();
    }

    private void UpdateMapDisplay()
    {
        for (int i = 0; i < maps.Length; i++)
        {
            // Nếu i trùng với chỉ số hiện tại thì hiện, ngược lại thì ẩn
            maps[i].SetActive(i == index);
        }
    }
    public void StartGame()
    {
        // Tên scene trong mảng phải khớp 100% với tên file scene bạn đặt
        string[] sceneNames = { "Map1", "Map2", "Map3" };
        SceneManager.LoadScene(sceneNames[index]);
    }
}