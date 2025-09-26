using UnityEngine;

public class GemCollectEffect : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private GemCollector gemCollector;
    private RectTransform uiElement;
    private Vector3 start, end, point1;
    private float percent, duration;
    private readonly float minDuration = 0.5f, maxDuration = 2f, r = 3f;

    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetUp(GemCollector gemCollector, RectTransform uiElement)
    {
        this.gemCollector = gemCollector;
        this.uiElement = uiElement;
        percent = 0f;
        start = transform.position;
        duration = Random.Range(minDuration, maxDuration);
        point1 = Utils.GetNewPoint(start, Random.Range(0, 360), r);

        StartCoroutine(FadeEffect.Fade(spriteRenderer, 1, 0, percent));
    }

    private void Update()
    {
        if(percent >= 1f)
        {
            gemCollector.OnGemCollect(gameObject);
            return;
        }

        UpdateEndPoint();
        percent += Time.deltaTime / duration;
        transform.position = Utils.QuadraticCurve(start, point1, end, percent);
    }

    private void UpdateEndPoint()
    {
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, uiElement.position);

        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(
            uiElement, screenPoint, mainCamera, out Vector3 worldPoint)) end = worldPoint;
    }
}
