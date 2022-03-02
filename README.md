# INTELIGENCIA ARTIFICIAL PARA VIDEOJUEGOS - PRÁCTICA 2 - GRUPO 05

Sergio Molinero Aparicio - Andrés Carnicero Esteban

Resumen de objetivos.

## PUNTO DE PARTIDA
Estas son las clases ya implementadas al comienzo de la práctica.

### Clase Graph:

Es la clase base abstracta que representa el grafo.

Consta de un float con la velocidad angular y un Vector3 con la lineal.

Variables de clase:

      float angular
      Vector3 lineal
      
### Clase Vertex:

Representa cada vertice del grafo.

Consta de 3 parámetros:

      -Un id (int) para poder identificarlo en la lista de vertices del grafo
      -Lista de vertices vecinos (Edges)
      -Vertice previo (en funcion a la lista de vertices del grafo).

### Clase Edge:

Comprendiendo por ahora.

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

#### -LateUpdate
se usa para corregir y ajustar movimientos finales, limitar máximas velocidades, restear variables para su uso en próximas iteraciones o elegir direcciones prioritarias en caso de que haya varias.

#### Funciones auxiliares
Esta clase usa una serie de funciones como setters y getters para facilitar el uso y obtencion de determinados valores.
##### -SetDireccion(Direccion direccion)
##### -SetDireccion(Direccion direccion, float peso)
##### -SetDireccion(Direccion direccion, int prioridad)
##### -Direccion GetPrioridadDireccion()
##### -Vec3 OriToVec(float orientacion)
