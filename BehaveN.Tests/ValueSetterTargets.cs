using System;
using System.Collections.Generic;

namespace BehaveN.Tests
{
    public class ReadWriteInts
    {
        public int IntField = 0;
        public int IntProperty { get { return IntField; } set { IntField = value; } }
        public int IntMethod() { return IntField; }
        public void IntMethod(int value) { IntField = value; }
    }

    [CoverageExclude]
    public class ReadOnlyInts
    {
        public readonly int IntField = 0;
        public int IntProperty { get { return IntField; } }
        public int IntMethod() { return IntField; }
    }

    public enum TestEnum
    {
        Foo,
        Bar,
        BazQuux
    }

    public class ReadWriteEnums
    {
        public TestEnum EnumField;
        public TestEnum? NullableEnumField;
        public TestEnum EnumProperty { get { return EnumField; } set { EnumField = value; } }
        public void EnumMethod(TestEnum value) { EnumField = value; }
    }

    public class ReadWriteStrings
    {
        public string StringField;
    }

    public class ReadWriteDateTimes
    {
        public DateTime DateTimeField;
    }

    public class ReadWriteLists
    {
        public List<int> ListOfInt;
        public IList<int> IListOfInt;
        public ICollection<int> ICollectionOfInt;
        public IEnumerable<int> IEnumerableOfInt;
    }
}
