using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fef : MonoBehaviour
{
    public static Fef Instanc;
    public List<ParticleSystem> hitef;
       public List<AudioClip> hitefAudioClips;
     public List<ParticleSystem> dustef;
      public List<AudioClip> dustefAudioClips;
        public List<AudioClip> sliderAudioClips;
    
    // Start is called before the first frame update
    void Start()
    {
        Instanc=this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
