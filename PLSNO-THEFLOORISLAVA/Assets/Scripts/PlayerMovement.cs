using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    #region Parameters
    private Rigidbody2D rig;
    public SpriteRenderer spriteRenderer;
    public Sprite[] spritesPlayers;

    [Header("Movement Parameters")]
    [SerializeField]private int moveSpeed;

    [Header("Jump Parameters")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Vector2 vecGravity;

    [SerializeField]private int jumpForce;

    [SerializeField]private float fallMultiplier;
    [SerializeField]private float jumpTime;
    [SerializeField]private float jumpMultiplier;
    private float jumpCounter;

    private bool isJumping;

    [Header("PowerUps Parameters")]
    public PowerUpController PUC;
    #endregion
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        spriteRenderer.sprite = spritesPlayers[OwnerClientId];

        switch (OwnerClientId)
        {
            case 0:
                transform.position = new Vector3(-16, 0, 0);
                break;
            case 1:
                transform.position = new Vector3(-8, 0, 0);
                break;
            case 2:
                transform.position = new Vector3(8, 0, 0);
                break;
            case 3:
                transform.position = new Vector3(16, 0, 0);
                break;
            default:
                break;
        }

        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        else
        {
            Movement();
            Jump();
        }
    }

    #region Movement
    private void Movement()
    {
        MovementServerRpc();
    }

    [ServerRpc]
    private void MovementServerRpc()
    {
        MovementClientRpc();
    }

    [ClientRpc]
    private void MovementClientRpc()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (!PUC.velocityBoost)
        {
            if (moveHorizontal < 0) //Looking left
            {
                transform.Translate(moveHorizontal * moveSpeed * -transform.right * Time.deltaTime, Space.World);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (moveHorizontal > 0) //Looking right
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(moveHorizontal * moveSpeed * transform.right * Time.deltaTime, Space.World);
            }
        }
        else
        {
            if (moveHorizontal < 0) //Looking left
            {
                transform.Translate(moveHorizontal * (moveSpeed + 10) * -transform.right * Time.deltaTime, Space.World);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (moveHorizontal > 0) //Looking right
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(moveHorizontal * (moveSpeed + 10) * transform.right * Time.deltaTime, Space.World);
            }
        }
    }
    #endregion
    #region Jump
    private void Jump()
    {
        JumpServerRpc();
    }
    [ServerRpc]
    private void JumpServerRpc()
    {
        JumpClientRpc();
    }
    [ClientRpc]
    private void JumpClientRpc()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rig.velocity = new Vector2(rig.velocity.x, jumpForce);
            isJumping = true;
            jumpCounter = 0;
        }
            
        if (rig.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) isJumping = false;

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rig.velocity += vecGravity * jumpMultiplier * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            jumpCounter = 0;

            if (rig.velocity.y < 0)
            {
                rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * .6f);
            }
        }

        if (rig.velocity.y < 0)
        {
            rig.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.8f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    #endregion
    #region Die
    private void Die()
    {
        DieServerRpc();
    }
    [ServerRpc]
    private void DieServerRpc()
    {
        DieClientRpc();
    }
    [ClientRpc]
    private void DieClientRpc()
    {
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Lava"))
        {
            Die();
        }
    }
    #endregion
}
