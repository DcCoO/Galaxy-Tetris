using UnityEngine;

[CreateAssetMenu(fileName = "Time Settings", menuName = "ScriptableObjects/Time Settings", order = 1)]
public class TimeSettings : ScriptableObject
{
    public float beginTickDuration;
    public float endTickDuration;
    public float tickTransitionTime;
}
