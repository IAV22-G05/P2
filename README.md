# INTELIGENCIA ARTIFICIAL PARA VIDEOJUEGOS - PRÁCTICA 2 - GRUPO 05

Sergio Molinero Aparicio - Andrés Carnicero Esteban

Resumen de objetivos.

## PUNTO DE PARTIDA
Estas son las clases ya implementadas al comienzo de la práctica.


### Clase Graph:

Es la clase base abstracta que representa el grafo.

Como variables de clase principales nos encontramos con

      Lista de vertices
      Y por cada vertice:
            Lista de vecinos 
            Lista de costes
            
      Lista de vertices de determina un camino
            
Las siguientes funciones devuelven caminos entre 2 vertices del grafo

#### -GetPathBFS
Algoritmo de recorrido en anchura.
Visita todos los nodos en orden de distancia desde el nodo origen y se queda con el camino mas corto.

#### -GetPathDFS
Algoritmo de recorrido en profundidad.
Recorre todos los vertices alcanzables desde el origen guardando para cada vertice si lo ha visitado o no.
Este algoritmo en principio no nos interesa para realizar la practica.

#### Funciones auxiliares
Esta clase usa una serie de funciones que facilitan algunos calculos de coordenadas entre vertices y puntos.
##### -BuildPath (reconstruye el camino para ser valido)
##### -GetNeighbours (devuelve los vecinos de un vertice)
##### -GetSize (devuelve el tamaño de la lista de vertices del grafo)
##### -EuclidDist (Vertex a, Vertex b) (Heurística de distancia euclídea)
##### -ManhattanDist (Vertex a, Vertex b) (Heurística de distancia Manhattan)

      
### Clase Vertex:

Representa cada vertice del grafo.

Consta de 3 parámetros:

      -Un id (int) para poder identificarlo en la lista de vertices del grafo
      -Lista de vertices vecinos (Edges)
      -Vertice previo (en funcion a la lista de vertices del grafo).


### Clase Edge:

Es la clase que representa una artista entre vertices.

Esta asociada a un vertice y tiene una variable con el coste.


### Clase GraphGrid:

Clase hija de "Graph" que se encarga de leer y crear el mapa, representandolo con el grafo.

Aquí se explican brevemente sus funciones.

#### -LoadMap
Carga del mapa desde fichero.

Crea un tablero por filas y columnas.

Inicializa las listas de nodos.

Recorre el tablero y por cada casilla decide qué instanciar, suelo o muro.

Mete esa casilla como vertice añadiendole su componente y añadiendolo a la lista de vertices del tablero.

Despues de añadir cada vertice y configurarlo, se encarga de asignar los vecinos de cada uno.

#### -SetNeighbours
Por cada vertice que le llega, crea su lista de vertices vecinos y asocia las posiciones.

Tiene en cuenta si la posicion es valida (esta dentro del mapa).

#### -GetNearestVertex
Le llega una posición y devuelve el vértice sobre el que se encuentre.

#### Funciones auxiliares
Esta clase usa una serie de funciones que facilitan algunos calculos de coordenadas entre vertices y puntos.
##### -GridToId(int x, int y)
##### -IdToGrid(int id)
##### -Load()


### Clase TesterGraph

#### -Update
 El origen se marca haciendo click.
 
 El destino simplemente poniendo el ratón encima
 
 Con la barra espaciadora se activa la búsqueda del camino mínimo

Si hay ya camino calculado ponemos el anterior en blanco y creamos uno nuevo

Elegimos con que algoritmo queremos calcular el camino

Si tenemos activado el suavizado de caminos 

#### -OnDrawGizmos

Se encarga de dibujar los caminos que vamos calculando.

Hay que tener en cuenta que solo se ve en el modo Scene.

#### -ShowPath

Cambia de color los vertices que representen un camino.

#### -GetNodeFromScreen

Sirve para traducir la posicion del raton a coordenadas del grafo y el tablero.


### Clase BinaryHeap
Es un TAD auxiliar que nos va a ayudar a implementar las colas de prioridad que se usan en los algortimos.

