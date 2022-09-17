using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondCollectableGem : MonoBehaviour, ICollectableGem
{
    public GemType gemType =>GemType.Diamond;
}
