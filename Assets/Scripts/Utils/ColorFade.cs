using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFade : MonoBehaviour
{
    public static IEnumerator FadeSpriteColor(SpriteRenderer sprite, Color targetColor, float duration)
    {
        Color startingColor = sprite.color;

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            sprite.color = Color.Lerp(startingColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sprite.color = targetColor;
    }
}
