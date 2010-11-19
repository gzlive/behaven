using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class ValueGetterTests
    {
        [Test]
        public void It_doesnt_throw_AmbiguousMatchException_when_getting_a_property_that_overrides_a_virtual_property_that_uses_a_generic_type_parameter()
        {
            var pi = ValueGetter.GetPropertyInfo(typeof(Customer), "Id");
            pi.Should().Not.Be.Null();
            pi.DeclaringType.Should().Be(typeof(Customer));
        }

        public class DomainObject<T>
        {
            public virtual T Id { get; set; }
        }

        public class Customer : DomainObject<string>
        {
            public override string Id { get; set; }
        }
    }
}
