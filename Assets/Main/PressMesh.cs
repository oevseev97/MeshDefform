using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(TouchObject))]
public class PressMesh : MonoBehaviour
{
    public float Radius = 1.5f;
    public float PressPower = 0.2f;
    public float PressRadius = 0.2f;

    [HideInInspector]
    public float deformationСoefficient = 1;

    public Action<Vector3> DeformationMesh;

    protected MeshFilter _meshFilter;
    protected MeshCollider _collider;
    protected TouchObject _touchObject;
    protected Vector3[] _vertexs;   

    public void Start()
    {
        deformationСoefficient = 1;
        _meshFilter = GetComponent<MeshFilter>();
        _collider = GetComponent<MeshCollider>();
        _touchObject = GetComponent<TouchObject>();      
        _touchObject.Touching += OnTouching;
        _vertexs = _meshFilter.sharedMesh.vertices;
    }

    public void OnTouching(Vector3 localContactPosition)
    {
        for (int i = 0; i < _vertexs.Length; i++)
        {
            float distance = (localContactPosition - _vertexs[i]).magnitude;

            if (distance < PressRadius)
            PressСalculate(i, Mathf.InverseLerp(0, PressRadius, distance) * PressPower);
        }
        if(DeformationMesh != null)
        DeformationMesh.Invoke( localContactPosition);
        ChekDeformationСoefficient();
        UpdateMesh();
    }

    private void PressСalculate(int indexVert, float force)
    {
        Vector3 etalonPoint = _vertexs[indexVert] * (Radius / _vertexs[indexVert].magnitude);
        _vertexs[indexVert] = _vertexs[indexVert] + (etalonPoint - _vertexs[indexVert]) * force;
     
    }

    private void ChekDeformationСoefficient()
    {
        float allVertexCof = 0;
        for (int i = 0; i < _vertexs.Length; i++)
        {
            Vector3 etalonPoint = _vertexs[i] * (Radius / _vertexs[i].magnitude);
            allVertexCof += (etalonPoint - _vertexs[i]).magnitude;
        }

        deformationСoefficient = allVertexCof / _vertexs.Length;
       // Debug.Log(deformationСoefficient);
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
