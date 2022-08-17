using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DragonBones
{
    /// <summary>
    /// Load a DragonBones file
    /// Based on both <a href="https://github.com/DragonBones/DragonBonesCSharp/tree/master/Unity">DragonBonesCSharp</a> and <a href="https://github.com/DragonBones/DragonBonesJS/tree/master/Phaser/">DragonBonesJS(Phaser)</a>
    /// </summary>
    public class MonoGameDragonBones
    {
        internal static MonoGameFactory factory;
        internal static DragonBones dbInstance;

        public MonoGameDragonBones()
        {
            factory = new MonoGameFactory();
            dbInstance = new DragonBones(new DragonBonesEventDispatcher());
        }

        public static MonoGameArmature CreateArmature(string dragonBonesJSONPath, string textureAtlasJSONPath, string skinName)
        {
            var dragonBonesData = factory.LoadDragonBonesData(dragonBonesJSONPath);
            var textureAtlasData = factory.LoadTextureAtlasData(textureAtlasJSONPath);
            var armature = factory.BuildArmature("Armature", dragonBonesData.name, skinName, textureAtlasData.name);
            var display = factory.BuildArmatureDisplay(armature.name, dragonBonesData.name, skinName, textureAtlasData.name, 1.0f);
            return display;
        }

        public void Update(GameTime gameTime)
        {
            dbInstance.clock.AdvanceTime((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
