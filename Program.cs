using System.Numerics;

class MortgageCalculator {
    static double CalculateMonthlyRepayment(double loanAmount, double annualInterestRate, int loanTermYears) {
        // This is used to convert the annual interest rate into a monthly interest rate.
        double monthlyInterestRate = annualInterestRate / 12;

        // This is used to convert the loan term from years to months for use in the formula for the monthly repayment.
        int loanTermMonths = loanTermYears * 12;
        
        // This is used to calculate the monthly repayment
        double monthlyRepayment = loanAmount * (monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, loanTermMonths)) / (Math.Pow(1 + monthlyInterestRate, loanTermMonths) - 1);

        return monthlyRepayment;
    }

    static double CalculateTotalInterestPaid(double loanAmount, double annualInterestRate, int loanTermYears) {
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        
        double totalPayment = CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);
        double totalInterestPaid = totalPayment - loanAmount;

        return totalInterestPaid;
    }

    static double CalculateTotalAmountPaid(double loanAmount, double annualInterestRate, int loanTermYears) {
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        double totalPayment = monthlyRepayment * loanTermYears * 12;
        
        return totalPayment;
    }

    static void GenerateAmortizationSchedule(double loanAmount, double annualInterestRate, int loanTermYears) {

    }

    static void Main(String[] args) {

    }
}