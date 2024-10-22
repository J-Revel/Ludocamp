using UnityEngine;

[CreateAssetMenu(fileName = "DocumentConfig", menuName = "Scriptable Objects/DocumentConfig")]
public class DocumentConfig : ScriptableObject
{
    public string id;
    public RectTransform document_prefab;
}
