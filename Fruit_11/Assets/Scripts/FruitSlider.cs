using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitSlider : MonoBehaviour
{
    public Target[] fruits;

    public Target coin;

    private Target nextFruit;

    public static FruitSlider instance;

    public GameObject spawner;

    private int point = 0;
    private float adjustment;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //Start the slider
        StartSlider();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Select the next fruit for the queue and returns it
    public Target SelectFruit()
    {
        int selection = Random.Range(0, fruits.Length);
        return fruits[selection];
    }

    IEnumerator SpawnAndWait()
    {
        //Infinite fruit spawn
        while (MainMenu.instance.sliderRunning)
        {
            if (MainMenu.instance.creditsEnabled == false)
            {
                //Spawn a random fruit
                nextFruit = SelectFruit();

                //Give it speed
                nextFruit.mainMenu = true;
                nextFruit.menuSpeed = 2f;

                //Change the adjustment based on the point
                adjustment = adjustmentCalculator(point);

                //Instantiate it at the spawner
                Instantiate(nextFruit).transform.position = new Vector2 (spawner.transform.position.x, spawner.transform.position.y + adjustment);

                //Advance the point; bring it back to 0 if it goes over 7
                point++;
                if(point >= 8)
                {
                    point = 0;
                }

                //Wait
                yield return new WaitForSeconds(1);
            }
            else
            {
                //Give coins speed
                coin.mainMenu = true;
                coin.menuSpeed = 2f;

                //Change the adjustment based on the point
                adjustment = adjustmentCalculator(point);

                //Instantiate it at the spawner
                Instantiate(coin).transform.position = new Vector2(spawner.transform.position.x, spawner.transform.position.y + adjustment);

                //Move the point
                point++;
                if (point >= 8)
                {
                    point = 0;
                }

                //Wait
                yield return new WaitForSeconds(1);
            }
        }
    }

    public void StartSlider()
    {
        //I did this so I could start up from other scripts
        StartCoroutine(SpawnAndWait());
    }

    public float adjustmentCalculator(int point)
    {
        //Sin wave motion
        //  9 points
        //  0.5 peak/trough
        //  0.25 mid
        //  0 on the line
         
        if (point == 0)
        {
            //adjustment = -0.5f;
            adjustment = 0.7f * Mathf.Sin((3 * Mathf.PI) / 2);
        }
        else if (point == 1 || point == 7)
        {
            //adjustment = -0.25f;
            adjustment = 0.7f * Mathf.Sin((7 * Mathf.PI) / 4);
        }
        else if (point == 2 || point == 6)
        {
            adjustment = 0f;
        }
        else if (point == 3 || point == 5)
        {
            //adjustment = 0.25f;
            adjustment = 0.7f * Mathf.Sin(Mathf.PI / 4);
        }
        else if (point == 4)
        {
            //adjustment = 0.5f;
            adjustment = 0.7f * Mathf.Sin(Mathf.PI / 2);
        }

        return adjustment;
    }
}
