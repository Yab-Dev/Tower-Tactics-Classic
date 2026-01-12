using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowInteract : MonoBehaviour, IPointerDownHandler
{
    private readonly string AnimatorRingSpeedParameter = "CloseSpeed";
    private readonly string AnimatorRingCloseParameter = "StartClose";

    [Header("Attributes")]
    [SerializeField] private float lifetime;
    [SerializeField] private ArrowMode arrowMode;
    [SerializeField] private float arrowSpawnX;
    [SerializeField] private int arrowDamage;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private int arrowCount;
    [SerializeField] private float arrowDelay;
    [SerializeField] private float fadeOutDuration;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer baseSprite;
    [SerializeField] private SpriteRenderer ringSprite;
    [SerializeField] private Animator ringAnimator;

    [Header("Prefabs")]
    [SerializeField] private GameObject arrowBullet;

    private bool hasClicked = false;
    private bool lifetimeOver = false;



    private void Start()
    {
        ringAnimator.SetFloat(AnimatorRingSpeedParameter, 1/lifetime);
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

        switch (arrowMode)
        {
            case ArrowMode.Bullet:
                StartCoroutine(BulletArrowVolley());
                break;
            case ArrowMode.Target:

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

    private IEnumerator BulletArrowVolley()
    {
        for (int i = 0; i < arrowCount; i++)
        {
            BulletBehavior.CreateBullet(arrowBullet, new Vector2(arrowSpawnX, transform.position.y + Random.Range(-0.5f, 0.5f)), null, IDamage.Team.Tower, arrowDamage, arrowSpeed);
            yield return new WaitForSeconds(arrowDelay);
        }
        Destroy(gameObject);
    }

    private enum ArrowMode { Bullet, Target }
}
