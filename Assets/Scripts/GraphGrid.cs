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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UCM.IAV.Movimiento;

    public class GraphGrid : Graph
    {
        public GameObject TheseoPrefab;
        public GameObject MinotaurPrefab;
        public GameObject obstaclePrefab;
        public string mapsDir = "Maps"; // Directorio por defecto
        public string mapName = "arena.map"; // Fichero por defecto
        public bool get8Vicinity = false;
        public float cellSize = 1f;
        [Range(0, Mathf.Infinity)]
        public float defaultCost = 1f;
        [Range(0, Mathf.Infinity)]
        public float maximumCost = Mathf.Infinity;

        int numCols;
        int numRows;
        GameObject[] vertexObjs;
        bool[,] mapVertices;
        GameObject teseo;
        GameObject minotauro;

        Vertex exitVertex;

        //Convertidor de posicion a id para las listas
        private int GridToId(int x, int y)
        {
            return Math.Max(numRows, numCols) * y + x;
        }

        //Convertidor de id a posicion 
        private Vector2 IdToGrid(int id)
        {
            Vector2 location = Vector2.zero;
            location.y = Mathf.Floor(id / numCols);
            location.x = Mathf.Floor(id % numCols);
            return location;
        }

        //Carga del mapa
        private void LoadMap(string filename)
        {
            string path = Application.dataPath + "/" + mapsDir + "/" + filename;
            try
            {
                StreamReader strmRdr = new StreamReader(path);
                using (strmRdr)
                {
                    int j = 0;
                    int i = 0;
                    int id = 0;
                    string line;

                    //Lectura de numero de filas y columnas del mapa
                    Vector3 position = Vector3.zero;
                    Vector3 scale = Vector3.zero;
                    line = strmRdr.ReadLine();// non-important line
                    line = strmRdr.ReadLine();// height
                    numRows = int.Parse(line.Split(' ')[1]);
                    line = strmRdr.ReadLine();// width
                    numCols = int.Parse(line.Split(' ')[1]);
                    line = strmRdr.ReadLine();// "map" line in file

                    //Inicializamos las listas de nodos
                    vertices = new List<Vertex>(numRows * numCols);
                    neighbors = new List<List<Vertex>>(numRows * numCols);
                    costs = new List<List<float>>(numRows * numCols);
                    vertexObjs = new GameObject[numRows * numCols];
                    mapVertices = new bool[numRows, numCols];

                    //Recorremos el mapa creando los objetos
                    for (i = 0; i < numRows; i++)
                    {
                        line = strmRdr.ReadLine();
                        for (j = 0; j < numCols; j++)
                        {
                            //Si lees ".", es una casilla de suelo
                            bool isGround = true;
                            if (line[j] == 'T')
                                isGround = false;
                          
                            mapVertices[i, j] = isGround;

                            //Tamaño de la casilla
                            position.x = j * cellSize;
                            position.z = i * cellSize;

                            //Cada casilla tiene su id
                            id = GridToId(j, i);

                            //Si es suelo, creamos un objeto de suelo
                            if (isGround)
                                vertexObjs[id] = Instantiate(vertexPrefab, position, Quaternion.identity) as GameObject;
                            //Si es muro, creamos un objeto de muero
                            else
                            {
                                position.y += 0.5f;
                                vertexObjs[id] = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
                                position.y -= 0.5f;
                            }
                            //Esto es para quitar el nombre de "Clone" que pone el prefab 
                            vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());

                            //Añadimos el componente de vertice (nodo) al objeto incluído
                            //Añadimos el vertcie a la lista y además añadimos una lista de vecinos y costes
                            Vertex v = vertexObjs[id].AddComponent<Vertex>();
                            v.id = id;
                            vertices.Add(v);
                            neighbors.Add(new List<Vertex>());
                            costs.Add(new List<float>());

                            //Nos guardamos el vertice de salida
                            if (line[j] == '*')
                                exitVertex = v;

                            //Theseo
                            else if(line[j] == 'p')
                            {
                                position.y += 1.5f;
                                teseo = Instantiate(TheseoPrefab, position, Quaternion.identity);
                                MovimientoAutomatico mov = teseo.GetComponent<MovimientoAutomatico>();
                                mov.setGraph(this);
                                position.y -= 1.5f;
                            }

                            else if(line[j] == 'M')
                            {
                                position.y += 1.5f;
                                minotauro = Instantiate(MinotaurPrefab, position, Quaternion.identity);
                                MerodeoMinotauro mer = minotauro.GetComponent<MerodeoMinotauro>();
                                mer.setGraph(this);
                                PercepcionMinotauro per = minotauro.GetComponent<PercepcionMinotauro>();
                                per.setGraph(this);
                                position.y -= 1.5f;
                            }

                            //Ajustamos tamaños 
                            float y = vertexObjs[id].transform.localScale.y;
                            scale = new Vector3(cellSize, y, cellSize);
                            vertexObjs[id].transform.localScale = scale;
                            vertexObjs[id].transform.parent = gameObject.transform;
                        }
                    }

                    minotauro.GetComponent<PercepcionMinotauro>().setTeseo(teseo);

                    //Añadimos los vecinos de cada vertice
                    for (i = 0; i < numRows; i++)
                    {
                        for (j = 0; j < numCols; j++)
                        {
                            SetNeighbours(j, i);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void Load()
        {
            LoadMap(mapName);
        }

        //Se llama por cada vertice para setear sus vecinos
        protected void SetNeighbours(int x, int y, bool get8 = false)
        {
            //Obtenemos el vertice exacto
            int col = x;
            int row = y;
            int i, j;
            int vertexId = GridToId(x, y);

            //Introducimos una lista de vertices vecinos por cada nodo (creo que esto se hace arriba y realmente lo estamos sobrescribiendo)
            //Lo mismo para costes
            neighbors[vertexId] = new List<Vertex>();
            costs[vertexId] = new List<float>();

            //Creamos el array de posiciones vecinas
            Vector2[] pos = new Vector2[0];

            if (get8)
            {
                pos = new Vector2[8];
                int c = 0;
                for (i = row - 1; i <= row + 1; i++)
                {
                    for (j = col - 1; j <= col; j++)
                    {
                        pos[c] = new Vector2(j, i);
                        c++;
                    }
                }
            }
            else
            {
                //En principio deberia entrar aqui siempre
                //Cada vertice tiene 4 vecinos
                pos = new Vector2[4];
                pos[0] = new Vector2(col, row - 1);
                pos[1] = new Vector2(col - 1, row);
                pos[2] = new Vector2(col + 1, row);
                pos[3] = new Vector2(col, row + 1);
            }

            //Para cada posicion vecina comprobar que es valida
            //Que no esta fuera del mapa
            foreach (Vector2 p in pos)
            {
                i = (int)p.y;
                j = (int)p.x;
                if (i < 0 || j < 0)
                    continue;
                if (i >= numRows || j >= numCols) 
                    continue;
                if (i == row && j == col)
                    continue;
                if (!mapVertices[i, j])
                    continue;

                //Si ha sido valida la introducimos definitivamente
                int id = GridToId(j, i);
                neighbors[vertexId].Add(vertices[id]);
                costs[vertexId].Add(defaultCost);
            }
        }

        //Devuelve el 
        public override Vertex GetNearestVertex(Vector3 position)
        {
            //Obtenemos el vertice de la posicion
            int col = (int)Math.Round(position.x / cellSize);
            int row = (int)Math.Round(position.z / cellSize);
            Vector2 p = new Vector2(col, row);

            //Creamos una lista de posiciones (nodos) explorados
            //Y una cola 
            List<Vector2> explored = new List<Vector2>();
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(p);

            //
            do
            {
                p = queue.Dequeue();
                col = (int)p.x;
                row = (int)p.y;
                int id = GridToId(col, row);
                if (mapVertices[row, col])
                    return vertices[id];

                if (!explored.Contains(p))
                {
                    explored.Add(p);
                    int i, j;
                    for (i = row - 1; i <= row + 1; i++)
                    {
                        for (j = col - 1; j <= col + 1; j++)
                        {
                            if (i < 0 || j < 0)
                                continue;
                            if (j >= numCols || i >= numRows)
                                continue;
                            if (i == row && j == col)
                                
                            queue.Enqueue(new Vector2(j, i));
                        }
                    }
                }
            } while (queue.Count != 0);
            return null;
        }

        public Vertex getRandomVertex()
        {
            Vertex v = null;
            int r = 0;
            Vector2 pos;
            do
            {
                r = UnityEngine.Random.Range(0, vertices.Count);
                pos = IdToGrid(r);
                v = vertices[r];
            } while (!mapVertices[(int)pos.x, (int)pos.y]);

            return v;
        }
        public Vertex getExit()
        {
            return exitVertex;
        }

    }
}
