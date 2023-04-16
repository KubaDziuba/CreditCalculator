namespace CreditCalculator.Models
{
    public class Installment
    {
        public int InstallmentNumber { get; set; }
        public double Capital { get; set; }
        public double Interest { get; set; }
        public double TotalToPay { get; set; }

        public Installment(int installmentNumber, double capital, double interest, double totalToPay)
        {
            InstallmentNumber = installmentNumber;
            Capital = capital;
            Interest = interest;
            TotalToPay = totalToPay;
        }
    }
}
