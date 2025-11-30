using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private IDamage.Team targets;
    [SerializeField] private float laneRange;
    [SerializeField] private float areaRange;
    [SerializeField] private List<GameObject> targetList;

    [Header("Cache")]
    [SerializeField] private BoxCollider2D laneCollider;
    [SerializeField] private CircleCollider2D areaCollider;



    private void Start()
    {
        SetSize(laneRange, areaRange);
    }

    public List<GameObject> GetTargets()
    {
        return targetList;
    }

    private void SetSize(float _laneRange, float _areaRange)
    {
        laneRange = _laneRange;
        areaRange = _areaRange;

        laneCollider.size = new Vector2(laneRange, 1.0f);
        areaCollider.radius = areaRange;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage damageInterface = collision.GetComponent<IDamage>();
        if (damageInterface != null)
        {
            if (damageInterface.GetTeam() != targets) {  return; }

            if (!targetList.Contains(collision.gameObject))
            {
                targetList.Add(collision.gameObject);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (targetList.Contains(collision.gameObject))
        {
            targetList.Remove(collision.gameObject);
        }
    }
}
