using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FPLibrary;
using SoarSDK;

[System.Serializable]
public class VolumetricAnimationData
{
    public string clip;
    //public string clipName;
    public Fix64 length = 0;

    public int _EnumMoveType;

    public bool IsPlaying;

    public enum _MoveType
    {
        IDEL,
        FWRD,
        BACK,
        LOW,
        JUMP,
        FALL,
        HIT,
        HIGHHIT,
        LOWHIT,
        AIRHIT,
        BLOCK,
        LOWBLOCK,
        KNOCKDOWN,
        GETUP,
        TTECH,
        TATTEMPT,
        TREACT,
        STANDA,
        STANDB,
        STANDC,
        LOWA,
        LOWB,
        LOWC,
        AIRA,
        AIRB,
        AIRC,
        SUPER
    }



    #region trackable definitions
    public Fix64 normalizedTime = 1;
    public Fix64 secondsPlayed = 0;
    public Fix64 ticksPlayed = 0;
    public Fix64 framesPlayed = 0;
    public Fix64 realFramesPlayed = 0;
    public int timesPlayed = 0;
    public float time;
    public Fix64 speed = 1;
    #endregion
    [HideInInspector] public VolumetricRender animState;
}

[RequireComponent(typeof(string))]
public class VolumetricControl : MonoBehaviour
{

    public VolumetricAnimationData[] animations = new VolumetricAnimationData[0];
    public bool debugMode = false;
    public bool overrideAnimatorUpdate = false;
    public VolumetricRender animator;

    #region trackable definitions
    [HideInInspector] public VolumetricAnimationData currentAnimationData;
    [HideInInspector] public bool currentMirror;
    [HideInInspector] public Fix64 globalSpeed = 1;
    [HideInInspector] public Vector3 lastPosition;
    public Vector3 deltaDisplacement;
    #endregion

    void Awake()
    {
        animator = gameObject.GetComponent<VolumetricRender>();
        lastPosition = transform.position;
    }

    void Start()
    {
        if (animations[0] == null) Debug.LogWarning("No animation found!");
        currentAnimationData = animations[0];
    }

    public void DoFixedUpdate()
    {
        if (animator == null || currentAnimationData == null || !animator.playing || !overrideAnimatorUpdate) return;

        currentAnimationData.secondsPlayed += (UFE.fixedDeltaTime * globalSpeed);
        currentAnimationData.ticksPlayed += UFE.fixedDeltaTime * UFE.fps * 1;
        currentAnimationData.framesPlayed = (int)FPMath.Floor(currentAnimationData.ticksPlayed);
        currentAnimationData.realFramesPlayed += FPMath.Abs(UFE.fixedDeltaTime * UFE.fps * 1);
        currentAnimationData.time = (float)animator.GetFullDuration(0) * 1000000;
        //if (currentAnimationData.secondsPlayed >= currentAnimationData.length && currentAnimationData.clip.wrapMode == WrapMode.Loop) SetCurrentClipPosition(0);
        //animator.Sample();
    }

    void OnGUI()
    {
        //Toggle debug mode to see the live data in action
        if (debugMode)
        {
            GUI.Box(new Rect(Screen.width - 340, 40, 340, 300), "Animation Data");
            GUI.BeginGroup(new Rect(Screen.width - 330, 60, 400, 300));
            {
                GUILayout.Label("Global Speed: " + globalSpeed);
                GUILayout.Label("Current Animation Data");
                GUILayout.Label("-Clip Name: " + currentAnimationData.clip);
                GUILayout.Label("-Speed: " + currentAnimationData.speed);
                //GUILayout.Label("-Normalized Speed: " + currentAnimationData.normalizedSpeed);
                GUILayout.Label("Animation State");
                GUILayout.Label("-Time: " + animator.GetFullDuration(0) * 1000000);
               // GUILayout.Label("-Normalized Time: " + currentAnimationData.animState.);
                //GUILayout.Label("-Lengh: " + currentAnimationData.animState.length);
                //GUILayout.Label("-Speed: " + currentAnimationData.animState.speed);
            }
            GUI.EndGroup();
        }
    }



    // LEGACY CONTROL METHODS
    public void RemoveClip(string name)
    {
        List<VolumetricAnimationData> animationDataList = new List<VolumetricAnimationData>(animations);
        animationDataList.Remove(GetAnimationData(name));
        animations = animationDataList.ToArray();
    }

