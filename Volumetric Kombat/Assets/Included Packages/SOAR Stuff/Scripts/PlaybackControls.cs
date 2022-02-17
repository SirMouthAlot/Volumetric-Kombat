using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoarSDK;
using UnityEngine.UI;

public class PlaybackControls : MonoBehaviour
{

    public VolumetricRender playbackComponent;
    public Slider scrubbingSlider;
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
