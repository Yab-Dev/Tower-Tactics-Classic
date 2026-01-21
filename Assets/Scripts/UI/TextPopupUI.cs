using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopupUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text colorText;
    [SerializeField] private TMPro.TMP_Text shadowText;

    private float lifetimeTimer = 1.0f;



    public static void CreateTextPopup(GameObject _popupObject, string _message, Color _color)
    {
        GameObject popupObject = Instantiate(_popupObject, GameObject.FindWithTag("MainCanvas").transform);
        TextPopupUI popupData = popupObject.GetComponent<TextPopupUI>();
        if (popupData != null)
        {
            popupData.colorText.text = _message;
            popupData.shadowText.text = _message;

            popupData.colorText.color = _color;
        }
    }

    private void Update()
    {
        if (lifetimeTimer <= 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            lifetimeTimer -= Time.deltaTime;
        }
    }
}
