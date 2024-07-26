namespace Ordering.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public static Customer Create(CustomerId CustomerId, string Name, string Email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(Email);

            Customer customer = new Customer
            {
               Id = CustomerId,
               Name = Name,
               Email = Email
            };
            return customer;
        }
    }
}
