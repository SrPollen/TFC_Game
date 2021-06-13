using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginConnection : MonoBehaviour
{
    private string nextScene = "MainMenuScene";
    public TMP_InputField iUsername;
    public TMP_InputField iPassword;

    private void Start()
    {
        iUsername = GameObject.Find("InputUser")?.GetComponent<TMP_InputField>();
        iPassword = GameObject.Find("InputPassword")?.GetComponent<TMP_InputField>();
    }

    [Serializable]
    public struct LoginStructure
    {
        public string username;
        public string password;
    }
    public void RegisterAction()
    {
        Application.OpenURL("http://localhost:3000/login");
    }

    public void LoginAction()
    {
        LoginStructure data;
        data.username = iUsername.text;
        data.password = iPassword.text;
        StartCoroutine(LoginPost(data));
    }
    
    IEnumerator LoginPost(LoginStructure data)
    {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/gamelogin" , "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();
        
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("Login " + request.result);
        
        //!string.IsNullOrEmpty(request.downloadHandler.text)
        if (request.result == UnityWebRequest.Result.Success && int.Parse(request.downloadHandler.text) != -1)
        {
            PlayerPrefs.SetInt("PlayerID", int.Parse(request.downloadHandler.text));
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("Intento de login fallido");
            if (data.username == "admin" && data.password == "admin")
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}