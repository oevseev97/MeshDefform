using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CastomPress : PressMesh
{   
    private Vector3[] _vertexsTarget;
    public new void Start()
    {
        deformation—oefficient = 1;
        _meshFilter = GetComponent<MeshFilter>();
        _collider = GetComponent<MeshCollider>();
        _touchObject = GetComponent<TouchObject>();
        _touchObject.Touching += OnTouching;
        _vertexsTarget = _meshFilter.sharedMesh.vertices;
        GetTargetVertex();
    }

    private void GetTargetVertex()
    {
        ToSphere();       
    }

    private void ToSphere()
    {
        _vertexs = _meshFilter.sharedMesh.vertices;
        for (int i = 0; i < _vertexs.Length; i++)
        {
            _vertexs[i] = _vertexs[i] * (Radius / _vertexs[i].magnitude);
        }
        UpdateMesh();
    }

    public new void OnTouching(Vector3 localContactPosition)
    {
        for (int i = 0; i < _vertexs.Length; i++)
        {
            float distance = (localContactPosition - _vertexs[i]).magnitude;

            if (distance < PressRadius)
                Press—alculate(i, Mathf.InverseLerp(0, PressRadius, distance) * PressPower);
        }
        if (DeformationMesh != null)
            DeformationMesh.Invoke(localContactPosition);
        ChekDeformation—oefficient();
        UpdateMesh();
    }

    private void Press—alculate(int indexVert, float force)
    {       
        _vertexs[indexVert] = _vertexs[indexVert] + (_vertexsTarget[indexVert] - _vertexs[indexVert]) * force;
       
    }

    private void ChekDeformation—oefficient()
    {
        float allVertexCof = 0;
        for (int i = 0; i < _vertexs.Length; i++)
        {           
            allVertexCof += (_vertexsTarget[i] - _vertexs[i]).magnitude;
        }

        deformation—oefficient = allVertexCof / _vertexs.Length;
        Debug.Log(deformation—oefficient);
    }

    private void UpdateMesh()
    {
        _meshFilter.mesh.MarkDynamic();
        _meshFilter.sharedMesh.vertices = _vertexs;
        _meshFilter.sharedMesh.RecalculateNormals();
        _meshFilter.sharedMesh.RecalculateBounds();
        _collider.sharedMesh = _meshFilter.sharedMesh;

    }
}
