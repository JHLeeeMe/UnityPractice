using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    [SerializeField] private Image _mask;

    private float _originalSize;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _originalSize = _mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        _mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _originalSize * value);
    }
}
