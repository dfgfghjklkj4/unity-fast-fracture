using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCollisionEvent : MonoBehaviour
{
    public Rigidbody rb;
    public AudioSource audio_;
    public ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        hitcol.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (groundcount)
        {
             if (rb.velocity.magnitude < 0.1f)
        {
          groundcount=false;
            {
                ps.Play();
            audio_.Play();
            
            }
        }
        }
       
    }
    HashSet<Collider> hitcol = new HashSet<Collider>();
    bool groundcount ;
    private void OnCollisionEnter(Collision other)
    {

        if (other.transform.CompareTag("ground"))
        {
            groundcount=true;
        }
       
        if (!hitcol.Contains(other.collider))
        {
            hitcol.Add(other.collider);
            //  print(other.collider.gameObject.name);
        }
    }
}
