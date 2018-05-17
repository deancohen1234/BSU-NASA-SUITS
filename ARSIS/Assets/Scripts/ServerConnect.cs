using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SocketIO;
using System;
using UnityEngine.UI;

public class ServerConnect : MonoBehaviour
{
    // Singleton 
    public static ServerConnect S; 

    public SocketOptions options;
    public SocketManager socketManager;

    public AudioClip m_messageRecievedAudio; 
    
    void Start()
    {
        S = this; 

        CreateSocketRef();
        socketManager.Socket.On("connect", OnConnect);

        socketManager.Socket.On("picture", getPicture); 
    }

    public void sendPicture(Texture2D tx)
    {
        socketManager.GetSocket().Emit("pictureFromHolo", tx.EncodeToPNG()); 
    }

    public void sos()
    {
        socketManager.GetSocket().Emit("SOS"); 
    }

    void getPicture(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("We got a picture");
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("Params " + i + " " + args[i]); 
        }

        Dictionary<String, object> fromSocket = (Dictionary<String, object>)args[0];
        Debug.Log(fromSocket["sendtext"]);
        Debug.Log(fromSocket["image"]);
        String b64String = (String)fromSocket["image"];
        Debug.Log(b64String.Length);
        Debug.Log(b64String.Length - 22);
        b64String = b64String.Remove(0, 22);
        Debug.Log(b64String.Length);
        GameObject the_cube = GameObject.Find("Cube");
        byte[] b64Bytes = System.Convert.FromBase64String(b64String);
        Texture2D tx = new Texture2D(1, 1);
        tx.LoadImage(b64Bytes);

        HoloLensSnapshotTest.S.SetImage(tx);
        HoloLensSnapshotTest.S.SetText(fromSocket["sendtext"].ToString());

        VoiceManager vm = (VoiceManager)GameObject.FindObjectOfType(typeof(VoiceManager));
        vm.m_Source.clip = m_messageRecievedAudio;
        vm.m_Source.Play(); 
    }
    
    void OnConnect(Socket socket, Packet packet, params object[] args)
    {
      // Debug.Log("Connected to server");
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

[System.Serializable]
public class FromServerData
{
    public String b64String;
    public String sendText;
}
