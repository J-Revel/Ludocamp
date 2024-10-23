using UnityEngine;

[CreateAssetMenu(fileName = "DocumentConfig", menuName = "Scriptable Objects/DocumentConfig")]
public class DocumentConfig : UnlockableConfig
{
    public string id;
    public RectTransform document_prefab;
    public string fullTitle;
    public string shortTitle;
    //public bool HasBeenViewed; // { get; set; }
}
