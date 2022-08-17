using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace DragonBones
{
    /*
    public class MonoGameSlot2 : Slot
    {
        private MonoGameArmature _proxy;
        private BlendMode _currentBlendMode;

        private Transform _textureTransform;
        private DisplayData _renderDisplay;

        internal bool _isActive = false;
        private int _vertexOffset = -1;

        public MonoGameTextureAtlasData currentTextureAtlasData
        {
            get
            {
                if (this._textureData == null || this._textureData.parent == null)
                {
                    return null;
                }

                return this._textureData.parent as MonoGameTextureAtlasData;
            }
        }

        protected override void _AddDisplay()
        {
            _proxy = _armature.proxy as MonoGameArmature;
            var container = _proxy;

            // TODO
        }

        protected override void _DisposeDisplay(object value, bool isRelease)
        {
            // TODO
        }

        protected override void _IdentityTransform()
        {
            // TODO
        }

        protected override void _InitDisplay(object value, bool isRetain)
        { }

        protected override void _OnUpdateDisplay()
        {
            _renderDisplay = (_display != null ? _display : _rawDisplay) as DisplayData;

            _proxy = _armature.proxy as MonoGameArmature;

            if (this._meshDisplay == null)
            {
                this._meshDisplay = new MonoGameSlot();
            }
        }

        protected override void _RemoveDisplay()
        {
            // TODO
        }

        protected override void _ReplaceDisplay(object value)
        {
            var container = _proxy;
            var prevDisplay = value as MonoGameArmature;
            //int index = prevDisplay.index;
            // Hide prevDisplay: prevDisplay.SetActive(false);

            // Show _renderDisplay: _renderDisplay.SetActive(true);

            // Replace it with previous order
            //_SetZorder(prevDisplay.zIndex);
        }

        protected override void _UpdateColor()
        {
            (this._childArmature.proxy as MonoGameArmature).color = _colorTransform;
        }

        protected override void _UpdateFrame()
        {
            // if (this._renderDisplay is DisplayContainer) return;
            var currentVerticesData = (this._deformVertices != null && this._display == this._meshDisplay) ? this._deformVertices.verticesData : null;
            var currentTextureData = this._textureData as MonoGameTextureData;

            this._isActive = false;
            if (this._displayIndex >= 0 && this._display != null && currentTextureData != null)
            {
                var currentTextureAtlas = currentTextureAtlasData.texture;
                if (currentTextureAtlas != null)
                {
                    this._isActive = true;

                    var textureAtlasWidth = currentTextureAtlasData.width > 0f ? (int)currentTextureAtlasData.width : currentTextureAtlas.Width;
                    var textureAtlasHeight = currentTextureAtlasData.height > 0f ? (int)currentTextureAtlasData.height : currentTextureAtlas.Height;

                    var textureScale = _armature.armatureData.scale * currentTextureData.parent.scale;
                    var sourceX = currentTextureData.region.x;
                    var sourceY = currentTextureData.region.y;
                    var sourceWidth = currentTextureData.region.width;
                    var sourceHeight = currentTextureData.region.height;

                    if (currentVerticesData != null)
                    {
                        var data = currentVerticesData.data;
                        var meshOffset = currentVerticesData.offset;
                        var intArray = data.intArray;
                        var floatArray = data.floatArray;
                        var vertexCount = intArray[meshOffset + (int)BinaryOffset.MeshVertexCount];
                        var triangleCount = intArray[meshOffset + (int)BinaryOffset.MeshTriangleCount];
                        int vertexOffset = intArray[meshOffset + (int)BinaryOffset.MeshFloatOffset];
                        if (vertexOffset > 0)
                        {
                            vertexOffset += 65536;
                        }

                        var uvOffset = vertexOffset + vertexCount * 2;
                        if (this._meshDisplay.uvBuffers == null || this._meshDisplay.Length != vertexCount)
                        {
                            this._meshDisplay.uvBuffers = new Vector2[vertexCount];
                        }

                        if (this._meshDisplay.rawVertex)
                    }
                }
            }
            else
            {
                this._renderDisplay.x = 0;
                this._renderDisplay.y = 0;
                this._renderDisplay.x = 0;
            }
        }

        protected override void _UpdateMesh()
        {
            // TODO: https://github.com/DragonBones/DragonBonesJS/blob/master/Phaser/3.x/src/dragonBones/phaser/display/Slot.ts#L215:L301
            var scale = this._armature._armatureData.scale;
            var deformVertices = this._deformVertices.vertices;
            var bones = this._deformVertices.bones;
            var hasDeform = deformVertices.Count > 0;
            var verticesData = this._deformVertices.verticesData;
            var weightData = verticesData.weight;

            var data = verticesData.data;
            var intArray = data.intArray;
            var floatArray = data.floatArray;
            var vertexCount = intArray[verticesData.offset + (int)BinaryOffset.MeshVertexCount];

            var mesh = meshDisplay as MeshSlot;

            if (weightData != null)
            {
                var weightFloatOffset = (int)intArray[weightData.offset + (int)BinaryOffset.WeigthFloatOffset];

                if (weightFloatOffset < 0)
                {
                    weightFloatOffset += 65536;
                }

                var iB = weightData.offset + (int)BinaryOffset.WeigthBoneIndices + weightData.bones.Count;
                var iV = weightFloatOffset;
                var iF = 0;

                for (int i = 0; i < vertexCount; i++)
                {
                    var boneCount = intArray[iB++];
                    var xG = 0f;
                    var yG = 0f;
                    
                    for (var j = 0; j < boneCount; j++)
                    {
                        var boneIndex = intArray[iB++];
                        var bone = bones[boneIndex];
                        if (bone != null)
                        {
                            var matrix = bone.globalTransformMatrix;
                            var weight = floatArray[iV++];
                            var xL = floatArray[iV++] * scale;
                            var yL = floatArray[iV++] * scale;

                            if (hasDeform)
                            {
                                xL += deformVertices[iF++];
                                yL += deformVertices[iF++];
                            }
                            xG += (matrix.a * xL + matrix.c * yL + matrix.tx) * weight;
                            yG += (matrix.a * xL + matrix.c * yL + matrix.ty) * weight;
                        }
                    }

                    meshDisplay.fakeVertices[i + this._vertexOffset] = xG;
                    meshDisplay.fakeVertices[i + this._vertexOffset] = yG;
                }

            }
            else if (deformVertices.Count > 0)
            {
                var vertexOffset = (int)data.intArray[verticesData.offset + (int)BinaryOffset.MeshFloatOffset];
                if (vertexOffset < 0)
                {
                    vertexOffset += 65536;
                }

                var a = globalTransformMatrix.a;
                var b = globalTransformMatrix.b;
                var c = globalTransformMatrix.c;
                var d = globalTransformMatrix.d;
                var tx = globalTransformMatrix.tx;
                var ty = globalTransformMatrix.ty;

                var index = 0;
                var rx = 0f;
                var ry = 0f;
                var vx = 0f;
                var vy = 0f;

                // MeshBuffer meshBuffer = null;
                var iV = 0;
                var iF = 0;
                for (int i = 0; i < vertexCount; i++)
                {
                    rx = (data.floatArray[vertexOffset + iV++] * scale + deformVertices[iF++]);
                    ry = (data.floatArray[vertexOffset + iV++] * scale + deformVertices[iF++]);

                    // this._meshBuffer.rawVertexBuffers[i].x = rw, .y = -ry
                    // this._meshBuffer.vertexBuffers[i].x = rw, .y = -ry

                    if (meshDisplay != null)
                    {
                        index = i + this._vertexOffset;
                        vx = (rx * a + ry * c + tx);
                        vy = (rx * b + ry * d + tx);

                        meshDisplay.fakeVertices[index].x = vx;
                        meshDisplay.fakeVertices[index].y = vy;
                    }
                }

                if (meshDisplay != null)
                {
                    meshDisplay.vertexDirty = true;
                }
                else
                {
                    this.meshDisplay.UpdateVertices();
                }
            }

        }

        protected override void _UpdateTransform()
        {
            this.UpdateGlobalTransform();

            var transform = this.global;
            this._textureTransform = transform; // We'll draw later using this
        }

        protected override void _UpdateZOrder()
        {
            // TODO
        }

        internal override void _UpdateBlendMode()
        {
            if (this._currentBlendMode == this._blendMode)
            {
                return;
            }

            foreach (var slot in _childArmature.GetSlots())
            {
                slot._blendMode = this._blendMode;
                slot._UpdateBlendMode();
            }

            this._currentBlendMode = this._blendMode;
        }

        internal override void _UpdateVisible()
        {
            // Refresh visibility value according to parent
            // this._renderDisplay.SetActive(this._parent.visible);
        }
    }
    */

    public class MonoGameSlot : Slot
    {
        private float _textureScale;
        private int _displayDepth = 0;
        private Transform _displayTransform = new Transform();
        private Texture2D _renderDisplay;
        private BlendMode _currentBlendMode;

        protected override void _OnClear()
        {
            base._OnClear();

            this._textureScale = 1.0f;

            if (this._renderDisplay != null)
            {
                this._renderDisplay.Dispose();
                this._renderDisplay = null;
            }
        }

        internal override void _UpdateBlendMode()
        {
            if (this._currentBlendMode == this._blendMode)
            {
                return;
            }

            this._currentBlendMode = this._blendMode;
        }

        protected override void _InitDisplay(object value, bool isRetain)
        { }

        protected override void _DisposeDisplay(object value, bool isRelease)
        { }

        protected override void _OnUpdateDisplay()
        {
            this._renderDisplay = (this._display ?? this._rawDisplay) as Texture2D;
        }

        protected override void _AddDisplay()
        {
            //this._armature.display.Add(this._renderDisplay);
        }

        protected override void _ReplaceDisplay(object value)
        {
            //this.armature.display.Replace(value, this._renderDisplay);
        }

        protected override void _RemoveDisplay()
        {
            this._renderDisplay = null;
        }

        //protected override void _UpdateDisplayData

        protected override void _UpdateZOrder()
        {
            if (this._displayDepth == this._zOrder) return;

            this._displayDepth = this._zOrder;
        }

        internal override void _UpdateVisible()
        {
            //this._renderDisplay.SetVisible(this._parent.visible && this._visible);
        }

        protected override void _UpdateColor()
        {
            var c = this._colorTransform;
            //var a = c.alphaMultiplier + c.alphaOffset;
            //this._renderDisplay.SetAlpha(a);

            var r = 0xff * c.redMultiplier + c.redOffset;
            var g = 0xff * c.greenMultiplier + c.greenOffset;
            var b = 0xff * c.blueMultiplier + c.blueOffset;
            //var rgb = (r << 16) | (g << 8) | b;
            //this._renderDisplay.SetTint(rgb);
        }

        protected override void _UpdateFrame()
        {
            var currentTextureData = this._textureData as MonoGameTextureData;

            if (this._displayIndex >= 0 && this._display != null && currentTextureData != null)
            {
                var currentTextureAtlasData = currentTextureData.parent as MonoGameTextureAtlasData;
                if (this.armature.replacedTexture != null)
                {
                    if (this.armature._replaceTextureAtlasData == null)
                    {
                        currentTextureAtlasData = BaseObject.BorrowObject<MonoGameTextureAtlasData>();
                        currentTextureAtlasData.CopyFrom(currentTextureData.parent);
                        currentTextureAtlasData.RenderTexture = this.armature.replacedTexture as Texture2D;
                        this._armature._replaceTextureAtlasData = currentTextureAtlasData;
                    }
                    else
                    {
                        currentTextureAtlasData = this._armature._replaceTextureAtlasData as MonoGameTextureAtlasData;
                    }
                }

                var frame = currentTextureData;
                if (frame != null)
                {
                    if (this._deformVertices != null)
                    {
                        var data = this._deformVertices.verticesData.data;
                        var intArray = data.intArray;
                        var floatArray = data.floatArray;
                        var vertexCount = intArray[this._deformVertices.verticesData.offset + (int)BinaryOffset.MeshVertexCount];
                        var triangleCount = intArray[this._deformVertices.verticesData.offset + (int)BinaryOffset.MeshTriangleCount];
                        var vertexOffset = (int)intArray[this._deformVertices.verticesData.offset + (int)BinaryOffset.MeshFloatOffset];

                        if (vertexOffset < 0)
                        {
                            vertexOffset += 65536;
                        }

                        var uvOffset = vertexOffset + vertexCount * 2;
                        var scale = this._armature._armatureData.scale;

                        //var meshDisplay = this._renderDisplay as MonoGameMesh;
                        // https://github.com/DragonBones/DragonBonesJS/blob/25ead4ef71a9fbdd5f204a752c653ff5e909e41a/Phaser/3.x/src/dragonBones/phaser/display/Slot.ts#L157-L196
                    }
                    else
                    {
                        this._renderDisplay = frame.renderTexture;
                        this._displayTransform.x = this._pivotX;
                        this._displayTransform.y = this._pivotY;
                        this._textureScale = currentTextureData.parent.scale * this.armature.armatureData.scale;
                        this._displayTransform.scaleX = this._textureScale;
                        this._displayTransform.scaleY = this._textureScale;
                    }

                    this._visibleDirty = true;
                    return;
                }
            }
            else
            {
                this._displayTransform.x = 0f;
                this._displayTransform.y = 0f;
                this._renderDisplay = null;
            }
        }

        protected override void _UpdateMesh()
        {
            // Not doing it =]
        }

        protected override void _UpdateTransform()
        {
            this.UpdateGlobalTransform();

            var transform = this.global;
            this._displayTransform = transform;
        }

        protected override void _IdentityTransform()
        {
            this._displayTransform.x = 0;
            this._displayTransform.y = 0;
            this._displayTransform.rotation = 0;
            this._textureScale = 1f;
            this._displayTransform.skew = 0;
        }
    }
}
