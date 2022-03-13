using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{

    using UnityEngine;
    using UCM.IAV.Navegacion;
    using System.Collections.Generic;

    /// <summary>
    /// Clara para el comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class MerodeoMinotauro : ComportamientoAgente
    {
        // Tiempo entre elecciones de vértice objetivo
        [SerializeField]
        float tiempoElegir = 5.0f;

        // Tiempo desde la última elección de vértice
        float tiempo = 0.0f;

        //Nodos relevantes
        Vertex final;
        Vertex vertexObjetive;
        Vertex vertexActual;

        // Camino hasta el vétice objetivo
        int idPath;
        List<Vertex> path;
        GraphGrid graph;

        private void Start()
        {
            setNewPath();
        }

        public override Direccion GetDireccion()
        {

            // movimiento
            //Guardamos pos del vertice
            Vector3 objetivePos = vertexObjetive.gameObject.transform.position;
            Vector3 actualVertexPos = vertexActual.gameObject.transform.position;
            Vector3 actualVertexPos1 = transform.position;
            Vector3 finalPos = final.gameObject.transform.position;



            // Toca seguir un camino
            if (tiempo < tiempoElegir)
            {

                //Si llegamos al ultimo nodo del camino 
                if (transform.position.x <= finalPos.x + 0.1
                    && transform.position.x >= finalPos.x - 0.1
                    && transform.position.z <= finalPos.z + 0.1
                    && transform.position.z >= finalPos.z - 0.1)
                {

                    setNewPath();

                    objetivePos = vertexObjetive.gameObject.transform.position;
                    actualVertexPos = vertexActual.gameObject.transform.position;
                    actualVertexPos1 = transform.position;
                    finalPos = final.gameObject.transform.position;
                }

                //Si llegamos al nodo objetivo, pasamos al siguiente
                //Hay que cambiar el margen
                if (transform.position.x <= objetivePos.x + 0.1
                    && transform.position.x >= objetivePos.x - 0.1
                    && transform.position.z <= objetivePos.z + 0.1
                    && transform.position.z >= objetivePos.z - 0.1)
                {

                    vertexActual = vertexObjetive;
                    idPath++;
                    vertexObjetive = path[idPath];
                }

                tiempo += Time.deltaTime;
            }
            // Toca elegir un nuevo camino
            else
            {
                setNewPath();
            }


            //Movemos al minotauro
            Direccion direccion = new Direccion();
            direccion.lineal = (objetivePos - actualVertexPos1);
            direccion.lineal = direccion.lineal.normalized * 5;

            return direccion;
        }

        private void setNewPath()
        {
            // elección de objetivo
            // generación de camino
            vertexActual = graph.GetNearestVertex(transform.position);
            final = graph.getRandomVertex();
            path = graph.GetPathBFS(final.gameObject, vertexActual.gameObject);

            //Asignamos el nuevo nodo objetivo
            idPath = 0;
            vertexObjetive = path[idPath];

            //Reseteamos el tiempo
            tiempo = 0;
        }

        public void setGraph(GraphGrid g)
        {
            graph = g;
        }
    }

    
}
