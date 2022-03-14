using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UCM.IAV.Navegacion;
using UnityEngine;

public class PercepcionMinotauro : ComportamientoAgente
{
    // Si ve a teseo o no
    bool seeing;

    List<Vertex> path;

    GraphGrid graph;

    MerodeoMinotauro merodeo;
    GameObject teseo;
    Vertex vertexActual;
    Vertex vertexObj;
    Animator anim;
    int idPath;
    bool seen = false;

    private void Start()
    {
        merodeo = GetComponent<MerodeoMinotauro>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Ray cast.
        Vertex fromPt = graph.GetNearestVertex(transform.position);
        Vertex toPt = graph.GetNearestVertex(teseo.transform.position);

        Vector3 dir = transform.TransformDirection(Vector3.forward);
        float distance = Vector3.Distance(transform.position, teseo.transform.position);
        int playerLayerMask = 1 << 6;
        int invertedPlayerLayerMask = ~playerLayerMask;
        // 6 es el id de la capa donde está el jugador y 0 el id de la capa de las paredes
        if (Physics.Raycast(transform.position, dir, distance, playerLayerMask) && !Physics.Raycast(teseo.transform.position, dir, distance, invertedPlayerLayerMask)){
            merodeo.enabled = false;
            seen = true;
            path = graph.GetPathAstar(fromPt.gameObject, toPt.gameObject, graph.EuclidDist);
            idPath = 0;
            vertexActual = fromPt;
            vertexObj = path[0];
        }
        else
        {
            merodeo.enabled = true;
            seen = false;
        }
    }

    public override Direccion GetDireccion()
    {
        if (seen)
        {
            //Guardamos pos del vertice
            Vector3 objetivePos = vertexObj.gameObject.transform.position;
            Vector3 actualVertexPos = transform.position;

            //Movemos al jugador
            Direccion direccion = new Direccion();
            direccion.lineal = objetivePos - actualVertexPos;
            direccion.lineal = direccion.lineal.normalized;
            //Debug.Log(direccion.lineal);

            //Si llegamos al nodo objetivo, pasamos al siguiente
            //Hay que cambiar el margen
            if (transform.position.x <= objetivePos.x + 0.1
                && transform.position.x >= objetivePos.x - 0.1
                && transform.position.z <= objetivePos.z + 0.1
                && transform.position.z >= objetivePos.z - 0.1)
            {

                vertexActual = vertexObj;
                idPath++;
                vertexObj = path[idPath];
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
        return new Direccion();
    }

    public void setGraph(GraphGrid g)
    {
        graph = g;
    }

    public void setTeseo(GameObject t)
    {
        teseo = t;
    }
}
