using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReflexsionPress : MonoBehaviour
{
    public float colorRadius = 20;
    public Color colorTarget = Color.black;
    public Texture2D mainTexture;

    private Renderer _meshRender;
    private PressMesh _pressMesh;


    private const float RANGE_FACTOR_POINT = 1.5f;

    public void Start()
    {
        _meshRender = GetComponent<Renderer>();
        _pressMesh = GetComponent<PressMesh>();      
        _pressMesh.DeformationMesh += OnUpdateColor;
        LoadTexture();
    }

    private void LoadTexture()
    {
        Texture2D tempTexture = new Texture2D(mainTexture.width, mainTexture.height);
        Graphics.CopyTexture(mainTexture, tempTexture);
        _meshRender.sharedMaterial.SetTexture("_MainTex", tempTexture);
    }

    private void OnUpdateColor(Vector3 point)
    {
        RaycastHit hit;
        Vector3 dir = (transform.position - transform.TransformPoint(point)) * RANGE_FACTOR_POINT;
        Physics.Raycast(transform.TransformPoint(point) - dir, dir * RANGE_FACTOR_POINT, out hit);
        Vector2 pixelUV = hit.textureCoord;


        Texture2D tex = _meshRender.sharedMaterial.GetTexture("_MainTex") as Texture2D;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        int xCor = (int)pixelUV.x;
        int yCor = (int)pixelUV.y;

        Vector2 posCor = new Vector2(xCor, yCor);
                   
          int index = 0;
          for(int y = 0; y < tex.height; y++)
          {
              for(int x = 0; x < tex.width; x++)
              {
                    index++;
                    Vector2 pos = new Vector2(x, y);
                    if ((pos - posCor).magnitude < colorRadius)
                    {
                        Color temp = tex.GetPixel(x, y);
                        tex.SetPixel(x, y, Blend(temp, colorTarget, (pos - posCor).magnitude / colorRadius));
                    }
              }
          }
        tex.Apply();
       
    }

    private Color Blend(Color colorPixel, Color targetColor, float cof)
    {
        float blend = Mathf.Lerp(255, 0, cof);
        float R = colorPixel.r + (targetColor.r - colorPixel.r) * blend / 255;
        float G = colorPixel.g + (targetColor.g - colorPixel.g) * blend / 255;
        float B = colorPixel.b + (targetColor.r - colorPixel.b) * blend / 255;
        Color result = new Color(R,G,B);
        return result;
    }
 
}
