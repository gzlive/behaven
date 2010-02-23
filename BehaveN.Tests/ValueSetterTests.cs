using System;
using System.Data;
using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class ValueSetterTests
    {
        [Test]
        public void Invalid()
        {
            Assert.That(ValueSetter.CanSetValue(this, "x"), Is.False);
            ValueSetter setter = ValueSetter.GetValueSetter(this, "x");
            Assert.That(setter.CanSetValue(), Is.False);
            Assert.Throws(typeof(InvalidOperationException), delegate { setter.GetValueType(); });
            Assert.Throws(typeof(InvalidOperationException), delegate { setter.SetValue(null); });
        }

        [Test]
        public void IntField()
        {
            ReadWriteInts target = new ReadWriteInts();
            Assert.That(ValueSetter.GetValueType(target, "IntField"), Is.SameAs(typeof(int)));
            Assert.That(ValueSetter.CanSetValue(target, "IntField"), Is.True);
            ValueSetter.SetValue(target, "IntField", 123);
            Assert.That(target.IntField, Is.EqualTo(123));
        }

        [Test]
        public void IntFieldWithWrongCase()
        {
            ReadWriteInts target = new ReadWriteInts();
            Assert.That(ValueSetter.CanSetValue(target, "iNTfIELD"), Is.True);
            ValueSetter.SetValue(target, "iNTfIELD", 123);
            Assert.That(target.IntField, Is.EqualTo(123));
        }

        [Test]
        public void IntProperty()
        {
            ReadWriteInts target = new ReadWriteInts();
            Assert.That(ValueSetter.CanSetValue(target, "IntProperty"), Is.True);
            ValueSetter.SetValue(target, "IntProperty", 123);
            Assert.That(target.IntProperty, Is.EqualTo(123));
        }

        [Test]
        public void IntMethod()
        {
            ReadWriteInts target = new ReadWriteInts();
            Assert.That(ValueSetter.CanSetValue(target, "IntMethod"), Is.True);
            ValueSetter.SetValue(target, "IntMethod", 123);
            Assert.That(target.IntMethod(), Is.EqualTo(123));
        }

        [Test]
        public void ReadOnlyIntField()
        {
            ReadOnlyInts target = new ReadOnlyInts();
            Assert.That(ValueSetter.CanSetValue(target, "IntField"), Is.False);
        }

        [Test]
        public void ReadOnlyIntProperty()
        {
            ReadOnlyInts target = new ReadOnlyInts();
            Assert.That(ValueSetter.CanSetValue(target, "IntProperty"), Is.False);
        }

        [Test]
        public void ReadOnlyIntMethod()
        {
            ReadOnlyInts target = new ReadOnlyInts();
            Assert.That(ValueSetter.CanSetValue(target, "IntMethod"), Is.False);
        }

        [Test]
        public void DataRowView()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IntColumn", typeof(int));
            dt.Rows.Add(new object[] { 123 });
            DataRowView target = dt.DefaultView[0];
            Assert.That(ValueSetter.CanSetValue(target, "IntColumn"), Is.True);
            Assert.That(ValueSetter.GetValueType(target, "IntColumn"), Is.SameAs(typeof(int)));
            ValueSetter.SetValue(target, "IntColumn", 456);
            Assert.That((int)target["IntColumn"], Is.EqualTo(456));
        }

        [Test]
        public void EnumField()
        {
            ReadWriteEnums target = new ReadWriteEnums();
            ValueSetter.SetFormattedValue(target, "EnumField", "Bar");
            Assert.That(target.EnumField, Is.EqualTo(TestEnum.Bar));
        }

        [Test]
        public void EnumProperty()
        {
            ReadWriteEnums target = new ReadWriteEnums();
            ValueSetter.SetFormattedValue(target, "EnumProperty", "Bar");
            Assert.That(target.EnumProperty, Is.EqualTo(TestEnum.Bar));
        }

        [Test]
        public void EnumMethod()
        {
            ReadWriteEnums target = new ReadWriteEnums();
            ValueSetter.SetFormattedValue(target, "EnumMethod", "Bar");
            Assert.That(target.EnumProperty, Is.EqualTo(TestEnum.Bar));
        }

        [Test]
        public void NullableEnumWhenNull()
        {
            // There's no way to pass in null yet.

            //ReadWriteEnums target = new ReadWriteEnums();
            //ValueSetter.SetFormattedValue(target, "NullableEnumField", "");
            //Assert.That(target.NullableEnumField, Is.Null);

            // I think I meant that there's no string representation for null.
            // This is a problem for both reference and nullable types.
        }

        [Test]
        public void NullableEnumWhenNotNull()
        {
            ReadWriteEnums target = new ReadWriteEnums();
            ValueSetter.SetFormattedValue(target, "NullableEnumField", "Bar");
            Assert.That(target.NullableEnumField, Is.EqualTo(TestEnum.Bar));
        }

        [Test]
        public void SettingFormattedValueOfNullOnIntFieldSetsItTo0()
        {
            ReadWriteInts target = new ReadWriteInts();
            target.IntField = 123;
            ValueSetter.SetFormattedValue(target, "IntField", null);
            Assert.That(target.IntField, Is.EqualTo(0));
        }

        [Test]
        public void SettingFormatedValueOfNullOnStringFieldSetsItToNull()
        {
            ReadWriteStrings target = new ReadWriteStrings();
            target.StringField = "xxx";
            ValueSetter.SetFormattedValue(target, "StringField", null);
            Assert.That(target.StringField, Is.Null);
        }

        [Test]
        public void SettingFormatedValueOnInt32FieldUsesInt32Parser()
        {
            ReadWriteInts target = new ReadWriteInts();
            target.IntField = int.MinValue;
            ValueSetter.SetFormattedValue(target, "IntField", "123rd");
            Assert.That(target.IntField, Is.EqualTo(123));
        }

        [Test]
        public void SettingFormatedValueOnDateTimeFieldUsesDateTimeParser()
        {
            ReadWriteDateTimes target = new ReadWriteDateTimes();
            target.DateTimeField = DateTime.MinValue;
            ValueSetter.SetFormattedValue(target, "DateTimeField", "Now");
            DateTimeParserTests.AssertThatDateTimeIsCloseEnoughToNow(target.DateTimeField);
        }

        [Test]
        public void ListOfIntField()
        {
            ReadWriteLists target = new ReadWriteLists();
            ValueSetter.SetFormattedValue(target, "ListOfInt", "123, 456");
            Assert.That(target.ListOfInt, Is.EquivalentTo(new int[] { 123, 456 }));
        }

        [Test]
        public void IListOfIntField()
        {
            ReadWriteLists target = new ReadWriteLists();
            ValueSetter.SetFormattedValue(target, "IListOfInt", "123, 456");
            Assert.That(target.IListOfInt, Is.EquivalentTo(new int[] { 123, 456 }));
        }

        [Test]
        public void ICollectionOfIntField()
        {
            ReadWriteLists target = new ReadWriteLists();
            ValueSetter.SetFormattedValue(target, "ICollectionOfInt", "123, 456");
            Assert.That(target.ICollectionOfInt, Is.EquivalentTo(new int[] { 123, 456 }));
        }

        [Test]
        public void IEnumerableOfIntField()
        {
            ReadWriteLists target = new ReadWriteLists();
            ValueSetter.SetFormattedValue(target, "IEnumerableOfInt", "123, 456");
            Assert.That(target.IEnumerableOfInt, Is.EquivalentTo(new int[] { 123, 456 }));
        }
    }
}
