using System.Collections.Generic;
using Project.Scripts.Fractures;
using UnityEngine;
using System.Linq;

namespace Project.Scripts.Weapon
{
    public class FireGun : MonoBehaviour
    {
        public static FireGun instance;
        public bullet b;
        [SerializeField] private Transform barrelEnd;
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private float velocity = 1000f;
        [SerializeField] private float mass = .5f;

        public float Radius
        {
            get => radius;
            set => radius = value;
        }

        public float Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        private void Awake()
        {
            
            instance = this;
        }
        void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            if (Input.GetKeyDown(KeyCode.F))
            {
                FireBullet();
            }
        }







        List<ChunkNode> hitNode = new List<ChunkNode>(128);
        private void FireBullet()
        {
            hitNode.Clear();
            //Ray ray=Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f));
            Ray ray = new Ray(barrelEnd.position, barrelEnd.forward);
            Vector3 targetPos;
            RaycastHit hit;
            bool hitChunk = false;
            // if (Physics.Raycast(ray, out hit, 100f))
            RaycastHit[] hits = Physics.SphereCastAll(barrelEnd.position, 0.2f, barrelEnd.forward, 100);
            if (hits.Length > 0)

            //  if (false)
            {






                // var p = hit.point;
                //  node.cm.hitpos.Add(new HitPoint(p, node.cm.fs.R, 0.25f, Time.time + 1));

                // var hits = Physics.OverlapSphere(p, node.cm.fs.R);
                // var hits = Physics.OverlapSphere(p, 0.2f);
                for (var j = 0; j < hits.Length; j++)
                {
                    var n1 = hits[j].collider.GetComponent<ChunkNode>();
                    if (n1)
                    {
                        hitNode.Add(n1);
                    }




                }
                if (hitNode.Count > 0)
                {     Rigidbody r=null;
                    hitNode.Sort((a, b) => a.boundSize.CompareTo(b.boundSize));

                    int key = UnityEngine.Random.Range(0, Fef.Instanc.hitef.Count);
                  var ef=  ObjPool.GetComponent<ParticleSystem>(Fef.Instanc.hitef[key], hitNode[0].col.bounds.center, Quaternion.identity, 5f);
                             AudioSource audio_=ef.gameObject.GetComponent<AudioSource>();
                             audio_.clip=Fef.Instanc.hitefAudioClips[UnityEngine.Random.Range(0,Fef.Instanc.hitefAudioClips.Count) ];
                     audio_.Play();
                     AudioSource.PlayClipAtPoint(Fef.Instanc.sliderAudioClips[UnityEngine.Random.Range(0,Fef.Instanc.sliderAudioClips.Count) ],hits[0].collider.bounds.center);

                    
                      int c = hitNode.Count - 1;//直接用gos.Count - 1或c
                      int cc=0;
                 
                    for (int j = c; j > -1; j--)
                  
                    {cc++;
                    
                       var n = hitNode[j];
                        n.Unfreeze();
                      //  n.render.material = BakeChunk.Instanc.mat;
                        // n.gameObject.SetActive(false);
                      
                  
                        n.rb.mass = n.boundSize * 1f;
                        if (cc>20)
                        { 
                            //n.render.material = BakeChunk.Instanc.mat;
                            n.gameObject.SetActive(false);
                        }else
                        {r=n.rb;
                          n. cm.unFrozenNode.Add(n);
                        }
                     
                        // print(n.boundSize+"  "+n.outside);
                       


                    }

                    for (var j = 0; j < hitNode.Count; j++)
                    {
                        var n = hitNode[j];


                        n.Touch();


                    }
                     r.AddExplosionForce(10000f, hitNode[0].col.bounds.center, 0.3f);
                }
                //体积从小到大

                  

            }
            else
            {
                targetPos = ray.GetPoint(100);
            }

            var bullet = ObjPool.GetComponent<bullet>(b, barrelEnd.position, barrelEnd.rotation, 2);
            // bullet.Init(targetPos); 
            bullet.gameObject.SetActive(true);

            //  var mat = bullet.GetComponent<Renderer>().material;
            //  mat.color = Color.red;
            //   mat.EnableKeyword("_EMISSION");
            // mat.SetColor("_EmissionColor", Color.white);

            var rb = bullet.rb;
            rb.velocity = transform.forward * Velocity;
            rb.mass = mass;
            //  rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;


        }
    }
}