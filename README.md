# blockmania-game
A 3D multiplayer puzzle platformer.

## Background
This game was created during a university game development module. The aim of this multiplayer puzzle platfomer is to collect keys to escape from Mousey. Along the way, collectibles can be found to increase your score. Try to reach the highest score!

| Main Menu  | Level Selector |
| ------------- | ------------- |
| <img src="https://user-images.githubusercontent.com/72221490/165172440-2a9dcac0-6712-41fe-8575-1d7ee31e180f.png" alt="Main Menu" width=320px height=200px>  | <img src="https://user-images.githubusercontent.com/72221490/165173017-6e7018f7-b9b6-43c4-8d6e-3a39973c4f0d.png" alt="Level Selector" width=320px height=200px>  |

| Level 1  | Level 2 |
| ------------- | ------------- |
| <img src="https://user-images.githubusercontent.com/72221490/165172930-312077ac-6244-4110-a72d-548ce44b1266.png" alt="Level 1" width=320px height=200px>  | <img src="https://user-images.githubusercontent.com/72221490/165172938-3d9bddba-edbe-4a41-abb0-ccdb76ee62ca.png" alt="Level 2" width=320px height=200px>  |

## Controls
* WASD to move
* Mouse to look around
* E to interact
* Esc to open the menu

## Features
* Multiplayer (using Photon)
* Enemy AI that chases or patrols a defined area
* Save and load level progress (this only works in singleplayer)
* Collectibles, lives, keys
* Levers and locked areas
* Per-level high score (using Backendless)
* Player models and animations

## Possible Improvements
* Additional levels
* Improved level design
* Improved HUD and main menu
* Cloud saves
* Enhanced obstacles
* Multiplayer specific levels
* Improved multiplayer lobbies
* Advanced puzzles

## Resources List

### General
* The font used throughout the UI: [ChunkFive](https://www.fontsquirrel.com/fonts/chunkfive).
* The package used to enable multiplayer: [Photon Unity Networking Classic](https://assetstore.unity.com/packages/tools/network/photon-unity-networking-classic-free-1786).

### World
* The building block models for the level and the collectibles: [BlockWorld](https://assetstore.unity.com/packages/3d/environments/block-world-68107).
  * Collectibles were modified to be collectable.
  * The scripts in ```ToyBox/Scripts/``` were modified to to work in multiplayer.
  * The lever was modified to be interactable.
  * The animation of the lever was adjusted to make the sphere turn green, when activated.
* The model for the keys: [Handpainted Keys](https://assetstore.unity.com/packages/3d/handpainted-keys-42044).
  * Keys were modified to be collectable.
* The sound playing in the background of every level: [Fast Feel Banana Peel](https://www.chosic.com/download-audio/28655/).
* All character models were taken from [mixamo](https://www.mixamo.com/).
  * Timmy, Ty, Amy, Claire, Mousey
  * Animations
    * Idle – weight shift idle
    * Jumping – jumping from action idle
    * Walking – walking with a swagger
    * Attacking (for Mousey) - mutant swiping 
* The player's character: [First Person Character Controller](https://assetstore.unity.com/packages/essentials/starter-assets-first-person-character-controller-196525#content).
  * The script ```Assets/FirstPersonCharacter/Characters/FirstPersonCharacter/FirstPersonController.cs``` was modified to work with the above animations.
  * The same script was also modified to make the character work with multiplayer.

### HUD
* The heart icon to displaying the player's lives: [Heart Game Icon](https://www.vhv.rs/viewpic/iwToRxR_heart-game-icon-png-transparent-png/).
  * (The heart icon was modified by greying it out to represent a lost heart).
* The key icon to display the player's keys: [Key](https://www.pixilart.com/art/key-c0274e0785c3672).
  * (The jey icon was modified by greying it out to represent a key to be obtained).

### Menus
* A control for the game in the help menu: [Mouse Icon](https://www.flaticon.com/free-icon/mouse_88221).
* The sound played when pressing 'play': [Drawer Opening](https://soundbible.com/613-Drawer-Opening.html).
