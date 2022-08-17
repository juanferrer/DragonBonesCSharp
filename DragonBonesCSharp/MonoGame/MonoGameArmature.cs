namespace DragonBones
{
    public class MonoGameArmature : DragonBonesEventDispatcher, IArmatureProxy
    {
        internal Armature _armature = null;

        public Armature armature => _armature;
        public Animation animation => _armature.animation;

        internal readonly ColorTransform _colorTransform = new ColorTransform();

        public ColorTransform color
        {
            get => _colorTransform;
            set
            {
                this._colorTransform.CopyFrom(value);

                foreach (var slot in this._armature.GetSlots())
                {
                    slot._colorDirty = true;
                }    
            }
        }

        public void DBClear()
        {
            if (this._armature != null)
            {
                this._armature = null;
            }
        }

        public void DBInit(Armature armature)
        {
            this._armature = armature;
        }

        public void DBUpdate()
        { }

        public void Dispose(bool disposeProxy)
        {
            if (_armature != null)
            {
                _armature.Dispose();
            }
        }
    }
}
