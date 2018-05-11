using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SocketIO;
using System;
using UnityEngine.UI;

public class ServerConnect : MonoBehaviour
{
    public SocketOptions options;
    public SocketManager socketManager;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("Initializing server connect"); 
        CreateSocketRef();
        socketManager.Socket.On("connect", OnConnect);

        socketManager.Socket.On("picture", getPicture); 
        
    }

    void sendPicture(Texture tx)
    {
        socketManager.GetSocket().Emit("pictureFromHolo", tx); 
    }

    void getPicture(Socket socket, Packet packet, params object[] args)
    {

    }
    
    void OnConnect(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("Connected to server");
    }

    void OnApplicationQuit()
    {
        LeaveRoomFromServer();
        DisconnectMySocket();
    }

    public void CreateSocketRef()
    {
        //Debug.Log("Trying to connect"); 
        TimeSpan miliSecForReconnect = TimeSpan.FromMilliseconds(1000);

        options = new SocketOptions();
        options.ReconnectionAttempts = 3;
        options.AutoConnect = true;
        options.ReconnectionDelay = miliSecForReconnect;

        //Server URI
        socketManager = new SocketManager(new Uri("http://321letsjam.com:3000/socket.io/"), options);
    }

    public void DisconnectMySocket()
    {
        socketManager.GetSocket().Disconnect();
    }

    public void LeaveRoomFromServer()
    {
        socketManager.GetSocket().Emit("leave", OnSendEmitDataToServerCallBack);
    }

    private void OnSendEmitDataToServerCallBack(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("Send Packet Data : " + packet.ToString());
    }

    public void SetNamespaceForSocket()
    {
        //namespaceForCurrentPlayer = socketNamespace;
        //mySocket = socketManagerRef.GetSocket(“/ Room - 1);
    }

}
