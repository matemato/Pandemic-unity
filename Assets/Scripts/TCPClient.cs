// This work is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License. 
// To view a copy of this license, visit http://creativecommons.org/licenses/by-sa/4.0/ 
// or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    public Queue<byte> input;
    public Queue<byte> output;

    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private float tempTime;
    private const float outputTime = 0.1f;

    // Use this for initialization 	
    void Start()
    {
        input = new Queue<byte>();
        output = new Queue<byte>();
        ConnectToTcpServer();
    }
    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > outputTime)
        {
            tempTime = 0;
            SendOutput();
        }
    }
    private void PrintByteArray(byte[] bytes, string Type)
    {
        var sb = new StringBuilder(Type + ": { ");
        foreach (var b in bytes)
        {
            sb.Append(b + ", ");
        }
        sb.Append("}");
        Debug.Log(sb.ToString());
    }

    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient("localhost", 43594);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message.
                        for(int i = 0; i < length; i++)
                        {
                            input.Enqueue(incommingData[i]);
                        }
                        PrintByteArray(incommingData, "received");
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    private void SendOutput()
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                //string clientMessage = "This is a message from one of your clients.";
                // Convert string message to byte array.                 
                //byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                byte[] send = new byte[output.Count];
                for(int i = 0; i < send.Length; i++)
                {
                    send[i] = output.Dequeue();
                }
                // Write byte array to socketConnection stream.                 
                //stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                stream.Write(send);
                PrintByteArray(send, "sent");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}