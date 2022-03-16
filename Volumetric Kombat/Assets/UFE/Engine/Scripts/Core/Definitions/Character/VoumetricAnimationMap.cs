using UnityEngine;
using FPLibrary;
using UFE3D;
using System.Collections.Generic;

[System.Serializable]
public class VolumetricAnimationMap
{
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
        //public AnimationMap[] animationMaps = new AnimationMap[0];

    List<string> _volumetricMoves = new List<string>();

    public string _move;

    public GameObject _timeline;
    public CustomHitBoxesInfo customHitBoxDefinition;
    public Fix64 length;
    public bool bakeSpeed = false;
    public HitBoxDefinitionType hitBoxDefinitionType;

    private void Start()
    {
        _timeline = GameObject.FindGameObjectWithTag("Timeline");

    }
}