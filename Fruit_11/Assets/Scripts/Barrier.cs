using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{

    public Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Animation event will trigger this function, indicating that the animation is over and destruction can begin
    public void AlertObservers(string message)
    {
        //The item has been shot and once the animation ends, it will send this message
        if (message.Equals("DestroyAnimationEnded"))
        {
            //Destroy the object
            Destroy(gameObject);
        }
    }

    //Function (called from another script) that begins the barrier's destruction animation
    public void DestroyBarrier()
    {
        myAnim.SetBool("isDestroyed", true);
    }
}
