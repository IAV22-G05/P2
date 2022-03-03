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
            
Las siguientes funciones devuelven caminos entre 2 vértices del grafo

#### -GetPathBFS
Algoritmo de recorrido en anchura.
Visita todos los nodos en orden de distancia desde el nodo origen y se queda con el camino mas corto.

#### -GetPathDFS
Algoritmo de recorrido en profundidad.
Recorre todos los vértices alcanzables desde el origen guardando para cada vértices si lo ha visitado o no.
Este algoritmo en principio no nos interesa para realizar la práctica.

#### Funciones auxiliares
Esta clase usa una serie de funciones que facilitan algunos calculos de coordenadas entre vértices y puntos.
##### -BuildPath (reconstruye el camino para ser valido)
##### -GetNeighbours (devuelve los vecinos de un vértice)
##### -GetSize (devuelve el tamaño de la lista de vertices del grafo)
##### -EuclidDist (Vertex a, Vertex b) (Heurística de distancia euclídea)
##### -ManhattanDist (Vertex a, Vertex b) (Heurística de distancia Manhattan)

      
### Clase Vertex:

Representa cada vértice del grafo.

Consta de 3 parámetros:

      -Un id (int) para poder identificarlo en la lista de vértices del grafo
      -Lista de vértices vecinos (Edges)
      -Vértice previo (en funcion a la lista de vértices del grafo).


### Clase Edge:

Es la clase que representa una artista entre vértices.

Esta asociada a un vértice y tiene una variable con el coste.


### Clase GraphGrid:

Clase hija de "Graph" que se encarga de leer y crear el mapa, representándolo con el grafo.

Aquí se explican brevemente sus funciones.

#### -LoadMap
Carga del mapa desde fichero.

Crea un tablero por filas y columnas.

Inicializa las listas de nodos.

Recorre el tablero y por cada casilla decide qué instanciar, suelo o muro.

Mete esa casilla como vértice añadiendole su componente y añadiendolo a la lista de vértices del tablero.

Despues de añadir cada vértice y configurarlo, se encarga de asignar los vecinos de cada uno.

#### -SetNeighbours
Por cada vértice que le llega, crea su lista de vértices vecinos y asocia las posiciones.

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

## RESOLUCION DE LA PRACTICA

En este apartado se explica cómo vamos a implementar la solución a la practica.

## RESUMEN
Esta práctica se basa en el mito del Laberinto del Minotauro.

Como su nombre indica, tendremos un laberinto representado internamente con un grafo, el cual tedrá entrada y salida.

En el laberinto estarán 2 agentes, Teseo (el jugador) y el Minotauro (enemigo).

El jugador podrá mover por el laberinto a Teseo mediante input, pero si mantiene el espacio, se calculará el camino más óptimo


## JUGADOR
El jugador tiene 2 funcionamientos, movimiento octodireccional por input o automático siguiendo el camino óptimo hasta encontrar la salida al laberinto.

### CLASE ControlJugador
Esta clase lleva el control del input para el movimiento por teclas

#### Update

Maneja el movimiento por input del jugador

      // Movimiento octodireccional //
      Vector3 direccion
      if(Input.w)
            direccion.z++
      if(Input.s)
            direccion.z--
      if(Input.a)
            direccion.x--
      if(Input.d)
            direccion.x++
      
      Calculamos la velocidad en funcion a la direccion acumulada
      Orientamos al jugador en la direccion
      
      
      // Movimiento automatico //
      if(Input.Space)
            Activamos script de movimiento automatico
      else if(Input.Space soltamos)
            Desactivamos script de movimiento automaico
            
      // Camino Suavizado //
      if(!MovimientoAuto.enable && Input.s)
            activamos o desactivamos el booleano de suavizado del MovimientoAuto 
            MovimientoAuto.SetSmooth(!MovimientoAuto.GetSmooth) 
      
### CLASE MovimientoAutomatico
Esta clase pide el cálculo del camino y mueve al jugador a la siguiente casilla

