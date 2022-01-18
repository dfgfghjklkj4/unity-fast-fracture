using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Scripts.Fractures;

public class BakeChunk : MonoBehaviour
{
public List<Material> mats=new List<Material>();
public List<Transform> li=new List<Transform>();
    public Material mat;
     public Material mat2;
    public LayerMask bakeMask_;
    public int l;
    public static BakeChunk Instanc;

    
    
     private void Awake() {
         Instanc=this;
         l=LayerMask.GetMask("bakeChunk");
    }
    // Start is called before the first frame update
    void Start()
    {
Invoke("f1",2.5f);
       // transform.localScale=new Vector3(0.9f,0.9f,0.9f);
    }

private void LateUpdate() {
   // transform.localScale=new Vector3(0.8f,0.8f,0.8f);
     if (li.Count>0)
        {
            
        }
}
    // Update is called once per frame
    void Update()
    {
       
    }

private void f1() {
    transform.localScale=new Vector3(0.8f,0.8f,0.8f);
    
}

    //////////////////////////////
private void OnTriggerEnter0(Collider other) {
    if (!li.Contains(other.transform))
    {
      li.Add(other.transform);
    }
     print(other.gameObject.name);
}


private void OnTriggerExit0(Collider other) {
     print(other.gameObject.name+"  Exit");
}

private void OnCollisionExit(Collision other) {
     if (!li.Contains(other.transform))
    {
        li.Add(other.transform);
      var node=  other.transform.GetComponent<ChunkNode>();
      if (node)
      {
          node.outside=true;
      }
    }
   // print(other.gameObject.name);
}

}
