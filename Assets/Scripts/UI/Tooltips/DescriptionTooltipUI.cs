using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionTooltipUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text descriptionText;


    public void DisplayDescription(string _description)
    {
        descriptionText.text = _description;
    }
}
