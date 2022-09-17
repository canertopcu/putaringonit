using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectableGem : MonoBehaviour, ICollectableGem
{
    public GemType gemType =>GemType.Heart;
 
}