    //public void RemoveClip(string clip)
    //{
    //    List<VolumetricAnimationData> animationDataList = new List<VolumetricAnimationData>(animations);
    //    animationDataList.Remove(GetAnimationData(clip));
    //    animations = animationDataList.ToArray();
    //}

    public void RemoveAllClips()
    {
        animations = new VolumetricAnimationData[0];
    }

    public void AddClip(string clip)
    {
        AddClip(clip, 1);
    }

   
    public void AddClip(string clip, Fix64 speed)
    {
        if (GetAnimationData(clip) != null) Debug.LogWarning("An animation with the name '" + clip + "' already exists.");
        VolumetricAnimationData animData = new VolumetricAnimationData();
      

        List<VolumetricAnimationData> animationDataList = new List<VolumetricAnimationData>(animations);
        animationDataList.Add(animData);
        animations = animationDataList.ToArray();

        //animator.LoadNewClip(clip, newName, _animation);
       


        //foreach (AnimationState animState in animator)
        //{
        //    if (animState.name == newName) animData.animState = animState;
        //}
    }

    
    public VolumetricAnimationData GetAnimationData(VolumetricAnimationData._MoveType clipName)
    {
        foreach (VolumetricAnimationData animData in animations)
        {
            if (animData._EnumMoveType == (int)clipName)
            {
                return animData;
            }
        }
        return null;
    }

    public VolumetricAnimationData GetAnimationData(string clip)
    {
        foreach (VolumetricAnimationData animData in animations)
        {
            if (animData.clip == clip)
            {
                return animData;
            }
        }
        return null;
    }

    public bool IsPlaying(string clipName)
    {
        if (currentAnimationData == GetAnimationData(clipName)) return true;
        return (animator.playing);
    }

    public bool IsPlaying(VolumetricAnimationData animData)
    {
        return (currentAnimationData == animData);
    }

    public int GetTimesPlayed(string clipName)
    {
        return GetAnimationData(clipName).timesPlayed;
    }


    public void Play(string _clip)
    {
        if (animations[0] == null)
        {
            Debug.LogError("No animation found.");
            return;
        }
        Play(animations[0], _clip);
    }

    public void Play(VolumetricAnimationData animData, string _clip)
    {
        if (animData == null) return;

        //if (currentAnimationData != null)
        //{
        //    currentAnimationData.speed = currentAnimationData.originalSpeed;
        //    currentAnimationData.normalizedSpeed = 1;
        //}

        currentAnimationData = animData;

        if (UFE.config != null && (UFE.isConnected || UFE.config.debugOptions.emulateNetwork) && UFE.config.networkOptions.disableBlending)
        {
            animator.LoadNewClip(_clip, 0);
            animator.StartPlayback(0);
        }
        

        //SetSpeed(currentAnimationData.speed);
        //deltaDisplacement = new Vector3();

        //SetCurrentClipPosition(normalizedTime);
    }

    public void SetCurrentClipPosition(Fix64 normalizedTime)
    {
        SetCurrentClipPosition(normalizedTime, false);
    }

    public void SetCurrentClipPosition(Fix64 normalizedTime, bool pause)
    {
        //normalizedTime = FPMath.Clamp(normalizedTime, 0, 1);
        //currentAnimationData.secondsPlayed = normalizedTime * currentAnimationData.length;
        //currentAnimationData.ticksPlayed = currentAnimationData.secondsPlayed * UFE.config.fps;
        //currentAnimationData.framesPlayed = (int)FPMath.Floor(currentAnimationData.ticksPlayed);
        //currentAnimationData.realFramesPlayed = normalizedTime * currentAnimationData.length * UFE.fps;
        //currentAnimationData.normalizedTime = normalizedTime;
        //currentAnimationData.animState.normalizedTime = (float)normalizedTime;
        //animator.Sample();

        //if (pause) Pause();
    }

    public Fix64 GetCurrentClipPosition()
    {
        //return currentAnimationData.animState.normalizedTime;
        return 0;
    }

    public Fix64 GetCurrentClipTime()
    {
        return currentAnimationData.secondsPlayed;
    }