Variables de clase
      lista vértices path (el camino hasta la salida)
      vertexObjetive (siguiente nodo del camino)
      idPath (para saber sobre que nodo llegamos)
      Vertex final (nodo de salida del laberinto)
      bool smooth (para decir si queremos camino suavizado o no)

#### OnEnable
Esta función se llama automaticamente al activar el script, se encarga de calcular el camino desde el punto en el que se encuentra en ese momento el jugador.

      Vertex inicio = GetNearestVertex(jugador.pos)
      path = GetPathAstar (inicio, final)
      //Miramos si el camino lo queremos suavizado o no 
      if(smooth)
            path = Smooth(path)
      idPath = 0
      vertexObjetive = path[idPath] //el nodo objetivo es el siguiente al que tiene que ir el jugador
     

#### Update
Como ya tiene el camino calculado, lo que hace es ir moviendo al jugador entre los nodos del camino.
      
      //Movemos al jugador
      Vector3 direccion = vertexObjetive.pos - jugador.pos
      velocidad = direccion
      
      //Si llegamos al nodo objetivo, pasamos al siguiente
      if(jugador.pos = vertexObjetive.pos (con margen))
            idPath++
            vertexObjetive = path[idPath]
            
##### SetSmooth(bool)
##### GetSmooth(bool)

## MINOTAURO
Cuando no esté viendo al jugador, el minotauro se moverá mediante un merodeo, eligiendo de manera aleatoria un nodo cada cierto tiempo y buscando el camino más rápido hasta él.

### CLASE MerodeoMinotauro

Variables de clase

   float tiempoElegir // tiempo entre elección de siguiente nodo
      
   float tiempo // tiempo desde la última elección de nodo
      
   Nodo nodoObjetivo // nodo elegido hacia el que moverse
      
   Camino camino // camino para llegar al nodo objetivo

#### Update

      if nodoObjetivo != null && tiempo < tiempoElegir
            -movimiento hacia el nodo por el camino
            tiempo++
      else
            nodoObjetivo = (nodo aleatorio del mapa)
            camino = (camino hasta el nodo elegido por A*)
	
Al mismo tiempo que realiza este movimiento se comprobará si ve al jugador, para ello se mirará si este se encuentra en alguno
de los nodos alcanzables en línea recta en la dirección en la que esté mirando (hasta llegar a una pared o una distancia máxima) por el minotauro.

### CLASE PercepcionMinotauro

Variables de clase

   float distancia // distancia máxima
      
   int sentidoX // (1 si está mirando hacia x positiva, -1 si está mirando hacia x negativa, 0 si está mirando hacia y)
      
   int sentidoy // (1 si está mirando hacia y positiva, -1 si está mirando hacia y negativa, 0 si está mirando hacia x)
      
#### Update

      (cálculo de sentidoX y sentidoY)
      bool encontrado
      int i = 0
      while i < distancia && !encontrado
            Vector3 posM(minotauro.pos.x + (i * sentidoX) , minotauro.pos.y (i * sentidoY), minotauro.pos.z)
            Nodo n = nodoDesdePosicion(posM)
            if n == null // Si n es null significa que hay una pared 
                  break
            encontrado = (nodoDesdePosicion(jugador.pos) == n)
            i += (distancia entre nodos)

Mientras tenga al jugador a la vista, el minotauro intentará alcanzarle mediante un algoritmo de persecución, buscando el camino
más corto al jugador mediante A*.
            
## ALGORITMO A*
Se utilizará este algoritmo para realizar las búsquedas de caminos que necesite cada personaje.

