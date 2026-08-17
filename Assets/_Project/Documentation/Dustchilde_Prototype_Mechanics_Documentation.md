# Dustchilde Prototype — Mechanics Documentation

## Table of Contents

1. Input System (PlayerControls)
2. IInteractable
3. PlayerMovement
4. PlayerInteractor
5. PickupItem
6. DialogueManager
7. NPCDialogue
8. PuzzleManager
9. PuzzleItem
10. Scene Setup Checklist 

## 1. Input System (PlayerControls)
**File:** `Assets/_Project/Input/PlayerControls.inputactions`
### What the Script Does:
Defines all input the player can perform. There are three actions inside one Action Map called `Player`:
- **Move** — Vector2, bound to WASD
- **Look** — Vector2, bound to Mouse Delta
- **Interact** — Button, bound to Left Mouse Button
### How to edit it
- Open the Input Actions Editor.
- To change a key binding (e.g. rebind Interact to `E` instead of Left Click): click the binding under the action → change the Path field.
- To add a new action (e.g. a "Sprint" button later): click **+** next to Actions, set its Action Type, add a binding, then **Save Asset**. You'd then need to reference the new action in a script the same way Move/Look/Interact are referenced.
- **Always click Save Asset after changes**, and if "Generate C# Class" is checked, click **Apply** in the Inspector so the generated class updates.


## 2. IInteractable (Interface)
**File:** `Assets/_Project/Scripts/IInteractable.cs`
### What the Script Does:
This is a contract, not a component you attach directly. Any script that implements `IInteractable` must provide:
- `Interact()` — what happens when the player interacts with it
- `GetInteractPrompt()` — the text shown on screen (e.g. "Press to pick up Red Dust Packet")
This is what allows `PlayerInteractor` to work with ANY object — pickups, NPCs, puzzle items — without needing to know what kind of object it is. It just calls `.Interact()` on whatever it hits.
### How to edit it
You generally won't need to change this file. If you want every interactable object to support a new shared feature (e.g. a sound effect on interact), you could add a new method here — but that would require updating every script that implements it (`PickupItem`, `NPCDialogue`, `PuzzleItem`).


