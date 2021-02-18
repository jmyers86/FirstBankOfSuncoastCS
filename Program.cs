using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace FirstBankOfSuncoast
{
    class Transaction
    {
        public string Type;
        public int Amount;
        public string Account;

    }

    class Program
    {
        static List<Transaction> Transactions = new List<Transaction>();
        static void Main(string[] args)
        {
            var fileReader = new StreamReader("accounts.csv");
            var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);



            // Main Menu
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1) Deposit to Checking");
                Console.WriteLine("2) Withdrawl from Checking");
                Console.WriteLine("3) Show Checking Balance");
                Console.WriteLine("");
                Console.WriteLine("4) Deposit to Savings");
                Console.WriteLine("5) Withdrawl from Savings");
                Console.WriteLine("6) Show Savings Balance");

                var option = GetIntegerFromUser("Choose an option (1-6) or 0 to quit.");

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Goodbye.");
                        break;
                    case 1:
                        Deposit("Checking");
                        break;
                    case 2:
                        Withdrawal("Checking");
                        break;
                    case 3:
                        PrintBalance("Checking");
                        break;
                    case 4:
                        Deposit("Savings");
                        break;
                    case 5:
                        Withdrawal("Savings");
                        break;
                    case 6:
                        PrintBalance("Savings");
                        break;
                    default:
                        Console.WriteLine("That's not a valid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }

                if (option == 0) break;
            }
        }

        static void Deposit(string account)
        {
            Console.Clear();
            Console.WriteLine($"Deposit into {account}");
            var amount = GetMoneyFromUser("How much do you want to deposit?");
            var transaction = new Transaction()
            {
                Type = "Deposit",
                Account = account,
                Amount = amount,
            };
            Transactions.Add(transaction);

            var fileWriter = new StreamWriter("accounts.csv");
            var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(Transactions);
            fileWriter.Close();
            // TODO: Write to file.
            PrintBalance(account);

            Console.ReadKey();
        }
    
        static void Withdrawal(string account)
        {
            Console.Clear();
            Console.WriteLine($"Withdrawal from {account}");
            var amount = GetMoneyFromUser("How much do you want to withdraw?");
            var transaction = new Transaction()
            {
                Type = "Withdrawal",
                Account = account,
                Amount = amount,
            };
            Transactions.Add(transaction);
            var fileWriter = new StreamWriter("accounts.csv");
            var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(Transactions);
            fileWriter.Close();
            PrintBalance(account);

            // TODO: Implement Withdrawl
            Console.ReadKey();
        }

        static void PrintBalance(string account)
        {
            // TODO: Implement Balance Correctly
            var depositTotal = Transactions
              .Where(t => t.Type == "Deposit" && t.Account == account)
              .Sum(t => t.Amount);

            var withdrawalTotal = Transactions
              .Where(t => t.Type == "Withdrawal" && t.Account == account)
              .Sum(t => t.Amount);

            var balanceTotal = (depositTotal - withdrawalTotal);
            Console.Clear();
            Console.WriteLine($"Balance of {account}: {balanceTotal}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static int GetIntegerFromUser(string message)
        {
            Console.WriteLine("");
            Console.WriteLine(message);
            string input;
            int value;
            do
            {
                Console.Write("> ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out value));
            return value;
        }

        static int GetMoneyFromUser(string message)
        {
            Console.WriteLine("");
            Console.WriteLine(message);
            string input;
            float valueInDollars;

            do
            {
                Console.Write("> $");
                input = Console.ReadLine();
            } while (!float.TryParse(input, out valueInDollars));

            return Convert.ToInt32(valueInDollars * 100);
        }
    }
}