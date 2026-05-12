# Fase 4: enemigos básicos

Esta fase añade enemigos simples para el prototipo 2D: patrulla, persecución, vida, daño recibido por ataques del jugador y drops al morir.

## Scripts creados

- `Assets/Scripts/Enemies/EnemyHealth.cs`: vida del enemigo, curación opcional, evento de muerte y destrucción del objeto al morir.
- `Assets/Scripts/Enemies/EnemyPatrol2D.cs`: movimiento con `Rigidbody2D` entre dos puntos configurables.
- `Assets/Scripts/Enemies/EnemyChase2D.cs`: persecución del jugador si entra en un radio de detección.
- `Assets/Scripts/Enemies/EnemyDropOnDeath.cs`: instancia prefabs de monedas u objetos cuando el enemigo muere.
- `Assets/Scripts/Player/PlayerAttack2D.cs`: ataque cuerpo a cuerpo del jugador usando `Fire1`, rango circular y daño a `EnemyHealth`.

## Configurar un enemigo patrullero

1. Crea un GameObject `EnemyPatrol` con `Rigidbody2D` y `Collider2D`.
2. Añade `EnemyHealth`.
3. Añade `EnemyPatrol2D`.
4. Crea dos hijos o puntos vacíos en la escena, por ejemplo `PointA` y `PointB`.
5. Asigna esos puntos en `pointA` y `pointB`.
6. Ajusta `moveSpeed` y `arriveDistance`.
7. Si quieres que dañe al jugador al tocarlo, añade también `DamageOnContact2D` y deja `targetTag` como `Player`.

## Configurar un enemigo perseguidor

1. Crea un GameObject `EnemyChaser` con `Rigidbody2D` y `Collider2D`.
2. Añade `EnemyHealth`.
3. Añade `EnemyChase2D`.
4. Comprueba que el jugador tenga el tag `Player`.
5. Ajusta `detectionRadius`, `stopDistance` y `moveSpeed`.
6. Si quieres que el enemigo haga daño al tocar al jugador, añade `DamageOnContact2D`.

> Consejo: usa `EnemyPatrol2D` para enemigos que solo patrullan y `EnemyChase2D` para enemigos que solo persiguen. Si los pones juntos en el mismo objeto, ambos intentarán controlar el mismo `Rigidbody2D`.

## Configurar ataque del jugador

1. Añade `PlayerAttack2D` al GameObject del jugador.
2. Usa el botón `Fire1` del Input Manager clásico de Unity, o cambia `attackButton` al nombre que uses.
3. Asigna `enemyLayers` a la capa donde estén los enemigos. Si lo dejas en `Everything`, el script buscará `EnemyHealth` en lo que entre en rango.
4. Opcionalmente crea un hijo `AttackPoint` delante del jugador y asígnalo en `attackPoint` para controlar desde dónde sale el golpe.
5. Si el jugador ya tiene `Animator`, crea un trigger llamado `Attack` o cambia `attackTriggerParameter`.

## Drops de monedas u objetos

1. Crea un prefab de moneda u objeto recogible.
2. Si es un objeto de inventario, añade `PickupItem` al prefab y asígnale un `ItemData`.
3. Añade `EnemyDropOnDeath` al enemigo.
4. En la lista `drops`, añade el prefab, probabilidad (`dropChance`), cantidades mínima/máxima y offset aleatorio.
5. Al morir el enemigo, los prefabs se instancian alrededor de su posición.

## Siguiente paso sugerido

La siguiente fase puede añadir una UI de vida para enemigos, animaciones de ataque/daño, knockback o una moneda separada del inventario.

## Siguiente fase

Continúa con [Fase 5: pulido](Fase5_Pulido.md).
