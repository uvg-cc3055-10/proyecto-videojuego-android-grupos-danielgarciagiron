// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using UnityEngine;
using System.Collections;

public class PlayerRotation : MonoBehaviour
{

    private float horizontal;
    private float vertical;
    private Vector3 rotationDirection;
    public VirtualJoystick joystick;
    private bool canMove = true;

    public GameObject player;
    private PlayerMovement playerScript;

    private void Start()
    {
        playerScript = (PlayerMovement)player.GetComponent(typeof(PlayerMovement));
    }

    void FixedUpdate()
    {
        canMove = playerScript.currentMobility;
        if (canMove == true)
        {
            horizontal = joystick.horizontal();
            vertical = joystick.vertical();
            if (horizontal != 0.0f || vertical != 0.0f)
            {
                rotationDirection = new Vector3(horizontal, 0, vertical);
                transform.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
            }
        }
    }
}
