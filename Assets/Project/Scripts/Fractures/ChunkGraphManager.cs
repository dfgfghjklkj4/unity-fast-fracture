using System.Collections;
using System;
using System.Collections.Generic;

using System.Linq;
using Project.Scripts.Utils;
using UnityEngine;
using Project.Scripts.Fractures;

namespace Project.Scripts.Fractures
{

    public class NodeGroup
    {
        public List<ChunkNode> nodes;
        public bool leaved;
        public ChunkNode minHight;
        // public ChunkNode maxHight;

        public static Stack<NodeGroup> NodePool = new Stack<NodeGroup>();

        public static NodeGroup Get()
        {
            if (NodePool.Count > 0)
            {
                var n = NodePool.Pop();
                n.leaved = false;
                n.minHight = null;
                return n;
            }
            else
            {
                var n = new NodeGroup();
                n.nodes = new List<ChunkNode>(512);
                return n;
            }
        }

        public void ReturnPool()
        {
            nodes.Clear();

            minHight = null;
            NodePool.Push(this);
        }


    }
}

public class ChunkGraphManager : MonoBehaviour

{
    //  public Bounds bound;
    public List<ChunkNode> unFrozenNode = new List<ChunkNode>(48);
    public HashSet<ChunkNode> touchNode = new HashSet<ChunkNode>(48);
    public int unActivecount;
    public FractureThis fs;
    //  [HideInInspector]
    public ChunkNode[] nodes;

    public FractureGO fgo;

    public List<FractureGO> fgos = new List<FractureGO>();
    public void Setup()
    {

        for (int i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            node.cm = this;
            node.Setup();
        }
    }




    private void FixedUpdate0()
    {

        int count = unFrozenNode.Count;
        if (count > 0)
        {
            if (Time.frameCount % 5 == 0)
            {
                int c = unFrozenNode.Count - 1;//直接用gos.Count - 1或c
                for (int i = c; i > -1; i--)
                {
                    var node = unFrozenNode[i];
                    // if (node.unfrozenTime < Time.time)
                    //  {
                    // unFrozenNode.RemoveAt(i);
                    //node.Freeze();
                    //  }
                    if (node.leaved)
                    {
                        unFrozenNode.RemoveAt(i);
                        break;
                    }
                    if (node.rb != null)
                    {
                        if (node.rb.velocity.magnitude == 0 && node.rb.centerOfMass.y < -1.85f)
                        {
                            node.Freeze();
                            //  Destroy(node.rb);
                            unFrozenNode.RemoveAt(i);
                        }
                    }
                    else
                    {
                        unFrozenNode.RemoveAt(i);
                    }


                }
            }

        }
    }




    int key;
    private void Update()
    {
        if (jojo.finish)
        {



            StartCoroutine(resetPos());
        }

        if (NodeGroups.Count > 0)
        {

            int count = NodeGroups.Count - 1;
            for (int i = count; i > -1; i--)

            {
                var item = NodeGroups[i];
                if (item.leaved)
                {
                    n4.Clear();


                    int c = 0;
                    foreach (var n in item.nodes)
                    {

                        if (c % 4 == 0)
                        {
                            n4.Add(n);
                        }
                        c++;
                        if (n4.Count > 20)
                        {
                            break;
                        }


                    }


                    Vector3 centerPos = Vector3.zero;
                    for (int j = 0; j < n4.Count; j++)
                    {
                        centerPos += n4[j].col.bounds.center;
                    }
                    centerPos /= n4.Count;
                    // centerPos.x= (float)Math.Round(centerPos.x,4);
                    // centerPos.y= (float)Math.Round(centerPos.y,4);
                    //  centerPos.z= (float)Math.Round(centerPos.z,4);
                    //  centerPos.z=0;
                    // FractureGO cp = Instantiate<FractureGO>(fgo);
                    FractureGO cp = ObjPool.GetComponent<FractureGO>(fgo, centerPos, transform.rotation);
                    fgos.Add(cp);
                    cp.group = item;
                    //  cp.transform.SetPositionAndRotation(centerPos, transform.rotation);
                    foreach (var item2 in item.nodes)
                    {
                        Destroy(item2.rb);
                        item2.Freeze();
                        item2.col.enabled = false;
                        item2.boxcol.enabled = true;
                        item2.resetPos = false;
                        item2.leaved = true;
                        //item2.frozen = false;
                        item2.transform.parent = cp.transform;
                        //    item2.render.material = BakeChunk.Instanc.mat;


                    }
                    var rb = cp.rb;
                    rb.angularDrag = 0.1f;
                    rb.mass = item.nodes.Count * 5f;

                    rb.AddForce(0, 0, 10f);
                }
                else
                {
                    item.ReturnPool();
                }








            }
            NodeGroups.Clear();


        }

        if (touchNode.Count > 0)
        //if (false)
        {
            // a :  int k=UnityEngine.Random.Range(0, BakeChunk.Instanc.mats.Count);
            //  if (k==key)
            //   {
            //      goto a;
            //    }
            //   key=k;
            //  var mat2 = BakeChunk.Instanc.mats[key];
            //   foreach (var item in touchNode)
            //  {
            //    item.render.material=mat2;
            //  }

            float t = Time.realtimeSinceStartup;
            touchNode.UnionWith(nodes);//有些bug 所以就全查询一遍 性能上没多少区别
            fun2();
            float t2 = Time.realtimeSinceStartup;

            //  print(t2 - t);
            touchNode.Clear();



            int c = NodeGroups.Count - 1;
            for (int i = c; i > -1; i--)

            {
                var item = NodeGroups[i];
                var mat = BakeChunk.Instanc.mats[UnityEngine.Random.Range(0, BakeChunk.Instanc.mats.Count)];
                Vector3 pos = item.minHight.col.bounds.min;
                pos.y += 0.0001f;
                Ray ray = new Ray(pos, Vector3.down);
                //      print(item.minHight.gameObject.name+"  "+item.minHight.col.bounds.min);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.1f))
                {
                    //  print(hitInfo.collider.name + "  ------");

                }
                else
                {
                    item.leaved = true;
                    mat = null;
                }

                //  var nodelist = item.nodes;
                //                print(item.nodes.Count);
                foreach (var item2 in item.nodes)
                {
                    //   item2.render.material = mat;
                    item2.searched = false;

                }
                /// item.ReturnPool();
            }
            // NodeGroups.Clear();

        }




    }




