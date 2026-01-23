using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class SteamValve : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float fullMoveDuration;
    [SerializeField] private float startDegrees;
    [SerializeField] private float endDegrees;
    [SerializeField] private float yellowRangeDegrees;
    [SerializeField] private float redRangeDegrees;
    [SerializeField] private Color flashingColor;
    [SerializeField] private float flashingDuration;
    [SerializeField] private float overheatAttackSpeedDebuff;
    [SerializeField] private int overheatDamageDebuff;

    [Header("Cache")]
    [SerializeField] private Image valveImage;
    [SerializeField] private RectTransform needleTransform;
    [SerializeField] private Button releaseButton;
    [SerializeField] private TraitData steamTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject steamPushGoodPrefab;
    [SerializeField] private GameObject steamPushBadPrefab;

    private bool isRotating = true;
    private float rotatedAmount;
    private bool flashingRed;



    private void Awake()
    {
        releaseButton.onClick.AddListener(Release);
        GameManager.OnDefensePhaseEnd += SelfDestruct;
    }

    private void OnDisable()
    {
        releaseButton.onClick.RemoveAllListeners();
        GameManager.OnDefensePhaseEnd -= SelfDestruct;
    }

    private void Start()
    {
        needleTransform.rotation = Quaternion.Euler(0, 0, startDegrees);
        rotatedAmount = 0;

        StartCoroutine(RedFlashing());
    }

    private void Update()
    {
        if (isRotating)
        {
            float totalDist = Mathf.Abs(startDegrees - endDegrees);
            float moveAmount = totalDist * Time.deltaTime / fullMoveDuration;
            needleTransform.Rotate(new Vector3(0, 0, -moveAmount));
            rotatedAmount += moveAmount;

            flashingRed = rotatedAmount > Mathf.Abs(startDegrees - redRangeDegrees);

            if (rotatedAmount > totalDist)
            {
                isRotating = false;
                releaseButton.gameObject.SetActive(false);

                List<TowerBehavior> steamTowers = GameManager.GetInstance().GetTowersOfTrait(steamTrait);
                foreach (TowerBehavior tower in steamTowers)
                {
                    tower.AddStatModification(_hitSpeed: overheatAttackSpeedDebuff, _damage: overheatDamageDebuff);
                }
            }
        }
    }

    private IEnumerator RedFlashing()
    {
        while (this != null)
        {
            yield return new WaitUntil(() => flashingRed);

            Color baseColor = valveImage.color;

            StartCoroutine(ColorFade.FadeSpriteColor(valveImage, flashingColor, flashingDuration));
            yield return new WaitForSeconds(flashingDuration);

            StartCoroutine(ColorFade.FadeSpriteColor(valveImage, baseColor, flashingDuration));
            yield return new WaitForSeconds(flashingDuration);
        }
    }

    private void Release()
    {
        if (rotatedAmount > Mathf.Abs(startDegrees - redRangeDegrees))
        {
            Instantiate(steamPushBadPrefab);
        }
        else if (rotatedAmount > Mathf.Abs(startDegrees - yellowRangeDegrees))
        {
            Instantiate(steamPushGoodPrefab);
        }

        needleTransform.rotation = Quaternion.Euler(0, 0, startDegrees);
        rotatedAmount = 0;
    }

    private void SelfDestruct(int _waveCount)
    {
        Destroy(gameObject);
    }
}
