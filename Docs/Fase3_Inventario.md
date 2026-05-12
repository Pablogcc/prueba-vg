# Fase 3: Sistema de inventario

Esta fase añade un inventario sencillo para recoger objetos, mostrarlos en UI y distinguir entre consumibles y objetos clave.

## Estructura añadida

- `Assets/Scripts/Inventory/ItemType.cs`: enum con los tipos de objeto soportados: `Consumable` y `Key`.
- `Assets/Scripts/Inventory/ItemData.cs`: `ScriptableObject` con los datos de cada objeto: ID, nombre, descripción, icono, tipo, stack máximo y efecto de curación si es consumible.
- `Assets/Scripts/Inventory/InventorySlot.cs`: modelo serializable para guardar un objeto y su cantidad.
- `Assets/Scripts/Inventory/InventoryManager.cs`: singleton de inventario que añade, elimina, consume y consulta objetos.
- `Assets/Scripts/Inventory/PickupItem.cs`: componente para objetos recogibles en la escena.
- `Assets/Scripts/UI/InventoryUI.cs`: UI básica que se actualiza cuando cambia el inventario.
- `Assets/Scripts/UI/InventoryItemRowUI.cs`: fila opcional para mostrar icono, nombre, cantidad y tipo de cada objeto.

## Crear objetos con ItemData

1. En Unity, haz clic derecho en el Project Browser.
2. Selecciona `Create > Adventure Survival > Inventory > Item Data`.
3. Configura:
   - `Item Id`: ID estable, por ejemplo `small_potion` o `dungeon_key`.
   - `Display Name`: nombre visible en UI.
   - `Icon`: sprite opcional.
   - `Item Type`: `Consumable` o `Key`.
   - `Max Stack`: cuántas unidades pueden apilarse por slot.
   - `Heal Amount`: vida que recupera si es consumible.
   - `Remove When Consumed`: si debe desaparecer al consumirse.

## Crear objetos recogibles

1. Crea un GameObject en la escena con sprite y `Collider2D`.
2. Marca el collider como `Is Trigger`.
3. Añade `PickupItem`.
4. Arrastra el `ItemData` al campo `Item`.
5. Ajusta `Quantity` para definir cuántas unidades entrega.

Cuando el jugador con tag `Player` entra en el trigger, `PickupItem` añade el objeto al `InventoryManager` y destruye o desactiva el GameObject.

## Crear el InventoryManager

1. Crea un GameObject llamado `InventoryManager`.
2. Añade el script `InventoryManager`.
3. Deja `Keep Between Scenes` activado si quieres conservar el inventario entre niveles.

Si no hay un `InventoryManager` en escena, los scripts lo crearán automáticamente, pero es recomendable añadirlo manualmente para verlo y configurarlo desde el inspector.

## UI básica

Opción rápida:

1. Crea un `Canvas`.
2. Añade un `Text` para la lista del inventario.
3. Añade `InventoryUI` a un GameObject del Canvas.
4. Arrastra ese `Text` al campo `Inventory List Text`.

Opción con filas:

1. Crea un contenedor UI, por ejemplo un `Vertical Layout Group`.
2. Crea un prefab de fila con `InventoryItemRowUI`.
3. Asigna en la fila los campos opcionales `Icon Image`, `Name Text`, `Quantity Text` y `Type Text`.
4. En `InventoryUI`, asigna `Rows Container` y `Row Prefab`.
5. Opcionalmente asigna `Empty Text` para mostrar un mensaje cuando no hay objetos.

## Consumibles y llaves

- Los objetos `Consumable` pueden consumirse desde código con `InventoryManager.Instance.TryConsumeItem(itemData, playerHealth)`.
- Si el consumible tiene `Heal Amount`, llamará a `PlayerHealth.Heal` sobre el jugador indicado.
- Los objetos `Key` no se consumen con `TryConsumeItem`; se consultan con `HasItem` para abrir puertas, cofres o bloqueos en fases posteriores.

## Siguiente fase

Continúa con [Fase 4: enemigos básicos](Fase4_Enemigos.md).
