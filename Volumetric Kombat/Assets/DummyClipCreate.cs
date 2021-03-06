#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;



public class DummyClipCreate : EditorWindow
{

    public Object _soarObject;

    VideoPlayer _soarClip;


    const string _constAsset = "Assets/StreamingAssets/VOD";

    string _clipName;

    float _clipLength;

    [MenuItem("Tools/Create Dummy Clip")]
    public static void ShowWindow()
    {
        GetWindow<DummyClipCreate>("DummyClipCreator");
    }
    void OnGUI()
    {
        GUILayout.Label("Create Dummy Animation Clips", EditorStyles.boldLabel);

        _clipName = EditorGUILayout.TextField("Clip Name: ", _clipName);

        GUILayout.Label("Clip Length", EditorStyles.boldLabel);
        //_soarObject = EditorGUILayout.ObjectField(_soarObject, typeof(Object), true);

        //if (_soarObject != null)
        //{
        //    string assetPath = AssetDatabase.GetAssetPath(_soarObject);

        //    if (assetPath.StartsWith(_constAsset))
        //    {
        //        assetPath = assetPath.Substring(_constAsset.Length);

        //    }

        //    _soarClip.url = assetPath;
        //    _clipLength = _soarClip.frameCount / 30.0f;
        //}



        GUILayout.BeginVertical();



        _clipLength = EditorGUILayout.FloatField("Clip Length: ", _clipLength);
        if (GUILayout.Button("Create Clip"))
        {
            CreateDummyClip();
        }
        GUILayout.EndVertical();

    }
    void CreateDummyClip()
    {
        var path = EditorUtility.SaveFilePanelInProject("Save Dummy Animation Clip", _clipName, "anim", "");
        AnimationClip clip = new AnimationClip();
        clip.name = _clipName;
        AnimationCurve curve = AnimationCurve.Constant(0.0f, (_clipLength), 1.0f);
        EditorCurveBinding binding = EditorCurveBinding.FloatCurve(string.Empty, typeof(UnityEngine.Animator), "DummyAnimationClip");
        AnimationUtility.SetEditorCurve(clip, binding, curve);
        AssetDatabase.CreateAsset(clip, path);
    }
}

#endif