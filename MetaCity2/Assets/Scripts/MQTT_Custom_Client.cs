using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using TMPro;

public class MQTT_Custom_Client : M2MqttUnityClient
{
    public bool printReceivedMessages;

    private string prefix = "touchpad/";

    [Header("Output Variables")] // REMOVE THIS AT SOME POINT AND MOVE TO ACTIONS
    public bool TouchUp;
    public bool OnTouchDown;
    

    //[Header("UI LOGIC REFERENCES")]
    public static event Action OnFunction_TouchUp;
    public static event Action OnFunction_OnTouchUp;
    public static event Action OnFunction_OnTouchDown;
    public static event Action<float> OnFunction_Rotate;
    public static event Action<float,float> OnFunction_Translate;
    public static event Action OnFunction_OnRotationMode;
    public static event Action OnFunction_OnTranslationMode;

    // GAZE
    public static event Action<float> OnFunction_DeltaGazePosNormalizedX;

    // GESTURE
    public static event Action OnFunction_OnDragStart;
    public static event Action OnFunction_OnDragEnd;
    public static event Action<float> OnFunction_DeltaDragPosNormalizedX;

    // MFL
    public static event Action OnFunction_OnMFLUp;
    public static event Action OnFunction_OnMFLDown;



    public void Publish(string topic, string payload)
    {
        if(client == null || client.IsConnected == false)
        {
            print("MQTT -> CLIENT NULL");
        }
        else
        { 
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(payload), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }

    public void SetBrokerAddress(string brokerAddress)
    {
        this.brokerAddress = brokerAddress;
    }

    public void SetBrokerPort(string brokerPort)
    {
        int.TryParse(brokerPort, out this.brokerPort);
    }

    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }

    protected override void OnConnecting()
    {
        base.OnConnecting();
        //print("MQTT -> Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        //print("MQTT -> Connected to broker on " + brokerAddress + "\n");
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { prefix + "TouchUp" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { prefix + "OnTouchUp" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { prefix + "GestureMode" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { prefix + "OnTouchDown" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { prefix + "Rotate" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { prefix + "Translate" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { prefix + "OnRotationModeEnter" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { prefix + "OnTranslationModeEnter" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        // Gaze
        client.Subscribe(new string[] { "spatial/DeltaGazePosNormalizedX" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        // Gesture
        client.Subscribe(new string[] { "spatial/OnDragStart" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "spatial/OnDragEnd" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "spatial/DeltaDragPosNormalizedX" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        // MFL
        client.Subscribe(new string[] { "mfl" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        //client.Subscribe(new string[] { "mfl/MFL_DOWN" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { prefix + "TouchUp" });
        client.Unsubscribe(new string[] { prefix + "OnTouchUp" });
        client.Unsubscribe(new string[] { prefix + "GestureMode" });
        client.Unsubscribe(new string[] { prefix + "OnTouchDown" });
        client.Unsubscribe(new string[] { prefix + "Rotate" });
        client.Unsubscribe(new string[] { prefix + "Translate" });
        client.Unsubscribe(new string[] { prefix + "OnRotationModeEnter" });
        client.Unsubscribe(new string[] { prefix + "OnTranslationModeEnter" });

        // Gaze
        client.Unsubscribe(new string[] { "spatial/DeltaGazePosNormalizedX" });

        // Gesture
        client.Unsubscribe(new string[] { "spatial/OnDragStart" });
        client.Unsubscribe(new string[] { "spatial/OnDragEnd" });
        client.Unsubscribe(new string[] { "spatial/DeltaDragPosNormalizedX" });

        // mfl
        client.Unsubscribe(new string[] { "mfl" });
        //client.Unsubscribe(new string[] { "mfl/MFL_DOWN" });
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        print("MQTT -> CONNECTION FAILED! " + errorMessage);
    }

    protected override void OnDisconnected()
    {
        print("MQTT -> Disconnected.");
    }

    protected override void OnConnectionLost()
    {
        print("MQTT -> CONNECTION LOST!");
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);

        if (printReceivedMessages)
        {
            //Debug.Log("MQTT Received - Topic: " + topic + " Payload: " + msg);
        }

        /*

        // EXAMPLE

        */
        if (topic == prefix + "TouchUp")
        {
            OnFunction_TouchUp?.Invoke(); // Trigger the event to notify subscribers

            TouchUp = true;
            OnTouchDown = false;
        }
        else if (topic == prefix + "OnTouchUp")
        {
            OnFunction_OnTouchUp?.Invoke(); // Trigger the event to notify subscribers
        }
        else if (topic == prefix + "OnTouchDown")
        {
            OnFunction_OnTouchDown?.Invoke(); // Trigger the event to notify subscribers

            OnTouchDown = true;
            TouchUp = false;
        }
        else if (topic == prefix + "Rotate")
        {
            float valueR = float.Parse(msg);
            OnFunction_Rotate?.Invoke(valueR); // Trigger the event to notify subscribers

            TouchUp = false;
            OnTouchDown = false;
        }
        else if (topic == prefix + "Translate")
        {
            // Split the input string by the comma
            string[] values = msg.Split(',');

            // Parse each value as a float
            float valueX = float.Parse(values[0]);
            float valueY = float.Parse(values[1]);

            OnFunction_Translate?.Invoke(valueX, valueY); // Trigger the event to notify subscribers

            TouchUp = false;
            OnTouchDown = false;
            
        }
        else if (topic == prefix + "OnRotationModeEnter")
        {
            OnFunction_OnRotationMode?.Invoke(); // Trigger the event to notify subscribers
        }
        else if (topic == prefix + "OnTranslationModeEnter")
        {
            OnFunction_OnTranslationMode?.Invoke(); // Trigger the event to notify subscribers
        }
        else if (topic == "spatial/DeltaGazePosNormalizedX")
        {
            float valueX = float.Parse(msg);
            OnFunction_DeltaGazePosNormalizedX?.Invoke(valueX); // Trigger the event to notify subscribers
        }
        else if (topic == "spatial/OnDragStart")
        {
            OnFunction_OnDragStart?.Invoke(); // Trigger the event to notify subscribers
        }
        else if (topic == "spatial/OnDragEnd")
        {
            OnFunction_OnDragEnd?.Invoke(); // Trigger the event to notify subscribers
        }
        else if (topic == "spatial/DeltaDragPosNormalizedX")
        {
            float valueX = float.Parse(msg);
            OnFunction_DeltaDragPosNormalizedX?.Invoke(valueX); // Trigger the event to notify subscribers
        }
        else if (topic == "mfl")
        {
            if (msg == "MFL_UP")
            {
                OnFunction_OnMFLUp?.Invoke(); // Trigger the event to notify subscribers
            }
            else if(msg == "MFL_DOWN")
            {
                OnFunction_OnMFLDown?.Invoke();
            }   
        }
    }

    protected override void Update()
    {
        base.Update(); // call ProcessMqttEvents()
    }

    private void OnDestroy()
    {
        Disconnect();
    }
}

