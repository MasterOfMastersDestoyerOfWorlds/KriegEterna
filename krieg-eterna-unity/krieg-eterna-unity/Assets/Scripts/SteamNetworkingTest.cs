using UnityEngine;
using System.Collections;
using Steamworks;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class SteamNetworkingTest : MonoBehaviour
{
    private Vector2 m_ScrollPos;
    private CSteamID m_RemoteSteamId;
    public SteamNetworkingIdentity m_RemoteSteamNetworkingIdentity;

    protected Callback<P2PSessionRequest_t> m_P2PSessionRequest;
    protected Callback<P2PSessionConnectFail_t> m_P2PSessionConnectFail;
    protected Callback<SocketStatusCallback_t> m_SocketStatusCallback;
    protected Callback<LobbyCreated_t> m_LobbyCreated_t;

    protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested_t;

    protected Callback<LobbyChatUpdate_t> m_LobbyChatUpdate_t;

    protected Callback<SteamNetworkingMessagesSessionRequest_t> m_SteamNetworkingMessagesSessionRequest_t;



    public int seed;
    public System.Random random;
    public CSteamID lobbyId;
    internal bool lobbyUpdated = false;
    internal bool host = false;
    internal bool isNetworkGame = false;

    internal bool lobbyOverride = true;

    public void OnEnable()
    {
        // You'd typically get this from a Lobby. Hardcoding it so that we don't need to integrate the whole lobby system with the networking.
        m_RemoteSteamId = new CSteamID(76561199527818303);

        m_P2PSessionRequest = Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
        m_P2PSessionConnectFail = Callback<P2PSessionConnectFail_t>.Create(OnP2PSessionConnectFail);
        m_SocketStatusCallback = Callback<SocketStatusCallback_t>.Create(OnSocketStatusCallback);
        m_LobbyCreated_t = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        m_GameLobbyJoinRequested_t = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        m_LobbyChatUpdate_t = Callback<LobbyChatUpdate_t>.Create(OnLobbyStateChange);
        m_SteamNetworkingMessagesSessionRequest_t = Callback<SteamNetworkingMessagesSessionRequest_t>.Create(OnSessionOpen);
        DontDestroyOnLoad(gameObject);
    }

    void OnDisable()
    {
        // Just incase we have it open when we close/assemblies get reloaded.
        if (!m_RemoteSteamId.IsValid())
        {
            SteamNetworking.CloseP2PSessionWithUser(m_RemoteSteamId);
        }
        if (lobbyId != null)
        {
            SteamMatchmaking.LeaveLobby(lobbyId);
        }
    }

    enum MsgType : uint
    {
        Ping,
        Ack,
    }

    public void RenderOnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
        GUILayout.Label("Variables:");
        GUILayout.Label("m_RemoteSteamId: " + m_RemoteSteamId);
        GUILayout.EndArea();

        GUILayout.BeginVertical("box");
        m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

        if (!m_RemoteSteamId.IsValid())
        {
            GUILayout.Label("Please fill m_RemoteSteamId with a valid 64bit SteamId to use SteamNetworkingTest.");
            GUILayout.Label("Alternatively it will be filled automatically when a session request is recieved.");
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            return;
        }

        // Session-less connection functions
        if (GUILayout.Button("SendP2PPacket(m_RemoteSteamId, bytes, (uint)bytes.Length, EP2PSend.k_EP2PSendReliable)"))
        {
            byte[] bytes = new byte[4];
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
            using (System.IO.BinaryWriter b = new System.IO.BinaryWriter(ms))
            {
                b.Write((uint)MsgType.Ping);
            }
            bool ret = SteamNetworking.SendP2PPacket(m_RemoteSteamId, bytes, (uint)bytes.Length, EP2PSend.k_EP2PSendReliable);
            print("SteamNetworking.SendP2PPacket(" + m_RemoteSteamId + ", " + bytes + ", " + (uint)bytes.Length + ", " + EP2PSend.k_EP2PSendReliable + ") : " + ret);
        }

        {
            uint MsgSize;
            bool ret = SteamNetworking.IsP2PPacketAvailable(out MsgSize);
            GUILayout.Label("IsP2PPacketAvailable(out MsgSize) : " + ret + " -- " + MsgSize);

            GUI.enabled = ret;

            if (GUILayout.Button("ReadP2PPacket(bytes, MsgSize, out newMsgSize, out SteamIdRemote)"))
            {
                byte[] bytes = new byte[MsgSize];
                uint newMsgSize;
                CSteamID SteamIdRemote;
                ret = SteamNetworking.ReadP2PPacket(bytes, MsgSize, out newMsgSize, out SteamIdRemote);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                using (System.IO.BinaryReader b = new System.IO.BinaryReader(ms))
                {
                    MsgType msgtype = (MsgType)b.ReadUInt32();
                    // switch statement here depending on the msgtype
                    print("SteamNetworking.ReadP2PPacket(bytes, " + MsgSize + ", out newMsgSize, out SteamIdRemote) - " + ret + " -- " + newMsgSize + " -- " + SteamIdRemote + " -- " + msgtype);
                }

            }

            GUI.enabled = true;
        }

        //SteamNetworking.AcceptP2PSessionWithUser() // Only called from within P2PSessionRequest Callback!

        if (GUILayout.Button("CloseP2PSessionWithUser(m_RemoteSteamId)"))
        {
            bool ret = SteamNetworking.CloseP2PSessionWithUser(m_RemoteSteamId);
            print("SteamNetworking.CloseP2PSessionWithUser(" + m_RemoteSteamId + ") : " + ret);
        }

        if (GUILayout.Button("CloseP2PChannelWithUser(m_RemoteSteamId, 0)"))
        {
            bool ret = SteamNetworking.CloseP2PChannelWithUser(m_RemoteSteamId, 0);
            print("SteamNetworking.CloseP2PChannelWithUser(" + m_RemoteSteamId + ", " + 0 + ") : " + ret);
        }

        {
            P2PSessionState_t ConnectionState;
            bool ret = SteamNetworking.GetP2PSessionState(m_RemoteSteamId, out ConnectionState);
            GUILayout.Label("GetP2PSessionState(m_RemoteSteamId, out ConnectionState) : " + ret + " -- " + ConnectionState);
        }

        if (GUILayout.Button("AllowP2PPacketRelay(true)"))
        {
            bool ret = SteamNetworking.AllowP2PPacketRelay(true);
            print("SteamNetworking.AllowP2PPacketRelay(" + true + ") : " + ret);
        }

        // LISTEN / CONNECT style interface functions
        //SteamNetworking.CreateListenSocket() // TODO

        //SteamNetworking.CreateP2PConnectionSocket() // TODO

        //SteamNetworking.CreateConnectionSocket() // TODO

        //SteamNetworking.DestroySocket() // TODO

        //SteamNetworking.DestroyListenSocket() // TODO

        //SteamNetworking.SendDataOnSocket() // TODO

        //SteamNetworking.IsDataAvailableOnSocket() // TODO

        //SteamNetworking.RetrieveDataFromSocket() // TODO

        //SteamNetworking.IsDataAvailable() // TODO

        //SteamNetworking.RetrieveData() // TODO

        //SteamNetworking.GetSocketInfo() // TODO

        //SteamNetworking.GetListenSocketInfo() // TODO

        //SteamNetworking.GetSocketConnectionType() // TODO

        //SteamNetworking.GetMaxPacketSize() // TODO

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    //Todo need some sort of loading symbol and timeout
    private void OnLobbyCreated(LobbyCreated_t param)
    {
        Debug.LogError("Lobby created? " + param.m_eResult);
        this.lobbyId = new CSteamID(param.m_ulSteamIDLobby);
        lobbyUpdated = true;
        host = true;
        isNetworkGame = true;
        if (lobbyOverride)
        {
            SteamMatchmaking.InviteUserToLobby(lobbyId, m_RemoteSteamId);
        }
        else
        {
            SteamFriends.ActivateGameOverlayInviteDialog(lobbyId);
        }

    }

    void OnP2PSessionRequest(P2PSessionRequest_t pCallback)
    {
        Debug.LogError("[" + P2PSessionRequest_t.k_iCallback + " - P2PSessionRequest] - " + pCallback.m_steamIDRemote);

        bool ret = SteamNetworking.AcceptP2PSessionWithUser(pCallback.m_steamIDRemote);
        print("SteamNetworking.AcceptP2PSessionWithUser(" + pCallback.m_steamIDRemote + ") - " + ret);

        m_RemoteSteamId = pCallback.m_steamIDRemote;
    }

    void OnP2PSessionConnectFail(P2PSessionConnectFail_t pCallback)
    {
        Debug.LogError("[" + P2PSessionConnectFail_t.k_iCallback + " - P2PSessionConnectFail] - " + pCallback.m_steamIDRemote + " -- " + pCallback.m_eP2PSessionError);
    }

    void OnSocketStatusCallback(SocketStatusCallback_t pCallback)
    {
        Debug.LogError("[" + SocketStatusCallback_t.k_iCallback + " - SocketStatusCallback] - " + pCallback.m_hSocket + " -- " + pCallback.m_hListenSocket + " -- " + pCallback.m_steamIDRemote + " -- " + pCallback.m_eSNetSocketState);
    }

    void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t param)
    {
        Debug.LogError("Joining Lobby: " + param.m_steamIDLobby + " Invite From Friend: " + param.m_steamIDFriend);
        SteamMatchmaking.JoinLobby(param.m_steamIDLobby);
    }
    private void OnLobbyStateChange(LobbyChatUpdate_t param)
    {
        if (param.m_rgfChatMemberStateChange == ((uint)EChatMemberStateChange.k_EChatMemberStateChangeEntered))
        {
            Debug.LogError("User Joined? " + param.m_ulSteamIDUserChanged);
            m_RemoteSteamId = new CSteamID(param.m_ulSteamIDUserChanged);
            m_RemoteSteamNetworkingIdentity = new SteamNetworkingIdentity();
            m_RemoteSteamNetworkingIdentity.SetSteamID(m_RemoteSteamId);
        }
        else
        {
            Debug.LogError("User Left? " + param.m_ulSteamIDUserChanged);
            m_RemoteSteamId = new CSteamID(0);
            isNetworkGame = false;
        }

    }
    internal void setSeed()
    {
        this.seed = Environment.TickCount;
        this.random = new System.Random(seed);
    }

    public (PacketType, int) getNextMessage()
    {

        IntPtr[] messages = new IntPtr[1];
        int messagesRecieved = SteamNetworkingMessages.ReceiveMessagesOnChannel(0, messages, 1);
        Debug.LogError("Getting next Message, num received on channel: " + messagesRecieved);
        if (messagesRecieved > 0)
        {
            string str = messages[0].ToString();
            Debug.LogError(str);
            Debug.LogError(BitConverter.ToString(Encoding.ASCII.GetBytes(str)));
            SteamNetworkingMessage_t netMessage = Marshal.PtrToStructure<SteamNetworkingMessage_t>(messages[0]);
            byte[] messageBytes = new byte[netMessage.m_cbSize];
            Marshal.Copy(netMessage.m_pData, messageBytes, 0, messageBytes.Length);
            (PacketType, int) message = unpackMessage(messageBytes);
            if (message.Item1 == PacketType.SEED)
            {
                this.random = new System.Random(message.Item2);
            }
            SteamNetworkingMessage_t.Release(messages[0]);
            Debug.LogError("Message type recieved: " + message.Item1 + " Message: " + message.Item2);
            return message;
        }
        else
        {
            Debug.Log("No Message recieved");
            return (PacketType.NONE, -1);
        }

    }

    public void sendNextMessage(PacketType type, int message)
    {
        Debug.LogError("Sending Message: type:" + type.ToString() + " message: " + message + " recipiant: " + m_RemoteSteamNetworkingIdentity.GetSteamID());
        byte[] bytes = new byte[sizeof(int) + sizeof(uint)];
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
        using (System.IO.BinaryWriter b = new System.IO.BinaryWriter(ms))
        {
            b.Write((uint)type);
            b.Write(message);
        }
        Debug.LogError("Bytes Sent: " + BitConverter.ToString(bytes));
        GCHandle pinnedArray = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        IntPtr pointer = pinnedArray.AddrOfPinnedObject();
        EResult result = SteamNetworkingMessages.SendMessageToUser(ref m_RemoteSteamNetworkingIdentity, pointer, (uint)bytes.Length, 8, 0);
        Debug.LogError("Send Message Result: " + result.ToString());
        pinnedArray.Free();
    }

    private void OnSessionOpen(SteamNetworkingMessagesSessionRequest_t param)
    {
        m_RemoteSteamNetworkingIdentity = param.m_identityRemote;
        SteamNetworkingMessages.AcceptSessionWithUser(ref m_RemoteSteamNetworkingIdentity);
        IntPtr[] messages = new IntPtr[1];
        int messagesRecieved = SteamNetworkingMessages.ReceiveMessagesOnChannel(0, messages, 1);
        if (messagesRecieved > 0)
        {
            string str = messages[0].ToString();
            Debug.LogError(str);
            Debug.LogError(BitConverter.ToString(Encoding.ASCII.GetBytes(str)));
            SteamNetworkingMessage_t netMessage = Marshal.PtrToStructure<SteamNetworkingMessage_t>(messages[0]);
            byte[] messageBytes = new byte[netMessage.m_cbSize];
            Marshal.Copy(netMessage.m_pData, messageBytes, 0, messageBytes.Length);
            (PacketType, int) message = unpackMessage(messageBytes);
            if (message.Item1 == PacketType.SEED)
            {
                this.random = new System.Random();
            }
            else
            {
                Debug.LogError("REEEEEE first message not seed");
            }
            SteamNetworkingMessage_t.Release(messages[0]);
        }
        else
        {
            Debug.LogError("F, no message then how did the sesh get opened?");

        }

    }
    static (PacketType, int) unpackMessage(byte[] packedMessage)
    {
        Debug.LogError("Message Recieved: " + BitConverter.ToString(packedMessage));
        int message = BitConverter.ToInt32(packedMessage, 4);
        PacketType messageType = (PacketType)BitConverter.ToInt32(packedMessage, 0);
        return (messageType, message);
    }
}