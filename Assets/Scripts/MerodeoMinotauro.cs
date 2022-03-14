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
            Vector3 actualVertexPos = transform.position;


            // Toca seguir un camino
            if (tiempo < tiempoElegir)
            {

                //Si llegamos al nodo objetivo, pasamos al siguiente
                if (transform.position.x <= objetivePos.x + 0.1
                    && transform.position.x >= objetivePos.x - 0.1
                    && transform.position.z <= objetivePos.z + 0.1
                    && transform.position.z >= objetivePos.z - 0.1)
                {
                    
                    vertexActual = vertexObjetive;

                    //Miramos si estamos en el nodo final o no
                    if (idPath == path.Count)
                        setNewPath();
                    else
                    {
                        vertexObjetive = path[idPath];
                        idPath++;
                        Debug.Log(path.Count);
                        Debug.Log(idPath);
                        
                    }    
                }

                tiempo += Time.deltaTime;
            }
            // Toca elegir un nuevo camino
            else
            {
                setNewPath();
            }

            //Recalculamos las posiciones para calcular la direccion
            objetivePos = vertexObjetive.gameObject.transform.position;
            actualVertexPos = transform.position;


            //Movemos al minotauro
            Direccion direccion = new Direccion();
            direccion.lineal = (objetivePos - actualVertexPos);
            direccion.lineal = direccion.lineal.normalized * 3;
            //Orientacion
            direccion.lineal.y = 0;
            transform.rotation = Quaternion.LookRotation(direccion.lineal, Vector3.up);

            return direccion;
        }

        private void setNewPath()
        {
            // elección de objetivo
            // generación de camino
            vertexActual = graph.GetNearestVertex(transform.position);
            final = graph.getRandomVertex();
            path = graph.GetPathAstar(final.gameObject, vertexActual.gameObject, graph.EuclidDist);
            path = graph.Smooth(path);

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
