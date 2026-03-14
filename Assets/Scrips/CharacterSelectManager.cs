using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    private int indexP2 = 1; // Mặc định P2 ở ô số 2
    public bool p2Ready = false;

    [Header("Grid Setup")]
    public List<RectTransform> gridSlots;

    [Header("Fighting Game Style")]
    public Color p1Color = new Color(0.95f, 0.2f, 0.2f, 1f);
    public Color p2Color = new Color(0.2f, 0.5f, 0.95f, 1f);
    public Color selectedBorderColor = new Color(1f, 0.85f, 0.2f, 1f);
    public Color readyBorderColor = new Color(0.2f, 0.9f, 0.35f, 1f);
    public float nameOutlineWidth = 0.2f;
    public Color nameOutlineColor = new Color(0.05f, 0.05f, 0.05f, 1f);

    public TextMeshProUGUI readyTextP1;
    public TextMeshProUGUI readyTextP2;
    public TextMeshProUGUI titleText;
    public bool borderPulseEffect = true;

    private Image borderImageP1;
    private Image borderImageP2;
    private float pulseTimer;
    private bool loadingTriggered;

    void Start()
    {
        // Lấy Component Image từ Border để đổi màu
        borderImageP1 = borderP1 != null ? borderP1.GetComponent<Image>() : null;
        borderImageP2 = borderP2 != null ? borderP2.GetComponent<Image>() : null;

        ApplyFightingGameStyle();

        if (readyTextP1 != null) readyTextP1.gameObject.SetActive(false);
        if (readyTextP2 != null) readyTextP2.gameObject.SetActive(false);
        if (titleText != null) titleText.text = "SELECT YOUR FIGHTER";

        UpdateUI_P1();
        UpdateUI_P2();
    }

    void Update()
    {
        // --- LOGIC PLAYER 1 (A, D, SPACE) ---
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
                UpdateBorderColors();
            }
        }

        // --- LOGIC PLAYER 2 (Mũi tên, ENTER) ---
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
                UpdateBorderColors();
            }
        }

        // Hiệu ứng nhấp nháy viền
        if (borderPulseEffect)
        {
            pulseTimer += Time.deltaTime * 4f;
            float pulse = 0.85f + 0.15f * Mathf.Sin(pulseTimer);
            if (!p1Ready && borderImageP1 != null) borderImageP1.color = selectedBorderColor * pulse;
            if (!p2Ready && borderImageP2 != null) borderImageP2.color = selectedBorderColor * pulse;
        }

        // KIỂM TRA CHUYỂN CẢNH
        if (p1Ready && p2Ready && !loadingTriggered)
        {
            loadingTriggered = true;
            // Đợi 2 giây để người chơi thấy chữ READY rồi mới đi
            Invoke(nameof(TryStartGame), 2f);
        }
    }

    public void TryStartGame()
    {
        // BỐC DỮ LIỆU LÊN "XE" GAMEDATAMANAGER
        if (GameDataManager.instance != null)
        {
            GameDataManager.instance.selectedP1 = characterList[indexP1];
            GameDataManager.instance.selectedP2 = characterList[indexP2];

            Debug.Log("Đã lưu nhân vật: " + characterList[indexP1].charName + " và " + characterList[indexP2].charName);

            // Chuyển sang cảnh đánh nhau (Đảm bảo tên Scene là "MainGame")
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            Debug.LogError("LỖI: Chưa có GameDataManager trong Scene! Hãy tạo 1 Object và gắn script GameDataManager vào.");
            SceneManager.LoadScene("MainGame"); // Chữa cháy nếu quên
        }
    }

    void UpdateUI_P1()
    {
        if (characterList.Count == 0) return;
        previewP1.sprite = characterList[indexP1].fullBodySprite;
        nameTextP1.text = characterList[indexP1].charName;
        borderP1.position = gridSlots[indexP1].position;
    }

    void UpdateUI_P2()
    {
        if (characterList.Count == 0) return;
        previewP2.sprite = characterList[indexP2].fullBodySprite;
        nameTextP2.text = characterList[indexP2].charName;
        borderP2.position = gridSlots[indexP2].position;
    }

    void UpdateBorderColors()
    {
        if (borderImageP1 != null) borderImageP1.color = p1Ready ? readyBorderColor : selectedBorderColor;
        if (borderImageP2 != null) borderImageP2.color = p2Ready ? readyBorderColor : selectedBorderColor;

        if (readyTextP1 != null) readyTextP1.gameObject.SetActive(p1Ready);
        if (readyTextP2 != null) readyTextP2.gameObject.SetActive(p2Ready);
    }

    void ApplyFightingGameStyle()
    {
        if (nameTextP1 != null) nameTextP1.color = p1Color;
        if (nameTextP2 != null) nameTextP2.color = p2Color;
        UpdateBorderColors();
    }
}