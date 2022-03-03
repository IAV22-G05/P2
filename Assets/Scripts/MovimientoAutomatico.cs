using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCM.IAV.Navegacion;

public class MovimientoAutomatico : MonoBehaviour
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

    //Id del siguiente nodo al que ir
    int idPath;

    //Referencia al Rigidbody
    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        //Cogemos los vertices de inicio y fin
        Vertex inicio = graph.GetNearestVertex(transform.position);
        final = graph.getExit();
        //path = graph.GetPathAstar(inicio.gameObject, final.gameObject);
        path = graph.GetPathBFS(inicio.gameObject, final.gameObject);

        //Miramos si el camino lo queremos suavizado o no 
        smooth = false;
        if (smooth)
            path = graph.Smooth(path);

        //Incializamos el id
        idPath = 0;

        //el nodo objetivo es el siguiente al que tiene que ir el jugador
        vertexObjetive = path[idPath]; 
    }

    void Update()
    {
        //Guardamos pos del vertice
        Vector3 objetivePos = vertexObjetive.gameObject.transform.position;

        //Movemos al jugador
        Vector3 direccion;
        direccion = objetivePos - transform.position;
        rb.velocity = direccion ;

        //Si llegamos al nodo objetivo, pasamos al siguiente
        //Hay que cambiar el margen
        if (transform.position == objetivePos)
        {
            idPath++;
            vertexObjetive = path[idPath];
        }
    }

    public void setGraph(GraphGrid g)
    {
        graph = g;
    }
}
