using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ald = NPC.Ally;
using UnityEngine.SceneManagement;

namespace NPC
{
    

    
    namespace Enemy
    {

        public class Walker : MonoBehaviour
        {
            /// <summary>
            /// declaramos 2 enumeradores uno para el estado del zombie y otro para la comida preferida
            /// asi como abrimos un espacio para lamacenar un contador que usaremos mas adelante
            /// </summary>
            public InfoZomb infoZomb = new InfoZomb();
            float t;
            float edad;
            GameObject[] ganado;
            GameObject aldeanoObject;
            GameObject playerObject;
            Vector3 directionH;
            Vector3 directionA;
            float distanceH;
            float distanceA;
            public float speed;
            public Vector3 direction;
            bool hambriento = false;
            bool infect = false;
            
            public enum Comer
            {
                Cerebro,
                Pierna,
                Vesicula,
                Brazo,
                Costilla
            }

            public enum Estado
            {
                idel,
                moving,
                rotating,
                pursuing
            }

            Comer comer;
            public Estado estado;
            /// <summary>
            /// le otorgamso un rigidboy como en las otras dos calses para efectuar la colisiones
            /// y congelamos en todas las posiciones para evitar comportamientos erraticos debido a estas colisiones
            /// se determina de que color sera el zombie de maenra aleatorea
            /// y se posicionea en un lugar random del mundo entre 10 y menos 10 de los ejes "z" y "x"
            /// </summary>
            public void SinCerbro()
            {
                if (!infect)
                {
                    comer = (Comer)Random.Range(0, 5);
                    edad = (int)Random.Range(15, 100);
                    int cambio = Random.Range(0, 3);
                    Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    rb.useGravity = false;
                    this.gameObject.name = "Zombie"; 
                    float x = Random.Range(-10, 10);
                    float z = Random.Range(-10, 10);
                    hambriento = false;
                    gameObject.transform.position = new Vector3(x, 0, z);
                    infoZomb.gusto = comer.ToString();
                    gameObject.name = infoZomb.nombre;
                    infoZomb.edad = edad;
                    if (cambio == 0)
                    {
                        this.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                    }
                    if (cambio == 1)
                    {
                        this.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                    }
                    if (cambio == 2)
                    {
                        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
                    }
                }
                else
                {
                    edad = infoZomb.edad;
                    this.gameObject.name = infoZomb.nombre;
                }
                StartCoroutine(buscaAldeanos());
                speed = 10 / edad;
            }
            /// <summary>
            /// almacenamos en el struct del zombi su preferencia alimenticia
            /// </summary>
          
            IEnumerator buscaAldeanos()
            {
                playerObject = GameObject.FindGameObjectWithTag("Player");
                ganado = GameObject.FindGameObjectsWithTag("Villiger");
                foreach (GameObject aGameObject in ganado)
                {
                    yield return new WaitForEndOfFrame();
                    Component aComponent = aGameObject.GetComponent<ald.Aldeano>();
                    if (aComponent != null)
                    {   
                        aldeanoObject = aGameObject;
                        directionH = Vector3.Normalize(playerObject.transform.position - transform.position);
                        distanceH = Mathf.Sqrt(Mathf.Pow((playerObject.transform.position.x - transform.position.x), 2) + Mathf.Pow((playerObject.transform.position.y - transform.position.y), 2) + Mathf.Pow((playerObject.transform.position.z - transform.position.z), 2));
                        directionA = Vector3.Normalize(aldeanoObject.transform.position - transform.position);
                        distanceA = Mathf.Sqrt(Mathf.Pow((aldeanoObject.transform.position.x - transform.position.x), 2) + Mathf.Pow((aldeanoObject.transform.position.y - transform.position.y), 2) + Mathf.Pow((aldeanoObject.transform.position.z - transform.position.z), 2));
                        if (!hambriento)
                        {
                            if (distanceH <= 5 && distanceA <= 5)
                            {
                                hambriento = true;
                                estado = Estado.pursuing;
                                direction = directionA;
                            }
                            else if (distanceH <= 5)
                            {
                                hambriento = true;
                                estado = Estado.pursuing;
                                direction = directionH;
                            }
                            else if (distanceA <= 5)
                            {
                                hambriento = true;
                                estado = Estado.pursuing;
                                direction = directionA;
                            }
                        }
                        if (hambriento)
                        {
                            if (distanceA > 5 && distanceH > 5)
                            {
                                hambriento = false;
                            }
                        }                       
                    }                        
                }
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaAldeanos());
            }
            /// <summary>
            /// /utilisamos un contador "t" para determinar el tiempo entre cada estado del zombie 
            /// y acceder al siguiente estado de manera alkeatorea cada 5 segundos y cambia la direccion en la que mira
            /// en el estado moving el zombie se desplaza hacia el vector z con una velociad reducida 
            /// </summary>
            private void Update()
            {
               
                if (t >= 3)
                {
                    estado = (Estado)Random.Range(0, 3);

                    t = 0;
                }

               if (!hambriento ) 
               {
                    switch (estado)
                    {
                        case Estado.idel:
                            break;
                        case Estado.moving:
                            this.gameObject.transform.Translate(0f, 0f, ((0.012f * 100) / edad));
                            break;
                        case Estado.rotating:
                            this.gameObject.transform.Rotate(0, Random.Range(1f, 15f), 0, 0);
                            break;
                        case Estado.pursuing:                      
                            this.gameObject.transform.position += direction * speed;
                            break;
                        default:
                            break;
                    }
               }
             
            }
            /// <summary>
            /// collision encargada de la infeccion de los humanos y la condicion de perder
            /// </summary>
            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Villiger")
                {
                    Destroy(collision.gameObject.GetComponent<ald.Aldeano>());
                    collision.gameObject.AddComponent<Walker>().infoZomb = collision.gameObject.GetComponent<ald.Aldeano>().infoAlde;
                    collision.gameObject.GetComponent<Walker>().infect = true;
                   
                }

                if (collision.gameObject.tag == "Player")
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

}