    public List<ChunkNode> n4 = new List<ChunkNode>(128);


    public void fun2()
    {
        NodeGroups.Clear();
        // n2.Clear();
        //   nkong.Clear();
        //   int cccc = 0;
        foreach (var item in touchNode)
        {
            if (!item.searched && item.resetPos && !item.leaved)
            {

                searchTemp = NodeGroup.Get();

                searchTemp.nodes.Add(item);
                NodeGroups.Add(searchTemp);

                searchTemp.minHight = item;
                SearchGraph(item);
            }


        }
        searchTemp = null;
        // print(ccc + " n  ++++  k  " + ccc2);

    }

    bool touchDown2 = false;




    public List<NodeGroup> NodeGroups = new List<NodeGroup>();
    NodeGroup searchTemp;
    private void SearchGraph(ChunkNode o)
    {
        o.searched = true;
        //  if (touchDown)
        //  {

        //  return;
        // }
        //    ccc2++;


        for (var i = 0; i < o.Neighbours.Length; i++)
        {

            var neighbour = o.Neighbours[i];

            if (!neighbour.searched && neighbour.resetPos && !neighbour.leaved)
            {
                //  ccc++;
                if (neighbour.minH < searchTemp.minHight.minH)
                {
                    searchTemp.minHight = neighbour;
                }

                // if (0.5f<0.6f)
                // {
                //  searchTemp.minHight=neighbour;
                //  }
                searchTemp.nodes.Add(neighbour);
                // neighbour.Group = searchTemp;
                SearchGraph(neighbour);


            }

        }

        // }
    }




    public int ccc = 0;
    public int ccc2 = 0;
    bool touchDown = false;





    public float bakeSize;









    public IEnumerator resetPos()
    {
        jojo.finish = false;
        int cc = fgos.Count - 1;
        for (int i = cc; i > -1; i--)

        {
            var item = fgos[i];
            foreach (var item2 in item.group.nodes)
            {
                item2.col.enabled = true;
                item2.boxcol.enabled = false;
                item2.leaved = false;
                item2.Freeze();
                item2.transform.parent = this.transform;
            }
            item.group.ReturnPool();
            item.group = null;
            fgos.RemoveAt(i);
            ObjPool.ReturnGO<FractureGO>(item, fgo);
            //  Destroy(item);
            yield return null;
        }
        fgos.Clear();



        for (int i = 0; i < nodes.Length; i++)
        {
            if (!nodes[i].gameObject.activeSelf)
            {
                nodes[i].gameObject.SetActive(true);

            }
            nodes[i].Freeze();
            if (i % 100 == 0)
            {
                yield return null;
            }

        }

        float T = Time.time + 5;
        int c = 0;
    a:  c = 0;
        bool r = true;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (T < Time.time)
            {
                goto b;
            }
            if (!nodes[i].resetPos)
            {

                r = false;
                ChunkNode node = nodes[i];


                if (node.transform.localPosition != Vector3.zero || node.transform.localRotation != Quaternion.identity)
                {
                    node.transform.localPosition = Vector3.MoveTowards(node.transform.localPosition, Vector3.zero, 0.1f);
                    node.transform.localRotation = Quaternion.RotateTowards(node.transform.localRotation, Quaternion.identity, 10f);
                    if (node.transform.localPosition == Vector3.zero && node.transform.localRotation == Quaternion.identity)
                    {
                  
                        node.resetPos = true;
                    }
                    c++;
                    if (c > 50)
                    {
                        c = 0;
                     //   yield return null;
                    }

                }



            }
           
        }
        if (r == false)
        {  
            yield return null;
            goto a;
        }
        print(3333333333333333);
        yield break;
    b: print(6666666);
        c = 0;
        for (int i = 0; i < nodes.Length; i++)
        {

            if (!nodes[i].resetPos)
            {

                ChunkNode node = nodes[i];


                node.transform.localPosition = Vector3.zero;
                node.transform.localRotation = Quaternion.identity;
                node.resetPos = true;
                c++;
                if (c > 10)
                {
                    c = 0;
                    yield return null;
                }



            }
           
        }

        yield return 0;
    }



}
