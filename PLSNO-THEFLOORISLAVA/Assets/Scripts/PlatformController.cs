using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlatformController : NetworkBehaviour
{
    private float speed = 0.01f;

    private void Start()
    {
        Debug.Log("Empiezo");
        NetworkObject.Spawn();
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlatforms();
    }
    private void MovePlatforms()
    {
        MovePlatformsServerRpc();
    }
    [ServerRpc]
    private void MovePlatformsServerRpc()
    {
        if(!IsServer && !IsHost)
        {
            transform.position += new Vector3(0, -speed, 0);
        }
        PruebonServerRpc();
    }
    [ServerRpc]
    private void PruebonServerRpc()
    {
        MovePlatformss();
    }
    private void MovePlatformss()
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 1)
        {
            transform.position += new Vector3(0, -speed, 0);
        }
    }
}
