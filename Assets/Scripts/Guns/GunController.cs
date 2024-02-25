using UnityEngine;

public class GunController : MonoBehaviour
{
    public float upYOffset = 0.2f;
    public float downYOffset = -0.2f;
    public float sideYOffset = -0.1f;

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

        // Calculate the angle difference between player's forward direction and the gun direction
        float angleDifference = Mathf.DeltaAngle(playerTransform.eulerAngles.z, angle);

        int sortingOrder = playerSortingOrder; // Start with player's sorting order

        // Check the angle difference to adjust the sorting order
        if (angleDifference >= -22.5f && angleDifference < 22.5f)
        {
            // Facing right
            sortingOrder += 1;
            transform.localPosition = new Vector3(0, sideYOffset, 0);
        }
        else if (angleDifference >= 22.5f && angleDifference < 67.5f)
        {
            // Facing up and right
            sortingOrder -= 2;
            transform.localPosition = new Vector3(0, upYOffset, 0);
        }
        else if (angleDifference >= 67.5f && angleDifference < 112.5f)
        {
            // Facing up
            sortingOrder -= 3;
            transform.localPosition = new Vector3(0, upYOffset, 0);
        }
        else if (angleDifference >= 112.5f && angleDifference < 157.5f)
        {
            // Facing up and left
            sortingOrder -= 4;
            transform.localPosition = new Vector3(0, upYOffset, 0);
        }
        else if (angleDifference >= -157.5f && angleDifference < -112.5f)
        {
            // Facing down and left
            sortingOrder += 4;
            transform.localPosition = new Vector3(0, downYOffset, 0);
        }
        else if (angleDifference >= -112.5f && angleDifference < -67.5f)
        {
            // Facing down
            sortingOrder += 3;
            transform.localPosition = new Vector3(0, downYOffset, 0);
        }
        else if (angleDifference >= -67.5f && angleDifference < -22.5f)
        {
            // Facing down and right
            sortingOrder += 2;
            transform.localPosition = new Vector3(0, downYOffset, 0);
        }
        else
        {
            // Facing left
            sortingOrder += 1;
            transform.localPosition = new Vector3(0, sideYOffset, 0);
        }

        // Apply the adjusted sorting order to the gun sprite renderer
        gunSpriteRenderer.sortingOrder = sortingOrder;
    }
}