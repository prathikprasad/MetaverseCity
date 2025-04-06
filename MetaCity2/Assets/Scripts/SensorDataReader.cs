using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI; // For TextMeshPro (recommended)

public class SensorDataReader : MonoBehaviour
{
    [Header("API Settings")]
    public string apiUrl = "http://10.27.238.192:5000/api/solar"; // Replace with your Python server IP
    public float updateInterval = 1f;

    [Header("UI Display")]
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI humidityText;
    public TextMeshProUGUI statusText;

    public Image humiditybar;
    public Image tempbar;

    public Slider timeslider;

    public GameObject chargingwire;
    public GameObject dischargingwire;

    void Start()
    {
        StartCoroutine(FetchDataRepeatedly());
    }

    IEnumerator FetchDataRepeatedly()
    {
        while (true)
        {
            yield return StartCoroutine(FetchData());
            yield return new WaitForSeconds(updateInterval);
        }
    }

    IEnumerator FetchData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            statusText.text = "Fetching data...";
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = webRequest.downloadHandler.text;
                //print(jsonResponse);
                ProcessJsonData(jsonResponse);
                statusText.text = "Data updated!";
            }
            else
            {
                statusText.text = $"Error: {webRequest.error}";
                Debug.LogError($"API Error: {webRequest.error}");
            }
        }
    }

    void ProcessJsonData(string jsonString)
    {
        try
        {
            // Simple JSON parsing without creating a class
            int tempStart = jsonString.IndexOf("\"temperature\":") + 14;
            int tempEnd = jsonString.IndexOf(",", tempStart);
            float temperature = float.Parse(jsonString.Substring(tempStart, tempEnd - tempStart).Replace(".", ","));
            //string temperature = (jsonString.Substring(tempStart, tempEnd - tempStart)).Replace(".",",");

            int humiStart = jsonString.IndexOf("\"humidity\":") + 11;
            int humiEnd = jsonString.IndexOf(",", humiStart);
            float humidity = float.Parse(jsonString.Substring(humiStart, humiEnd - humiStart).Replace(".", ","));

            /* 
            int intensityStart = jsonString.IndexOf("\"intensity\":") + 12;
            int intensityEnd = jsonString.IndexOf(",", intensityStart);
            float intensity = float.Parse(jsonString.Substring(intensityStart, intensityEnd - intensityStart).Replace(".", ","));
            */

            // Update UI
            temperatureText.text = "Temperature:" + temperature + "°C";
            humidityText.text = $"Humidity:"+humidity +"%";
            humiditybar.fillAmount = humidity / 100f; // Assuming humidity is between 0 and 100
            tempbar.fillAmount = temperature / 100f; // Assuming humidity is between 0 and 100

            if (humidity < 30 || humidity > 50)
            {
                humiditybar.color = Color.red;
            }
            else
            {
                humiditybar.color = Color.green;
            }

            if (humidity < 20 || humidity > 24)
            {
                tempbar.color = Color.red;
            }
            else
            {
                tempbar.color = Color.green;
            }

            if (timeslider.value < 0.25f || timeslider.value > 0.75)
            {
                chargingwire.SetActive(false);
                dischargingwire.SetActive(true);
            }
            else
            {
                chargingwire.SetActive(true);
                dischargingwire.SetActive(false);
            }



            Debug.Log(temperature + "and" + humidity);
        }
        catch (System.Exception e)
        {
            statusText.text = "Data parse error!";
            Debug.LogError($"Parse Error: {e}");
        }
    }
}