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

    void Update()
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

        //Movemos al jugador
        Vector3 direccion;
        direccion = objetivePos - transform.position;
        Debug.Log(objetivePos.ToString());
        rb.velocity = direccion * 10;

        //Si llegamos al nodo objetivo, pasamos al siguiente
        //Hay que cambiar el margen
        if (transform.position.x <= objetivePos.x + 0.1
            && transform.position.x >= objetivePos.x - 0.1
            && transform.position.z <= objetivePos.z + 0.1
            && transform.position.z >= objetivePos.z - 0.1)
        {

            idPath++;
            vertexObjetive = path[idPath];
        }

        //Orientacion
        direccion.y = 0;
        transform.rotation = Quaternion.LookRotation(direccion, Vector3.up);

        //Animacion
        if (direccion.magnitude > 0)
            anim.SetBool("walk", true);
        else
            anim.SetBool("walk", false);

    }

    public void setGraph(GraphGrid g)
    {
        graph = g;
    }
}
