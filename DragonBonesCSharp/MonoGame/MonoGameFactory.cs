using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DragonBones
{
    public class MonoGameFactory : BaseFactory
    {
        internal static DragonBones _dragonBonesInstance = null;
        private static MonoGameFactory factory = null;

        private MonoGameArmature _armatureProxy = null;

        private readonly List<DragonBonesData> cacheDragonBonesData = new List<DragonBonesData>();

        /// <summary>
        /// Singleton factory
        /// </summary>
        public static MonoGameFactory Factory
        {
            get
            {
                if (factory == null)
                {
                    factory = new MonoGameFactory();
                    _dragonBonesInstance = new DragonBones(new DragonBonesEventDispatcher());
                }
                return factory;
            }
        }

        public MonoGameFactory (DataParser dataParser = null) : base(dataParser)
        {
            Init();
        }

        private void Init()
        {
            if (_dragonBonesInstance == null)
            {
                _dragonBonesInstance = new DragonBones(null);
            }

            _dragonBones = _dragonBonesInstance;
        }

        protected override TextureAtlasData _BuildTextureAtlasData(TextureAtlasData textureAtlasData, object textureAtlas)
        {
            if (textureAtlasData != null)
            {
                if (textureAtlas != null)
                {
                    (textureAtlasData as MonoGameTextureAtlasData).RenderTexture = (textureAtlas as MonoGameDragonBonesData.TextureAtlas).texture;
                }
            }
            else
            {
                textureAtlasData = BaseObject.BorrowObject<MonoGameTextureAtlasData>();
            }

            return textureAtlasData;
        }

        protected override Armature _BuildArmature(BuildArmaturePackage dataPackage)
        {
            var armature = BaseObject.BorrowObject<Armature>();
            var armatureDisplay = _armatureProxy == null ? new MonoGameArmature() : _armatureProxy;
            var armatureProxy = _armatureProxy == null ? new MonoGameArmature() : _armatureProxy;

            armature.Init(dataPackage.armature, armatureProxy, armatureDisplay, this._dragonBones);

            _dragonBonesInstance.clock.Add(armature);

            return armature;
        }

        protected override Slot _BuildSlot(BuildArmaturePackage dataPackage, SlotData slotData, Armature armature)
        {
            var slot = BaseObject.BorrowObject<MonoGameSlot>();
            var armatureDisplay = armature.display as Texture2D;

            slot.Init(slotData, armature, null, null);

            return slot;
        }

        public MonoGameArmature BuildArmatureDisplay(string armatureName, string dragonBonesName, string skinName, string textureAtlasName, float textureScale)
        {
            Armature armature = null;

            if (this.GetDragonBonesData(dragonBonesName) != null)
            {
                armature = this.BuildArmature(armatureName, dragonBonesName, skinName, textureAtlasName);
            }

            return armature.display as MonoGameArmature;
        }

        public DragonBonesData LoadDragonBonesData(string dragonBonesJSONPath, string name = "", float scale = 0.01f)
        {
            if (dragonBonesJSONPath == null || !File.Exists(dragonBonesJSONPath))
            {
                return null;
            }

            var dragonBonesJSON = File.ReadAllText(dragonBonesJSONPath);

            if (!string.IsNullOrWhiteSpace(name))
            {
                var existingData = GetDragonBonesData(name);
                if (existingData != null)
                {
                    return existingData;
                }
            }

            DragonBonesData data = null;

            if (dragonBonesJSON == "DBDT")
            {
                data = ParseDragonBonesData(dragonBonesJSON, name, scale);
            }
            else
            {
                data = ParseDragonBonesData((Dictionary<string, object>)MiniJSON.Json.Deserialize(dragonBonesJSON), name, scale);
            }

            name = !string.IsNullOrWhiteSpace(name) ? name : data.name;

            _dragonBonesDataMap[name] = data;

            return data;
        }

        public TextureAtlasData LoadTextureAtlasData(string textureAtlasJSONPath, string name = "", float scale = 1.0f)
        {
            if (textureAtlasJSONPath == null || !File.Exists(textureAtlasJSONPath))
            { 
                return null;
            }

            var textureAtlasJSON = File.ReadAllText(textureAtlasJSONPath);

            var textureJSONData = (Dictionary<string, object>)MiniJSON.Json.Deserialize(textureAtlasJSON);
            TextureAtlasData data = ParseTextureAtlasData(textureJSONData, null, name, scale);

            if (data != null)
            {
                data.imagePath = Path.ChangeExtension(textureAtlasJSONPath, "png");
            }

            return data;
        }
    }
}
