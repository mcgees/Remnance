using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChoiceManager : MonoBehaviour
{
    // A reference to the UI
    public GameObject DialogueManager;

    // A reference to the actual UI element that will display the text
    public TextMeshProUGUI subtitleDisplay;

    // Represents the speed at which dialogue is "printed" out
    public float typingSpeed;

    // A boolean representing if the UI is visible or not
    public bool isVisible;

    // A reference to the Dialogue Handler
    public GameObject dialogueHandler;

    // A reference to the Game Display Textbox
    public Text gameDisplayText;

    // Represents how far into the dialogue tree a player is.
    private int progression;

    // Initializes our game timer - used to track dialogue readouts
    private float timer = 0.0f;

    // Represents the amount of time between subtitle readouts
    private static float timeGap = 2.0f;

    // If the UI is interactable
    bool interactableUI = false;

    // Varaibles used in tandem with DialogueAssets to print out the UI
    bool tutorialSubtitleOne = true;
    bool firstTime = true;
    bool skipAudioWait = false;

    bool tutorialGameDisplayOne = false;
    bool tutorialC1aSubtitles = false;
    bool c1aTree = false;
    bool tutorialC1aTreeSubtitles = false;

    bool movementFlag = false;

    private int counter = 0;

    private AudioSource audioSource;
    public AudioClip audioFile;

    /// <summary>
    /// Iterates through the inputted Dialogue Asset and prints it to the screen.
    /// </summary>
    /// <param name="sentencesToDisplay">The subtitle text</param>
    /// <returns></returns>
    void DisplaySubtitles(string[] sentencesToDisplay, int index, AudioClip audioToPlay)
    {
        subtitleDisplay.text = sentencesToDisplay[index];
        audioSource.clip = audioToPlay;
        audioSource.Play();
        progression++;
        timeGap = audioToPlay.length;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Progression to 0.
        progression = 0;

        // Disables the UI when the game launches
        isVisible = false;
        dialogueHandler.SetActive(false);
        gameDisplayText.enabled = false;

        // Set the Dialogue UI to display the first set of possible choices.
        DialogueManager.GetComponent<Dialogue>().UpdateChoices(DialogueAssets.tutorial_c1a_tree);

        // Sets the default text to empty
        subtitleDisplay.text = string.Empty;

        // Initialize an Audio Source
        audioSource = this.gameObject.AddComponent<AudioSource>();

        //Types out the initial message
        DisplaySubtitles(DialogueAssets.tutorial_subtitle_1, 0, DialogueAssets.tutorial_clip_1a);
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (tutorialSubtitleOne && timer > timeGap)
        {
            if (firstTime == true)
            {
                counter = 1;
                DisplaySubtitles(DialogueAssets.tutorial_subtitle_1, counter, DialogueAssets.tutorial_clip_1a);
                firstTime = false;
            }
            else if (counter == 1)
            {
                DisplaySubtitles(DialogueAssets.tutorial_subtitle_1, counter, DialogueAssets.tutorial_clip_1a); //CHANGE THIS TO AUDIO 2!
            }
            else if (counter == 2)
            {
                DisplaySubtitles(DialogueAssets.tutorial_subtitle_1, counter, DialogueAssets.tutorial_clip_1a); //CHANGE THIS TO AUDIO 3!
            }
            else
            {
                DisplaySubtitles(DialogueAssets.tutorial_subtitle_1, counter, DialogueAssets.tutorial_clip_1a); //CHANGE THIS TO AUDIO 4!
            }

            timer = 0;
            counter++;

            if(counter == DialogueAssets.tutorial_subtitle_1.Length)
            {
                tutorialSubtitleOne = false;
                tutorialGameDisplayOne = true;
                counter = 0;
            }
        }

        // Display the next dialogue prompt
        if(tutorialGameDisplayOne && timer > timeGap)
        {
            gameDisplayText.text = "Press 'Q' to bring up your wrist comlink.";
            gameDisplayText.enabled = true;
            interactableUI = true;
            timer = 0;
        }

        // Once a player brings up the UI, change the display message
        if(dialogueHandler.active == true && tutorialC1aSubtitles == false)
        {
            gameDisplayText.text = "Use the ‘up’ and ‘down’ arrow keys to navigate the comlink.";
            tutorialGameDisplayOne = false;
            timer = 0;
            skipAudioWait = true;
            c1aTree = true;
        }

        if (tutorialC1aSubtitles && timer > timeGap || tutorialC1aSubtitles && skipAudioWait)
        {
            gameDisplayText.text = String.Empty;
            gameDisplayText.enabled = false;

            if (counter == 0)
            {
                DisplaySubtitles(DialogueAssets.tutorial_c1a_subtitles, counter, DialogueAssets.tutorial_clip_1a); //CHANGE THIS AUDIO
            }
            else
            {
                DisplaySubtitles(DialogueAssets.tutorial_c1a_subtitles, counter, DialogueAssets.tutorial_clip_1a); //CHANGE THIS TO AUDIO 2!
            }

            timer = 0;
            counter++;

            if (counter == DialogueAssets.tutorial_c1a_subtitles.Length)
            {
                tutorialC1aSubtitles = false;
                tutorialC1aTreeSubtitles = true;
                counter = 0;
                skipAudioWait = false;
            }
        }

        if (tutorialC1aTreeSubtitles && timer > timeGap || tutorialC1aTreeSubtitles && skipAudioWait)
        {
            Debug.Log(counter);
            if (counter == 0)
            {
                DisplaySubtitles(DialogueAssets.tutorial_c1a_tree_2_subtitles, counter, DialogueAssets.tutorial_clip_1a); //CHANGE THIS AUDIO                
            }
            else if (counter == 1)
            {
                DisplaySubtitles(DialogueAssets.tutorial_c1a_tree_2_subtitles, counter, DialogueAssets.tutorial_clip_1a); //CHANGE THIS TO AUDIO 2!                
            }
            else if (counter == 2)
            {
                gameDisplayText.text = "Use ‘W,’ ‘A,’ ‘S,’ and ‘D’ for movement.";
                gameDisplayText.enabled = true;
                counter--;
                skipAudioWait = true;
                if(movementFlag)
                {
                    gameDisplayText.enabled = false;
                    counter++;                    
                }                
            }
            else
            {
                DisplaySubtitles(DialogueAssets.tutorial_c1a_tree_2_subtitles, counter-1, DialogueAssets.tutorial_clip_1a); //CHANGE THIS TO AUDIO 3!
            }

            timer = 0;
            counter++;

            if (counter == DialogueAssets.tutorial_c1a_tree_2_subtitles.Length + 1)
            {
                Debug.Log(counter + " counter number two");
                tutorialC1aTreeSubtitles = false;
                //next thing = true;
                counter = 0;
                dialogueHandler.SetActive(true);
                DialogueManager.GetComponent<Dialogue>().UpdateChoices(DialogueAssets.tutorial_tree_2);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible == false)
        {
            if (Input.GetKeyDown(KeyCode.Q) && interactableUI == true)
            {
                dialogueHandler.SetActive(true);
                isVisible = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (c1aTree == true)
                {
                    if (DialogueManager.GetComponent<Dialogue>().GetSelectedChoiceIndex() == 0) // Check to see if tutorial_c1a_tree is selected.
                    {
                        tutorialC1aSubtitles = true; 
                        c1aTree = false;
                    }
                    else if (DialogueManager.GetComponent<Dialogue>().GetSelectedChoiceIndex() == 1) // Check to see if tutorial_tree_2 is selected.
                    {
                        DialogueManager.GetComponent<Dialogue>().UpdateChoices(DialogueAssets.tutorial_tree_2);
                        c1aTree = false;
                    }
                }
                dialogueHandler.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            movementFlag = true;
        }

            if (progression == 1)
        {
            //Type(DialogueAssets.tutorial_gameDisplay_1);
        }
    }
}