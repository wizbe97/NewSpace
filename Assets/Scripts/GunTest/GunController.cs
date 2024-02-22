using UnityEngine;

public class GunController : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer gunSpriteRenderer;
    private int playerSortingOrder;

    void Start()
    {
        // Assuming the player GameObject is the parent of the gun GameObject
        playerTransform = transform.parent;
        gunSpriteRenderer = GetComponent<SpriteRenderer>();

        // Get the initial sorting order of the player
        playerSortingOrder = playerTransform.GetComponent<SpriteRenderer>().sortingOrder;
    }

    void Update()
    {
        RotateGunTowardsMouse();
        AdjustSortingOrder();
    }

    void RotateGunTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the gun
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Flip the gun sprite if angle is between 90 and -90 degrees
        if (angle > 90 || angle < -90)
        {
            gunSpriteRenderer.flipY = true;
        }
        else
        {
            gunSpriteRenderer.flipY = false;
        }
    }

    void AdjustSortingOrder()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        int sortingOrder = playerSortingOrder; // Start with player's sorting order

        // Adjust sorting order based on angle
        switch (Mathf.RoundToInt(angle / 45f))
        {
            case 1: // 45 degrees
            case 2: // 90 degrees
            case 3: // 135 degrees
                sortingOrder -= 1; // Decrease sorting order
                break;
            default:
                sortingOrder += 1; // Increase sorting order
                break;
        }

        // Apply the adjusted sorting order to the gun sprite renderer
        gunSpriteRenderer.sortingOrder = sortingOrder;
    }
}
