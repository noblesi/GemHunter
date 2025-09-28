using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollBar;
    [SerializeField]
    private float swipeTime = 0.2f;
    [SerializeField]
    private float swipeDistance = 50.0f;

    private float[] scrollPageValues;
    private float valueDistance = 0;
    private int currentPage = 0;
    private int maxPage = 0;
    private float startTouchX;
    private float endTouchX;
    private bool isSwipeMode = false;

    public int CurrentPage => currentPage;

    private void Start()
    {
        maxPage = transform.childCount;
        scrollPageValues = new float[transform.childCount];
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        for(int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance;
        }

        SetScrollBarValue(0);
    }

    private void Update()
    {
        UpdateInput();
    }

    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[index];
    }

    private void UpdateInput()
    {
        if (isSwipeMode == true) return;

#if UNITY_EDITOR
        if(Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                startTouchX = Mouse.current.position.ReadValue().x;
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                endTouchX = Mouse.current.position.ReadValue().x;
                UpdateSwipe();
            }
        }
#elif UNITY_ANDROID
        if(Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];
            if (touch.press.wasPressedThisFrame)
            {
                startTouchX = touch.position.ReadValue().x;
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                endTouchX = touch.position.ReadValue().x;
                UpdateSwipe();
            }
        }
#endif
    }

    private void UpdateSwipe()
    {
        if(Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        bool isLeft = startTouchX < endTouchX;
        if(isLeft == true)
        {
            if (currentPage == 0) return;

            currentPage--;
        }
        else
        {
            if (currentPage == maxPage - 1) return;

            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    private IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollBar.value;
        float percent = 0;

        isSwipeMode = true;

        while(percent < 1)
        {
            percent += Time.deltaTime / swipeTime;

            scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }

        isSwipeMode = false;
    }
}
