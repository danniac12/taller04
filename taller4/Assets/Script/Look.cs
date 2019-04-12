using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    /// <summary>
    /// se toma el tranform del objeto que tiene el script
    /// </summary>
    Transform movableTransform;

    private void Awake()
    {
        movableTransform = transform;
    }
   
    /// <summary>
    /// se le asignan input del teclado con los que se activaran la accion de rotar la mira del personaje 
    /// </summary>
  
    public void Arround()
    {


        if (Input.GetKey(KeyCode.A))
        {
            movableTransform.Rotate(0, -1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movableTransform.Rotate(0, 1, 0, 0);
        }

    }
}
