// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;


//Implementa interfaces las cuales detectan cuando se inicializa el poner y quitar el dedo de la pantalla (up/down). Drag Handler sirve para ver el movimiento del dedo en la pantalla.
public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //La imagen del fondo es la que registra cuando se toca la pantalla. La imagen del frente solo sirve para dar feedback al jugador de donde esta tocando la pantalla.
    public Image back;
    public Image front;
    private Vector3 inputVector;




    public virtual void OnDrag(PointerEventData data)
    {
        //Toma donde se toca la pantalla con respecto a la imagen de atras, normaliza este vector, donde 0,0 es el punto de enmedio y -1/1 es el valor maximo para X y Y.
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(back.rectTransform, data.position, data.pressEventCamera, out position))
        {
            position.x = (position.x / back.rectTransform.sizeDelta.x);
            position.y = (position.y / back.rectTransform.sizeDelta.y);

            inputVector = new Vector3(position.x * 2 + 1, 0, position.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //actualiza la posicion de la imagen de adelante. Asi se da feedback y se mira como que si fuese un joystick de verdad
            front.rectTransform.anchoredPosition = new Vector3(inputVector.x * (back.rectTransform.sizeDelta.x / 2), inputVector.z * (back.rectTransform.sizeDelta.y / 2));

        }
    }

    //se va a on drag.
    public virtual void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }

    //resetea la posicion del joystick al centro
    public virtual void OnPointerUp(PointerEventData data)
    {
        inputVector = Vector3.zero;
        front.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float horizontal()
    {
        return inputVector.x;
    }

    public float vertical()
    {
        return inputVector.z;
    }
}

