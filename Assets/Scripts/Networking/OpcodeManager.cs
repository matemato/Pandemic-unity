using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpcodeManager
{
    private MsgManager _msgManager;
    private ServerInput _serverInput;
    // Start is called before the first frame update
    public OpcodeManager(MsgManager msgManager, ServerInput serverInput)
    {
        _msgManager = msgManager;
        _serverInput = serverInput;
    }

    public void Send(OpcodeOut opcode)
    {
        opcode.Send(_msgManager);
    }

    private OpcodeIn GetOpcode(byte opcodeId)
    {
        var opcode = (ServerOpcode)opcodeId;
        switch(opcode)
        {
            case ServerOpcode.SERVER_MESSAGE: return new InServerMessage();
            case ServerOpcode.UPDATE_PLAYERS: return new InUpdatePlayers();
            case ServerOpcode.BEGIN_GAME: return new InBeginGame();
            default: return new InError();
        }
    }

    public void ReceiveAll()
    {
        while (_msgManager.PendingInput())
        {
            var opcodeId = _msgManager.ReadOpcode();
            var opcode = GetOpcode(opcodeId);
            opcode.Receive(_msgManager, _serverInput);
            Debug.LogFormat("server: opcode {0}", opcodeId);
            if (_serverInput.InvalidOpcode)
            {
                Debug.LogErrorFormat("opcode {0} was invalid/unimplemented", opcodeId);
            }
        }
    }
}
