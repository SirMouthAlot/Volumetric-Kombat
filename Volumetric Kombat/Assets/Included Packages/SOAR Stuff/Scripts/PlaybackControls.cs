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
    public Text _timeIndicator;
    private bool playbackPaused = false;
    private bool getSliderHandle;
    private int volumetricIndex;

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
                    float time = (playbackComponent.GetCurrentPosition(0) / 1000000.0f);

                    _timeIndicator.text = Math.Round(time, 2).ToString() + "s";
                }
                else
                {
                    Debug.Log("Scrubbing");
                }
            }
        }
    }

    public void SeekToTimestamp()
    {
        bool paused = playbackComponent.paused;

        getSliderHandle = false;
        playbackComponent.SeekToCursor(volumetricIndex, (int)scrubbingSlider.value);

        if (paused)
        {
            PlaybackPause();
        }
        else
        {
            PlaybackStart();
        }
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
