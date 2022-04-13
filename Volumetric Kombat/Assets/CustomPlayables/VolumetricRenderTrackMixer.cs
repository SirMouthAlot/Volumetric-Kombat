using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using SoarSDK;
using UnityEngine.Timeline;

public class VolumetricTimelineGlobals : MonoBehaviour
{
    public static List<PlaybackInstance> instanceList = new List<PlaybackInstance>();
}

public class VolumetricRenderTrackMixer : PlayableBehaviour
{

    public PlaybackState state;
    public PlaybackInstancePlayState instanceState;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        VolumetricRender volRender = playerData as VolumetricRender;
        if (!volRender.GetComponent<PlaybackInstance>()) { return; }
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0f)
            {
                ScriptPlayable<VolumetricRenderBehavior> inputPlayable = (ScriptPlayable<VolumetricRenderBehavior>)playable.GetInput(i);
                VolumetricRenderBehavior input = inputPlayable.GetBehaviour();
                PlayableGraph playableGraph = playable.GetGraph();
                PlayableDirector director = playable.GetGraph().GetResolver() as PlayableDirector;

                for (int j = 0; j < VolumetricTimelineGlobals.instanceList.Count; j++)
                {
                    state = volRender.GetInstanceState(j);
                    PlaybackInstancePlayState instanceState = volRender.GetComponent<PlaybackInstance>()._playState;
                    switch (state)
                    {
                        case PlaybackState.READY:
                            if(instanceState == PlaybackInstancePlayState.Playing)
                            {
                                Time.timeScale = 1;
                                director.time += (Time.deltaTime / (VolumetricTimelineGlobals.instanceList.Count * VolumetricTimelineGlobals.instanceList.Count));
                            }
                            break;
                        case PlaybackState.BUFFERING:
                            break;
                        default:
                            Time.timeScale = 0;
                            break;
                    }

                }

                if (director.state == PlayState.Paused)
                {

                    if (input.fileName.Length > 0)
                    {
                        if (input.clipLoaded)
                        {
                            if (input.clipPlaying)
                            {
                                var index = volRender.instanceRef.IndexOf(volRender.GetComponent<PlaybackInstance>());
                                volRender.SeekToCursor(index, (int)(inputPlayable.GetTime() * 1000000.0f));
                                input.seeking = true;
                            }

                            if (!input.clipPlaying)
                            {
                                var index = volRender.instanceRef.IndexOf(volRender.GetComponent<PlaybackInstance>());
                                PlaybackInstance instance = volRender.GetComponent<PlaybackInstance>();
                                if (!input.clipNotLoadedPause)
                                {
                                    volRender.GetComponent<MeshRenderer>().GetPropertyBlock(instance.props);
                                    instance.props.Clear();
                                    volRender.GetComponent<MeshRenderer>().SetPropertyBlock(instance.props);
                                    volRender.LoadNewClip(input.fileName, index);
                                    PlaybackInstance newInstance = volRender.GetComponent<PlaybackInstance>();
                                    VolumetricTimelineGlobals.instanceList.Add(newInstance);
                                    input.clipNotLoadedPause = true;
                                }
                                var newIndex = volRender.instanceRef.IndexOf(instance);
                                volRender.SeekToCursor(newIndex, (int)(inputPlayable.GetTime() * 1000000.0f));
                                input.seeking = true;
                            }
                        }

                        if (!input.clipLoaded)
                        {
                            var index = volRender.instanceRef.IndexOf(volRender.GetComponent<PlaybackInstance>());
                            PlaybackInstance instance = volRender.GetComponent<PlaybackInstance>();
                            volRender.GetComponent<MeshRenderer>().GetPropertyBlock(instance.props);
                            instance.props.Clear();
                            volRender.GetComponent<MeshRenderer>().SetPropertyBlock(instance.props);
                            volRender.LoadNewClip(input.fileName, index);
                            var newIndex = volRender.instanceRef.IndexOf(instance);
                            volRender.SeekToCursor(newIndex, (int)(inputPlayable.GetTime() * 1000000.0f));
                            PlaybackInstance newInstance = volRender.GetComponent<PlaybackInstance>();
                            VolumetricTimelineGlobals.instanceList.Add(newInstance);
                            input.seeking = true;
                        }
                    }


                }

                if (director.state == PlayState.Playing)
                {
                    if (input.fileName.Length > 0)
                    {
                        if (input.clipLoaded)
                        {
                            if (input.clipPlaying)
                            {
                                if (!input.seeking)
                                {
                                    if (!input.clipStarted)
                                    {
                                        var index = volRender.instanceRef.IndexOf(volRender.GetComponent<PlaybackInstance>());
                                        PlaybackInstance newInstance = volRender.GetComponent<PlaybackInstance>();
                                        var newIndex = VolumetricTimelineGlobals.instanceList.Count - 1;
                                        VolumetricTimelineGlobals.instanceList.Insert(newIndex, newInstance);
                                        VolumetricTimelineGlobals.instanceList.RemoveAt(VolumetricTimelineGlobals.instanceList.Count - 1);
                                        volRender.StartPlayback(index);
                                        input.clipStarted = true;
                                    }
                                }

                                if (input.seeking)
                                {
                                    var index = volRender.instanceRef.IndexOf(volRender.GetComponent<PlaybackInstance>());
                                    volRender.StartPlayback(index);
                                    input.seeking = false;
                                }
                            }

                            if (!input.clipPlaying)
                            {
                                if (!input.seeking)
                                {
                                    var index = volRender.instanceRef.IndexOf(volRender.GetComponent<PlaybackInstance>());
                                    PlaybackInstance instance = volRender.GetComponent<PlaybackInstance>();
                                    volRender.GetComponent<MeshRenderer>().GetPropertyBlock(instance.props);
                                    instance.props.Clear();
                                    volRender.GetComponent<MeshRenderer>().SetPropertyBlock(instance.props);
                                    volRender.LoadNewClip(input.fileName, index);
                                    var newIndex = volRender.instanceRef.IndexOf(instance);
                                    PlaybackInstance newInstance = volRender.GetComponent<PlaybackInstance>();
                                    VolumetricTimelineGlobals.instanceList.Add(newInstance);
                                    input.clipPlaying = true;
                                }

                                if (input.seeking)
                                {
                                    VolumetricTimelineGlobals.instanceList.Clear();
                                    PlaybackInstance instance = volRender.GetComponent<PlaybackInstance>();
                                    var newIndex = volRender.instanceRef.IndexOf(instance);
                                    volRender.SeekToCursor(newIndex, (int)(inputPlayable.GetTime() * 1000000.0f));
                                    PlaybackInstance newInstance = volRender.GetComponent<PlaybackInstance>();
                                    VolumetricTimelineGlobals.instanceList.Add(newInstance);
                                    input.seeking = false;
                                    input.clipPlaying = true;
                                }
                            }
                        }
                    }
                }
            }

            if (inputWeight == 0f)
            {
                ScriptPlayable<VolumetricRenderBehavior> inputPlayable = (ScriptPlayable<VolumetricRenderBehavior>)playable.GetInput(i);
                VolumetricRenderBehavior input = inputPlayable.GetBehaviour();
                PlayableDirector director = playable.GetGraph().GetResolver() as PlayableDirector;

                if (director.state == PlayState.Paused)
                {
                    if (input.clipPlaying)
                    {
                        PlaybackInstance instance = volRender.GetComponent<PlaybackInstance>();
                        VolumetricTimelineGlobals.instanceList.Remove(instance);
                    }
                    input.clipPlaying = false;
                    input.clipStarted = false;
                }

                if (director.state == PlayState.Playing)
                {
                    if (input.clipPlaying)
                    {
                        PlaybackInstance instance = volRender.GetComponent<PlaybackInstance>();
                        VolumetricTimelineGlobals.instanceList.Remove(instance);
                    }
                    input.clipPlaying = false;
                    input.clipStarted = false;
                }

            }
        }
    }
}
