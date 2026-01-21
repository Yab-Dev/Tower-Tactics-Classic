using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerPanelUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private string towerCapOverMessage;
    [SerializeField] private Color towerCapOverColor;

    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text phaseText;
    [SerializeField] private Button startWaveButton;

    [Header("Prefabs")]
    [SerializeField] private GameObject textPopupPrefab;

    private Vector2 fadeOutPosition;
    private Vector2 fadeInPosition;

    private RectTransform rectTransform;
    private bool canStartWave;


    private void Awake()
    {
        GameManager.OnBuildPhaseStart += BuildPhaseStart;
        GameManager.OnBuildPhaseEnd += FadePanelOut;
        TowerDragDrop.OnTowerPlaceStart += DisableButtons;
        TowerDragDrop.OnTowerPlaceEnd += EnableButtons;
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
        TowerDragDrop.OnTowerPlaceEnd -= EnableButtons;
        GameManager.OnCurrentTowersUpdated -= TowersUpdated;

        startWaveButton.onClick.RemoveAllListeners();
    }

    private void StartWave()
    {
        if (canStartWave)
        {
            startWaveButton.interactable = false;

            GameManager.GetInstance().SetDefensePhase();
        }
        else
        {
            TextPopupUI.CreateTextPopup(textPopupPrefab, towerCapOverMessage, towerCapOverColor);
        }
    }

    private void BuildPhaseStart(int _waveCount)
    {
        startWaveButton.interactable = true;

        FadePanelIn();
    }

    private void FadePanelIn()
    {
        StartCoroutine(MoveOverTime.MoveUIObjectOverTime(rectTransform, fadeInPosition, fadeDuration));
    }

    private void FadePanelOut(int _waveCount)
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
            //startWaveButton.interactable = false;
            canStartWave = false;
        }
        else
        {
            //startWaveButton.interactable = true;
            canStartWave = true;
        }
    }
}
