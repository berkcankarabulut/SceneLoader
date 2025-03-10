 
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement; 
using Project.SceneLoadSystem.Runtime;

namespace Project.SceneLoadSystem.Editor
{ 
    [CustomEditor(typeof(SceneLoader))]
    public class SceneLoaderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SceneLoader sceneLoader = (SceneLoader)target;
 
            string[] sceneNames = new string[SceneManager.sceneCountInBuildSettings];
            for (int i = 0; i < sceneNames.Length; i++)
            {
                sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
 
            sceneLoader.SelectedSceneIndex = EditorGUILayout.Popup("Select Scene", sceneLoader.SelectedSceneIndex, sceneNames);

            if (GUILayout.Button("Load Scene"))
            {
                sceneLoader.LoadScene();
            }

            EditorUtility.SetDirty(sceneLoader);
        }
    }

}