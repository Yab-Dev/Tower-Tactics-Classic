using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenCasino : MonoBehaviour
{
    [System.Serializable]
    private class CasinoResultData
    {
        public string name;
        public Sprite icon;
        public CasinoResult resultType;
        public float randomScalar;
    }

    [Header("Attributes")]
    [SerializeField] private float spinCooldown;
    [SerializeField] private List<CasinoResultData> resultData = new List<CasinoResultData>();
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinSlotOffset;
    [SerializeField] private float spinLingerDuration;
    [SerializeField] private float postSpinDuration;
    [SerializeField] private float tokenMultAmount;
    [SerializeField] private int towerAttackBuff;
    [SerializeField] private float enemyPushAmount;
    [SerializeField] private float enemyPushDuration;
    [SerializeField] private float spinCooldownBonusScalar;

    [Header("Cache")]
    [SerializeField] private Image resultImage;
    [SerializeField] private TMPro.TMP_Text resultText;
    [SerializeField] private List<Image> slotImages = new List<Image>();
    [SerializeField] private TowerData icyPillarData;

    [Header("Prefabs")]
    [SerializeField] private GameObject tokenRainSpawner;

    private List<GameObject> objectsToDestroy = new List<GameObject>();
    private float spinTimer;
    private bool isSpinning;

    private RectTransform rectTransform;


    private void Awake()
    {
        GameManager.OnDefensePhaseEnd += SelfDestruct;
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseEnd -= SelfDestruct;
    }

    private void Start()
    {
        spinTimer = spinCooldown;
    }

    private void Update()
    {
        if (spinTimer <= 0.0f && !isSpinning)
        {
            StartCoroutine(SpinCasino());
            isSpinning = true;
        }
        else
        {
            spinTimer -= Time.deltaTime;
        }
    }

    private void DisplayCasinoResult(CasinoResultData _data)
    {
        if (_data != null)
        {
            resultText.text = $": {_data.name}";
            resultImage.enabled = true;
            resultImage.sprite = _data.icon;
        }
        else
        {
            resultText.text = "";
            resultImage.enabled = false;
        }
    }

    private void SetSlotImage(int _slotIndex, CasinoResultData _data)
    {
        slotImages[_slotIndex].sprite = _data.icon;
    }

    private CasinoResultData RollResult()
    {
        CasinoResultData result = resultData[Random.Range(0, resultData.Count)];
        float randomCheck = Random.Range(0.0f, 1.0f);
        if (randomCheck <= result.randomScalar || result.randomScalar == 0)
        {
            return result;
        }
        else
        {
            return RollResult();
        }
    }

    private IEnumerator SpinCasino()
    {
        DisplayCasinoResult(null);
        CasinoResultData result = RollResult();
        StartCoroutine(MoveOverTime.MoveUIObjectOverTime(rectTransform, new Vector2(0, 0), 0.5f));

        float timeElapsed = 0.0f;
        while (timeElapsed < spinDuration)
        {
            SetSlotImage(0, resultData[Random.Range(0, resultData.Count)]);
            SetSlotImage(1, resultData[Random.Range(0, resultData.Count)]);
            SetSlotImage(2, resultData[Random.Range(0, resultData.Count)]);

            yield return new WaitForSeconds(spinLingerDuration);
            timeElapsed += spinLingerDuration;
        }
        SetSlotImage(0, result);

        timeElapsed = 0.0f;
        while (timeElapsed < spinSlotOffset)
        {
            SetSlotImage(1, resultData[Random.Range(0, resultData.Count)]);
            SetSlotImage(2, resultData[Random.Range(0, resultData.Count)]);

            yield return new WaitForSeconds(spinLingerDuration);
            timeElapsed += spinLingerDuration;
        }
        SetSlotImage(1, result);

        timeElapsed = 0.0f;
        while (timeElapsed < spinSlotOffset)
        {
            SetSlotImage(2, resultData[Random.Range(0, resultData.Count)]);

            yield return new WaitForSeconds(spinLingerDuration);
            timeElapsed += spinLingerDuration;
        }
        SetSlotImage(2, result);

        DisplayCasinoResult(result);

        yield return new WaitForSeconds(postSpinDuration);

        CompleteResult(result);
        StartCoroutine(MoveOverTime.MoveUIObjectOverTime(rectTransform, new Vector2(0, -700), 0.5f));

        spinTimer = spinCooldown;
        isSpinning = false;
    }

    private void CompleteResult(CasinoResultData _result)
    {
        switch (_result.resultType)
        {
            case CasinoResult.TokenRain:
                Instantiate(tokenRainSpawner);
                break;
            case CasinoResult.TokenMult:
                ShopManager.GetInstance().TowerTokens = Mathf.RoundToInt(ShopManager.GetInstance().TowerTokens * tokenMultAmount);
                break;
            case CasinoResult.TowerBuff:
                List<TowerBehavior> towersToBuff = GameManager.GetInstance().GetTowersOfTrait(null);
                foreach (TowerBehavior tower in towersToBuff)
                {
                    tower.AddStatModification(_damage: towerAttackBuff);
                }
                break;
            case CasinoResult.EnemyPush:
                List<EnemyBehavior> enemies = WaveManager.GetInstance().GetEnemies();
                foreach (EnemyBehavior enemy in enemies)
                {
                    enemy.Knockback(enemyPushDuration, enemyPushAmount, false);
                }
                break;
            case CasinoResult.IcyPillar:
                objectsToDestroy.Add(GameManager.GetInstance().SpawnTowerOnSlot(icyPillarData));
                GameManager.GetInstance().UpdateCurrentTowers();
                break;
            case CasinoResult.TowerExp:
                List<TowerBehavior> towersToLevel = GameManager.GetInstance().GetTowersOfTrait(null);
                towersToLevel[Random.Range(0, towersToLevel.Count)].AddExp(1);
                break;
            case CasinoResult.BonusMode:
                spinCooldown *= spinCooldownBonusScalar;
                break;
        }
    }

    private void SelfDestruct(int _waveCount)
    {
        foreach (GameObject gameObject in objectsToDestroy)
        {
            Destroy(gameObject);
        }
        objectsToDestroy.Clear();
        GameManager.GetInstance().UpdateCurrentTowers();

        Destroy(gameObject);
    }

    public enum CasinoResult { TokenRain, TokenMult, TowerBuff, EnemyPush, IcyPillar, TowerExp, BonusMode }
}
