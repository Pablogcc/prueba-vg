# Fase 1: Base del juego 2D

Esta fase deja preparados los sistemas mínimos para un juego 2D de aventura/survival en Unity.

## Estructura de scripts

- `Assets/Scripts/Player/PlayerMovement2D.cs`: movimiento del jugador con `Rigidbody2D` y actualización opcional de parámetros de `Animator`.
- `Assets/Scripts/Player/PlayerHealth.cs`: vida, invulnerabilidad temporal, muerte y respawn.
- `Assets/Scripts/Camera/CameraFollow2D.cs`: seguimiento suave de cámara hacia el jugador.
- `Assets/Scripts/Gameplay/DamageOnContact2D.cs`: daño al tocar enemigos, pinchos u otros peligros.
- `Assets/Scripts/Gameplay/RespawnPoint2D.cs`: punto de control que cambia el lugar de respawn del jugador.

## Configuración recomendada en Unity

1. Crea un `GameObject` para el jugador y asígnale el tag `Player`.
2. Añade al jugador estos componentes:
   - `Rigidbody2D` con `Gravity Scale` en `0` si el juego es top-down.
   - `Collider2D` acorde al sprite.
   - `PlayerMovement2D`.
   - `PlayerHealth`.
   - `Animator`, si ya tienes un controlador de animaciones.
3. Si usas `Animator`, crea parámetros con estos nombres o cambia los nombres en el inspector:
   - `Speed` (`float`).
   - `Horizontal` (`float`).
   - `Vertical` (`float`).
   - `IsMoving` (`bool`).
4. Añade `CameraFollow2D` a la cámara principal y arrastra el transform del jugador al campo `Target`, o deja que lo busque por tag al iniciar.
5. Para enemigos o pinchos, añade un `Collider2D` y `DamageOnContact2D`. Activa `Is Trigger` si quieres que el daño ocurra por superposición.
6. Para puntos de control, crea un objeto con `Collider2D` marcado como `Is Trigger` y añade `RespawnPoint2D`.


## Siguiente fase

La fase 2 añade escenas/niveles, puertas de cambio de escena, `LevelManager` y `SpawnPoint2D`. Consulta `Docs/Fase2_Niveles.md`.
