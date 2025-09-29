using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform rectBackground;
    [SerializeField]
    private RectTransform rectController;

    private Vector2 touchPosition;

    public float Horizontal => touchPosition.x;
    public float Vertical => touchPosition.y;

    private void Awake()
    {
        rectBackground.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectBackground.gameObject.SetActive(true);

        rectBackground.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectBackground, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            touchPosition.x = (touchPosition.x / rectBackground.sizeDelta.x * 2);
            touchPosition.y = (touchPosition.y / rectBackground.sizeDelta.y * 2);

            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            rectController.anchoredPosition = new Vector2(
                touchPosition.x * rectBackground.sizeDelta.x * 0.5f,
                touchPosition.y * rectBackground.sizeDelta.y * 0.5f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectController.anchoredPosition = Vector2.zero;

        touchPosition = Vector2.zero;

        rectBackground.gameObject.SetActive(false);
    }
}
