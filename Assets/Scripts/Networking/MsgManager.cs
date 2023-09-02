using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgManager
{
    // Start is called before the first frame update
    private TCPClient _tcpClient;

    public MsgManager(TCPClient tcpClient)
    {
        _tcpClient = tcpClient;
    }

    public byte ReadByte()
    {
        if (_tcpClient.input.Count != 0)
        {
            var ret = (byte)_tcpClient.input.Dequeue();
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

    public string ReadString(byte length)
    {
        string str = "";
        for(int i = 0; i < length;i++)
        {
            char c = (char)ReadByte();
            str += c;
        }
        return str;
    }

    public void ReadDiscard(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (_tcpClient.input.Count != 0)
            {
                _tcpClient.input.Dequeue();
            }
        }
    }

    public void WriteByte(byte value)
    {
        _tcpClient.output.Enqueue(value);
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

    public bool PendingInput()
    {
        return _tcpClient.input.Count != 0;
    }
}
