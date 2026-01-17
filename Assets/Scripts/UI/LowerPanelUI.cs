using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerPanelUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float fadeDuration = 0.2f;

    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text phaseText;
    [SerializeField] private Button startWaveButton;

    private Vector2 fadeOutPosition;
    private Vector2 fadeInPosition;

    private RectTransform rectTransform;


    private void Awake()
    {
        GameManager.OnBuildPhaseStart += BuildPhaseStart;
        GameManager.OnBuildPhaseEnd += FadePanelOut;
        TowerDragDrop.OnTowerPlaceStart += DisableButtons;
        //TowerDragDrop.OnTowerPlaceEnd += EnableButtons;
        GameManager.OnCurrentTowersUpdated += TowersUpdated;

        startWaveButton.onClick.AddListener(StartWave);

        rectTransform = GetComponent<RectTransform>();
        fadeInPosition = rectTransform.anchoredPosition;
        fadeOutPosition = rectTransform.anchoredPosition - new Vector2(0, rectTransform.sizeDelta.y);
    }

    private void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= BuildPhaseStart;
        GameManager.OnBuildPhaseEnd -= FadePanelOut;
        TowerDragDrop.OnTowerPlaceStart -= DisableButtons;
        //TowerDragDrop.OnTowerPlaceEnd -= EnableButtons;
        GameManager.OnCurrentTowersUpdated -= TowersUpdated;

        startWaveButton.onClick.RemoveAllListeners();
    }

    private void StartWave()
    {
        startWaveButton.interactable = false;

        GameManager.GetInstance().SetDefensePhase();
    }

    private void BuildPhaseStart(int waveCount)
    {
        startWaveButton.interactable = true;

        FadePanelIn();
    }

    private void FadePanelIn()
    {
        StartCoroutine(MoveOverTime.MoveUIObjectOverTime(rectTransform, fadeInPosition, fadeDuration));
    }

    private void FadePanelOut(int waveCount)
    {
        StartCoroutine(MoveOverTime.MoveUIObjectOverTime(rectTransform, fadeOutPosition, fadeDuration));
    }

    private void DisableButtons()
    {
        startWaveButton.interactable = false;
    }

    private void EnableButtons()
    {
        startWaveButton.interactable = true;
    }

    private void TowersUpdated(List<GameObject> _towers, int _towersTowardsCapCount, List<(TraitData trait, int count)> _traits, int _towerCap)
    {
        if (_towersTowardsCapCount > _towerCap)
        {
            startWaveButton.interactable = false;
        }
        else
        {
            startWaveButton.interactable = true;
        }
    }
}
