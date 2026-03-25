using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform player1;
    private Transform player2;

    [Header("Zoom Danh Nhau")]
    public float minSize = 4f;
    public float maxSize = 7f;
    public float zoomSpeed = 5f;
    public float distanceMultiplier = 0.5f;

    [Header("Zoom Chien Thang")]
    public float victoryZoomSize = 2f;
    public float victoryZoomSpeed = 2f;
    private Transform winnerTransform;
    private bool isVictory = false;

    public Vector3 offset = new Vector3(0, 2, -10);

    void LateUpdate()
    {
        // 1. Zoom Chien Thang
        if (isVictory && winnerTransform != null)
        {
            Vector3 targetPos = new Vector3(winnerTransform.position.x, winnerTransform.position.y + 1f, offset.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * victoryZoomSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, victoryZoomSize, Time.deltaTime * victoryZoomSpeed);
            return;
        }

        // 2. TIM NHAN VAT (Sửa lại cho đúng Tag viết liền Player1 và Player2)
        if (player1 == null)
        {
            GameObject p1Obj = GameObject.FindWithTag("Player1");
            if (p1Obj != null) player1 = p1Obj.transform;
        }
        if (player2 == null)
        {
            GameObject p2Obj = GameObject.FindWithTag("Player2");
            if (p2Obj != null) player2 = p2Obj.transform;
        }

        if (player1 == null || player2 == null) return;

        // 3. Zoom To Nho (Khi dang danh nhau)
        Vector3 centerPoint = (player1.position + player2.position) / 2;
        Vector3 targetPosition = centerPoint + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed);

        float distance = Vector3.Distance(player1.position, player2.position);
        float targetSize = Mathf.Clamp(distance * distanceMultiplier, minSize, maxSize);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }

    public void SetVictoryZoom(int winnerID)
    {
        isVictory = true;
        if (winnerID == 1) winnerTransform = player1;
        else winnerTransform = player2;
    }
}