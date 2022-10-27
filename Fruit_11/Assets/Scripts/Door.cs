using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator myAnim;

    public GameObject portal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If there's no targerts left, begin the destruction animation.
        if(GameManager.instance.currentSceneTargets <= 0)
        {
            myAnim.SetBool("isDestroyed", true);
        }
    }

    public void AlertObservers(string message)
    {
        //The item has been shot and once the animation ends, it will send this message
        if (message.Equals("DestroyAnimationEnded"))
        {
            //Destroy the object
            Destroy(gameObject);

            //Activate the portal
            portal.SetActive(true);
        }
    }
}
