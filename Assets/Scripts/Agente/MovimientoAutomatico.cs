

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
        bool smooth = false;

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

        //LineRenderer
        LineRenderer line;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            line = GetComponent<LineRenderer>();
        }


        private void OnEnable()
        {
            //Cogemos los vertices de inicio y fin
            Vertex inicio = graph.GetNearestVertex(transform.position);
            vertexActual = inicio;
            final = graph.getExit();
            //path = graph.GetPathAstar(inicio.gameObject, final.gameObject);
            path = graph.GetPathAstar(final.gameObject, inicio.gameObject, graph.EuclidDist);


            //Miramos si el camino lo queremos suavizado o no 
            if (smooth)
                path = graph.Smooth(path);

            //Incializamos el id
            idPath = 0;

            //el nodo objetivo es el siguiente al que tiene que ir el jugador
            vertexObjetive = path[idPath];

            //Dibujamos el camino
            if(path.Count>=2)
            {
                int nextIndex = 0;
                    Debug.Log("Carballo");
                line.positionCount = path.Count;
                line.SetWidth(0.1f, 0.1f);

                //Primera posicion
                Vertex next = path[nextIndex];
                Vector3 pos = next.gameObject.transform.position;
                pos.y += 1;
                line.SetPosition(nextIndex, inicio.gameObject.transform.position);
                foreach (Vertex v in path)
                {
                    nextIndex++;
                    next = path[nextIndex];
                    pos = next.gameObject.transform.position;
                    pos.y += 1;
                    line.SetPosition(nextIndex, pos);
                }
            }
            
        }
        private void OnDisable()
        {
            line.positionCount = 0;
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
            Vector3 actualVertexPos1 = transform.position;

            //Movemos al jugador
            Direccion direccion =  new Direccion();
            direccion.lineal = objetivePos - actualVertexPos1;
            direccion.lineal = direccion.lineal.normalized;
            //Debug.Log(direccion.lineal);

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

            //ME DICE JOSSEDA QUE LO CAMBIE CON LA MAGNITUD DE LA DISTANCIA TIENE RAZON


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

        public void setSmooth(bool s)
        {
            smooth = s;
        }
        public bool getSmooth()
        {
            return smooth;
        }
    }
}

