using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService
{
    private string _jsonApi;
    private const string webImage = "https://avatars.mds.yandex.net/i?id=a5841dd92c70f9093f1e7f729b150ed1-5573304-images-thumbs&n=13";
    private const string localApi = "http://weather/weather.php";

    public string city { get; set; }

    private IEnumerator CallAPI(string url, WWWForm form, Action<string> callback)
    {
        using (UnityWebRequest request = (form == null) ? UnityWebRequest.Get(url) : UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError("network problem: " + request.error);
            else if (request.responseCode != (long)System.Net.HttpStatusCode.OK)
                Debug.LogError("response error: " + request.responseCode);
            else
                callback(request.downloadHandler.text);
        }
    }

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        Debug.Log("Sending city: " + city);
        _jsonApi = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=0599ad315248da1ad9adf75b7a1f50b5";
        return CallAPI(_jsonApi, null, callback);
    }

    public IEnumerator DowmloadImage(Action <Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        yield return request.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(request));
    }

    public IEnumerator LodWether(string name, float cloudValue, Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("message", name);
        form.AddField("cloud_value", cloudValue.ToString());
        form.AddField("timestamp", DateTime.UtcNow.Ticks.ToString());

        return CallAPI(localApi, form, callback);
    }
}
