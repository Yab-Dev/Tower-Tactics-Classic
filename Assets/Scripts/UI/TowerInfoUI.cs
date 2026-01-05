using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfoUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private Transform traitTextContent;
    [SerializeField] private TMPro.TMP_Text towerCostText;

    [Header("Prefabs")]
    [SerializeField] private GameObject traitTextObject;
    


    public void SetTowerInfo(TowerData _towerData)
    {
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
    }
}
