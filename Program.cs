class MortgageCalculator
{
    public static double CalculateMonthlyRepayment(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        // This is used to convert the annual interest rate into a monthly interest rate.
        double monthlyInterestRate = annualInterestRate / 12 / 100;

        // This is used to convert the loan term from years to months for use in the formula for the monthly repayment.
        int loanTermMonths = loanTermYears * 12;

        // This is used to calculate the monthly repayment
        double monthlyRepayment = loanAmount * (monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, loanTermMonths)) / (Math.Pow(1 + monthlyInterestRate, loanTermMonths) - 1);

        return Math.Round(monthlyRepayment, 2);
    }

    public static double CalculateTotalInterestPaid(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        double totalPayment = CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);
        double totalInterestPaid = totalPayment - loanAmount;

        return Math.Round(totalInterestPaid, 2);
    }

    public static double CalculateTotalAmountPaid(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
        double totalPayment = monthlyRepayment * loanTermYears * 12;

        return Math.Round(totalPayment, 2);
    }

    public static List<AmortizationEntry> GenerateAmortizationSchedule(double loanAmount, double annualInterestRate, int loanTermYears)
    {
        List<AmortizationEntry> amortizationSchedule = new List<AmortizationEntry>();
        double monthlyInterestRate = annualInterestRate / 12 / 100;
        int numberOfPayments = loanTermYears * 12;
        double monthlyRepayment = CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);

        double remainingBalance = loanAmount;
        for (int i = 1; i <= numberOfPayments; i++)
        {
            double interestPayment = remainingBalance * monthlyInterestRate;
            double principalPayment = monthlyRepayment - interestPayment;

            if (i == numberOfPayments)
            {
                principalPayment = remainingBalance;
                monthlyRepayment = principalPayment + interestPayment;
            }

            remainingBalance -= principalPayment;

            AmortizationEntry entry = new AmortizationEntry(i, monthlyRepayment, interestPayment, principalPayment, remainingBalance);
            amortizationSchedule.Add(entry);
            if (remainingBalance <= 0) break;
        }

        return amortizationSchedule;
    }
}

// Class to represent an entry in the amortization schedule
public class AmortizationEntry
{
    public int PaymentNumber { get; }
    public double PaymentAmount { get; }
    public double InterestPaid { get; }
    public double PrincipalPaid { get; }
    public double RemainingBalance { get; }

    public AmortizationEntry(int paymentNumber, double paymentAmount, double interestPaid, double principalPaid, double remainingBalance)
    {
        PaymentNumber = paymentNumber;
        PaymentAmount = paymentAmount;
        InterestPaid = interestPaid;
        PrincipalPaid = principalPaid;
        RemainingBalance = remainingBalance;
    }
}

