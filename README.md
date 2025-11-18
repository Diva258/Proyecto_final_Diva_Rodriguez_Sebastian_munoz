Proyecto Final – Juego de Ping Pong Multijugador (Unity + Photon Fusion)

Este proyecto es un juego clásico de Ping Pong que puede jugarse tanto en escritorio como en línea. Fue desarrollado en Unity e implementa conectividad multijugador online utilizando Photon Fusion bajo un modelo Host–Server.


Características principales

Multijugador Host–Client con Photon Fusion:
El Host actúa como servidor y controla toda la lógica del juego, mientras que el cliente se sincroniza en tiempo real.

Movimiento fluido de los jugadores:
Cada jugador controla una paleta sincronizada con NetworkTransform, permitiendo un desplazamiento preciso y estable.

Pelota con física de red:
Su movimiento, rebotes y colisiones se sincronizan entre los jugadores para mantener coherencia durante la partida.

Detección de puntos en el servidor:
El Host valida los goles y actualiza los marcadores.

Respawn automático de la pelota:
Al anotar un punto, la pelota se destruye y reaparece en el centro del campo.

Marcador sincronizado:
Ambos jugadores visualizan el mismo puntaje en tiempo real.

Pantalla de victoria:
Cuando un jugador alcanza los tres puntos, se muestra un panel indicando el ganador.

Sonidos y música:
Incluye efectos de sonido al anotar y música de fondo con control de volumen mediante un slider.



Cómo se juega

El jugador Host crea la partida.

El jugador Cliente se conecta a la partida.

Ambos controlan su paleta de forma vertical.

La pelota rebota entre las paredes.

Si un jugador falla, el oponente gana un punto.

El primer jugador en llegar a tres puntos gana la partida.

Estructura general del proyecto

GameFusionSpawner.cs:
Gestiona la creación de las paletas y la pelota según el rol (Host o Cliente).

PaddleNetworkController.cs:
Controla el movimiento sincronizado de cada jugador.

FusionBallController.cs:
Administra la física, los rebotes y el respawn de la pelota.

ScoreManager.cs:
Controla el sistema de puntaje, la detección del ganador y la interfaz de victoria.

Interfaz de usuario (UI):
Incluye marcadores sincronizados, panel de victoria, botones de reinicio y menú, y control de volumen para la música.
