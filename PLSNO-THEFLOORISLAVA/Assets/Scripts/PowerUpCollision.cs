using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollision : MonoBehaviour
{
    public int type;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (type)
        {
            case 1:
                if (!collision.isTrigger && collision.gameObject.GetComponent<PowerUpController>())
                {
                    collision.gameObject.GetComponent<PowerUpController>().pushBoost = true;
                    Destroy(this.gameObject);
                }
                break;
            case 2:
                if (!collision.isTrigger && collision.gameObject.GetComponent<PowerUpController>())
                {
                    collision.gameObject.GetComponent<PowerUpController>().velocityBoost = true;
                    Destroy(this.gameObject);
                }
                break;
            default:
                break;
        }
    }
}
