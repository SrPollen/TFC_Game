using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiStats: MonoBehaviour
{
    public int Id { get; set; }
    public int Waves { get; set; }
    public int PlayTime { get; set; }
    public int Kills { get; set; }
    public int Damage { get; set; }
    
    [Serializable]
    public struct PutStructure
    {
        public int maxWave;
        public int playtime;
        public int kills;
        public int damage;
    }
    
    public void PutGame()
    {
        PutStructure data;
        data.maxWave = Waves;
        data.playtime = PlayTime;
        data.kills = Kills;
        data.damage = Damage;
        
        StartCoroutine(Upload(data));
    }

    IEnumerator Upload(PutStructure data) {

        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/update/" + Id , "PUT");
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
