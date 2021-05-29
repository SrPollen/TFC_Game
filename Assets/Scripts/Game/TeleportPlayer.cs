using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Vector3 tpPos = new Vector3(530, 8, 48);
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterController>().Move(tpPos);
        }
    }
}
