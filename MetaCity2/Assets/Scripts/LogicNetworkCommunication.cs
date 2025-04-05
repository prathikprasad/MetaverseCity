using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNetworkCommunication : MonoBehaviour
{
    public MQTT_Custom_Client logicClientMQTT;

    public void SendMessage(string topic, string payload)
    {
        // MQTT
        string prefix = "touchpad/";
        logicClientMQTT.Publish(prefix + topic, payload);

        // ProtoPie
        // TBD
    }

}
