using UnityEngine;
using UnityEngine.UI;

public class GameGlobalStats : MonoBehaviour
{
    private float _playTimeHours, _minutes, _seconds;
    public bool endGame;

    private int _currentEnemies;

    [SerializeField] private SpawnManager spawnManager;
    
    [SerializeField] private Animator waveAnimator;
    
    [SerializeField] private Text waveCountdownText;
    
    [SerializeField] private Text waveNumberText;
    
    [SerializeField] private Text enemiesText;
    
    [SerializeField] private Text timerText;

    void Start()
    {
        _playTimeHours = 0;
        _currentEnemies = 0;
        _minutes = 0;
        _seconds = 0;
        InvokeRepeating(nameof(GetCurrentEnemies),5, 1);
    }

    void Update()
    {
        _minutes = (int)(Time.time / 60f);
        _seconds = (int)(Time.time % 60f);
        _playTimeHours += Time.deltaTime / 3600;
        timerText.text = "Tiempo: " + _minutes.ToString("00") + ":" + _seconds.ToString("00");

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
        enemiesText.text = "Enemigos: " + _currentEnemies;
    }
}
