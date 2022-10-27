using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//**********************************************************************************************************************************//
//If the camera is not respecting the proper bounds, it's probably because it's looking at deleted tiles and still registering them.//
//To fix this, click on the Tilemap, click the tiny gear icon, and select "Compress Tilemap Bounds"                                 //
//                                                                                                                                  //
//Rotate tiles by using "[" or "]"                                                                                                  //
//                                                                                                                                  //
//**********************************************************************************************************************************//

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Tilemap theMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfHeight;
    private float halfWidth;

    public bool unmovingCamera = false;

    // Start is called before the first frame update
    void Start()
    {
        //Set the target (the thing the camera follows) to be the player
        target = PlayerController.instance.transform;

        //Set the half height and half width of the camera
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        //Find the Tilemap's corners (accounting for the half height and width)
        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
        topRightLimit = theMap.localBounds.max - new Vector3(halfWidth, halfHeight, 0);

        //Set the bounds that the player can walk in
        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    // LateUpdate is called once per frame after update
    void LateUpdate()
    {
        if (!unmovingCamera) //If this bool is unchecked, the camera follows the player; if it is checked, the camera stays it one place.
        {
            if (target != null)
            {
                //Set the camera's position to be that of the target's
                transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

                //Keep the camera inside the bounds of the map
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
                                                 Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
                                                 transform.position.z);
            }
        }
    }
}
