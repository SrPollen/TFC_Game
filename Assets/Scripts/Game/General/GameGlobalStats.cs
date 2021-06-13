using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameGlobalStats : MonoBehaviour
{
    private float _playTimeHours, _minutes, _seconds;
    public bool endGame;
    private bool _isSended;


    private int _currentEnemies;

    [SerializeField] private SpawnManager spawnManager;
    
    //waves
    [SerializeField] private Animator waveAnimator;
    [SerializeField] private Text waveCountdownText;
    [SerializeField] private Text waveNumberText;
    
    //texts
    [SerializeField] private Text enemiesText;
    [SerializeField] private Text timerText;

    public bool gameStarted;
    
    public int Kills { get; set; }
    public int Damage { get; set; }
    
    
    [Serializable]
    public struct PutStructure
    {
        public int maxWave;
        public float playtime;
        public int kills;
        public int damage;
    }

    void Start()
    {
        gameStarted = false;
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
        
        if (!gameStarted) return;
        waveAnimator.SetTrigger("GameStarted");
        
        switch (spawnManager.state)
        {
            case SpawnManager.SpawnState.Counting:
                UpdateCountingUI();
                break;
            case SpawnManager.SpawnState.Spawning:
                UpdateSpawningUI();
                break;
        }
        
        if (endGame && !_isSended)
        {
            PutGame();
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
    
    public void PutGame()
    {
        _isSended = true;
        
        PutStructure data;
        data.maxWave = spawnManager.currentWave;
        data.playtime = _playTimeHours;
        data.kills = Kills;
        data.damage = Damage;
        
        StartCoroutine(Upload(data));
    }

    IEnumerator Upload(PutStructure data) {
        int id = PlayerPrefs.GetInt("PlayerID");
        Debug.Log(id);
        
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/update/" + id , "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();
 
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("Put " + request.result);
        if (request.result != UnityWebRequest.Result.Success) {
            Debug.Log(request.error);
        }
        else {
            Debug.Log("Upload complete!");
        }
    }
}
