using System.Collections;
using UnityEngine;

public class GrassLogic : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool isBig;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        EventHandler.SeasonChange += OnSeasonChange;
    }
    private void OnDisable()
    {
        EventHandler.SeasonChange -= OnSeasonChange;
    }

    private void OnSeasonChange(Season season)
    {
        StartCoroutine(SpriteChenge((int)season));
    }
    IEnumerator SpriteChenge(int num)
    {
        yield return new WaitForSeconds(0.35f);
        spriteRenderer.sprite = isBig ? GrassManager.Instance.bigGarssSprite[num] : GrassManager.Instance.smallGarssSprite[num];
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //AUDIO:草丛音效
        animator.Play("GrassShake");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //AUDIO:草丛音效
        animator.Play("GrassShake");
    }
}
