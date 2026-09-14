using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkButtons : NetworkBehaviour
{
    public void HostButton()
    {
        NetworkManager.Singleton.StartHost();
        this.gameObject.SetActive(false);
    }
    public void ServerButton()
    {
        NetworkManager.Singleton.StartServer();
        this.gameObject.SetActive(false);
    }
    public void ClientButton()
    {
        NetworkManager.Singleton.StartClient();
        this.gameObject.SetActive(false);
    }
}
