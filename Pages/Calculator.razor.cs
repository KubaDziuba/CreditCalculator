using CreditCalculator.Models;
using System.Reflection.Emit;

namespace CreditCalculator.Pages
{
    public partial class Calculator
    {
        private bool _isCalculated;
        //user input fields
        private int numberOfInstallments; //user input
        private float loan; //user input - total loan amount
        private float rate; //user inpu - yearly rate of the loan

        //decreasing installments fields
        private double capital; //capital part for decreasing installments, equal in every payment
        private double interestD; //interest part for given decreasing installment
        private double totalPaymentD; //total payment for given decreasing installment
        private double totalInterestD; //sum of all interest for total row
        private double totalPayD; //sum of all payments

        //fixed installments fields
        private double remainingCapital; //capital remained after each  installment
        private double factor; //variable for easier formula in total fixed payment calculation
        private double totalPaymentF; //total payment for given fixed installment
        private double interestF; //interest part for given fixed installment
        private double capitalF; //capital part for given fixed installment
        private double totalInterestF; //sum of all interest for total row
        private double totalPayF; //sum of all payments

        private double differenceInInterest; //diffrence betwen total interest for both payment variants
        private bool _isInputOK; //check if user input is correct

        //List of decreasing installments
        private readonly List<Installment> decreasingInstallments = new();
        //List of fixed installments
        private readonly List<Installment> fixedInstallments = new();

        protected override Task OnInitializedAsync()
        {
            _isCalculated = false;
            return base.OnInitializedAsync();
        }
        /// <summary>
        /// Function to calculate interest part of decreasing installment
        /// </summary>
        /// <param name="loanD">Total loan amount</param>
        /// <param name="rateD">Loan rate</param>
        /// <param name="installmentNbr">Number of given installment</param>
        /// <param name="capitalD">Capital part of installment</param>
        /// <returns>Interest as double for given installment</returns>
        private static double InterestOfDecreasingInstallment(float loanD, float rateD, int installmentNbr, double capitalD)
        {
            return (loanD - capitalD * (installmentNbr - 1)) * ((rateD / 100) / 12);
        }

        /// <summary>
        /// Function to prepare list of decreasing installments
        /// </summary>
        private void DecreasingInstallments()
        {
            decreasingInstallments.Clear();
            capital = loan / numberOfInstallments; //capital part of installment, equal in every installment
            totalInterestD = 0;
            totalPayD = 0;
            for (int i = 1; i <= numberOfInstallments; i++)
            {
                interestD = InterestOfDecreasingInstallment(loan, rate, i, capital);
                totalPaymentD = interestD + capital;
                decreasingInstallments.Add(new Installment(i, capital, interestD, totalPaymentD));
                totalInterestD += interestD;
                totalPayD += totalPaymentD;
            }
        }

        /// <summary>
        /// Function to calculate inetrest part of fixed installments
        /// </summary>
        /// <param name="remainingCapital">Capital remaining after each payment</param>
        /// <param name="rate">Loan rate</param>
        /// <returns>Interest as double for given installment</returns>
        private static double InterestOfFixedInstallment(double remainingCapital, float rate)
        {
            return remainingCapital * (rate / 100) / 12;
        }

        /// <summary>
        /// Calculates total payment amount of fixed installments
        /// </summary>
        /// <param name="remainingCapital">Capital remaining after each payment</param>
        /// <param name="numberOfInstallment">Number of given installment</param>
        /// <param name="rate">Loan rate</param>
        /// <returns></returns>
        private double TotalPaymentOfFixedInstallments(float loan, int numberOfInstallment, float rate) 
        {
            factor = 1 + (rate / 100 / 12);
            double factorPowered = Math.Pow(1 + (rate / 100 / 12), numberOfInstallment) ;
            return (loan * factorPowered * (factor - 1)) / (factorPowered - 1);
        }

        /// <summary>
        /// Preparing list of fixed installments
        /// </summary>
        private void FixedInstallments()
        {
            fixedInstallments.Clear();
            remainingCapital = loan;
            totalPaymentF = TotalPaymentOfFixedInstallments(loan, numberOfInstallments, rate);
            totalInterestF = 0;
            totalPayF = 0;
            for (int i = 1; i <= numberOfInstallments; i++)
            {
                interestF = InterestOfFixedInstallment(remainingCapital, rate);
                capitalF = totalPaymentF - interestF;
                fixedInstallments.Add(new Installment(i, capitalF, interestF, totalPaymentF));
                remainingCapital -= capitalF;
                totalInterestF += interestF;
                totalPayF += totalPaymentF;
            }
        }

        private void CalculateInstallments()
        {
            if (numberOfInstallments > 0 && loan > 0 && rate > 0)
            {
                _isCalculated = true;
                FixedInstallments();
                DecreasingInstallments();
                differenceInInterest = totalInterestF - totalInterestD;
            }
        }


    }
}
