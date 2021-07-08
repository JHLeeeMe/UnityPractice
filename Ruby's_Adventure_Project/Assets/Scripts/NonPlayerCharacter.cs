using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    [SerializeField] private float _displayTime = 4.0f;
    [SerializeField] private GameObject _dialogBox;

    private float _timerDisplay;

    private void Start()
    {
        _dialogBox.SetActive(false);
        _timerDisplay = -1.0f;
    }

    private void Update()
    {
        if (_timerDisplay >= 0)
        {
            _timerDisplay -= Time.deltaTime;
            if (_timerDisplay < 0)
            {
                _dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        _timerDisplay = _displayTime;
        _dialogBox.SetActive(true);
    }
}
