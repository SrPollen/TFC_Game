using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPlayer : MonoBehaviour
{
    public Vector3 tpPos = new Vector3(530, 8, 48);
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hola");
            other.gameObject.GetComponent<CharacterController>().Move(tpPos);
        }
    }
}
