using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollectableGem : MonoBehaviour,ICollectableGem
{
    public GemType gemType => GemType.Cube;
 
}
