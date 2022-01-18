using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class jojo : MonoBehaviour
{
    public VideoPlayer vp;
    public GameObject vgo;
    public RenderTexture rt;
    public static bool finish;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate=1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        { 
            vp.Play();
           // vgo.SetActive(true);
           
        }

   if (vp.frame == 1)
        {
           
            vgo.SetActive(true);
           
        }

        if (vp.frame == (long)vp.frameCount-1)
        {print(88888888888888);
            vp.Stop();
            vgo.SetActive(false);
            finish=true;
        }
    }
}
