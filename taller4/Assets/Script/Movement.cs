using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
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
    /// se obtiene un flotante que determina la velocidad con la que se movera el objeto que tiene este script
    /// y se le asignan input del te clado con los que se activaran esta acciones
    /// </summary>
    /// <param name="speedChange"></param>
    public void Move()
    {
        Move move = new Move(Random.Range(0.09f, 0.19f));

        if (Input.GetKey(KeyCode.W))
        {
            movableTransform.Translate(0, 0, move.speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movableTransform.Translate(0, 0, -move.speed);
        }
        
    }

}
public class Move
{
    public readonly float speed;
    public Move(float veloz)
    {
        speed = veloz;
    }

}



