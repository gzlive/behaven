using System;

namespace BehaveN
{
    internal class GridBlockType : BlockType
    {
        public override bool HandlesType(Type type)
        {
            return GetCollectionItemType(type) != null;
        }

        public override object GetObject(Type type, IConvertibleObject convertibleObject)
        {
            Type itemType = GetCollectionItemType(type);
            return convertibleObject.ToList(itemType);
        }
    }
}