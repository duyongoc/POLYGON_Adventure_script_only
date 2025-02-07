using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class MyDatabase : MonoBehaviour
{


    // http://localhost:3001/
    // https://backend-zero-fe26.onrender.com


    [Header("CONFIG")]
    public bool runLocalServer = false;
    public string baseURL = string.Empty;


    [Header("DATA")]
    public DataActor[] data;
    public DataActor actor;
    public UserInfo userData;


    // [private]
    private string urlLocal = "http://localhost:3001";
    private string urlCloud = "https://backend-zero-fe26.onrender.com";



    #region UNITY
    private void Start()
    {
        Init();
    }

    // private void Update()
    // {
    // }
    #endregion




    public void Init()
    {
        if (runLocalServer)
            baseURL = urlLocal;
        else
            baseURL = urlCloud;

        // apiUser.GetData(baseURL);
        // apiCard.GetData(baseURL);
        // GetData();
    }


    private async void GetData()
    {
        // await Task_GET_Users();
        // await Task_POST_Users(userData);
        // await Task_PUT_Users(userData);
        // await Task_DELETE_Users(userData);

        // var param = new string[] { "admin", "admin" };
        // var param = JsonUtility.ToJson(new string[] { "admin", "admin" });
        // await Task_POST_Login(param);
        // await UniTask.Yield();

        // await Task_POST("/characters", actor);
        // await Task_GET("/characters", x => { data = JsonConvert.DeserializeObject<DataActor[]>(x); });
        // await Task_PUT("/characters", actor);
        await Task_DELETE("/characters", actor);
    }




    public async UniTask Task_GET(string uri, Action<string> callback = null)
    {
        var url = baseURL + uri;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", $"Bearer {CONST.TOKEN}");
            webRequest.SetRequestHeader("client-site", $"game");
            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"[API] GET: {url} | Exception {webRequest.error}");
                return;
            }

            var response = webRequest.downloadHandler.text;
            callback?.Invoke(response);
            Debug.Log($"[API] GET: {url} \nResponse: {response}");
        }
    }

    public async UniTask Task_POST(string uri, object data, Action<string> callback = null)
    {
        // use JsonConvert, JsonUtility not support array
        var json = JsonConvert.SerializeObject(data);
        var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        var url = baseURL + uri;

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.SetRequestHeader("Authorization", $"Bearer {CONST.TOKEN}");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"[API] POST: {url} | Exception {webRequest.error}");
                return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] POST: {url} \nResponse: {response}");
        }
    }

    public async UniTask Task_PUT(string uri, object data, Action<string> callback = null)
    {
        // use JsonConvert, JsonUtility not support array
        var json = JsonConvert.SerializeObject(data);
        var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        var url = baseURL + uri;

        // using (UnityWebRequest webRequest = UnityWebRequest.Put(url, json))
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "PUT"))
        {
            webRequest.SetRequestHeader("Authorization", $"Bearer {CONST.TOKEN}");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"[API] PUT: {url} | Exception {webRequest.error}");
                return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] PUT: {url} \nResponse: {response}");
        }
    }

    public async UniTask Task_DELETE(string uri, object data, Action<string> callback = null)
    {
        // use JsonConvert, JsonUtility not support array
        var json = JsonConvert.SerializeObject(data);
        var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        var url = baseURL + uri;

        // using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "DELETE"))
        {
            webRequest.SetRequestHeader("Authorization", $"Bearer {CONST.TOKEN}");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"[API] DELETE: {url} | Exception {webRequest.error}");
                return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] DELETE: {url} \nResponse: {response}");
        }
    }










    public async UniTask Task_POST_Login(string json)
    {
        var url = baseURL + "/login";
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            try
            {
                var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                await webRequest.SendWebRequest();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[API] Exception {ex}"); return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] Url: {url} \nResponse: {response}");
        }
    }



    #region CRUD METHODS
    public async UniTask Task_GET_Users(Action<string> callback = null)
    {
        var url = baseURL + "/users";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            try
            {
                webRequest.SetRequestHeader("client-site", $"game");
                await webRequest.SendWebRequest();
            }
            catch (Exception ex)
            {
                Debug.Log($"[API] Exception {ex}");
                return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] Url: {url} \nResponse: {response}");
        }
    }

    public async UniTask Task_POST_Users(UserInfo user)
    {
        var url = baseURL + "/users";
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            try
            {
                var json = JsonUtility.ToJson(user);
                var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                await webRequest.SendWebRequest();
            }
            catch (Exception ex)
            {
                Debug.Log($"[API] Exception {ex}"); return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] Url: {url} \nResponse: {response}");
        }
    }

    public async UniTask Task_PUT_Users(UserInfo user)
    {
        var url = baseURL + "/users";
        var json = JsonUtility.ToJson(user);
        // using (UnityWebRequest webRequest = new UnityWebRequest(url, "PUT"))
        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, json))
        {
            try
            {
                // var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
                // webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                // webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                await webRequest.SendWebRequest();
            }
            catch (Exception ex)
            {
                Debug.Log($"[API] Exception {ex}"); return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] Url: {url} \nResponse: {response}");
        }
    }

    public async UniTask Task_DELETE_Users(UserInfo user)
    {
        var url = baseURL + "/users";
        var json = JsonUtility.ToJson(user);
        // using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "DELETE"))
        {
            try
            {
                var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                await webRequest.SendWebRequest();
            }
            catch (Exception ex)
            {
                Debug.Log($"[API] Exception {ex}");
                return;
            }

            var response = webRequest.downloadHandler.text;
            Debug.Log($"[API] Url: {url} \nResponse: {response}");
        }
    }
    #endregion



}
