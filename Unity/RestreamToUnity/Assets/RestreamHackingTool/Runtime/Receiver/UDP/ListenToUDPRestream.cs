using RestreamChatHacking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using RestreamChatHacking;

public class ListenToUDPRestream : MonoBehaviour
{
    public int portToListen=2512;
    public UDPReceive m_udpReceiver;
    public string m_lastMessage;
    public RestreamChatMessage m_lastRCM;
    public RestreamMessageUnityEvent m_receivedMessage;
    void Start()
    {
        m_udpReceiver = new UDPReceive(portToListen);
    }
    private void Update()
    {
        m_lastMessage = m_udpReceiver.m_lastReceivedUDPPacket;
        string msg = null;
        do
        {
            if (m_udpReceiver.m_allReceivedUDPPackets.Count > 0)
            {
                msg = m_udpReceiver.m_allReceivedUDPPackets.Dequeue();
                if (msg != null) { 
                  // Debug.Log(msg);
                    m_lastRCM.SetWithOneLiner(msg.Trim());
                    if (m_lastRCM.IsCorrectlyDefined())
                        m_receivedMessage.Invoke(m_lastRCM.Duplicate()) ;
                    m_lastRCM.Reset();
                }
            }
            else msg = null;
        }
        while (msg != null);
    }
    void OnDisable()
    {
        m_udpReceiver.Kill();
        
    }
}


public class UDPReceive 
{

    Thread m_receiveThread;

    UdpClient m_client;

    public int m_port; 
    public string m_lastReceivedUDPPacket = "";
    public Queue<string> m_allReceivedUDPPackets= new Queue<string>();

    public UDPReceive(int port) {
        m_port = port;
        init();
    }

    

    // init
    private void init()
    {
        
        m_receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        m_receiveThread.IsBackground = true;
        m_receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {

        m_client = new UdpClient(m_port);
        while (true)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = m_client.Receive(ref anyIP);
                string text = Encoding.Unicode.GetString(data);
                m_lastReceivedUDPPacket = text;
                m_allReceivedUDPPackets.Enqueue(text);

            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }

    public string getLatestUDPPacket()
    {
       
        return m_lastReceivedUDPPacket;
    }

    public void Kill()
    {
        if (m_receiveThread != null)
            m_receiveThread.Abort();

        m_client.Close();
    }
}