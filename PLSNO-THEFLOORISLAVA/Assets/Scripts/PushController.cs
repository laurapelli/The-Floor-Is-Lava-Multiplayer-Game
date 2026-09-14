using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PushController : NetworkBehaviour
{
    [Header("Push Parameters")]
    public bool canPush = true;
    public bool pushing = false;
    
    public List<GameObject> players = new List<GameObject>();

    [Header("PowerUps Parameters")]
    public PowerUpController PUC;

    private void Update()
    {
        if (!IsOwner) return;
        else
        {
            PushingLocal();
        }
    }

    #region Push
    private void PushingLocal()
    {
        if (Input.GetKeyDown(KeyCode.F) && canPush)
        {
            PushServerRpc();
        }
    }

    [ClientRpc]
    private void PushClientRpc()
    {
        pushing = true;
        canPush = false;

        GoPush();

        Invoke(nameof(CanPushAgain), 1f);
        Invoke(nameof(DeactivatePush), 1f);
    }

    [ServerRpc]
    private void PushServerRpc()
    {
        GoPush();

        PushClientRpc();
    }

    private void GoPush()
    {
        if (pushing)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if(!PUC.pushBoost)
                {
                    Debug.Log("Sin el boost");
                    players[i].GetComponent<Rigidbody2D>().AddForce(transform.right * 10, ForceMode2D.Impulse);
                }
                else
                {
                    Debug.Log("Con el boost");
                    players[i].GetComponent<Rigidbody2D>().AddForce(transform.right * 20, ForceMode2D.Impulse);
                }
                
            }
        }
    }

    private void DeactivatePush()
    {
        pushing = false;
    }

    private void CanPushAgain()
    {
        canPush = true;
    }
    #endregion
    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.GetComponent<PlayerMovement>() && !players.Contains(collision.gameObject))
            {
                players.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.GetComponent<PlayerMovement>())
            {
                players.Remove(collision.gameObject);
            }
        }
    }
    #endregion
}
