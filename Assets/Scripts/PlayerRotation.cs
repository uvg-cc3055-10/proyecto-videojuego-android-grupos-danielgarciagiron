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
        //referencia al script de player movement.
        playerScript = (PlayerMovement)player.GetComponent(typeof(PlayerMovement));
    }

    //lo unico que hace es tomar los valores del joystick virtual y ajustar la rotacion para que coincida hacia donde se mueve el personaje con la direccion a la cual este esta viendo.
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
