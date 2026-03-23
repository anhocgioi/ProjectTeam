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
    public bool p1Ready = false;

    [Header("Player 2 UI")]
    public Image previewP2;
    public TextMeshProUGUI nameTextP2;
    public RectTransform borderP2;
    private int indexP2 = 1;
    public bool p2Ready = false;

    [Header("Grid Setup")]
    public List<RectTransform> gridSlots;

    void Start()
    {
        // ===== CHECK =====
        if (characterList == null || characterList.Count == 0)
        {
            Debug.LogError("Character list rỗng!");
            return;
        }

        // Nếu chỉ có 1 nhân vật
        if (characterList.Count == 1)
        {
            indexP2 = 0;
        }

        // ===== SINGLE PLAYER =====
        if (GameDataManager.instance.isSinglePlayer)
        {
            indexP2 = Random.Range(0, characterList.Count); // 🔥 Random ngay UI
            p2Ready = true;
        }

        UpdateUI_P1();
        UpdateUI_P2();
    }

    void Update()
    {
        if (characterList == null || characterList.Count == 0) return;

        // ================= P1 =================
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
                borderP1.GetComponent<Image>().color = Color.gray;
            }
        }

        // ================= P2 (CHỈ MULTI) =================
        if (!GameDataManager.instance.isSinglePlayer)
        {
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
                {
                    p2Ready = true;
                    borderP2.GetComponent<Image>().color = Color.gray;
                }
            }
        }

        // ================= LOAD GAME =================
        if (p1Ready && p2Ready)
        {
            SaveSelection();
            Invoke("LoadMainGame", 1f);
        }
    }

    // ================= SAVE =================
    void SaveSelection()
{
    if (GameDataManager.instance == null)
    {
        Debug.LogError("GameDataManager NULL!");
        return;
    }

    // ================= P1 =================
    GameDataManager.instance.selectedP1 = characterList[indexP1];

    // ================= P2 =================
    if (GameDataManager.instance.isSinglePlayer)
    {
        int randomIndex = Random.Range(0, characterList.Count);

        GameDataManager.instance.selectedP2 = characterList[randomIndex];

        Debug.Log("P1: " + characterList[indexP1].charName);
        Debug.Log("P2 (BOT RANDOM): " + characterList[randomIndex].charName);
    }
    else
    {
        GameDataManager.instance.selectedP2 = characterList[indexP2];

        Debug.Log("P1: " + characterList[indexP1].charName);
        Debug.Log("P2: " + characterList[indexP2].charName);
    }
}

    // ================= LOAD SCENE =================
    void LoadMainGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    // ================= UI =================
    void UpdateUI_P1()
    {
        if (indexP1 < 0 || indexP1 >= characterList.Count) return;

        previewP1.sprite = characterList[indexP1].fullBodySprite;
        nameTextP1.text = characterList[indexP1].charName;

        if (indexP1 < gridSlots.Count)
            borderP1.position = gridSlots[indexP1].position;
    }

    void UpdateUI_P2()
    {
        if (indexP2 < 0 || indexP2 >= characterList.Count) return;

        previewP2.sprite = characterList[indexP2].fullBodySprite;
        nameTextP2.text = characterList[indexP2].charName;

        if (indexP2 < gridSlots.Count)
            borderP2.position = gridSlots[indexP2].position;
    }
}