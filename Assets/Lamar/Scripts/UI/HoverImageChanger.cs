using UnityEngine;

public class HoverImageChanger : MonoBehaviour
{
    // did this logic for my game Cosmic Thread game: https://github.com/lamarjambi/cosmic-thread
    [Header("Hover Settings")]
    [SerializeField] private Sprite hoverSprite;
    private SpriteRenderer spriteRenderer;
    private Sprite normalSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = spriteRenderer.sprite;
    }

    void OnMouseEnter()
    {
        if (hoverSprite != null)
            spriteRenderer.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = normalSprite;
    }
}