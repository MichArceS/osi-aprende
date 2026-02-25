using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    private RectTransform rectTrans;
    private Image lineImage;

    void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
        lineImage = GetComponent<Image>();
        rectTrans.pivot = new Vector2(0, 0.5f);
    }

    public void SetColor(Color color)
    {
        if (lineImage != null) lineImage.color = color;
    }

    public void DrawLine(Vector2 startPos, Vector2 endPos)
    {
        rectTrans.position = startPos;

        Vector2 dir = endPos - startPos;
        float distance = dir.magnitude;

        rectTrans.sizeDelta = new Vector2(distance / transform.lossyScale.x, rectTrans.sizeDelta.y);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTrans.rotation = Quaternion.Euler(0, 0, angle);
    }
}