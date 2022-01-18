using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitPoint 
{

    public Vector3 hitPos;
    public float hitRadius;
 public float hitVolum;
   

    public float hitTime;

    public HitPoint(Vector3 pos,float dis,float cc,float t)
    {
hitPos=pos;
hitRadius=dis;
hitVolum=cc;
hitTime=t;
    }
  
}
