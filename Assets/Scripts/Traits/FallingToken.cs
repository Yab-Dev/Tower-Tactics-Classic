using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FallingToken : MonoBehaviour, IPointerDownHandler
{
    [Header("Attributes")]
    [SerializeField] private float lifetime;
    [SerializeField] private int tokenValue;
    [SerializeField] private float fallingSpeed;



    private void Update()
    {
        lifetime -= Time.deltaTime;

        transform.position -= new Vector3(0, fallingSpeed * Time.deltaTime);

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShopManager.GetInstance().AddTowerTokens(tokenValue);
        Destroy(gameObject);
    }
}
