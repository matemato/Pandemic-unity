// This work is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License. 
// To view a copy of this license, visit http://creativecommons.org/licenses/by-sa/4.0/ 
// or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TCPClient
{
    public Queue<byte> input;
    public Queue<byte> output;

    private TcpClient socketConnection;

    public TCPClient()
    {
        input = new Queue<byte>();
        output = new Queue<byte>();
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
    public bool ConnectToTcpServer(string ip)
    {
        const int connectTimeoutMilliseconds = 2000;

        socketConnection = new TcpClient();
        var connectionTask = socketConnection
            .ConnectAsync(ip, 43594).ContinueWith(task => {
                return task.IsFaulted ? null : socketConnection;
            }, TaskContinuationOptions.ExecuteSynchronously);
        var timeoutTask = Task.Delay(connectTimeoutMilliseconds)
            .ContinueWith<TcpClient>(task => null, TaskContinuationOptions.ExecuteSynchronously);
        var resultTask = Task.WhenAny(connectionTask, timeoutTask).Unwrap();

        var resultTcpClient = resultTask.GetAwaiter().GetResult();
        // Or by using `await`:
        // var resultTcpClient = await resultTask.ConfigureAwait(false);

        if (resultTcpClient != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    public void ReadInput()
    {
        Byte[] bytes = new Byte[1024];
        if(socketConnection != null && socketConnection.Connected)
        {
            // Get a stream object for reading 				
            NetworkStream stream = socketConnection.GetStream();
            if (stream.DataAvailable)
            {
                int length;
                // Read incomming stream into byte arrary. 					
                if ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);
                    // Convert byte array to string message.
                    for (int i = 0; i < length; i++)
                    {
                        input.Enqueue(incommingData[i]);
                    }
                    PrintByteArray(incommingData, "received");
                }
            }
        }
    }

    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public void SendOutput()
    {
        if (socketConnection == null || !socketConnection.Connected)
        {
            return;
        }
        try
        {
            // Get a stream object for writing.	
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite && output.Count > 0)
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