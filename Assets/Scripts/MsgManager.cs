using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgManager : MonoBehaviour
{
    // Start is called before the first frame update
    private TCPClient TcpClient;

    void Start()
    {
        TcpClient = GameObject.Find("Client").GetComponent<TCPClient>();
    }

    public byte ReadByte()
    {
        if (TcpClient.input.Count != 0)
        {
            var ret = (byte)TcpClient.input.Dequeue();
            return ret;
        }
        else
        {
            Debug.LogError("MsgManager::ReadByte: underflow");
            return 0;
        }
    }

    public byte ReadOpcode()
    {
        var opcode = (byte)((ReadByte()) & 0xFF);
        return opcode;
    }

    public ushort ReadShort()
    {
        ushort a = (ushort)ReadByte();
        ushort b = (ushort)ReadByte();

        return (ushort)((a << 8) + b);
    }

    public uint ReadInt()
    {
        var a = (uint)ReadShort();
        var b = (uint)ReadShort();
        return (uint)((a << 16) + b);
    }

    public ulong ReadLong()
    {
        var a = (ulong)ReadInt();
        var b = (ulong)ReadInt();
        return (ulong)((a << 32) + b);
    }

    public string ReadString()
    {
        string str = "";
        char c = (char)ReadByte();
        while (c != '\n')
        {
            str += c;
            c = (char)ReadByte();
        }
        return str;
    }

    public void ReadDiscard(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (TcpClient.input.Count != 0)
            {
                TcpClient.input.Dequeue();
            }
        }
    }

    public void WriteByte(byte value)
    {
        TcpClient.output.Enqueue(value);
    }

    public void WriteOpcode(byte op)
    {
        WriteByte((byte)((op) & 0xFF));
    }

    public void WriteNull(int len)
    {
        for (int i = 0; i < len; i++)
        {
            WriteByte(0);
        }
    }

    public void WriteShort(ushort num)
    {
        WriteByte((byte)((num >> 8) & 0xFF));
        WriteByte((byte)(num & 0xFF));
    }

    public void WriteInt(uint num)
    {
        WriteByte((byte)((num >> 24) & 0xFF));
        WriteByte((byte)((num >> 16) & 0xFF));
        WriteByte((byte)((num >> 8) & 0xFF));
        WriteByte((byte)(num & 0xFF));
    }

    public void WriteLong(ulong num)
    {
        for (int i = 0; i < 8; i++)
        {
            int current = 7 - i;
            byte curr_byte = (byte)((num >> (current * 8)) & 0xff);
            //printf("curr_byte: %d\n", curr_byte);
            WriteByte(curr_byte);
        }
    }

    public void WriteString(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            WriteByte((byte)str[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
