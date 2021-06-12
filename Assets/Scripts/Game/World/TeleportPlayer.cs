using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Vector3 tpPos = new Vector3(530, 8, 48);
    
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameGlobalStats globalStats;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            globalStats.gameStarted = true;
            spawnManager.gameStarted = true;
            other.gameObject.GetComponent<CharacterController>().Move(tpPos);
        }
    }
}
