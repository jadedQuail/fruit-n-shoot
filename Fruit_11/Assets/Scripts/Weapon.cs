using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject arrowPrefab;

    public Vector3 mousePosition;
    public Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Stop the player from shooting if he's out of arrows
        if (GameManager.instance != null)
        {
            if (GameManager.instance.arrowCount <= 0)
            {
                PlayerController.instance.canShoot = false;
            }
        }

        //If the player shoots and the menu is not active, then they fire.
        //if (Input.GetMouseButtonDown(0) && PlayerController.instance.canShoot)
        if (Input.GetButtonDown("Fire1") && PlayerController.instance.canShoot)
        {
            Shoot();

            //Arrow count decreases
            GameManager.instance.arrowCount -= 1;

            //The shenangians with spacebar randomly loading scenes was solved by setting
            //every button's Navigation from "Automatic" to "None" (settings for each button)
        }

    }

    //Instantiates the arrow
    void Shoot()
    {
        //Fire
        Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        //Play the sound effect (Fire)
        AudioManager.instance.PlaySFX(2);

    }
}
