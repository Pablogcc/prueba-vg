# Fase 5: pulido

Esta fase añade sistemas de presentación y feedback para que el prototipo se sienta más completo: menú principal, pausa, pantalla de muerte, audio básico, efectos al recoger objetos y feedback visual al recibir daño.

## Scripts creados o actualizados

- `Assets/Scripts/UI/MainMenuController.cs`: controla botones de menú principal para iniciar partida, cargar una escena concreta y salir del juego.
- `Assets/Scripts/UI/PauseMenuController.cs`: permite pausar/reanudar con `Escape`, mostrar un panel de pausa, reiniciar nivel y volver al menú principal.
- `Assets/Scripts/UI/DeathScreenUI.cs`: escucha la muerte/respawn del jugador para mostrar u ocultar una pantalla de muerte, reiniciar nivel o volver al menú.
- `Assets/Scripts/Audio/GameAudioManager.cs`: singleton básico para música y efectos de sonido persistente entre escenas.
- `Assets/Scripts/Audio/SceneMusicPlayer.cs`: reproduce música específica al cargar una escena.
- `Assets/Scripts/Audio/UIButtonSound.cs`: método simple para conectar sonidos a botones UI desde el evento `OnClick`.
- `Assets/Scripts/Feedback/PlayerDamageFeedback.cs`: parpadeo visual, sonido y efecto opcional cuando el jugador recibe daño.
- `Assets/Scripts/Inventory/PickupItem.cs`: ahora permite asignar sonido y prefab de efecto al recoger objetos.
- `Assets/Scripts/Player/PlayerHealth.cs`: ahora expone el evento `Damaged` para que otros sistemas reaccionen al daño recibido.

## Menú principal

1. Crea una escena llamada, por ejemplo, `MainMenu` y añádela a `File > Build Settings`.
2. Crea un Canvas con botones `Jugar` y `Salir`.
3. Añade `MainMenuController` a un GameObject del menú.
4. En `firstGameSceneName`, escribe el nombre de la primera escena jugable, por ejemplo `Level1`.
5. Conecta el botón `Jugar` a `MainMenuController.StartGame`.
6. Conecta el botón `Salir` a `MainMenuController.QuitGame`.

## Pausa

1. En cada escena jugable, crea un Canvas o panel llamado `PausePanel` y déjalo desactivado.
2. Añade `PauseMenuController` a un GameObject de la escena.
3. Asigna `PausePanel` al campo `pausePanel`.
4. Conecta botones opcionales:
   - `Continuar` -> `PauseMenuController.ResumeGame`.
   - `Reiniciar` -> `PauseMenuController.RestartLevel`.
   - `Menú principal` -> `PauseMenuController.BackToMainMenu`.
5. Por defecto se pausa con `Escape` usando `Time.timeScale = 0`.

## Pantalla de muerte

1. Crea un panel UI llamado `DeathPanel` y déjalo desactivado.
2. Añade `DeathScreenUI` a un GameObject del Canvas.
3. Asigna el jugador en `playerHealth`, o deja que lo busque por tag `Player`.
4. Asigna `DeathPanel` en el campo `deathPanel`.
5. Conecta botones como `Reintentar` a `RestartLevel` y `Menú principal` a `BackToMainMenu`.
6. Si activas `pauseGameOnDeath`, el juego quedará pausado hasta que el jugador pulse un botón de la pantalla de muerte.

## Sonidos básicos

1. Crea un GameObject `GameAudioManager` en la primera escena o menú.
2. Añade `GameAudioManager`.
3. Asigna un `startupMusic` si quieres música de fondo inicial.
4. Para música por escena, añade `SceneMusicPlayer` a un GameObject de la escena y asigna `sceneMusic`.
5. Para sonidos de botones, añade `UIButtonSound` a un botón y conecta `UIButtonSound.PlayClickSound` al evento `OnClick`.

## Efectos al recoger objetos

En cada prefab con `PickupItem` puedes configurar:

- `pickupSound`: sonido al recoger.
- `pickupSoundVolume`: volumen del sonido.
- `pickupEffectPrefab`: prefab de partículas, animación o efecto visual que se instancia en la posición del objeto.

## Feedback visual al recibir daño

1. Añade `PlayerDamageFeedback` al jugador.
2. Si el jugador tiene varios sprites, asígnalos en `spriteRenderers`; si no, el script intenta encontrarlos automáticamente en hijos.
3. Ajusta `damageColor`, `flashDuration` y `flashCount`.
4. Opcionalmente asigna `damageSound` y `damageEffectPrefab`.

## Siguiente paso sugerido

La siguiente fase puede añadir guardado/carga de partida, ajustes de volumen persistentes o una UI de vida más completa.
