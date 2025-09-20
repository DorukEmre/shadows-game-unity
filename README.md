# shadows-game-unity

A puzzle game built with Unity, inspired by *Shadowmatic*.  
Players manipulate 3D objects so that their shadows form recognizable shapes to solve puzzles.


## Features

- **Multiple Levels:** Each level presents unique challenges involving object rotation and shadow alignment.
- **Object Manipulation:** Rotate objects on different axes and move them along the Y-axis as allowed by the level.
- **Victory & Pause Menus:** In-game UI for pausing and level completion.
- **Level Selection:** Interactive level picker with hints and completion effects.
- **Save System** Player progression is saved. 


## Project Structure

- `Assets/`
  - `Scripts/` – Game logic, level management, object controllers.
  - `Scenes/` – Unity scenes for each level and menus.
  - `Prefabs/`, `Models/`, `Materials/` – Game assets.


## Architecture

#### GameManager

Central controller for the overall game state. Manages level progression, and persistent data.

#### LevelManagers

Each level has its own LevelManager script (based on an abstract base). It initialises the level, monitors win conditions, and communicates with UI elements as needed (pause and victory menus)

#### Object Controllers

All interactable objects inherit from `AbstractObjectController`, which defines shared behavior: mouse input for dragging, rotating, and movement. 

Specialised controllers restrict object manipulation as required by different levels.


## Controls

- **Rotate Objects (Horizontal):** Click and drag with the mouse.  
- **Rotate Objects (Vertical):** Hold **Left Ctrl** + click and drag.  
- **Move Objects (Y-axis):** Hold **Left Shift** + click and drag.  
- **Pause:** Press `Escape` to open the pause menu.  
