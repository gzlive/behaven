using System;

namespace BehaveN
{
    internal class GridBlockType : BlockType
    {
        public override bool HandlesType(Type type)
        {
            return type != typeof(string) && GetCollectionItemType(type) != null;
        }

        public override object GetObject(Type type, IBlock block)
        {
            return block.ConvertTo(type);
        }
    }
}