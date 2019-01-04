using UnityEditor;

[CustomEditor(typeof(LoadTest))]
class EditorTest : Editor
{
    private void OnEnable()
    {
        EditorApplication.update += OnUpdate;
    }

    void OnUpdate()
    {
        
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }
}
