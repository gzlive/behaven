namespace BehaveN.Examples
{
    public class FormStepDefinitions
    {
        private string _name;

        public void When_creating_a_customer_named_arg1(string name)
        {
            _name = name;
        }

        public void Then_the_customer_object_should_look_like_this(out Customer customer)
        {
            customer = new Customer { Id = _name };
        }
    }

    public class DomainObject<TKey>
    {
        public virtual TKey Id { get; set; }
    }

    public class Customer : DomainObject<string>
    {
        public override string Id { get; set; }
    }
}