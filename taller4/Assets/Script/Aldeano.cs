using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zon = NPC.Enemy;

namespace NPC
{
    namespace Ally
    {
        public class Aldeano : MonoBehaviour
        {

            /// <summary>
            /// se realiza una enumeracion para guardar los posibles nombres de los aldeanos 
            /// y se le da nombre a esa enumeracion, tambien creamos un espacio para almacenar la edad de los aldeanos
            /// </summary>
            public InfoAlde infoAlde = new InfoAlde();
            int age;
            GameObject[] zombies;
            float time;
            GameObject zombieObject;
            Vector3 directionZ;
            float distanceZ;
            float t;
            float speed;
            bool corre;
            public enum Nombres
            {
                santiago, rio, tere, troy, Joe,
                Aleesha, Abdirahman, Kaycee, Chantal, Cherise,
                Taiba, Sanah, Jack, Pascal, Kelly,
                Everly, Muna, Anya, Marguerite, Kali

            }
            public enum Estado
            {
                Idle,
                Moving,
                Running,
                Rotating
            }
            public Estado estado;
            public Nombres nombres;
            /// <summary>
            /// igual en en el heroe se le agrega un comoponente  Rigidbody y se desactivan las influencias externas
            /// para que no se mueva debido a colisiones 
            /// le damos un valor random a la edad entre 15 y 100 
            /// y tomamos un nombre random del enumerador ademas de ponerlo en un lugar aleatorio del mapa 
            /// entre las posiciones 10 y -10 en los ejes "x" y "z"
            /// </summary>
            public void Start()
            {
                
            }
            public void Comun()
            {

                age = Random.Range(15, 100);
                nombres = (Nombres)Random.Range(0, 21);
                Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                this.gameObject.name = nombres.ToString();
                float x = Random.Range(-10, 10);
                float z = Random.Range(-10, 10);
                this.gameObject.transform.position = new Vector3(x, 0, z);  
                speed = 10 / age;
                corre = false;
                infoAlde.edad = age;
                infoAlde.name = nombres.ToString();
                StartCoroutine(buscaZombies());
            }
            /// <summary>
            /// almacenamos en el struct del aldeano el nombre que le dimos aleatoreamente
            /// y su edad
            /// </summary>
            /// <returns></returns>
           
            IEnumerator buscaZombies()
            {
                zombies = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (var aGameObject in zombies)
                {
                    Component aComponent = aGameObject.GetComponent<zon.Walker>();
                    if (aComponent != null)
                    {
                        zombieObject = aGameObject;
                        distanceZ = Mathf.Sqrt(Mathf.Pow((zombieObject.transform.position.x - transform.position.x), 2) + Mathf.Pow((zombieObject.transform.position.y - transform.position.y), 2) + Mathf.Pow((zombieObject.transform.position.z - transform.position.z), 2));              
                        if (!corre)
                        {
                            if (distanceZ < 5f)
                            {
                                estado = Estado.Running;
                                
                                corre = true;
                            }
                        }                       
                    }        
                }
                if (corre)                   
                {
                    if (distanceZ > 5f)
                    {
                       corre = false;
                    }
                }
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaZombies());
            }

            private void Update()
            {
                t += Time.deltaTime;                                          

                if(!corre)
                {
                    if (t >= 3)
                    {
                        estado = (Estado)Random.Range(0, 3);

                        t = 0;
                    }
                    switch (estado)
                    {
                        case Estado.Idle:
                            time += Time.deltaTime;
                            break;
                        case Estado.Moving:
                            time += Time.deltaTime;
                            this.gameObject.transform.Translate(0f, 0f, ((0.012f * 100) / age));
                            break;
                        case Estado.Rotating:
                            time += Time.deltaTime;
                            this.gameObject.transform.Rotate(0, Random.Range(1f, 15f), 0, 0);
                            break;
                        case Estado.Running:
                            directionZ = Vector3.Normalize(zombieObject.transform.position - transform.position);
                            transform.position -= directionZ * speed;
                            break;
                        default:
                            break;
                    }
                }
                
            }
            

        }
    }
}
