namespace HotelWise.Domain.Enuns.Hotel;

/// <summary>
/// Métodos de pagamento aceitos para confirmação e liquidação de reservas no sistema.
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Pagamento via Cartão de Crédito.
    /// </summary>
    CreditCard = 1,

    /// <summary>
    /// Pagamento via carteira digital PayPal.
    /// </summary>
    PayPal = 2,

    /// <summary>
    /// Pagamento via Transferência Bancária / Pix.
    /// </summary>
    BankTransfer = 3
}
