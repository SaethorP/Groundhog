namespace GroundHogApi.Entities
{
    public class Payment
    {
        public string DebtorName { get; set; }
        public string CreditorName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PaymentDate { get; set; }
        // Add other ISO 20022 standard fields as required
    }

}
