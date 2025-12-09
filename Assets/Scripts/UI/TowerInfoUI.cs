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
    


    public void SetTowerInfo(TowerData towerData)
    {
        nameText.text = towerData.name;
        towerCostText.text = towerData.cost.ToString();

        foreach (Transform child in traitTextContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < towerData.traits.Count; i++)
        {
            GameObject textObject = Instantiate(traitTextObject, traitTextContent);
            TMPro.TMP_Text traitText = textObject.GetComponent<TMPro.TMP_Text>();
            traitText.text = towerData.traits[i].name;
        }
    }
}
