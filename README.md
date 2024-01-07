# CommonAPI
CommonAPI is a general purpose plugin library for Bomb Rush Cyberfunk, which allows modders to do a variety of things more easily.

CommonAPI was used in the Millenium Winterland SlopCrew event to drive the NPCs, cutscenes and progress saving.
![Santa NPC](https://github.com/LazyDuchess/BRC-CommonAPI/assets/42678262/cc7c8663-89bb-4c5d-aee7-e545fc3d3562)

## Features
* Easy access to BRC shaders - Simply call `AssetAPI.GetShader(ShaderNames shaderName)` to retrieve a character or environment shader.
* Dialogue and Interaction systems - Create any interactable entity, such as an NPC, that can react to a button prompt from the player. Create custom sequences and dialogues completely in C# without touching the Unity Editor.
* Custom save data - Easily create custom save data attached to individual save slots, CommonAPI takes care of automatically loading and saving, with multithreaded file writing.
* Phone apps - Easily create a custom phone app with an user interface with buttons you can scroll through.

## Examples

Check out the [CommonAPI Sample Project](https://github.com/LazyDuchess/BRC-CommonAPI-Sample), it showcases how to create a custom app and custom save data.
![Custom Phone App](https://github.com/LazyDuchess/BRC-CommonAPI/assets/42678262/4199b387-ba13-49e3-b2a5-184cfbbab51a)


## Building
Make sure you have a `BRCPath` environment variable defined on your system, which points to the root folder of your Bomb Rush Cyberfunk installation. This way the project can find the required DLLs.
