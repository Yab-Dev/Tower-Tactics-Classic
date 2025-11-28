using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MoveOverTime : MonoBehaviour
{
    public static IEnumerator MoveUIObjectOverTime(RectTransform targetTransform, Vector2 endPosition, float duration)
    {
        Vector2 startingPosition = targetTransform.anchoredPosition;

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            targetTransform.anchoredPosition = Vector2.Lerp(startingPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetTransform.anchoredPosition = endPosition;
    }
}
