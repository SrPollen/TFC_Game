
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    //para que las barras de vida miren siempre a la camara
    
    public Transform cam;
    
    void LateUpdate()
    {
        //coge la posicion actual y muentra en la direccion de la camara
        transform.LookAt(transform.position +  cam.forward);
    }
}
