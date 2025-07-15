namespace Shall.Verify.Common.Enums;

public enum AddressTypes
{
    Standard,
    Billing,
    Delivery
}

public enum PhoneTypes
{
    None,
    Mobile,
    Landline
}

public enum RecordStatusType
{
    Placed,
    Rejected,
    Completed,
    Cancelled
}

public enum RecordDataTypes
{
    None,
    Email,
    Phone,
    Status
}

public enum PaymentTypes 
{
    None,
    CreditCard,
    DebitCard,
    DirectDebit,
    PayPal,
    Bitcoin,
    GooglePay,
    ApplePay,
    AmazonPay,
    Other
}