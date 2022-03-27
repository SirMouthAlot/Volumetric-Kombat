using UnityEditor;
using UnityEngine;



public class DummyClipCreate : EditorWindow
{
    

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

        AnimationCurve curve = AnimationCurve.Constant(0.0f, (_clipLength/60.0f), 1.0f);
        EditorCurveBinding binding = EditorCurveBinding.FloatCurve(string.Empty, typeof(UnityEngine.Animator), "DummyAnimationClip");
        AnimationUtility.SetEditorCurve(clip, binding, curve);
        AssetDatabase.CreateAsset(clip, path);
    }
}
