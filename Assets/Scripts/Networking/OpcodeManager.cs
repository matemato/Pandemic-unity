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
        return opcode switch
        {
            ServerOpcode.SERVER_MESSAGE => new InServerMessage(),
            ServerOpcode.UPDATE_PLAYERS => new InUpdatePlayers(),
            ServerOpcode.BEGIN_GAME => new InBeginGame(),
            ServerOpcode.UPDATE_PLAYER_CARD => new InUpdatePlayerCard(),
            ServerOpcode.TRIGGER_INFECTION => new InTriggerInfection(),
			ServerOpcode.UPDATE_TURN => new InUpdateTurn(),
			ServerOpcode.TRIGGER_EPIDEMIC => new InTriggerEpidemic(),
			ServerOpcode.JOIN_LOBBY => new InJoinLobby(),
			_ => new InError(),
        };
    }

    public void ReceiveAll()
    {
        while (_msgManager.PendingInput())
        {
			var startSize = _msgManager.GetInputSize();
			var peekSize = _msgManager.PeekPacketSize();

			if(peekSize > startSize)
			{
				Debug.LogWarningFormat("Did not get a full packet, waiting for full packet...");
				break;
			}

            var opcodeId = _msgManager.ReadOpcode();
			var packetSize = _msgManager.ReadShort();
            var opcode = GetOpcode(opcodeId);
            opcode.Receive(_msgManager, _serverInput);
			var totalSize = startSize - _msgManager.GetInputSize();
            Debug.LogFormat("server: opcode {0}", opcodeId);

			if(totalSize != packetSize || packetSize != peekSize)
			{
				Debug.LogErrorFormat("opcode {0} has unexpected size {1} {2} {3}", opcodeId, totalSize, packetSize, peekSize);
			}

			if (_serverInput.InvalidOpcode)
            {
                Debug.LogErrorFormat("opcode {0} was invalid/unimplemented", opcodeId);
            }
        }
    }
}
