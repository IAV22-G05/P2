using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Navegacion
{
    public class MerodeoMinotauro : MonoBehaviour
    {
        // Tiempo entre elecciones de vértice objetivo
        [SerializeField]
        float tiempoElegir = 5.0f;
        // Tiempo desde la última elección de vértice
        float tiempo = 0.0f;
        // Vértice al que se dirige el minotauro
        Vertex objetivo;
        // Camino hasta el vétice objetivo
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
                // elección de objetivo

                // generación de camino
                Vertex o = grafo.GetNearestVertex(transform.position);
                Vertex d = grafo.getRandomVertex();
                camino = grafo.GetPathBFS(o.gameObject, d.gameObject);
            }
        }
    }
}
