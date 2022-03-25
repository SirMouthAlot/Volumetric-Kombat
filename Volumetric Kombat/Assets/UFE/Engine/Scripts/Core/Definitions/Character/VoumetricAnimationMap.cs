using UnityEngine;
using FPLibrary;
using UFE3D;
using System.Collections.Generic;
using SoarSDK;

[System.Serializable]
public class VolumetricAnimationMap
{
  
    public AnimationMap[] animationMaps = new AnimationMap[0];

    public VolumetricRender playbackComponent;

    //public List<string> _volumetricMoves = new List<string>();

    public string _move;

    public CustomHitBoxesInfo customHitBoxDefinition;
    public Fix64 length;
    public bool bakeSpeed = false;
    public HitBoxDefinitionType hitBoxDefinitionType;

    private void Start()
    {
        //_timeline = GameObject.FindGameObjectWithTag("Timeline");
       
    }
}