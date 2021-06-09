
using System;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    //para que las barras de vida miren siempre a la camara
    
    private Transform _cam;

    private void Awake()
    {
        _cam = GameObject.Find("Main Camera").transform;
    }

    void LateUpdate()
    {
        //coge la posicion actual y muentra en la direccion de la camara
        transform.LookAt(transform.position +  _cam.forward);
    }
}
