using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.Data.Analysis;

class MortgageCalculator {
    public static double CalculateMonthlyRepayment(double loanAmount, double annualInterestRate, int loanTermYears) {
        // This is used to convert the annual interest rate into a monthly interest rate.
        double monthlyInterestRate = annualInterestRate / 12;

        // This is used to convert the loan term from years to months for use in the formula for the monthly repayment.
        int loanTermMonths = loanTermYears * 12;
        
        // This is used to calculate the monthly repayment
        double monthlyRepayment = loanAmount * (monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, loanTermMonths)) / (Math.Pow(1 + monthlyInterestRate, loanTermMonths) - 1);

        return monthlyRepayment;
    }

    public static double CalculateTotalInterestPaid(double loanAmount, double annualInterestRate, int loanTermYears) {
        double totalPayment = CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);
        double totalInterestPaid = totalPayment - loanAmount;

        return totalInterestPaid;
    }

    public static double CalculateTotalAmountPaid(double loanAmount, double annualInterestRate, int loanTermYears) {
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        double totalPayment = monthlyRepayment * loanTermYears * 12;
        
        return totalPayment;
    }

    public static dynamic GenerateAmortizationSchedule(double loanAmount, double annualInterestRate, int loanTermYears) {
        /* This method generates and returns an amortization schedule. Each entry in the schedule should detail the payment number, 
        payment amount, interest paid, principal paid, and remaining balance. */
        int loanTermMonths = loanTermYears * 12;
        double monthlyInterestRate = annualInterestRate / 12;
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        double remainingBalance = loanAmount;
        
        int[] paymentNumber = new int[loanTermMonths];
        double[] paymentAmount = new double[loanTermMonths];
        double[] interestPaid = new double[loanTermMonths];
        double[] principlePaid = new double[loanTermMonths];
        double[] remainingBalanceArray = new double[loanTermMonths];

        for (int i = 0; i < loanTermMonths; i++) {
            paymentNumber[i] = i + 1;

            double interest = remainingBalance * monthlyInterestRate;
            interestPaid[i] = interest;

            double principal = monthlyRepayment - interest;
            principlePaid[i] = principal;

            remainingBalance -= principal;
            remainingBalanceArray[i] = remainingBalance;

            paymentAmount[i] = monthlyRepayment;
        }

        DataFrameColumn[] columns = {
            // This is used to make the respective columns for the data frame.
            new PrimitiveDataFrameColumn<int>("Payment Number", paymentNumber),
            new PrimitiveDataFrameColumn<double>("Payment Amount", paymentAmount),
            new PrimitiveDataFrameColumn<double>("Interest Paid", interestPaid),
            new PrimitiveDataFrameColumn<double>("Principle Paid", principlePaid),
            new PrimitiveDataFrameColumn<double>("Remainiing Balance", remainingBalanceArray),
        };

        DataFrame amortizationSchedule = new(columns);

        return amortizationSchedule;
    }
}

class Program {
    public static void Main() {

    }
}