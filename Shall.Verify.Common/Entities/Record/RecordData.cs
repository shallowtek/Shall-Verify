using Shall.Verify.Common.Enums;
using System.ComponentModel.DataAnnotations;
namespace Shall.Verify.Common.Entities.Record;

public class RecordData
{
    public Name Name { get; set; }
    public IList<Address> Address { get; set; }
    public IList<Email> Email { get; set; }
    public IList<Phone> Phone { get; set; }
    /// <summary>
    /// ISO 8601 - yyyy-MM-dd
    /// </summary>
    [DataType(DataType.Date)]
    public string DateOfBirth { get; set; }
}

public class Name
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
}

public class Address
{
    /// <summary>
    /// Standard, billing, delivery
    /// </summary>
    [EnumDataType(typeof(AddressTypes), ErrorMessage = "Invalid address type")]
    public AddressTypes AddressType { get; set; }
    /// <summary>
    /// Street address, P.O box, company name, c/o
    /// </summary>
    public string AddressLine1 { get; set; }
    /// <summary>
    /// Apartment, suite, unit, building, floor etc
    /// </summary>
    public string AddressLine2 { get; set; }
    /// <summary>
    /// City, town, village
    /// </summary>
    public string City { get; set; }
    /// <summary>
    /// State, province, region
    /// </summary>
    /// 
    public string Region { get; set; }
    /// <summary>
    /// Zip, postal code
    /// </summary>
    public string PostCode { get; set; }
    public string Country { get; set; }
}

public class Email
{
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string EmailAddress { get; set; }
}

public class Phone
{
    [EnumDataType(typeof(PhoneTypes), ErrorMessage = "Invalid phone type")]
    public PhoneTypes PhoneType { get; set; }

    [Phone(ErrorMessage = "Invalid Phone Number")]
    public string PhoneNumber { get; set; }
}

public class Transaction
{
    public decimal Value { get; set; }
    public string Currency { get; set; }
    /// <summary>
    /// ISO 8601 - yyyy-MM-dd
    /// </summary>
    [DataType(DataType.Date)]
    public string TransactionDate { get; set; }
    public IList<TransactionItem> Items { get; set; }

    [EnumDataType(typeof(PaymentTypes), ErrorMessage = "Invalid payment type")]
    public PaymentTypes PaymentType { get; set; }
}

public class TransactionItem
{
    public string Reference { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
}