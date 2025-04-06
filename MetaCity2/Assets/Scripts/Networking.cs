using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Threading;


public class Networking : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    StreamReader reader;
    Thread receiveThread;

    void Start()
    {
        client = new TcpClient("192.168.103.154", 80); // IP and port of the server device
        stream = client.GetStream();
        reader = new StreamReader(stream);

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveData()
    {
        while (true)
        {
            try
            {
                string data = reader.ReadLine();
                Debug.Log("Received: " + data);
            }
            catch (IOException) { break; } // Connection lost
        }
    }

    void OnApplicationQuit()
    {
        reader?.Close();
        stream?.Close();
        client?.Close();
        receiveThread?.Abort();
    }

}
