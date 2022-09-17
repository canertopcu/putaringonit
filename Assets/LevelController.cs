using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour, ILevel
{
    public SplineComputer splineComputer => GetComponentInChildren<SplineComputer>();
}
