using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Weapon;
using UnityEngine;
using Project.Scripts.Fractures;

public class bullet : MonoBehaviour
{
  
    public Rigidbody rb;
    



    private void Update()
    {


        if (hitNode)
        {
            //  print(6666666666666666666);
            hitNode = false;

        

            hitObj.Clear();
            ObjPool.ReturnGO(this, FireGun.instance.b);
        }
    }
  

  


    bool hitNode;
    public Vector3 hitPoint;
    List<ChunkNode> hitObj = new List<ChunkNode>();
    void OnCollisionEnter(Collision collision) ////////////////////////////////////////////
    {
        var node = collision.transform.GetComponent<ChunkNode>();
        //   print(collision.gameObject.name);
        if (node != null)
        {
            if (!hitNode)
            {
                hitNode = true;
            }
            hitObj.Add(node);


        }

        //  ObjPool.ReturnGO(this, FireGun.instance.b);
    }






}
