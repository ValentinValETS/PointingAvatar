using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPCommunication : MonoBehaviour
{
    public string captureName = "";
    public string captureDatabasePath = "C:/temp";
    public string IPAddress;
    private int captureID;
    private UdpClient udpClient;
    //private bool isStarted;
    //private byte[] receivedData;
    //private IPEndPoint sender;
    //public string dataString;
    private int counter;
    private bool start;

    public void Start()
    {
        udpClient = new UdpClient(30);
        udpClient.Client.Blocking = false;
        udpClient.Client.SendTimeout = 2000;
        //udpClient.Client.ReceiveTimeout = 2000;
        //receivedData = new byte[0];
        //sender = new IPEndPoint(System.Net.IPAddress.Any, 30);
        int UDPID = ReadID();
        captureID = UDPID;
        //isStarted = false;
        counter= 0;
        start = true;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    StartCoroutine(SendPacket(true, captureName+ counter, "C:/temp"));
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    StartCoroutine(SendPacket(false, captureName+ counter, "C:/temp"));
        //}
       //ReceiveData();
    }

    public void SendPacket()
    {
        string s = "'Start'";
        if (!start) s = "'Stop'";
        captureID += 1;
        try
        {   
            udpClient.Connect(IPAddress, 30);
            byte[] sendBytes = null;
            // Sends a message to the host to which you have connected.
            //string text = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><CaptureStart><Name VALUE=\"Test\"/><DatabasePath VALUE=\"C:/ Temp\"/><PacketID VALUE=\"0\"/></CaptureStart>";
            if (start)
            {
                captureName = "Test" + "_"+ DateTime.Now.ToString("yyyyMMddHHmmss");
                sendBytes = Encoding.ASCII.GetBytes(StartUDP(captureName, captureDatabasePath));
                //Debug.Log(StartUDP(captureName + counter, captureDatabasePath));
                //start = false;
            }
            else
            {
                sendBytes = Encoding.ASCII.GetBytes(StopUDP(captureName, captureDatabasePath));
                //Debug.Log(StopUDP(captureName + counter, captureDatabasePath));
                counter += 1;
                //start = true;
            }
      

            udpClient.Send(sendBytes, sendBytes.Length);

            start = !start;

            //udpClient.Close();
            //isStarted = !isStarted;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }        
        Debug.Log("Message "+ s +" sent to Tracker");
        SaveID();
        //yield return null;
    }

    //public void SendPacket(bool start,string name)
    //{
    //    string s = "'Start'";
    //    if (!start) s = "'Stop'";
    //    captureID += 1;
    //    try
    //    {
    //        udpClient.Connect(IPAddress, 30);
    //        byte[] sendBytes = null;
    //        // Sends a message to the host to which you have connected.
    //        //string text = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><CaptureStart><Name VALUE=\"Test\"/><DatabasePath VALUE=\"C:/ Temp\"/><PacketID VALUE=\"0\"/></CaptureStart>";
    //        if (start)
    //        {
                
    //            sendBytes = Encoding.ASCII.GetBytes(StartUDP(name, captureDatabasePath));
    //            //Debug.Log(StartUDP(captureName + counter, captureDatabasePath));
    //            //start = false;
    //        }
    //        else
    //        {
    //            sendBytes = Encoding.ASCII.GetBytes(StopUDP(name, captureDatabasePath));
    //            //Debug.Log(StopUDP(captureName + counter, captureDatabasePath));
    //            counter += 1;
    //            //start = true;
    //        }


    //        udpClient.Send(sendBytes, sendBytes.Length);

    //        //udpClient.Close();
    //        //isStarted = !isStarted;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e.ToString());
    //    }
    //    Debug.Log("Message " + s + " sent to Tracker");
    //    SaveID();
    //    //yield return null;
    //}


    string StartUDP(string captureName, string captureDatabasePath)
    {
       string s ="<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>" 
       + "<CaptureStart>"
       + "<Name VALUE=\"" + captureName + "\"/>" 
       + "<DatabasePath VALUE=\"" + captureDatabasePath + "\"/>" 
       + "<PacketID VALUE=\"" + captureID + "\"/>"
       + "</CaptureStart>";

        return s;
    }

    string StopUDP(string captureName, string captureDatabasePath)
    {
       string s = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>" 
       + "<CaptureStop>"
       + "<Name VALUE=\"" + captureName + "\"/>" 
       + "<DatabasePath VALUE=\"" + captureDatabasePath + "\"/>" 
       + "<PacketID VALUE=\"" + captureID + "\"/>"
       + "</CaptureStop>";

        return s;
    }

    int ReadID()
    {
        List<string> lines = null;
        string[] IDFile = null;
        string dataPath = Application.streamingAssetsPath + "/Data/";
        string fileName = dataPath + "UDP_ID.txt";
        int IDInt = 0;

        if (File.Exists(fileName))
        {
            IDFile = System.IO.File.ReadAllLines(fileName);
            lines = new List<string>(IDFile);
            string IDString = lines[0];
            IDInt = Int32.Parse(IDString);
        }
        else 
        {
            Debug.Log("No UDP ID file found, ID = 0");          
        }

        return IDInt;
    }

    void SaveID()
    {
        string dataPath = Application.streamingAssetsPath + "/Data/";

        using (StreamWriter writer = new StreamWriter(dataPath + "UDP_ID.txt", false))
        {    
            writer.WriteLine(captureID); 
        }
    }

    //public void ReceiveData()
    //{
    //    if (udpClient.Available > 0)
    //    {
    //        receivedData = udpClient.Receive(ref sender);
    //        dataString = Encoding.ASCII.GetString(receivedData);

    //        if (Encoding.ASCII.GetString(receivedData) == "jump")
    //        {
    //            //
    //        }
    //    }
    //    else
    //    {
    //        print("No data to receive");
    //    }
    //}

}