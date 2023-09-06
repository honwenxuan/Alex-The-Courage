using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // AudioSource m_MyAudioSource;
    public AudioSource track1;
    public AudioSource track2;
    public AudioSource track3;
    public AudioSource track4;
    public AudioSource track5;
    public int TrackSelector;
    public int TrackHistory;

    // //Play the music
    // bool m_Play;
    // //Detect when you use the toggle, ensures music isn’t played multiple times
    // bool m_ToggleChange;

    // Start is called before the first frame update
    void Start()
    {
        // Play multiple soundtrack
        TrackSelector = Random.Range(0, 5);

        if (TrackSelector == 0)
        {
            track1.Play();
            TrackHistory = 1;
        }
        else if (TrackSelector == 1)
        {
            track2.Play();
            TrackHistory = 2;
        }
        else if (TrackSelector == 2)
        {
            track3.Play();
            TrackHistory = 3;
        }
        else if (TrackSelector == 3)
        {
            track4.Play();
            TrackHistory = 4;
        }
        else if (TrackSelector == 4)
        {
            track5.Play();
            TrackHistory = 5;
        }



        // //Fetch the AudioSource from the GameObject
        // m_MyAudioSource = GetComponent<AudioSource>();
        // //Ensure the toggle is set to true for the music to play at start-up
        // m_Play = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Play multiple soundtrack
        if (track1.isPlaying == false && track2.isPlaying == false && track3.isPlaying == false && track4.isPlaying == false && track5.isPlaying == false)
        {
            TrackSelector = Random.Range(0, 5);

            if (TrackSelector == 0 && TrackHistory != 1)
            {
                track1.Play();
                TrackHistory = 1;
            }
            else if (TrackSelector == 1 && TrackHistory != 2)
            {
                track2.Play();
                TrackHistory = 2;
            }
            else if (TrackSelector == 2 && TrackHistory != 3)
            {
                track3.Play();
                TrackHistory = 3;
            }
            else if (TrackSelector == 3 && TrackHistory != 4)
            {
                track4.Play();
                TrackHistory = 4;
            }
            else if (TrackSelector == 4 && TrackHistory != 5)
            {
                track5.Play();
                TrackHistory = 5;
            }
        }



        // //Check to see if you just set the toggle to positive
        // if (m_Play == true && m_ToggleChange == true)
        // {
        //     //Play the audio you attach to the AudioSource component
        //     m_MyAudioSource.Play();
        //     //Ensure audio doesn’t play more than once
        //     m_ToggleChange = false;
        // }
        // //Check if you just set the toggle to false
        // if (m_Play == false && m_ToggleChange == true)
        // {
        //     //Stop the audio
        //     m_MyAudioSource.Stop();
        //     //Ensure audio doesn’t play more than once
        //     m_ToggleChange = false;
        // }
    }


    // void OnGUI()
    // {
    //     //Switch this toggle to activate and deactivate the parent GameObject
    //     m_Play = GUI.Toggle(new Rect(10, 10, 100, 30), m_Play, "Play Music");

    //     //Detect if there is a change with the toggle
    //     if (GUI.changed)
    //     {
    //         //Change to true to show that there was just a change in the toggle state
    //         m_ToggleChange = true;
    //     }
    // }
}
