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

            if (i == numberOfPayments) {
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
                Console.WriteLine("Good day. Welcome to our console Mortgage Calculator.");

                Console.WriteLine("Please enter your loan amount:");
                userInput = Console.ReadLine();
                while (!double.TryParse(userInput, out loanAmount))
                {
                    Console.WriteLine("Invalid input. Please try again using numbers.");
                    userInput = Console.ReadLine();
                }

                Console.WriteLine("Please enter your annual interest rate:\n(If none is given the default will be set to the prime interest rate which is 11.75%)");
                userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput))
                {
                    if (double.TryParse(userInput, out annualInterestRate))
                    {
                        // Input is a valid number.
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Defaulting to prime interest rate of 11.75%");
                        annualInterestRate = 11.75;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Defaulting to prime interest rate of 11.75%");
                    annualInterestRate = 11.75;
                }

                Console.WriteLine("Please enter your mortgage term in years:");
                userInput = Console.ReadLine();
                while (!int.TryParse(userInput, out loanTermYears))
                {
                    Console.WriteLine("Invalid input. Please try again using numbers.");
                    userInput = Console.ReadLine();
                }

                while (true)
                {
                    Console.WriteLine("What would you like to calculate?");
                    Console.WriteLine("1. Monthly Repayment\n2. Total interest paid over the life of the loan\n3. Total amount paid over the life of the loan.\n4. Amortization schedule");
                    userInput = Console.ReadLine();
                    switch (userInput)
                    {
                        case "1":
                            double monthlyRepayment = MortgageCalculator.CalculateMonthlyRepayment(loanAmount, annualInterestRate, loanTermYears);
                            Console.WriteLine($"The montly repayment amount is R{monthlyRepayment}.");
                            break;
                        case "2":
                            double totalInterest = MortgageCalculator.CalculateTotalInterestPaid(loanAmount, annualInterestRate, loanTermYears);
                            Console.WriteLine($"The total interest paid over the life of the loan is R{totalInterest}");
                            break;
                        case "3":
                            double totalPaid = MortgageCalculator.CalculateTotalAmountPaid(loanAmount, annualInterestRate, loanTermYears);
                            Console.WriteLine($"The total paid over the life of the loan is R{totalPaid}");
                            break;
                        case "4":
                            List<AmortizationEntry> amortizationSchedule = MortgageCalculator.GenerateAmortizationSchedule(loanAmount, annualInterestRate, loanTermYears);
                            Console.WriteLine("\nAmortization Schedule:");
                            Console.WriteLine("Payment Number | Payment Amount | Interest Paid | Principal Paid | Remaining Balance");
                            foreach (var entry in amortizationSchedule)
                            {
                                Console.WriteLine($"{entry.PaymentNumber,-14} | {entry.PaymentAmount,-15:C} | {entry.InterestPaid,-13:C} | {entry.PrincipalPaid,-14:C} | {entry.RemainingBalance,-18:C}");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please choose a valid option (1-4).");
                            break;
                    }
                    Console.WriteLine("Do you want to perform another calculation? (yes/no)");
                    userInput = Console.ReadLine().ToLower();
                    if (userInput != "yes")
                    {
                        break;
                    }
                }
                Console.WriteLine("Thank you for using the Mortgage Calculator.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}
