# Fase 2: Sistema de niveles

Esta fase prepara el proyecto para trabajar con varias escenas/niveles y conservar el último punto de aparición del jugador.

## Estructura añadida

- `Assets/Scenes/`: carpeta preparada para guardar las escenas de cada nivel, por ejemplo `Level_01`, `Level_02` y `Level_03`.
- `Assets/Scripts/Levels/LevelManager.cs`: coordina el cambio de escenas, coloca al jugador en el `SpawnPoint2D` correcto y guarda el último punto de aparición.
- `Assets/Scripts/Levels/SpawnPoint2D.cs`: punto de aparición/checkpoint identificable por un `SpawnPointId`.
- `Assets/Scripts/Levels/LevelExit2D.cs`: puerta o meta que carga otra escena cuando el jugador entra en su trigger.

## Configuración recomendada en Unity

1. Crea una escena por nivel dentro de `Assets/Scenes/`.
2. Añade todas las escenas a `File > Build Settings > Scenes In Build` para que `SceneManager.LoadScene` pueda cargarlas.
3. En cada escena, crea un objeto vacío llamado `LevelManager` y añade el script `LevelManager`.
   - Por defecto se conserva entre escenas con `DontDestroyOnLoad`.
   - Si prefieres tener un manager independiente en cada escena, desactiva `Keep Between Scenes`.
4. En cada escena, coloca uno o varios objetos vacíos con `SpawnPoint2D`.
   - Usa IDs claros, por ejemplo `Start`, `FromLevel01`, `FromLevel02` o `CheckpointA`.
   - Marca `Is Default Spawn` en el punto principal de la escena.
   - Si activas `Save When Player Touches`, el punto también actúa como checkpoint.
5. En una puerta/meta, añade un `Collider2D` con `Is Trigger` y el script `LevelExit2D`.
   - `Target Scene Name`: nombre exacto de la escena destino, por ejemplo `Level_02`.
   - `Target Spawn Point Id`: ID del `SpawnPoint2D` donde aparecerá el jugador en la escena destino.
6. Mantén el tag `Player` en el jugador para que el manager, las puertas y los spawn points puedan detectarlo.

## Flujo esperado

1. El jugador toca una puerta/meta con `LevelExit2D`.
2. `LevelManager` guarda el spawn destino y carga la escena indicada.
3. Al cargar la escena, `LevelManager` busca el `SpawnPoint2D` con el ID guardado.
4. El jugador se coloca en ese punto, su velocidad se resetea y su posición de respawn se actualiza.
5. El último punto de aparición se guarda en `PlayerPrefs`, así el nivel puede reutilizarlo si se recarga sin una puerta previa.


## Siguiente fase

La fase 3 añade inventario, objetos recogibles, `ItemData`, `InventoryManager` y UI básica. Consulta `Docs/Fase3_Inventario.md`.
