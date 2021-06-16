using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Vector3 tpPos = new Vector3(530, 8, 48);
    
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameGlobalStats globalStats;
    [SerializeField] private GameObject tutorialText;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            globalStats.gameStarted = true;
            spawnManager.gameStarted = true;
            //FindObjectOfType<AudioManager>().Stop("TutorialVoice");
            tutorialText.SetActive(false);
            FindObjectOfType<AudioManager>().Stop("MainTheme");
            FindObjectOfType<AudioManager>().Play("BattleMusic");
            other.gameObject.GetComponent<CharacterController>().Move(tpPos);
        }
    }
}
