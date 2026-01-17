using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EarthInteract : MonoBehaviour, IPointerDownHandler
{
    private readonly string AnimatorRingSpeedParameter = "CloseSpeed";
    private readonly string AnimatorRingCloseParameter = "StartClose";

    [Header("Attributes")]
    [SerializeField] private float lifetime;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float bulletSpawnX;
    [SerializeField] private EarthMode earthMode;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer baseSprite;
    [SerializeField] private SpriteRenderer ringSprite;
    [SerializeField] private Animator ringAnimator;

    [Header("Prefabs")]
    [SerializeField] private GameObject knockbackProjectile;

    private bool hasClicked = false;
    private bool lifetimeOver = false;



    private void Start()
    {
        ringAnimator.SetFloat(AnimatorRingSpeedParameter, 1 / lifetime);
        ringAnimator.SetTrigger(AnimatorRingCloseParameter);
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f && !lifetimeOver)
        {
            lifetimeOver = true;
            StartCoroutine(FadeOutDestroy());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (hasClicked) { return; }
        hasClicked = true;
        baseSprite.gameObject.SetActive(false);

        switch (earthMode)
        {
            case EarthMode.KnockbackLane:
                BulletBehavior.CreateBullet(knockbackProjectile, new Vector3(bulletSpawnX, transform.position.y), null, IDamage.Team.Tower, 0, 5);
                Destroy(gameObject);
                break;
            case EarthMode.EnemyPush:

                break;
        }
    }

    private IEnumerator FadeOutDestroy()
    {
        ringAnimator.enabled = false;

        StartCoroutine(ColorFade.FadeSpriteColor(baseSprite, new Color(0.0f, 0.0f, 0.0f, 0.0f), fadeOutDuration));
        StartCoroutine(ColorFade.FadeSpriteColor(ringSprite, new Color(0.0f, 0.0f, 0.0f, 0.0f), fadeOutDuration));

        yield return new WaitForSeconds(fadeOutDuration);

        Destroy(gameObject);
    }

    public enum EarthMode { KnockbackLane, EnemyPush }
}
