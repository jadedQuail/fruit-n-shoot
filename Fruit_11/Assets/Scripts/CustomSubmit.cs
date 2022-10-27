using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSubmit : MonoBehaviour
{
    // Entry field and a boolean to detect if the submit button has been pressed
    public InputField field;
    private bool buttonPressed;

    public static CustomSubmit instance;

    // Start is called before the first frame update
    void Start()
    {
        //Limit the characters for the input field to 8
        field.characterLimit = 8;

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // When the button is pressed, submit the text and reset the button
        if(buttonPressed)
        {
            Submit(field.text);
            buttonPressed = false;

            //Open the submitted menu
            SubmittedMenu.instance.OpenMenu();
        }

    }

    public void PressSubmit()
    {
        // This function is called by the "Submit" button
        if (field.text.Length > 0)
        {
            buttonPressed = true;
        }
        else
        {
            // Activate the reminder text
            EntryMenu.instance.reminderText.gameObject.SetActive(true);
        }
    }

    private void Submit(string text)
    {
        // Submit a new score to the scoreboard
        Master.instance.topScores.Add(new Score(text, VictoryMenu.instance.scoreTotal));

        // Clear the field
        field.text = "";

        // Save the new scores
        Master.instance.ListToPrefs();
    }
}
