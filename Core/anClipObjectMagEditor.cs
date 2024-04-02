#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(anClipObjectMag))]
public class anClipObjectMagEditor : Editor
{
    public override void OnInspectorGUI()
    {
        anClipObjectMag mT = (anClipObjectMag)target;
        DrawDefaultInspector();
        mT.UseDistortion = EditorGUILayout.Toggle(nameof(mT.UseDistortion),mT.UseDistortion);
        if(mT.UseDistortion)
            mT.MaxDistortion =  EditorGUILayout.Slider(nameof(mT.MaxDistortion), mT.MaxDistortion, 0, 1);
        mT.UseHighPass = EditorGUILayout.Toggle(nameof(mT.UseHighPass), mT.UseHighPass);
        if (mT.UseHighPass) 
            mT.MaxHighPass = EditorGUILayout.Slider(nameof(mT.MaxHighPass), mT.MaxHighPass, 10, 22000);
    }
}
#endif