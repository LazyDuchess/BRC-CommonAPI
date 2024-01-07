# CommonAPI
CommonAPI is a plugin library for Bomb Rush Cyberfunk, which allows modders to do a variety of things more easily.

CommonAPI was used in the Millenium Winterland SlopCrew event to drive the NPCs, cutscenes and progress saving.
## Features
* Easy access to BRC shaders - Simply call `AssetAPI.GetShader(ShaderNames shaderName)` to retrieve a character or environment shader.
* Dialogue and Interaction systems - Create any interactable entity, such as an NPC, that can react to a button prompt from the player. Create custom sequences and dialogues completely in C# without touching the Unity Editor.
* Custom save data - Easily create custom save data attached to individual save slots, CommonAPI takes care of automatically loading and saving, with multithreaded file writing.
* Phone apps - Easily create a custom phone app with an user interface with buttons you can scroll through.
