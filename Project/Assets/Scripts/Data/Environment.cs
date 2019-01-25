using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Household/Environment", order = 1)]
public class Environment : ScriptableObject
{
    public float DayLength = 300;
    public Gradient SunColor;
    public AnimationCurve SunIntensity;
    public AnimationCurve SunAngle;
    public float DaylightThreshold = 0.25f;
}
