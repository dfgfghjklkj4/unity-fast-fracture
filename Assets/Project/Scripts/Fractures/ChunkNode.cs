using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Utils;
using UnityEngine;
using Project.Scripts.Fractures;






namespace Project.Scripts.Fractures
{

    public class ChunkNode : MonoBehaviour
    {

        public bool resetPos;
        public bool leaved;
        //包围框的体积
        public float boundSize;
        public Bounds b;
        public ChunkGraphManager cm;
        public MeshCollider col;
        public BoxCollider boxcol;
        public Vector3 pos, rot;
        public bool outside;
        public ChunkNode[] Neighbours;
        //  [HideInInspector]
     
   

        public Rigidbody rb;
        public bool frozen;
        public MeshRenderer render;
        public MeshFilter mf;

        public float unfrozenTime;
        public bool searched;
        public float minH;



        public void Setup()
        {

            // rb.angularDrag = 0;
            // rb.Sleep();
            // col.enabled=false;
            pos = col.bounds.center + cm.transform.position;
            b = col.bounds;
            boundSize = col.bounds.size.x * col.bounds.size.y * col.bounds.size.z;
            minH = col.bounds.min.y;
     boxcol = gameObject.AddComponent<BoxCollider>();
            boxcol.center = col.bounds.center;
            boxcol.size = col.bounds.size;
            boxcol.enabled = false;
            //boxcol.enabled=false;
            // JointToChunk.Clear();
            //  ChunkToJoint.Clear();

            //  gameObject.hideFlags = HideFlags.HideInHierarchy;
            resetPos = true;
            Freeze();

            //  gameObject.SetActive(false);
        }





        public void Touch()
        {
            for (int i = 0; i < Neighbours.Length; i++)
            {
                var n = Neighbours[i];
                if (n.resetPos)
                {
                    cm.touchNode.Add(n);
                    // n.col.enabled=true;
                }

            }
        }


        public void Leave()
        {

        }

        public void Unfreeze()
        {
            if (!frozen)
            {
                return;
            }
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic=false;
            rb.useGravity=true;
            rb.  WakeUp();
                
            
            resetPos = false;
            cm.unActivecount--;
            //   gameObject.isStatic = false;
            //   boxcol.enabled = false;
            //  col.enabled = true;
            // rb.WakeUp();
            frozen = false;
            if (cm.touchNode.Contains(this))
            {
                cm.touchNode.Remove(this);
            }
            // rb.constraints = RigidbodyConstraints.None;
            //   rb.useGravity = true;
            //  rb.isKinematic = false;
           // unfrozenTime = Time.time + 4f;
        
           // cm.unFrozenNode.Add(this);



            // rb.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        public void Freeze()
        {
                 if (rb != null)
            {
                 rb.isKinematic=true;
               rb.useGravity=false;
               rb.  Sleep();
            }
           
            //    gameObject.isStatic = true;
            //  boxcol.enabled = true;
            //  col.enabled = false;
            //  rb.isKinematic = true;
            //  rb.Sleep();
            frozen = true;
            //   rb.useGravity = false;

            //  rb.gameObject.layer = LayerMask.NameToLayer("FrozenChunks");
            // frozenPos = rb.transform.position;
            //  forzenRot = rb.transform.rotation;
        }


    }
}