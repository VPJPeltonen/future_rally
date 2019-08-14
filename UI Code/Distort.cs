using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distort : MonoBehaviour
{
public Material material;
private bool on = false;

    public bool On { get => on; set => on = value; }

    // Start is called before the first frame update
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (On){
            Graphics.Blit(src,dest,material);
        }else{
            Graphics.Blit(src,dest);
        }
    }
}
