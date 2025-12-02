using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitIconUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TraitData currentTrait;
    [SerializeField] private int traitCount;

    [Header("Cache")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image traitImage;
    [SerializeField] private TMPro.TMP_Text traitCountText;



    public void SetTraitData(TraitData trait, int count)
    {
        currentTrait = trait;
        traitCount = count;

        DisplayTraitData();
    }

    private void DisplayTraitData()
    {
        traitImage.sprite = currentTrait.traitIcon;
        traitCountText.text = traitCount.ToString();
    }
}
