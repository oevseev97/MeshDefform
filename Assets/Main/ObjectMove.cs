using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public float speed = 10f;
    public Transform mainCamera;

    private Vector3 _localTauchPoint;
    private TouchObject _touchObject;
    private Quaternion _deltaRotate = Quaternion.identity;

    public void Start()
    {
        _touchObject = GetComponent<TouchObject>();
        _touchObject.Touching += OnTouching;
        _deltaRotate = transform.rotation;
    }

    public void Update()
    {
        _localTauchPoint = transform.position - mainCamera.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, _deltaRotate, speed * Time.deltaTime);
    }

    public void OnTouching(Vector3 localContactPosition)
    {
        Vector3 globalVectorTauch = transform.position - transform.TransformPoint(localContactPosition);
        _deltaRotate = Quaternion.FromToRotation(globalVectorTauch, _localTauchPoint) * transform.rotation;      
    }




}
