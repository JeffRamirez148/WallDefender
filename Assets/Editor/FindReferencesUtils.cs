using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class FindReferences : EditorWindow
{
    [MenuItem("GameObject/Object References/Select References", false, 0)]
    static void SelectReferences()
    {
        UnityEngine.Object to = Selection.activeObject;
        SelectReferences(to, true);
    }

    [MenuItem("GameObject/Object References/List References", false, 0)]
    static void ListReferences()
    {
        UnityEngine.Object to = Selection.activeObject;
        SelectReferences(to, false);
    }

    static void SelectReferences(UnityEngine.Object to, bool bSelect)
    {
        var referencedBy = new List<UnityEngine.Object>();
        var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        for (int j = 0; j < allObjects.Length; j++)
        {
            var go = allObjects[j];

            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                var c = components[i];
                if (!c) continue;

                var so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        //sp.objectReferenceValue.GetInstanceID();
                        if (sp.objectReferenceValue == to)
                        {
                            Debug.Log(string.Format("referenced by {0}, {1}", GetPath(c), c.GetType()), c);
                            referencedBy.Add(c.gameObject);
                        }
                        else
                        {
                            GameObject tgo = to as GameObject;
                            if (null != tgo)
                            {
                                var tcomponents = tgo.GetComponents<Component>();
                                foreach (Component tc in tcomponents)
                                {
                                    if (sp.objectReferenceValue == tc)
                                    {
                                        Debug.Log(string.Format("referenced by {0}, {1}", GetPath(c), c.GetType()), c);
                                        referencedBy.Add(c.gameObject);
                                        break;
                                    }
                                }
                            }
                        }
                        


                    }
            }
        }

        if (referencedBy.Count > 0)
        {
            if (bSelect) Selection.objects = referencedBy.ToArray();
        }
        else Debug.Log("no references in scene");

        referencedBy.Clear();
    }

    public static string GetPath(Transform current)
    {
        if (current.parent == null)
            return "/" + current.name;
        return GetPath(current.parent) + "/" + current.name;
    }

    public static string GetPath(Component component)
    {
        return GetPath(component.transform) + "/" + component.GetType().ToString();
    }

    public static string GetPath(GameObject obj)
    {
        if (null == obj)
            return "ERROR";

        return GetPath(obj.transform);
    }

    public static string GetPath(UnityEngine.Object obj)
    {
        return GetPath(obj as GameObject);        
    }


}

public class MissingReferencesFinder : MonoBehaviour
{
    [MenuItem("GameObject/Missing References/Show Missing Object References in scene")]
    public static void FindMissingReferencesInCurrentScene()
    {
        var objects = GetSceneObjects();
        FindMissingReferences(EditorSceneManager.GetActiveScene().name, objects);
    }

    [MenuItem("GameObject/Missing References/Show Missing Object References in all scenes")]
    public static void MissingSpritesInAllScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            EditorSceneManager.OpenScene(scene.path);
            var objects = GetSceneObjects();
            FindMissingReferences(scene.path, objects);
        }
    }

    [MenuItem("GameObject/Missing References/Show Missing Object References in assets")]
    public static void MissingSpritesInAssets()
    {
        var allAssets = AssetDatabase.GetAllAssetPaths();
        var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

        FindMissingReferences("Project", objs);
    }

    private static void FindMissingReferences(string context, GameObject[] objects)
    {
        bool foundMissing = false;
        foreach (var go in objects)
        {
            var components = go.GetComponents<Component>();

            foreach (var c in components)
            {
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + FullPath(go), go);
                    foundMissing = true;
                    continue;
                }

                SerializedObject so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null
                            && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(context, go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                            foundMissing = true;
                        }
                    }
                }
            }
        }

        if (!foundMissing)
            Debug.Log("No missing references or components in " + context);
    }

    private static GameObject[] GetSceneObjects()
    {
        return Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go))
                   && go.hideFlags == HideFlags.None).ToArray();
    }

    private const string err = "Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}";

    private static void ShowError(string context, GameObject go, string c, string property)
    {
        Debug.LogError(string.Format(err, FullPath(go), c, property, context), go);
    }

    private static string FullPath(GameObject go)
    {
        return go.transform.parent == null
            ? go.name
                : FullPath(go.transform.parent.gameObject) + "/" + go.name;
    }

}