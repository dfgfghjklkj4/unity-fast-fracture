using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Scripts.Fractures;

public class FOp : MonoBehaviour
{
    public FractureThis ft;
    public Bounds szie;
    public MeshFilter mf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {if (Input.GetKeyDown(KeyCode.Space))
    {
            BakeChunks();
    }
    
    }

bool baked;
    void BakeChunks()
    {
float t=Time.realtimeSinceStartup;
//Physics
for (int i = 0; i < 2000; i++)
{
    var p= UnityEngine.Random.onUnitSphere*0.7f;

   // Debug.DrawLine(p,Vector3.zero,Color.black,100);

    if (Physics.Linecast(p,Vector3.zero,out RaycastHit hitInfo))
    {
      var node=  hitInfo.collider.GetComponent<ChunkNode>();
    if (node)
    { if( !node.render.enabled){
 node.render.enabled=true;
    }
     
    }

    }
} 
  float t2=Time.realtimeSinceStartup;
  Debug.Log(t2-t);
    }
}
