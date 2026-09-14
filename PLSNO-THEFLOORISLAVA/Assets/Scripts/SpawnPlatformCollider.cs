using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnPlatformCollider : NetworkBehaviour
{
    public GameObject[] platforms;
    public Transform spawnPos;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;
        if (collision.gameObject.CompareTag("InstantiatePlatform") && IsServer)
        {
            Debug.Log("Entro");
            IntantiatePlatformServerRpc();
        }
        Destroy(collision.gameObject);
    }

    [ServerRpc]
    private void IntantiatePlatformServerRpc()
    {
        if(!IsServer && !IsHost)
        {
            Debug.Log("Entro al IntantiatePlatformServerRpc");
            Instantiate(platforms[Random.Range(0, 1)], spawnPos.position, Quaternion.identity);
        }
        PruebaServerRpc();

    }
    [ServerRpc]
    private void PruebaServerRpc()
    {
        Debug.Log("Entro al PruebaServerRpc");
        IntantiatePlatform();
    }

    private void IntantiatePlatform()
    {
        Debug.Log("Entro al IntantiatePlatform");
        GameObject prueba = Instantiate(platforms[Random.Range(0, 1)], spawnPos.position, Quaternion.identity);
    }
}
