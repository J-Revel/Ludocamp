using UnityEngine;

[CreateAssetMenu(fileName = "DocumentConfig", menuName = "Scriptable Objects/DocumentConfig")]
public class DocumentConfig : UnlockableConfig
{
    public string id;
    public RectTransform document_prefab;
}
