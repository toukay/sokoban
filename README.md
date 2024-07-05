# Sokoban

![Level Design](Images/level_example.gif)

A simple 2D Sokoban game created in Unity.

You can download the windows build, or you can play the WebGL build directly in a browser [here](https://toukay.itch.io/sokoban).

## Overview

This project is a classic Sokoban puzzle game where the player pushes boxes to designated target locations.

## Features

- 2D top down view.
- Grid based movement.
- Undo / Redo movement history.
- 5 challenging levels.
- Pause, Resume, and Restart level functionality.
- Resolution, Quality, and Audio settings.
- Interactive sound effects and music.
- UI menues.

## Attribution

### 2D and UI Assets:
- [Kenney](https://kenney.nl/)

### Audio:
- [Kenney](https://kenney.nl/)
- [code_box](https://freesound.org/people/code_box/sounds/651533/)
- [GWriterStudio](https://assetstore.unity.com/packages/audio/music/8bit-music-062022-225623)

## Development Notes

- Used Command design pattern for movement and undo/redo features.
- Levels are designed and loaded from text files. The keys are as follows:
  - `#`: Wall
  - `.`: Empty Space
  - `@`: Player Start Position
  - `C`: Crate
  - `T`: Target Spot (where the crates need to be moved)
  
  ![Level Design](Images/level_design_example.png)
- Used Unity's New Input System for taking input.
