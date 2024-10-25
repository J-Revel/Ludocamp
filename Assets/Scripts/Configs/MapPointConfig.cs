using UnityEngine;

[CreateAssetMenu(fileName = "MapPointConfig", menuName = "Scriptable Objects/MapPointConfig")]
public class MapPointConfig : ScriptableObject
{
    public char location_id;
    public string title;
    public AudioClip ambiance;
}
