using UnityEngine;
using UnityEngine.UI;

public class GameGlobalStats : MonoBehaviour
{
    private float _minutes;
    public bool endGame;

    private int _currentEnemies;

    [SerializeField] private SpawnManager spawnManager;
    
    [SerializeField] private Animator waveAnimator;
    
    [SerializeField] private Text waveCountdownText;
    
    [SerializeField] private Text waveNumberText;
    
    [SerializeField] private Text enemiesText;

    void Start()
    {
        _currentEnemies = 0;
        InvokeRepeating(nameof(GetCurrentEnemies),5, 2);
    }

    void Update()
    {
        _minutes += Time.deltaTime / 60f;

        switch (spawnManager.state)
        {
            case SpawnManager.SpawnState.Counting:
                UpdateCountingUI();
                break;
            case SpawnManager.SpawnState.Spawning:
                UpdateSpawningUI();
                break;
        }

        if (endGame)
        {
            
        }
    }
    
    void UpdateCountingUI()
    {
        waveAnimator.SetTrigger("WaveCountdown");
        waveCountdownText.text = ((int)spawnManager.waveCountdown+1).ToString();
    }
    
    void UpdateSpawningUI()
    {
        waveAnimator.SetTrigger("WaveIncoming");
        waveNumberText.text = spawnManager.currentWave.ToString();
    }

    private void GetCurrentEnemies()
    {
        _currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
