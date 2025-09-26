using UnityEngine;
using UnityEngine.Events;

public class GemCollector : MonoBehaviour
{
    [SerializeField]
    private GameObject gemEffectPrefab;
    [SerializeField]
    private RectTransform uiElement;
    [SerializeField]
    private UnityEvent onGemCollectEvent;
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(gemEffectPrefab);
    }

    public void SpawnGemEffect(Vector2 point, int count = 5)
    {
        for(int i = 0; i < count; ++i)
        {
            GameObject gem = memoryPool.ActivatePoolItem(point);
            gem.GetComponent<GemCollectEffect>().SetUp(this, uiElement);
        }
    }

    public void OnGemCollect(GameObject gem)
    {
        onGemCollectEvent?.Invoke();
        memoryPool.DeactivatePoolItem(gem);
    }
}
