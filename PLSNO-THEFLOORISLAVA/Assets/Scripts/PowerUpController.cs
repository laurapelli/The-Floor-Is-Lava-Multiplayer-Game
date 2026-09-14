using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerUpController : NetworkBehaviour
{
    [Header("PowerUps Parameters")]
    public float timeExtraPush;
    public float timeVelocity;
    public bool pushBoost = false;
    public bool velocityBoost = false;

    private void FixedUpdate()
    {
        float dt = Time.deltaTime;
        if (!IsOwner) return;
        else
        {
            PowerUpsController(dt);
        }
    }

    #region PowerUps
    private void PowerUpsController(float dt)
    {
        if (pushBoost)
        {
            PushPowerUpCooldownServerRpc(dt);
        }
        if (velocityBoost)
        {
            VelocityPowerUpCooldownServerRpc(dt);
        }
    }
    //Push
    [ServerRpc]
    public void PushPowerUpCooldownServerRpc(float dt)
    {
        timeExtraPush += dt;
        if (timeExtraPush > 10)
        {
            PushPowerUpCooldownClientRpc(dt);
        }
    }
    [ClientRpc]
    public void PushPowerUpCooldownClientRpc(float dt)
    {
        pushBoost = false;
        timeExtraPush = 0;
    }
    //Velocity
    [ServerRpc]
    public void VelocityPowerUpCooldownServerRpc(float dt)
    {
        timeVelocity += dt;
        if (timeVelocity > 10)
        {
            VelocityPowerUpCooldownClientRpc(dt);
        }
    }
    [ClientRpc]
    public void VelocityPowerUpCooldownClientRpc(float dt)
    {
        velocityBoost = false;
        timeVelocity = 0;
    }
    #endregion
}
