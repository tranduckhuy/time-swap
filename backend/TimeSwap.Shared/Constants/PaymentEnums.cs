namespace TimeSwap.Shared.Constants
{
    public enum PaymentMethodType
    {
        CreditCard,
        BankAccount,
        EWallet
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }

    public enum TransactionEvent
    {
        PaymentInitiated,
        PaymentProcessed,
        PaymentCompleted,
        PaymentFailed
    }
}