class Program
{
    public static void Main()
    {
        string userInput;
        double annualInterestRate;
        int loanTermYears = 0;
        double loanAmount = 0;

        try
        {
            while (true)
            {
                Console.WriteLine("\t\t========================================\t\t");
                Console.WriteLine("\t\tGood day. Welcome to UXI Mortgage Loans.\t\t");
                Console.WriteLine("\t\t========================================\t\t\n\n");
                Console.WriteLine("\t\t========================================\t\t");
                Console.WriteLine("\t\tWhat would you like to do today?\n\t\t1. Take out a loan.\n\t\t2. Exit");
                Console.WriteLine("\t\t========================================\t\t");
                userInput = Console.ReadLine();
                if (userInput == "1")
                {
                    Console.WriteLine("\t\t========================================\t\t");
                    Console.WriteLine("\t\tPlease enter your loan amount:\t\t");
                    Console.WriteLine("\t\t========================================\t\t");
                    userInput = Console.ReadLine();
                    while (!double.TryParse(userInput, out loanAmount))
                    {
                        Console.WriteLine("\t\t========================================\t\t");
                        Console.WriteLine("\t\tInvalid input. Please try again using numbers.\t\t");
                        Console.WriteLine("\t\t========================================\t\t");
                        userInput = Console.ReadLine();
                    }

                    Console.WriteLine("\t\t========================================\t\t");
                    Console.WriteLine("\t\tPlease enter your annual interest rate:\n\t\t(If none is given the default will be set to the prime interest rate which is 11.75%)");
                    Console.WriteLine("\t\tPlease ensure to use ',' if you want to use a decimal value.\t\t");
                    Console.WriteLine("\t\t========================================\t\t");
                    userInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(userInput))
                    {
                        if (double.TryParse(userInput, out annualInterestRate))
                        {
                            // Input is a valid number.
                        }
                        else
                        {
                            Console.WriteLine("\t\t========================================\t\t");
                            Console.WriteLine("\t\tInvalid input. Defaulting to prime interest rate of 11.75%\t\t");
                            Console.WriteLine("\t\t========================================\t\t");
                            annualInterestRate = 11.75;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\t\t========================================\t\t");
                        Console.WriteLine("\t\tInvalid input. Defaulting to prime interest rate of 11.75%\t\t");
                        Console.WriteLine("\t\t========================================\t\t");
                        annualInterestRate = 11.75;
                    }

                    Console.WriteLine("\t\t========================================\t\t");
                    Console.WriteLine("\t\tPlease enter your mortgage term in years:\t\t");
                    Console.WriteLine("\t\t========================================\t\t");
                    userInput = Console.ReadLine();
                    while (!int.TryParse(userInput, out loanTermYears))
                    {
                        Console.WriteLine("\t\t========================================\t\t");
                        Console.WriteLine("\t\tInvalid input. Please try again using numbers.\t\t");
                        Console.WriteLine("\t\t========================================\t\t");
                        userInput = Console.ReadLine();
                    }

                    while (true)
                    {
                        Console.WriteLine("\t\t========================================\t\t");
                        Console.WriteLine("\t\tWhat would you like to calculate?\t\t");
                        Console.WriteLine("\t\t1. Monthly Repayment\n\t\t2. Total interest paid over the life of the loan\n\t\t3. Total amount paid over the life of the loan.\n\t\t4. Amortization schedule");
                        Console.WriteLine("\t\t========================================\t\t");
                        userInput = Console.ReadLine();
                        switch (userInput)
                        {
                            case "1":
                                double monthlyRepayment = MortgageCalculator.CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
                                Console.WriteLine("\t\t========================================\t\t");
                                Console.WriteLine($"\t\tThe montly repayment amount is R{monthlyRepayment}.\t\t");
                                Console.WriteLine("\t\t========================================\t\t");
                                break;
                            case "2":
                                double totalInterest = MortgageCalculator.CalculateTotalInterestPaid(loanAmount, annualInterestRate, loanTermYears);
                                Console.WriteLine("\t\t========================================\t\t");
                                Console.WriteLine($"\t\tThe total interest paid over the life of the loan is R{totalInterest}.\t\t");
                                Console.WriteLine("\t\t========================================\t\t");
                                break;
                            case "3":
                                double totalPaid = MortgageCalculator.CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);
                                Console.WriteLine("\t\t========================================\t\t");
                                Console.WriteLine($"\t\tThe total paid over the life of the loan is R{totalPaid}.\t\t");
                                Console.WriteLine("\t\t========================================\t\t");
                                break;
                            case "4":
                                List<AmortizationEntry> amortizationSchedule = MortgageCalculator.GenerateAmortizationSchedule(loanAmount, annualInterestRate, loanTermYears);
                                Console.WriteLine("\t\t========================================\t\t");
                                Console.WriteLine("\n\t\tAmortization Schedule:\t\t");
                                Console.WriteLine("\n\t\t========================================\t\t");
                                Console.WriteLine("Payment Number | Payment Amount | Interest Paid | Principal Paid | Remaining Balance");
                                foreach (var entry in amortizationSchedule)
                                {
                                    Console.WriteLine($"{entry.PaymentNumber,-14} | {entry.PaymentAmount,-15:C} | {entry.InterestPaid,-13:C} | {entry.PrincipalPaid,-14:C} | {entry.RemainingBalance,-18:C}");
                                }
                                break;
                            default:
                                Console.WriteLine("\t\t========================================\t\t");
                                Console.WriteLine("\t\tInvalid choice. Please choose a valid option (1-4).\t\t");
                                Console.WriteLine("\t\t========================================\t\t");
                                break;
                        }
                        Console.WriteLine("\t\t========================================\t\t");
                        Console.WriteLine("\t\tDo you want to perform another calculation? (yes/no)\t\t");
                        Console.WriteLine("\t\t========================================\t\t");
                        userInput = Console.ReadLine().ToLower();
                        if (userInput != "yes")
                        {
                            break;
                        }
                    }
                    Console.WriteLine("\t\t========================================\t\t");
                    Console.WriteLine("\t\tThank you for using the Mortgage Calculator.\t\t");
                    Console.WriteLine("\t\tPress any key to exit...\t\t");
                    Console.WriteLine("\t\t========================================\t\t");
                    Console.ReadKey();
                    return;
                }
                else if (userInput == "2")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("\t\t========================================\t\t");
                    Console.WriteLine("\t\tIncorrect option. Please try again.\t\t");
                    Console.WriteLine("\t\t========================================\t\t");
                }

            }

        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}
