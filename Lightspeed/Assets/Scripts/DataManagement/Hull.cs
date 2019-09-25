using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Hull_", menuName = "Hull")]
public class Hull : ShipComponent{
    public float RespawnTime;
    public int speed;
    public int maxWeight;
    public int maneuverability;
}
