
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class HighScoreResult
{
    public int score;
    public string deviceIdentifier;
    public string level;
    public string code; // error code
    public string message; // error message
}

[Serializable]
public class HighScoreResults
{
    public HighScoreResult[] results;
}

public class RemoteHighScoreManager : MonoBehaviour
{
    public static RemoteHighScoreManager Instance { get; private set; }

    private IEnumerator coroutineSend;
    private IEnumerator coroutineReceive;

    void Awake()
    {
        // force singleton instance
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }

        // do not destroy this object when we load the scene
        DontDestroyOnLoad(gameObject);
    }

    public void GetHighScore(String level, Action<int> OnCompleteCallback)
    {
        coroutineReceive = GetHighScoreCR(level, OnCompleteCallback);
        StartCoroutine(coroutineReceive);

    }

    public void SetHighScore(int score, String level, Action OnCompleteCallback)
    {
        coroutineSend = SetHighScoreCR(score, level, OnCompleteCallback);
        StartCoroutine(coroutineSend);
    }

    public IEnumerator GetHighScoreCR(String level, Action<int> OnCompleteCallback)
    {
        // Construct the url for our request, including the objectid.
        const string tableName = "HighScore";
        const string equals = "%3D";
        const string DC4 = "%20";
        string deviceIdentifier = SystemInfo.deviceUniqueIdentifier;

        const string url = "https://eu-api.backendless.com/" +
            Globals.APPLICATION_ID + "/" +
            Globals.REST_SECRET_KEY +
            "/data/" +
            tableName;
        string query = "?where=" +
            "deviceIdentifier" + equals + "'" + deviceIdentifier + "'" +
            DC4 + "AND" + DC4 +
            "level" + equals + "'" + level + "'";

        // Create a GET UnityWebRequest, passing in our URL
        UnityWebRequest webreq = UnityWebRequest.Get(url + query);

        // Set the request headers as dictated by the backendless documentation
        // (3 headers)
        webreq.SetRequestHeader("application-id", Globals.APPLICATION_ID);
        webreq.SetRequestHeader("secret-key", Globals.REST_SECRET_KEY);
        webreq.SetRequestHeader("application-type", "REST");

        // Send the webrequest and yield (so the script waits until it returns
        // with a result)
        yield return webreq.SendWebRequest();

        // Check for webrequest errors
        if (webreq.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("ConnectionError");
        }
        else
        {
            // Unity does not support top-level JSON deserialisation currently.
            // The text must be modified.
            string json = "{ \"results\": " + webreq.downloadHandler.text + "}";

            // Serialize the downloadHandler.text property to HighScoreResult
            HighScoreResults highScoreResults = JsonUtility.FromJson<HighScoreResults>(json);

            if (highScoreResults.results.Length == 0)
            {
                Debug.Log("High score result does not currently exist");
                OnCompleteCallback(0);
                yield break;
            }

            HighScoreResult highScoreData = highScoreResults.results[0];

            // Check for backendless errors
            if (!string.IsNullOrEmpty(highScoreData.code))
            {
                Debug.Log("Error:" + highScoreData.code + " " + highScoreData.message);
            }

            // Call the callback function, passing the score as the parameter
            OnCompleteCallback(highScoreData.score);
        }
    }

    public IEnumerator SetHighScoreCR(int score, String level, Action OnCompleteCallback)
    {
        // Construct the url for our request, including the objectid.
        const string tableName = "HighScore";
        const string equals = "%3D";
        const string DC4 = "%20";
        string deviceIdentifier = SystemInfo.deviceUniqueIdentifier;

        const string url = "https://eu-api.backendless.com/" +
            Globals.APPLICATION_ID + "/" +
            Globals.REST_SECRET_KEY +
            "/data/bulk/" +
            tableName;
        string query = "?where=" +
            "deviceIdentifier" + equals + "'" + deviceIdentifier + "'" +
            DC4 + "AND" + DC4 +
            "level" + equals + "'" + level + "'";

        // Construct JSON string of data we want to send.
        // Object would store message and code in DB, which we don't want
        // string data = "{\"score\": " + score + "}";
        string data =
            "{" +
                "\"score\": " + score + "," +
                "\"deviceIdentifier\": " + "\"" + deviceIdentifier + "\"" + "," +
                "\"level\": " + "\"" + level + "\"" +
            "}";

        // Create PUT UnityWebRequest passing our url and JSON data
        UnityWebRequest webreq = UnityWebRequest.Put(url + query, data);

        // set the request headers as dictated by the backendless documentation
        // (4 headers)
        webreq.SetRequestHeader("Content-Type", "application/json");
        webreq.SetRequestHeader("application-id", Globals.APPLICATION_ID);
        webreq.SetRequestHeader("secret-key", Globals.REST_SECRET_KEY);
        webreq.SetRequestHeader("application-type", "REST");

        // Send the webrequest and yield (so the script waits until it returns
        // with a result)
        yield return webreq.SendWebRequest();

        // Check for webrequest errors
        if (webreq.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("ConnectionError");
        }
        else if (webreq.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ProtocolError");
        }
        else if (webreq.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("DataProcessingError");
        }
        else
        {
            // Response body contains the number of objects updated
            // If none were updated it means we have no score for this level/device yet
            if ((Int32.Parse(webreq.downloadHandler.text) > 0))
            {
                OnCompleteCallback();
            }
            else 
            {
                // Send another request to set data
                const string setUrl = "https://eu-api.backendless.com/" +
                    Globals.APPLICATION_ID + "/" +
                    Globals.REST_SECRET_KEY +
                    "/data/" +
                    tableName;
                UnityWebRequest setWebreq = UnityWebRequest.Put(setUrl, data);

                setWebreq.SetRequestHeader("Content-Type", "application/json");
                setWebreq.SetRequestHeader("application-id", Globals.APPLICATION_ID);
                setWebreq.SetRequestHeader("secret-key", Globals.REST_SECRET_KEY);
                setWebreq.SetRequestHeader("application-type", "REST");

                yield return setWebreq.SendWebRequest();

                if (setWebreq.result == UnityWebRequest.Result.Success)
                {
                    OnCompleteCallback();
                }
            }
        }
    }
}
