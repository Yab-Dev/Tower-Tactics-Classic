using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningIcon : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private LanePosition lanePosition;
    [SerializeField] private Color fadeInColor;
    [SerializeField] private Color fadeOutColor;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D boxCollider;



    private void Awake()
    {
        GameManager.OnBuildPhaseStart += FadeIconIn;
        GameManager.OnBuildPhaseEnd += FadeIconOut;
    }

    private void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= FadeIconIn;
        GameManager.OnBuildPhaseEnd -= FadeIconOut;
    }

    private void Start()
    {
        StartCoroutine(ColorFade.FadeSpriteColor(sprite, fadeOutColor, 0.0f));
    }

    private void FadeIconIn(int _waveCount)
    {
        if (this == null) { return; }

        LevelWaves.WaveData waveData = WaveManager.GetInstance().GetWaveData(_waveCount);
        if
        (
            (waveData.topLaneHazard && lanePosition == LanePosition.Top) ||
            (waveData.midLaneHazard && lanePosition == LanePosition.Middle) ||
            (waveData.botLaneHazard && lanePosition == LanePosition.Bottom)
        )
        {
            boxCollider.enabled = true;
            StartCoroutine(ColorFade.FadeSpriteColor(sprite, fadeInColor, 0.2f));
        }
        else
        {
            FadeIconOut(_waveCount);
        }
    }

    private void FadeIconOut(int _waveCount)
    {
        if (this == null) { return; }

        boxCollider.enabled = false;
        StartCoroutine(ColorFade.FadeSpriteColor(sprite, fadeOutColor, 0.2f));
    }

    public enum LanePosition { Top, Middle, Bottom }
}
