

namespace UCM.IAV.Movimiento
{

    using UnityEngine;
    using UCM.IAV.Navegacion;
    using System.Collections.Generic;

    /// <summary>
    /// Clara para el comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class MovimientoAutomatico : ComportamientoAgente
    {
        //Indica si queremos camino suavizado o no
        bool smooth;

        //Camino a seguir
        List<Vertex> path;

        //Referencia al grafo del mapa
        GraphGrid graph;

        //Nodos relevantes
        Vertex final;
        Vertex vertexObjetive;
        Vertex vertexActual;

        //Id del siguiente nodo al que ir
        int idPath;

        //Referencia al Rigidbody
        Rigidbody rb;
        Animator anim;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
        }


        private void OnEnable()
        {
            //Cogemos los vertices de inicio y fin
            Vertex inicio = graph.GetNearestVertex(transform.position);
            vertexActual = inicio;
            final = graph.getExit();
            //path = graph.GetPathAstar(inicio.gameObject, final.gameObject);
            path = graph.GetPathBFS(final.gameObject, inicio.gameObject);


            //Miramos si el camino lo queremos suavizado o no 
            smooth = false;
            if (smooth)
                path = graph.Smooth(path);

            //Incializamos el id
            idPath = 0;

            //el nodo objetivo es el siguiente al que tiene que ir el jugador
            vertexObjetive = path[idPath];
        }

        public override Direccion GetDireccion()
        {
            //PRUEBAS
            //Vertex actualVertex = graph.GetNearestVertex(transform.position);
            //if (actualVertex == vertexObjetive)
            //{
            //    idPath++;
            //    vertexObjetive = path[idPath];
            //}

            //Guardamos pos del vertice
            Vector3 objetivePos = vertexObjetive.gameObject.transform.position;
            Vector3 actualVertexPos = vertexActual.gameObject.transform.position;

            //Movemos al jugador
            Direccion direccion =  new Direccion();
            direccion.lineal = objetivePos - actualVertexPos;
            direccion.lineal = direccion.lineal.normalized;
            Debug.Log(objetivePos.ToString());

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

            //Orientacion
            direccion.lineal.y = 0;
            transform.rotation = Quaternion.LookRotation(direccion.lineal, Vector3.up);

            //Animacion
            if (direccion.lineal.magnitude > 0)
                anim.SetBool("walk", true);
            else
                anim.SetBool("walk", false);


            //Devolvemos la direccion
            direccion.lineal *= 10;
            return direccion;

        }

        public void setGraph(GraphGrid g)
        {
            graph = g;
        }
    }
}
