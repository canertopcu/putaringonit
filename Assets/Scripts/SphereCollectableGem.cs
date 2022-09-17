using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollectableGem : MonoBehaviour,ICollectableGem
{
    public GemType gemType => GemType.Sphere;
     
}
