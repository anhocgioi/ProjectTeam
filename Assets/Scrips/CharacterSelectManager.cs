using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterSelectManager : MonoBehaviour
{
    public List<CharacterData> characterList;

    [Header("Player 1 UI")]
    public Image previewP1;
    public TextMeshProUGUI nameTextP1;
    public RectTransform borderP1;
    private int indexP1 = 0;
    public bool p1Ready = false; // Biến mới

    [Header("Player 2 UI")]
    public Image previewP2;
    public TextMeshProUGUI nameTextP2;
    public RectTransform borderP2;
    private int indexP2 = 1;
    public bool p2Ready = false; // Biến mới

    [Header("Grid Setup")]
    public List<RectTransform> gridSlots;

    void Start()
    {
        UpdateUI_P1();
        UpdateUI_P2();
    }

    void Update()
    {
        // Player 1 chọn (A-D) và Khóa (Space)
        if (!p1Ready)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                indexP1 = (indexP1 + 1) % characterList.Count;
                UpdateUI_P1();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                indexP1 = (indexP1 - 1 + characterList.Count) % characterList.Count;
                UpdateUI_P1();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                p1Ready = true;
                borderP1.GetComponent<Image>().color = Color.gray; // Đổi màu khung khi khóa
            }
        }

        // Player 2 chọn (Mũi tên) và Khóa (Enter)
        if (!p2Ready)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                indexP2 = (indexP2 + 1) % characterList.Count;
                UpdateUI_P2();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                indexP2 = (indexP2 - 1 + characterList.Count) % characterList.Count;
                UpdateUI_P2();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            { // Phím Enter
                p2Ready = true;
                borderP2.GetComponent<Image>().color = Color.gray;
            }
        }

        // Nếu cả 2 cùng sẵn sàng thì chuyển Scene
        if (p1Ready && p2Ready)
        {
            // Chuyển sang scene tên là "MainGame" sau 2 giây
            Invoke("LoadMainGame", 2f);
        }
    }

    void LoadMainGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    void UpdateUI_P1()
    {
        previewP1.sprite = characterList[indexP1].fullBodySprite;
        nameTextP1.text = characterList[indexP1].charName;
        borderP1.position = gridSlots[indexP1].position;
    }

    void UpdateUI_P2()
    {
        previewP2.sprite = characterList[indexP2].fullBodySprite;
        nameTextP2.text = characterList[indexP2].charName;
        borderP2.position = gridSlots[indexP2].position;
    }
}