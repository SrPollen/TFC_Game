using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LoginConnection : MonoBehaviour
{
    private readonly HttpClient _client = new HttpClient();
    
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
        data.username = "Caracola111";
        data.password = "r1234";
        StartCoroutine(LoginPost(data));
    }
    
    IEnumerator LoginPost(LoginStructure data)
    {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/login" , "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();
        
        Debug.Log("Status Code: " + request.responseCode);
        
        if (request.result == UnityWebRequest.Result.Success && !string.IsNullOrEmpty(request.downloadHandler.text))
        {
            Debug.Log(request.downloadHandler.text);
            Debug.Log("Login correcto " + request.result);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    public async void CallApi()
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync("https://pokeapi.co/api/v2/pokemon/ditto");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            Debug.Log(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Exception Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}