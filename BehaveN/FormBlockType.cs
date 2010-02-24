using System;

namespace BehaveN
{
    internal class FormBlockType : BlockType
    {
        public override bool HandlesType(Type type)
        {
            return GetCollectionItemType(type) == null;
        }

        public override object GetObject(Type type, IBlock block)
        {
            return block.ConvertTo(type);
        }
    }
}