    public int GetCurrentClipFrame(bool bakeSpeed)
    {
        if (bakeSpeed) return (int)FPMath.Floor(currentAnimationData.realFramesPlayed);
        return (int)FPMath.Floor(currentAnimationData.framesPlayed);
    }

    public string GetCurrentClipName()
    {
        if (currentAnimationData == null) return null;
        return currentAnimationData.clip;
    }

    public Vector3 GetDeltaDisplacement()
    {
        deltaDisplacement += GetDeltaPosition();
        return deltaDisplacement;
    }

    public Vector3 GetDeltaPosition()
    {
        Vector3 deltaPosition = transform.position - lastPosition;
        lastPosition = transform.position;
        return deltaPosition;
    }

    public void Stop()
    {
        animator.StopModel(0);
    }

    public void Stop(string animName)
    {
        //animator.Stop(animName);
    }

    public void Pause()
    {
        globalSpeed = 0;
    }

    //public void SetSpeed(AnimationClip clip, Fix64 speed)
    //{
    //    SetSpeed(GetAnimationData(clip), speed);
    //}

    //public void SetSpeed(string clipName, Fix64 speed)
    //{
    //    SetSpeed(GetAnimationData(clipName), speed);
    //}

    //public void SetSpeed(VolumetricAnimationData animData, Fix64 speed)
    //{
    //    if (animData != null)
    //    {
    //        animData.speed = speed;
    //        animData.normalizedSpeed = speed / animData.originalSpeed;
    //        if (IsPlaying(animData)) SetSpeed(speed);
    //    }
    //}

    public void SetSpeed(Fix64 speed)
    {
        //globalSpeed = speed;
        //if (currentAnimationData != null) currentAnimationData.normalizedSpeed = speed / currentAnimationData.originalSpeed;

        //if (!overrideAnimatorUpdate)
        //{
        //    foreach (AnimationState animState in animator)
        //    {
        //        animState.speed = (float)speed;
        //    }
        //}
    }

    //public void SetNormalizedSpeed(AnimationClip clip, Fix64 normalizedSpeed)
    //{
    //    SetNormalizedSpeed(GetAnimationData(clip), normalizedSpeed);
    //}

    //public void SetNormalizedSpeed(string clipName, Fix64 normalizedSpeed)
    //{
    //    SetNormalizedSpeed(GetAnimationData(clipName), normalizedSpeed);
    //}

    //public void SetNormalizedSpeed(VolumetricAnimationData animData, Fix64 normalizedSpeed)
    //{
    //    animData.normalizedSpeed = normalizedSpeed;
    //    animData.speed = animData.originalSpeed * animData.normalizedSpeed;
    //    if (IsPlaying(animData)) SetSpeed(animData.speed);
    //}

    //public Fix64 GetSpeed(AnimationClip clip)
    //{
    //    return GetSpeed(GetAnimationData(clip));
    //}

    //public Fix64 GetSpeed(string clipName)
    //{
    //    return GetSpeed(GetAnimationData(clipName));
    //}

    //public Fix64 GetSpeed(VolumetricAnimationData animData)
    //{
    //    return animData.speed;
    //}

    //public Fix64 GetSpeed()
    //{
    //    return globalSpeed;
    //}

    //public Fix64 GetNormalizedSpeed()
    //{
    //    return GetNormalizedSpeed(currentAnimationData);
    //}

    //public Fix64 GetNormalizedSpeed(AnimationClip clip)
    //{
    //    return GetNormalizedSpeed(GetAnimationData(clip));
    //}

    //public Fix64 GetNormalizedSpeed(string clipName)
    //{
    //    return GetNormalizedSpeed(GetAnimationData(clipName));
    //}

    //public Fix64 GetNormalizedSpeed(VolumetricAnimationData animData)
    //{
    //    return animData.normalizedSpeed;
    //}

    //public void OverrideCurrentWrapMode(WrapMode wrap)
    //{
    //    currentAnimationData.wrapMode = wrap;
    //}

    public void RestoreSpeed()
    {
        //SetSpeed(currentAnimationData.speed);

        //if (!overrideAnimatorUpdate)
        //{
        //    foreach (AnimationState animState in animator)
        //    {
        //        animState.speed = (float)GetAnimationData(animState.name).speed;
        //    }
        }
    }
