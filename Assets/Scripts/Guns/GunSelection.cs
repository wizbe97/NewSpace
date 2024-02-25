using UnityEngine;

public class GunSelection : MonoBehaviour
{
    private GameObject currentGun;
    [SerializeField] private GameObject shotgun;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        shotgun = Instantiate(shotgun, transform);
        currentGun = shotgun;
        currentGun.SetActive(playerController.isHoldingGun); // Set the active state
    }

    void Update()
    {
        gunVisibility();
    }

    void gunVisibility()
    {
        if (currentGun == shotgun)
        {
            shotgun.SetActive(playerController.isHoldingGun);
        }
    }
}
