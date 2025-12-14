using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitPanelUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private Transform traitIconsContent;

    [Header("Prefabs")]
    [SerializeField] private GameObject traitIconObject;



    private void Awake()
    {
        GameManager.OnCurrentTowersUpdated += DisplayTraitIcons;
    }

    private void OnDisable()
    {
        GameManager.OnCurrentTowersUpdated -= DisplayTraitIcons;
    }

    private void DisplayTraitIcons(List<GameObject> towers, List<(TraitData trait, int count)> traits)
    {
        foreach (Transform child in traitIconsContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < traits.Count; i++)
        {
            TraitIconUI traitIcon = Instantiate(traitIconObject, traitIconsContent).GetComponent<TraitIconUI>();
            traitIcon.SetTraitData(traits[i].trait, traits[i].count);
        }
    }
}
