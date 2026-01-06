using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private Transform traitTextContent;
    [SerializeField] private TMPro.TMP_Text towerCostText;
    [SerializeField] private Image towerImage;

    [Header("Prefabs")]
    [SerializeField] private GameObject traitTextObject;

    private TowerData towerData;


    public void SetTowerInfo(TowerData _towerData)
    {
        towerData = _towerData;

        nameText.text = _towerData.name;
        towerCostText.text = _towerData.cost.ToString();

        foreach (Transform child in traitTextContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _towerData.traits.Count; i++)
        {
            GameObject textObject = Instantiate(traitTextObject, traitTextContent);
            TMPro.TMP_Text traitText = textObject.GetComponent<TMPro.TMP_Text>();
            traitText.text = _towerData.traits[i].name;
        }

        towerImage.sprite = _towerData.sprite;
        towerImage.SetNativeSize();
    }

    public TowerData TowerData
    {
        get { return towerData; }
        private set { towerData = value; }
    }
}
