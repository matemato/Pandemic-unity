using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcodeOut
{
    private readonly ClientOpcode id;
	public static readonly int HEADER_SIZE = 3;

    public OpcodeOut(ClientOpcode id)
    {
        this.id = id;
    }

    public void Send(MsgManager msgManager)
    {
		msgManager.ClearOutput();
		WriteBody(msgManager);
		WriteHeader(msgManager);
		msgManager.MergeOutput();
    }

	private void WriteHeader(MsgManager msgManager)
	{
		msgManager.WriteSize(HEADER_SIZE);
		msgManager.WriteOpcode((byte)id);
	}

	public virtual void WriteBody(MsgManager msgManager)
	{
		throw new NotImplementedException("This method must be overridden.");
	}
}
