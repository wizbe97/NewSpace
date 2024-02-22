using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    public Sprite[] sprites; // Array to store the sprites for different directions

    private SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdatePlayerSprite();
    }

    void UpdatePlayerSprite()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Normalize angle to range [0, 360)
        if (angle < 0)
        {
            angle += 360;
        }

        // Calculate index of the sprite array based on the angle
        int spriteIndex = Mathf.RoundToInt(angle / 45f) % 8;

        // Set the sprite based on the calculated index
        if (sprites.Length > spriteIndex && spriteIndex >= 0)
        {
            playerSpriteRenderer.sprite = sprites[spriteIndex];
        }
    }
}
