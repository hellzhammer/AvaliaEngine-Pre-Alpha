MUST CREATE A SPRITEFONT CALLED Game_Font, VIA THE CONTENT MANAGER (INSIDE CONTENT FOLDER).
IF YOU DONT ATT THE GAME FONT THEN THE GAME WILL CRASH.

GAME1 MUST INHERIT FROM ENGINE2D

This library is a beta test of our 2D engine. This is one of the core libraries developed for building 2D games and applications. 
Contains GUI components, rendering engine and input handling.

Working: 
Basic GUI engine. Can create UI easily and display views with minimalish code. Still fleshing out.

Procedural generation. Works great, but legacy system needs overhaul.
Tilemaps. Can create basic tilemaps or create chunks. Rendering and update optimized.
Pathfinding. Works with chunks and simpler tilemaps.

Scene management. Works as is but considering making scenes their own GameComponent.
View Management. Still in development, but working well enough.

Better examples will be provided soon.

Please refer to the documentation for more details on how to use the library and its features.
http://pixel-forge-engine.hooleyautomation.xyz

GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007


NOTES:
The procedural world generation system is still a work in progress. There will be significant changes in the future. 
You do not have to use the system, it is just there as an experimental feature. The texture dictionary is vital to know though.
Not adding these textures will cause game to crash. 

	-FOR WORLDMAP.BUILDMAP() TO WORK YOU MUST ADD THESE TILES TO YOUR TEXTURE DICTIONARY, NAMED EXACTLY AS FOLLOWS:
		Grass
		Mud
		Water
		Sand
		Mountain

The textures are loaded when loading content. All game textures are stored inside of the manager in order to simplify texture retrieval.
The only system NOT using the texture manager is the GUI system. Which still relies on direct texture loading. This will be changed in the future.

For example, this is the TextureManager:


    public class TextureManager
    {
        public static Dictionary<string, Texture2D> Texture_Dictionary 
        { 
            get; 
            protected set; 
        }

        public TextureManager(Dictionary<string, Texture2D> textdict) 
        {
            Texture_Dictionary = textdict;
        }

        public bool Add_Texture(string text_name, Texture2D text)
        {
            Texture_Dictionary.Add(text_name, text);
            return Texture_Dictionary.ContainsKey(text_name);
        }

        public void Update_Texture(string text_name, Texture2D text)
        {
            Texture_Dictionary[text_name] = text;
        }

        public bool Delete_Texture(string id)
        {
            Texture_Dictionary.Remove(id);
            return Texture_Dictionary.ContainsKey(id);
        }
    }