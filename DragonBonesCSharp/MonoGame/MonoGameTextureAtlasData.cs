using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DragonBones;

using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace DragonBones
{
    public class MonoGameTextureAtlasData : TextureAtlasData
    {
        internal bool _disposeEnabled;

        private Texture2D _renderTexture;
        public Texture2D RenderTexture
        {
            get { return _renderTexture; }
            set
            {
                if (value == null || this._renderTexture == value)
                {
                    return;
                }

                this._renderTexture = value;
            }
        }

        protected override void _OnClear()
        {
            base._OnClear();

            if (this._renderTexture != null)
            {
                this._renderTexture = null;
            }
        }

        public override TextureData CreateTexture()
        {
            return BaseObject.BorrowObject<MonoGameTextureData>();
        }
    }

    internal class MonoGameTextureData : TextureData
    {
        public Texture2D renderTexture { get; set; }
    }
}
