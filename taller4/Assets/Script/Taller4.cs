using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ald = NPC.Ally;
using zon = NPC.Enemy;
using UnityEngine.UI;

/// <summary>
/// Se toma de referencias los namespces de otros scripts para facilitar el llamado
/// </summary>
public class Taller4 : MonoBehaviour
{
    public Text zombies;
    public Text aldeanos;
   public GameObject[] zombian, aldino;
    int camp;
    int infec;

    /// <summary>
    /// se llenan los array de aldeano y zombie con los ojetos de tags correspondientes  
    /// </summary>

    void Start()
    {
        Creator creator = new Creator(Random.Range(5, 16));
       
    }
    private void Update()
    {
        zombian = GameObject.FindGameObjectsWithTag("Zombie");
        aldino = GameObject.FindGameObjectsWithTag("Villiger");
        foreach (GameObject item in zombian)
        {
            infec = zombian.Length;
            zombies.text = infec.ToString();

        }
        foreach (GameObject item in aldino)
        {
            camp = aldino.Length;
            aldeanos.text = camp.ToString();

        }
     

    }

}

public class Creator
{
    /// <summary>
    /// usamos un valor random para determinar cuantos GameObjects habran.
    /// de manera aleatorea se le otorgan componentes a estos GameObjects
    /// </summary>
    /// /// <summary>
    /// envia a velocidad random al movimiento
    /// </summary>
    const int MAXINS = 26;
    public readonly float probability;
    

    public Creator(float prob)
    {
        probability = prob;
        int unic = 0;
        for (int i = 0; i < Random.Range(probability, MAXINS); i++)
        {
            int entidades = Random.Range(unic, 3);

            if (unic == 0)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<Hero>();
                go.tag = "Player";
                unic = unic + 1;
            }
            if (entidades == 1)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<ald.Aldeano>().Comun();
                go.tag = "Villiger";
            }
            if (entidades == 2)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<zon.Walker>().SinCerbro();
                go.tag = "Zombie";
            }
        }

    }
}
public class Hero : MonoBehaviour
{
    /// <summary>
    /// se realiza una referencia para los scripts de movimiento y mirar del Heroe (jugador)
    /// Agregamos los componentes necesarios para realizar collisiones y se desactiva el uso de graveda 
    /// tambien se activan todas los constrains para hacer que solo se pueda mover por el codigo y no por alguna colision
    /// </summary>
    Movement movement;
    Look look;
    static public GameObject aldeanoObject;
    static public GameObject zombieObject;
    public GameObject[] zombian, aldino;
    Vector3 directionZ;
    Vector3 directionA;
    static public float distanceZ;
    static public float distanceA;
    Text textoAldeano;
    Text textoZombie;
    float time;
    InfoAlde infoAlde = new InfoAlde();
    InfoZomb infoZomb = new InfoZomb();

    void Start()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        gameObject.name = "Hero";
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        // se le agregan los scripts de movimiento y mirar al jugador 
        movement = gameObject.AddComponent<Movement>();
        look = gameObject.AddComponent<Look>();
        gameObject.AddComponent<Camera>();
        textoZombie = GameObject.FindGameObjectWithTag("TextZombie").GetComponent<Text>();
        textoAldeano = GameObject.FindGameObjectWithTag("TextAldeano").GetComponent<Text>();
        // determina una velocidad al azar para entregarcela al metodo de movimiento mas adelante
    }

    IEnumerator BuscaEntidades()
    {
        zombian = GameObject.FindGameObjectsWithTag("Zombie");
        aldino = GameObject.FindGameObjectsWithTag("Villager");

        // retroalimentacion para el aldeano
        foreach (GameObject item in aldino)
        {
            yield return new WaitForEndOfFrame();
            ald.Aldeano componenteAldeano = item.GetComponent<ald.Aldeano>();
            if (componenteAldeano != null)
            {
                distanceA = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                if (distanceA < 5f)
                {
                    time = 0;
                    infoAlde = item.GetComponent<ald.Aldeano>().infoAlde;
                    textoAldeano.text = "Hola, soy " + infoAlde.name + " y tengo " + infoAlde.edad.ToString() + " años";
                }
                if (time > 3)
                {
                    textoAldeano.text = " ";
                }
            }
        }

        // retroalimentacion para el zombie
        foreach (GameObject itemZ in zombian)
        {
            yield return new WaitForEndOfFrame();
            zon.Walker componenteZombie = itemZ.GetComponent<zon.Walker>();
            if (componenteZombie != null)
            {
                distanceZ = Mathf.Sqrt(Mathf.Pow((itemZ.transform.position.x - transform.position.x), 2) + Mathf.Pow((itemZ.transform.position.y - transform.position.y), 2) + Mathf.Pow((itemZ.transform.position.z - transform.position.z), 2));
                if (distanceZ < 5f)
                {
                    time = 0;
                    infoZomb = itemZ.GetComponent<zon.Walker>().infoZomb;
                    textoZombie.text = "Waaaarrrr quiero comer " + infoZomb.gusto;
                }
                if (time > 3)
                {
                    textoZombie.text = " ";
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(BuscaEntidades());
    }
    /// <summary>
    /// llama a los constructores en las clases de Movement y Look
    /// </summary>
    private void Update()
    {    
        movement.Move();
        look.Arround();     
        distanceZ = Mathf.Sqrt(Mathf.Pow((zombieObject.transform.position.x - transform.position.x), 2) + Mathf.Pow((zombieObject.transform.position.y - transform.position.y), 2) + Mathf.Pow((zombieObject.transform.position.z - transform.position.z), 2));       
        distanceA = Mathf.Sqrt(Mathf.Pow((aldeanoObject.transform.position.x - transform.position.x), 2) + Mathf.Pow((aldeanoObject.transform.position.y - transform.position.y), 2) + Mathf.Pow((aldeanoObject.transform.position.z - transform.position.z), 2));
        directionZ = Vector3.Normalize(zombieObject.transform.position - transform.position);
        directionA = Vector3.Normalize(aldeanoObject.transform.position - transform.position);
    }
    public void LateUpdate()
    {
        
    }
    /// <summary>
    /// se toman los valores almacendos en el estruc para imprimirlos cada que el heroe colisione con 
    /// el respectivo GameObject
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<zon.Walker>())
        {
           
        }
       
    }
}


