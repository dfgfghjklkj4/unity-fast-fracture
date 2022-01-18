using UnityEngine;
using Random = System.Random;

namespace Project.Scripts.Fractures
{
    public class FractureThis : MonoBehaviour
    { public FractureGO fgo;
    
        public LayerMask layer;
        [SerializeField] private Anchor anchor = Anchor.Bottom;
        [SerializeField] private int chunks = 500;
        [SerializeField] private float density = 50;
        [SerializeField] private float internalStrength = 100;

        //public float BreakForce;

        [SerializeField] private Material insideMaterial;
        [SerializeField] private Material outsideMaterial;

        private Random rng = new Random();
        public Material[] outmat;
        public Material[] intomat;
     
        private void Start()
        {

        
            Fracture.cmDIC.Add(this, new System.Collections.Generic.List<ChunkGraphManager>());
            for (int i = 0; i < 1; i++)
            {
                var temp = FractureGameobject();
                Fracture.cmDIC[this].Add(temp);
            }

            gameObject.SetActive(false);
        }

        public ChunkGraphManager FractureGameobject()
        {
            float t = Time.realtimeSinceStartup;
            var seed = rng.Next();
            ChunkGraphManager cm = Fracture.FractureGameObject(this,
                gameObject,
                anchor,
                seed,
                chunks,
                insideMaterial,
                outsideMaterial,
                internalStrength,
                density
            );
            cm.fgo=fgo;
            float t2 = Time.realtimeSinceStartup;
            //print(t2-t+"    ++++++++++++");
            return cm;
        }

        ///////////////////////////////////////////////


    }
}