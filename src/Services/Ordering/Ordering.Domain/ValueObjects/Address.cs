﻿namespace Ordering.Domain.ValueObjects
{
    public record Address
    {


        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string? EmailAddress { get; private set; } = default!;
        public string AddressLine { get; private set; } = default!;
        public string Country { get; private set; } = default!;
        public string State { get; private set; } = default!;
        public string ZipCode { get; private set; } = default!;

        

        private Address(string firstName, string lastName, string? emailAddress, string addressLine, string country, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            AddressLine = addressLine;
            Country = country;
            State = state;
            ZipCode = zipCode;
        }


        public static Address Of(string firstName, string lastName, string? emailAddress, string addressLine, string country, string state, string zipCode)
        {
            //check mandatory fields
            ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

            return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
        }
    }
}
