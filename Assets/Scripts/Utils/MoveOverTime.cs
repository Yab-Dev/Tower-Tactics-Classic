using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MoveOverTime : MonoBehaviour
{
    public static IEnumerator MoveUIObjectOverTime(RectTransform _targetTransform, Vector2 _endPosition, float _duration)
    {
        Vector2 startingPosition = _targetTransform.anchoredPosition;

        float elapsedTime = 0.0f;
        while (elapsedTime < _duration)
        {
            _targetTransform.anchoredPosition = Vector2.Lerp(startingPosition, _endPosition, elapsedTime / _duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _targetTransform.anchoredPosition = _endPosition;
    }
}
