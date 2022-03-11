/*    
    Obra original:
        Copyright (c) 2018 Packt
        Unity 2018 Artificial Intelligence Cookbook - Second Edition, by Jorge Palacios
        https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition
        MIT License

    Modificaciones:
        Copyright (C) 2020-2022 Federico Peinado
        http://www.federicopeinado.com

        Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
        Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
        Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Navegacion
{

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Abstract class for graphs
    /// </summary>
    
    public abstract class Graph : MonoBehaviour
    {
        struct NodeRecord
        {
            public Vertex node;
            public Vertex connection;
            public float costSoFar;
            public float estimatedTotalCost;

            public static NodeRecord operator <(NodeRecord a, NodeRecord b)
            {
                if (a.estimatedTotalCost < b.estimatedTotalCost)
                    return a;
                else
                    return b;
            }

            public static NodeRecord operator >(NodeRecord a, NodeRecord b)
            {
                if (a.estimatedTotalCost > b.estimatedTotalCost)
                    return a;
                else
                    return b;
            }
        }

        public GameObject vertexPrefab;
        protected List<Vertex> vertices;
        protected List<List<Vertex>> neighbors;
        protected List<List<float>> costs;
        //protected Dictionary<int, int> instIdToId;

        //// this is for informed search like A*
        public delegate float Heuristic(Vertex a, Vertex b);

        // Used for getting path in frames
        public List<Vertex> path;
        //public bool isFinished;

        public virtual void Start()
        {
            Load();
        }

        public virtual void Load() { }

        public virtual int GetSize()
        {
            if (ReferenceEquals(vertices, null))
                return 0;
            return vertices.Count;
        }

        public virtual Vertex GetNearestVertex(Vector3 position)
        {
            return null;
        }


        public virtual Vertex[] GetNeighbours(Vertex v)
        {
            if (ReferenceEquals(neighbors, null) || neighbors.Count == 0)
                return new Vertex[0];
            if (v.id < 0 || v.id >= neighbors.Count)
                return new Vertex[0];
            return neighbors[v.id].ToArray();
        }

        // Encuentra caminos óptimos
        public List<Vertex> GetPathBFS(GameObject srcO, GameObject dstO)
        {
            if (srcO == null || dstO == null)
                return new List<Vertex>();
            Vertex[] neighbours;
            Queue<Vertex> q = new Queue<Vertex>();
            Vertex src = GetNearestVertex(srcO.transform.position);
            Vertex dst = GetNearestVertex(dstO.transform.position);
            Vertex v;
            int[] previous = new int[vertices.Count];
            for (int i = 0; i < previous.Length; i++)
                previous[i] = -1;
            previous[src.id] = src.id; // El vértice que tenga de previo a sí mismo, es el vértice origen
            q.Enqueue(src);
            while (q.Count != 0)
            {
                v = q.Dequeue();
                if (ReferenceEquals(v, dst))
                {
                    return BuildPath(src.id, v.id, ref previous);
                }

                neighbours = GetNeighbours(v);
                foreach (Vertex n in neighbours)
                {
                    if (previous[n.id] != -1)
                        continue;
                    previous[n.id] = v.id; // El vecino n tiene de 'padre' a v
                    q.Enqueue(n);
                }
            }
            return new List<Vertex>();
        }

        // No encuentra caminos óptimos
        public List<Vertex> GetPathDFS(GameObject srcO, GameObject dstO)
        {
            if (srcO == null || dstO == null)
                return new List<Vertex>();
            Vertex src = GetNearestVertex(srcO.transform.position);
            Vertex dst = GetNearestVertex(dstO.transform.position);
            Vertex[] neighbours;
            Vertex v;
            int[] previous = new int[vertices.Count];
            for (int i = 0; i < previous.Length; i++)
                previous[i] = -1;
            previous[src.id] = src.id;
            Stack<Vertex> s = new Stack<Vertex>();
            s.Push(src);
            while (s.Count != 0)
            {
                v = s.Pop();
                if (ReferenceEquals(v, dst))
                {
                    return BuildPath(src.id, v.id, ref previous);
                }

                neighbours = GetNeighbours(v);
                foreach (Vertex n in neighbours)
                {
                    if (previous[n.id] != -1)
                        continue;
                    previous[n.id] = v.id;
                    s.Push(n);
                }
            }
           
            return new List<Vertex>();
        }

        public List<Vertex> GetPathAstar(GameObject srcO, GameObject dstO, Heuristic h = null)
        {

            //Cogemos los nodos de inicio y final
            Vertex start = GetNearestVertex(srcO.transform.position);
            Vertex end = GetNearestVertex(dstO.transform.position);
            NodeRecord current;

            //Inicializa el record para el nodo de inicio
            NodeRecord startRecord = new NodeRecord();
            NodeRecord closedRecord = new NodeRecord();
            NodeRecord openRecord = new NodeRecord();
            startRecord.node = start;
            startRecord.connection = null;
            startRecord.costSoFar = 0;
            startRecord.estimatedTotalCost = h.Invoke(start, end);

            //Inicializa las listas de abiertos y cerrados
            List<NodeRecord> open = new List<NodeRecord>();
            List<NodeRecord> closed = new List<NodeRecord>();
            open.Add(startRecord);
            List<Edge> connections;
            Vertex endNode;
            float endNodeCost;
            float endNodeHeuristic;

            //Itera procesando cada nodo
            while (open.Count > 0)
            {
                open.Sort();
                //Encuentra el menor elemento en la lista de abiertos (usando estimatedTotalCost).
                current = open[0];
                //Si es el nodo objetivo, termina el bucle.
                if (current.node == end)
                    break;

                //Si no obtiene sus conexiones a los siguientes nodos.
                connections = current.node.vecinos;

                //Itera por cada conexion
                foreach (Edge connection in connections)
                {
                    // Obtiene el coste estimado para el nodo final.
                    endNode = connection.vertex;
                    endNodeCost = current.costSoFar + connection.cost;

                    closedRecord = FindInClosed(closed, endNode);
                    openRecord = FindInOpen(open, endNode);
                    // Si el nodo está cerrado hay que saltarlo o eliminarlo de la lista de cerrados.
                    if (closedRecord.node == endNode)
                    {
                        // Si no encuentra una ruta más corta salta el nodo.
                        if (closedRecord.costSoFar <= endNodeCost)
                            continue;

                        // Si la encuentra lo elimina de la lista de cerrados.
                        closed.Remove(closedRecord);

                        // Usa los antiguos valores del nodo para calcular su heuristica sin llamar a la funcion heuristica.
                        endNodeHeuristic = closedRecord.estimatedTotalCost - closedRecord.costSoFar;
                    }
                    // Salta el nodo si está abierto y no encuentra una mejor ruta.
                    else if (openRecord.node == endNode)
                    {
                        // Si la ruta no es mejor, lo salta.
                        if (openRecord.costSoFar <= endNodeCost)
                            continue;

                        // Calcula su heuristica.
                        //float endNodeHeuristic = endRecord.cost - endRecord.costSoFar; //no se si es esta
                        endNodeHeuristic = openRecord.estimatedTotalCost - openRecord.costSoFar;
                    }
                    // Si no, tiene un nodo no visitado, asi que guarda su record.
                    else
                    {
                        endRecord = new NodeRecord();

                        endRecord.node = end;

                        // Calcula el valor heuristico usando la función, ya que no tiene un record que usar.
                        endNodeHeuristic = heuristic.estimate(endNode);
                    }

                    // Actualiza el coste, el estimado y la conexión del nodo.
                    endRecord.cost = endNodeCost;

                    endRecord.connection = connection;

                    endRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;

                    // Y lo añade a la lista de abiertos.
                    if (!open.Contains(end))
                        open.Add(endRecord.node);
                }

                // Quita el nodo de la lista de cerrados y lo añade a la de abiertos.
                open.Remove(current.node);
                closed.Add(current.node);

            }
            
            if (current.node != goal)
            {
                // No hay más nodos y no ha encontrado el final así que no hay solución.
                return null;
            }
            else
            {
                // Compila la lista de conexiones en el camino.
                path = [];

                // Recorre el camino hacia atrás acumulando conexiones.
                while (current.node != start)
                    path += current.connection;

                current = current.connection.getFromNode();

                // Da la vuelta al camino y lo devuelve.
                return BuildPath(path);
                return new List<Vertex>();
            }            
        }

        private NodeRecord FindInClosed(List<NodeRecord> closed, Vertex node)
        {
            for(int i = 0; i < closed.Count; ++i)
            {
                if (closed[i].node == node)
                    return closed[i];
            }
            return new NodeRecord();
        }

        private NodeRecord FindInOpen(List<NodeRecord> open, Vertex node)
        {
            for (int i = 0; i < open.Count; ++i)
            {
                if (open[i].node == node)
                    return open[i];
            }
            return new NodeRecord();
        }

        public List<Vertex> Smooth(List<Vertex> path)
        {
            // AQUÍ HAY QUE PONER LA IMPLEMENTACIÓN DEL ALGORITMO DE SUAVIZADO
            // ...

            return null; //newPath
        }

        // Reconstruir el camino, dando la vuelta a la lista de nodos 'padres' /previos que hemos ido anotando
        private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList)
        {
            List<Vertex> path = new List<Vertex>();
            int prev = dstId;
            do
            {
                path.Add(vertices[prev]);
                prev = prevList[prev];
            } while (prev != srcId);
            return path;
        }

        // Sí me parece razonable que la heurística trabaje con la escena de Unity
        // Heurística de distancia euclídea
        public float EuclidDist(Vertex a, Vertex b)
        {
            Vector3 posA = a.transform.position;
            Vector3 posB = b.transform.position;
            return Vector3.Distance(posA, posB);
        }

        // Heurística de distancia Manhattan
        public float ManhattanDist(Vertex a, Vertex b)
        {
            Vector3 posA = a.transform.position;
            Vector3 posB = b.transform.position;
            return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
        }
    }
}