using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Navegacion
{
    public class MerodeoMinotauro : MonoBehaviour
    {
        // Tiempo entre elecciones de v�rtice objetivo
        [SerializeField]
        float tiempoElegir = 5.0f;
        // Tiempo desde la �ltima elecci�n de v�rtice
        float tiempo = 0.0f;
        // V�rtice al que se dirige el minotauro
        Vertex objetivo;
        // Camino hasta el v�tice objetivo
        List<Vertex> camino;
        GraphGrid grafo;

        private void Update()
        {
            if(objetivo != null && tiempo < tiempoElegir)
            {
                // movimiento

                tiempo += Time.deltaTime;
            }
            else
            {
                // elecci�n de objetivo

                // generaci�n de camino
                Vertex o = grafo.GetNearestVertex(transform.position);
                Vertex d = grafo.getRandomVertex();
                camino = grafo.GetPathBFS(o.gameObject, d.gameObject);
            }
        }
    }
}
