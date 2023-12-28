using PereViader.Utils.Unity3d.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectScrollSample : MonoBehaviour
{
    public Slider SliderX;
    public Slider SliderY;
    public Slider SliderPos;

    public Button RunAllButton;
    public Button RunHorizontalButton;
    public Button RunVerticalButton;

    public ScrollRect ScrollRect;
    public RectTransform ContentRectTransform;
    public RectTransform ElementRectTransform;

    public void Start()
    {
        SliderX.onValueChanged.AddListener(UpdatePosition);
        SliderY.onValueChanged.AddListener(UpdatePosition);
        UpdatePosition(default);
        
        RunAllButton.onClick.AddListener(() => ScrollRect.normalizedPosition =
            ScrollRect.GetElementNormalizedPosition(ElementRectTransform, new Vector2(SliderPos.normalizedValue, SliderPos.normalizedValue)));
        RunVerticalButton.onClick.AddListener(() => ScrollRect.verticalNormalizedPosition =
            ScrollRect.GetElementVerticalNormalizedPosition(ElementRectTransform, SliderPos.normalizedValue));
        RunHorizontalButton.onClick.AddListener(() => ScrollRect.horizontalNormalizedPosition =
            ScrollRect.GetElementHorizontalNormalizedPosition(ElementRectTransform, SliderPos.normalizedValue));

    }

    private void UpdatePosition(float _)
    {
        var normalizedPosition = new Vector2(SliderX.normalizedValue, -SliderY.normalizedValue);
        ElementRectTransform.localPosition = ContentRectTransform.rect.size * normalizedPosition;
    }
}