### pathfindAStar(graph: Graph, start: Node, end: Node, heuristic: Heuristic):
	  // Esta estructura se utiliza para guardar la información de cada nodo
	  class NodeRecord:
		node: Node
		connection: Connection
		costSoFar: float
		estimatedTotalCost: float

	  // Inicializa el record para el nodo de inicio
	  startRecord = new NodeRecord()
	  startRecord.node = start
	  startRecord.connection = null
	  startRecord.costSoFar = 0
	  startRecord.estimatedTotalCost = heuristic.estimate(start)

	  // Inicializa las listas de abiertos y cerrados
	  open = new PathfindingList()
	  open += startRecord
	  closed = new PathfindingList()

	  // Itera procesando cada nodo
	  while length(open) > 0:

		// Encuentra el menor elemento en la lista de abiertos (usando estimatedTotalCost).
		current = open.smallestElement()
		// Si es el nodo objetivo, termina el bucle.
		if current.node == goal:
			break

		// Si no obtiene sus conexiones a los siguientes nodos.
		connections = graph.getConnections(current)

		// Itera por cada conexión.
		for connection in connections:
			// Obtiene el coste estimado para el nodo final.
			endNode = connection.getToNode()
			endNodeCost = current.costSoFar + connection.getCost()

			 // Si el nodo está cerrado hay que saltarlo o eliminarlo de la lista de cerrados.
			 if closed.contains(endNode):
			 // Encuentra el record para el nodo cerrado.
				endNodeRecord = closed.find(endNode)

				// Si no encuentra una ruta más corta salta el nodo.
				if endNodeRecord.costSoFar <= endNodeCost:
					continue

				// Si la encuentra lo elimina de la lista de cerrados.
				closed -= endNodeRecord

				// Usa los antiguos valores del nodo para calcular su heuristica sin llamar a la funcion heuristica.
				endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar

			// Salta el nodo si está abierto y no encuentra una mejor ruta.
			else if open.contains(endNode):
			// Encuentra el record en la lista de abiertos para el nodo final.
				endNodeRecord = open.find(endNode)

				// Si la ruta no es mejor, lo salta.
				if endNodeRecord.costSoFar <= endNodeCost:
					continue

				// Calcula su heuristica.
				endNodeHeuristic = endNodeRecord.cost - endNodeRecord.costSoFar

			// Si no, tiene un nodo no visitado, asi que guarda su record.
			else:
				endNodeRecord = new NodeRecord()
				endNodeRecord.node = endNode

				// Calcula el valor heuristico usando la función, ya que no tiene un record que usar.
				endNodeHeuristic = heuristic.estimate(endNode)

			// Actualiza el coste, el estimado y la conexión del nodo.
			endNodeRecord.cost = endNodeCost
			endNodeRecord.connection = connection
			endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic

			// Y lo añade a la lista de abiertos.
			if not open.contains(endNode):
				open += endNodeRecord

		// Añade el nodo a la lista de cerrados y lo añade a la de abiertos.
		open -= current
		closed += current

	   if current.node != goal:   
		// No hay más nodos y no ha encontrado el final así que no hay solución.
		return null

	   else:
		// Compila la lista de conexiones en el camino.
		path = []

		// Recorre el camino hacia atrás acumulando conexiones.
		while current.node != start:
			path += current.connection
			current = current.connection.getFromNode()

		// Da la vuelta al camino y lo devuelve.
		return reverse(path)

## ALGORITMO DE SUAVIZADO
Se utilizará para suavizar los caminos encontrados por A*

### smoothPath(inputPath: Vector[]):
	// Si el camino solo contiene dos nodos no puede suavizarlo.
	if len(inputPath) == 2:
		return inputPath
	// Compila un camino de salida.
	outputPath = [inputPath[0]]

	// Guarda la posición en el camino de entrada, empieza en 2 porque se assume que 2 nodos adyacentes pasarán el ray cast.
	inputIndex: int = 2

 	// Itera hasta encontrar el último item de la entrada.
 	while inputIndex < len(inputPath) - 1:
 		// Ray cast.
 		fromPt = outputPath[len(outputPath) - 1]
 		toPt = inputPath[inputIndex]
 		if not rayClear(fromPt, toPt):
			// Añade el último nodo que superó el ray cast al camino de salida.
			outputPath += inputPath[inputIndex - 1]

	// Considera el siguiente nodo.
 	inputIndex ++

 	// Añade el último nodo al camino de salida y lo devuelve.
 	outputPath += inputPath[len(inputPath) - 1]

	return outputPath

