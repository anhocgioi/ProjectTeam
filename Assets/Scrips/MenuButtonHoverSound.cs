using UnityEngine;
using UnityEngine.EventSystems;

// Gắn lên các Button để phát âm thanh khi con trỏ rê vào.
public class MenuButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    private MainMenuManager menu;

    // Chống spam khi con trỏ vào/ra nhanh.
    private float lastPlayTime;
    [SerializeField] private float minIntervalSeconds = 0.1f;

    private void Awake()
    {
        // Tìm MainMenuManager trong scene để dùng chung AudioSource.
        menu = FindFirstObjectByType<MainMenuManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (menu == null) return;
        if (Time.unscaledTime - lastPlayTime < minIntervalSeconds) return;

        lastPlayTime = Time.unscaledTime;
        menu.PlayHover();
    }
}

