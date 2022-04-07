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
    private bool getSliderHandle;
    private int volumetricIndex;
    public string newClipFileName;
    public Text CurrentTime;
    public Text totalTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (CurrentTime != null)
        {
            CurrentTime.text = (Math.Round((double)(playbackComponent.GetCurrentPosition(0) / 1000000.0), 3)).ToString() + "s";
        }

        if (totalTime != null)
        {
            totalTime.text = (Math.Round((double)(playbackComponent.GetFullDuration(0) / 1000000.0), 3)).ToString() + "s";
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                playbackComponent = hit.transform.GetChild(0).GetComponent<VolumetricRender>();
                volumetricIndex = playbackComponent.instanceRef.IndexOf(hit.transform.GetChild(0).GetComponent<PlaybackInstance>());
            }
        }

        if(playbackComponent != null)
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
    }

    public void PlaybackPause()
    {
        playbackComponent.PauseModel(volumetricIndex);
    }

    public void PlaybackStop()
    {
        playbackComponent.StopModel(volumetricIndex);
    }

    public void LoadNewClip()
    {
        playbackComponent.LoadNewClip(newClipFileName, volumetricIndex);
    }
}
