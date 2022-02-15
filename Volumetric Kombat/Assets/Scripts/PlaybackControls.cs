using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoarSDK;
using UnityEngine.UI;
using System;

public class PlaybackControls : MonoBehaviour
{

    public VolumetricRender playbackComponent;
    public Slider scrubbingSlider;
    public Text timeIndicator;
    private bool playbackPaused = false;
    private bool getSliderHandle;
    private int volumetricIndex;
    private float timeInSeconds = 0.0f;
    private float lastSliderVal = 0;

    // Start is called before the first frame update
    private void Update()
    {
        if (!playbackPaused)
        {
            if (playbackComponent != null)
            {
                scrubbingSlider.maxValue = playbackComponent.GetFullDuration(0);

                if (!getSliderHandle)
                {
                    scrubbingSlider.value = playbackComponent.GetCurrentPosition(0);

                    if (lastSliderVal < scrubbingSlider.value)
                    {
                        timeInSeconds += Time.deltaTime;
                    }
                    else
                    {
                        timeInSeconds = 0.0f;
                    }
                }
                else
                {
                    Debug.Log("Scrubbing");
                }

                timeIndicator.text = Math.Round(timeInSeconds, 4).ToString() + "s";
            }
        }
    }

    public void SeekToTimestamp()
    {
        getSliderHandle = false;
        playbackComponent.SeekToCursor(volumetricIndex, (int)scrubbingSlider.value);
        PlaybackStart();
    }

    public void GetSliderHandle()
    {
        getSliderHandle = true;
        PlaybackStop();
    }

    public void PlaybackStart()
    {
        playbackComponent.StartPlayback(volumetricIndex);

        playbackPaused = false;
    }

    public void PlaybackPause()
    {
        playbackComponent.PauseModel(volumetricIndex);

        playbackPaused = true;
    }

    public void PlaybackStop()
    {
        playbackComponent.StopModel(volumetricIndex);
    }
}
