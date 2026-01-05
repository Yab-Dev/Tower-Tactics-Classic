using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFade : MonoBehaviour
{
    public static IEnumerator FadeSpriteColor(SpriteRenderer _sprite, Color _targetColor, float _duration)
    {
        Color startingColor = _sprite.color;

        float elapsedTime = 0.0f;
        while (elapsedTime < _duration)
        {
            _sprite.color = Color.Lerp(startingColor, _targetColor, elapsedTime / _duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _sprite.color = _targetColor;
    }
}