## 3. PlayerMovement
**File:** `Assets/_Project/Scripts/PlayerMovement.cs`
**Attached to:** `Player` GameObject
### What the Script Does:
- Reads `Move` and `Look` input every frame.
- Moves the `CharacterController` relative to the direction the player is facing (WASD).
- Rotates the player body left/right (yaw) and the camera up/down (pitch) based on mouse movement, clamped so you can't flip the camera upside down.
- Applies simple gravity so the player stays grounded on the floor.
- Automatically freezes all movement/look while `DialogueManager.Instance.IsDialogueActive` is true (so you can't walk away mid-conversation).
### Required setup in Inspector
On the `Player` GameObject, the `Player Movement` component needs:
| Field | What to assign |
|---|---|
| Camera Transform | Drag in the `Main Camera` child object |
| Move Speed | Default `4` — how fast the player walks |
| Mouse Sensitivity | Default `0.1` — how fast the camera turns |
| Max Look Angle | Default `85` — how far up/down you can look before it clamps |
### How to edit / extend
- **To change walk speed:** adjust `Move Speed` in the Inspector — no code change needed.
- **To change mouse sensitivity:** adjust `Mouse Sensitivity` in the Inspector.
- **To add sprinting later:** you'd add a new bool input action (e.g. "Sprint"), read it in `HandleMove()`, and multiply `moveSpeed` conditionally.
- **To add footstep sounds, head-bob, etc.:** add that logic inside `HandleMove()`.


## 4. PlayerInteractor
**File:** `Assets/_Project/Scripts/PlayerInteractor.cs`
**Attached to:** `Player` GameObject
### What the Script Does:
- Every frame casts a ray straight forward from the camera.
- If that ray hits something within `Interact Range` that has a script implementing `IInteractable`, it shows the on-screen prompt text (via `DialogueManager`... no — via its own UI reference) and remembers that object as `currentInteractable`.
- If the ray hits nothing interactable, it hides the prompt.
- When the player presses the `Interact` input (left-click), it calls `.Interact()` on whatever `currentInteractable` currently is.
### Required setup in Inspector
On the `Player` GameObject, the `Player Interactor` component needs:
| Field | What to assign |
|---|---|
| Camera Transform | Drag in the `Main Camera` child object |
| Interact Range | Default `3` — how far the raycast reaches |
| Interactable Layer | Default: Everything |
| Interact Prompt Object | Drag in the `InteractPrompt` UI GameObject (the whole object, not just its text) |
| Interact Prompt Text | Drag in the same `InteractPrompt` object (Unity finds its TextMeshPro component automatically) |
### How to edit / extend
- **To increase/decrease interact range:** change `Interact Range` in the Inspector.
- **To restrict interaction to specific object types:** set `Interactable Layer` to a specific Layer instead of Everything, and make sure your interactable objects are on that layer.
- **Common mistake:** if the prompt text doesn't show up when looking at objects, the almost-always cause is that **Interact Prompt Object** or **Interact Prompt Text** wasn't dragged into the Inspector fields.


## 5. PickupItem
**File:** `Assets/_Project/Scripts/PickupItem.cs`
**Attached to:** any object meant to be picked up (currently: `TestItem_RedDustPacket`)
### What the Script Does:
Implements `IInteractable`. When interacted with, it:
1. Logs `"Picked up: [item name]"` to the Console.
2. Disables the GameObject (`SetActive(false)`) — simulating it being picked up and removed from the world.
This is intentionally simple for prototype purposes — no real inventory system yet.
### Required setup in Inspector
| Field | What to assign |
|---|---|
| Item Name | Text shown in the "Picked up: ..." log and the on-screen prompt |
The object also needs a **Collider** (any type, non-trigger) so the raycast can detect it.
### How to edit / extend
- **To make a new pickup item:** create any GameObject with a Collider, add the `Pickup Item` component, set its Item Name. That's it — no code changes needed for basic pickups.
- **To actually store picked-up items (inventory):** replace the `Debug.Log` + `SetActive(false)` inside `Interact()` with a call to an inventory manager script (not yet built — would be a future addition).
- **To make items give the player something else on pickup** (unlock a door, trigger a puzzle flag, etc.): add that logic inside `Interact()`, similar to how `PuzzleItem` calls `PuzzleManager.Instance.SubmitItem(...)`.


## 6. DialogueManager
**File:** `Assets/_Project/Scripts/DialogueManager.cs`
**Attached to:** `DialogueManager` empty GameObject (singleton — only one should exist per scene)
### What the Script Does:
Central controller for all dialogue in the scene. Any NPC can call into it rather than each NPC managing its own UI.
- `StartDialogue(speakerName, lines[])` — opens the dialogue panel, sets the speaker name, loads all lines into a queue, and displays the first line.
- `DisplayNextLine()` — shows the next queued line; if the queue is empty, closes the dialogue panel automatically.
- `IsDialogueActive` — a public flag other scripts (like `PlayerMovement`) check to know whether to freeze player movement.
### Required setup in Inspector
On the `DialogueManager` GameObject:
| Field | What to assign |
|---|---|
| Dialogue Panel | Drag in the `DialoguePanel` UI GameObject |
| Speaker Name Text | Drag in `SpeakerNameText` |
| Dialogue Text | Drag in `DialogueText` |
### How to edit / extend
- **You will rarely need to edit this script directly.** All actual dialogue content lives on individual `NPCDialogue` components (see below), not here.
- **To change dialogue panel appearance** (font, size, background color, position): edit the UI elements directly in the Canvas (DialoguePanel, SpeakerNameText, DialogueText) — no code changes needed.
- **Important:** only ever have ONE `DialogueManager` in a scene. It's a singleton — if a second one is accidentally added, it will self-destroy on `Awake()`.


## 7. NPCDialogue
**File:** `Assets/_Project/Scripts/NPCDialogue.cs`
**Attached to:** any NPC that should talk (currently: `NPC_Jacob`)
### What the Script Does:
Implements `IInteractable`. On interact:
- If no dialogue is currently active, it starts a new conversation using this NPC's `Speaker Name` and `Dialogue Lines`.
- If dialogue IS already active (i.e. the player is mid-conversation with this NPC), pressing interact again just advances to the next line.
### Required setup in Inspector
| Field | What to assign |
|---|---|
| Speaker Name | The name shown above the dialogue text, e.g. `Jacob` |
| Dialogue Lines | An array of strings — each line is one "page" of dialogue the player clicks through in order |
The object also needs a **Collider** so the raycast can detect it.
### How to edit / extend — THIS IS WHERE YOU WILL WRITE ACTUAL DIALOGUE
**To add a new NPC with dialogue:**
1. Create/place a GameObject in the scene (a model, capsule placeholder, whatever).
2. Make sure it has a Collider.
3. Add the `NPC Dialogue` component.
4. Set **Speaker Name**.
5. Set **Dialogue Lines** array size to however many lines you want, and type each line in its own slot.
**To change existing dialogue** (e.g. Jacob's lines from your design doc): just select `NPC_Jacob`, and edit the strings inside the **Dialogue Lines** array in the Inspector — no code involved.
**Current limitation to be aware of:** dialogue always plays back the SAME lines every time you talk to that NPC — there's no branching, no conditions, no "already talked to this person" tracking yet. That would be a future addition (e.g. tracking a bool per NPC, or supporting multiple dialogue sets that unlock based on story progress) — flag this to your team if the story needs it for the full narrative build, since Task 2 describes Jacob's dialogue unlocking progressively as you bring him alcohol.


## 8. PuzzleManager
**File:** `Assets/_Project/Scripts/PuzzleManager.cs`
**Attached to:** `PuzzleManager` empty GameObject (singleton — only one per scene)
### What the Script Does:
Tracks a sequence-order puzzle (matches Scene 6 in the design doc — sorting memory items into chronological order).
- Holds the **Correct Order** — a list of item IDs in the order they must be selected.
- Every time a `PuzzleItem` is interacted with, it calls `SubmitItem()`.
- If the item submitted matches the next expected item in the sequence, it's accepted and the on-screen status updates.
- If it's wrong, the whole attempt resets and the player must start selecting from the beginning.
- Once all items are submitted in the correct order, it displays "Puzzle Solved!" and logs it to the Console.
### Required setup in Inspector
On the `PuzzleManager` GameObject:
| Field | What to assign |
|---|---|
| Puzzle Panel | Drag in `PuzzlePanel` UI GameObject |
| Status Text | Drag in `PuzzleStatusText` |
| Correct Order | An array of item ID strings, in the exact order they must be picked (must match the `Item Id` values set on your `PuzzleItem` objects) |
### How to edit / extend
- **To change the correct solution order:** just reorder the strings inside **Correct Order** in the Inspector.
- **To add more steps to the puzzle:** increase the array size and add more item IDs — but remember every `PuzzleItem` object referenced needs a matching `Item Id` (see below).
- **To make something happen when the puzzle is solved** (e.g. trigger a cutscene, unlock a door, play music): add that logic where the script currently prints `"Puzzle solved correctly!"` inside `SubmitItem()`.
- **To support MULTIPLE separate puzzles in one scene** (e.g. one puzzle per level later): this current version is built for one puzzle only. Multiple puzzles would need either multiple PuzzleManager instances with unique names/IDs, or an upgraded version that tracks puzzles by a puzzle ID — worth flagging as a future task once you're past prototype stage.


## 9. PuzzleItem
**File:** `Assets/_Project/Scripts/PuzzleItem.cs`
**Attached to:** any object that's part of a sequence puzzle (currently: `PuzzleItem_Intro`, `PuzzleItem_Dust`, `PuzzleItem_Confession`)
### What the Script Does:
Implements `IInteractable`. On interact, it reports its own `Item Id` and `Display Label` to the `PuzzleManager`, which checks if it was picked in the right order.
### Required setup in Inspector
| Field | What to assign |
|---|---|
| Item Id | A short unique string, e.g. `intro`, `dust`, `confession` — **must exactly match** one of the entries in `PuzzleManager`'s Correct Order array |
| Display Label | Human-readable name shown in prompts/logs, e.g. `Red Dust Packet` |
The object also needs a **Collider**.
### How to edit / extend
**To set up a new sequence puzzle for a real level:**
1. Decide the correct chronological order of memory items (e.g. based on the story beats in Task 2).
2. Create one GameObject per memory item, each with a `Puzzle Item` component, giving each a unique **Item Id**.
3. On `PuzzleManager`, set the **Correct Order** array to list those same Item Ids, in the correct order.
4. Make sure Item Id spelling matches EXACTLY (case-sensitive) between `PuzzleItem` objects and the `PuzzleManager`'s array 


## 10. Scene Setup Checklist
For building a new test scene or level from scratch, this is the minimum required setup:
**Player:**
- [ ] `Player` GameObject with CharacterController, `Player Movement`, `Player Interactor`
- [ ] `Main Camera` as a child of Player, referenced in both movement/interactor scripts
**Managers (one each per scene):**
- [ ] `DialogueManager` GameObject with `DialogueManager` script, UI references wired
- [ ] `PuzzleManager` GameObject with `PuzzleManager` script, UI references wired, Correct Order filled in
**UI Canvas:**
- [ ] Crosshair (Image, centered)
- [ ] InteractPrompt (TMP text, disabled by default)
- [ ] DialoguePanel (Panel + SpeakerNameText + DialogueText, disabled by default)
- [ ] PuzzlePanel (Panel + PuzzleStatusText)
**Content objects:**
- [ ] Pickup items: any object + Collider + `Pickup Item` component
- [ ] NPCs: any object + Collider + `NPC Dialogue` component
- [ ] Puzzle items: any object + Collider + `Puzzle Item` component, IDs matching PuzzleManager's Correct Order
**Environment:**
- [ ] Floor + walls to contain the play area
