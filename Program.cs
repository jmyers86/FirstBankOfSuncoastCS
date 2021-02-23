using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace FirstBankOfSuncoast
{
    class Transaction
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public string Account { get; set; }
    }

    class Program
    {
        static int PENNIES_PER_DOLLAR = 100;
        static List<Transaction> Transactions = new List<Transaction>();
        static void Main(string[] args)
        {
            LoadTransactions();
            var isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("1) Deposit to Checking");
                Console.WriteLine("2) Withdraw from Checking");
                Console.WriteLine("3) Show Checking Balance");
                Console.WriteLine("");
                Console.WriteLine("4) Deposit to Savings");
                Console.WriteLine("5) Withdraw from Savings");
                Console.WriteLine("6) Show Savings Balance");

                var option = GetIntegerFromUser("Choose an option (1-6) or 0 to quit.");

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Goodbye.");
                        isRunning = false;
                        break;
                    case 1:
                        Deposit("Checking");
                        break;
                    case 2:
                        Withdraw("Checking");
                        break;
                    case 3:
                        PrintBalance("Checking");
                        break;
                    case 4:
                        Deposit("Savings");
                        break;
                    case 5:
                        Withdraw("Savings");
                        break;
                    case 6:
                        PrintBalance("Savings");
                        break;
                    default:
                        Console.WriteLine("That's not a valid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
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
            PersistTransactions();
            PrintBalance(account);
        }

        static void Withdraw(string account)
        {
            Console.Clear();
            Console.WriteLine($"Withdraw into {account}");
            var amount = GetMoneyFromUser("How much do you want to withdraw?");
            var transaction = new Transaction()
            {
                Type = "Withdraw",
                Account = account,
                Amount = amount,
            };

            if (GetBalance(account) >= transaction.Amount)
            {
                Transactions.Add(transaction);
                PersistTransactions();
            }
            else
            {
                Console.WriteLine("Insufficient Funds.");
            }
            PrintBalance(account);
        }

        static void PrintBalance(string account)
        {
            Console.WriteLine($"Balance of {account}: {FormatMoney(GetBalance(account))}");
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }

        static int GetBalance(string account)
        {
            var depositTotal = Transactions
              .Where(t => t.Type == "Deposit" && t.Account == account)
              .Sum(t => t.Amount);

            var withdrawTotal = Transactions
              .Where(t => t.Type == "Withdraw" && t.Account == account)
              .Sum(t => t.Amount);

            return depositTotal - withdrawTotal;
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

            return Convert.ToInt32(valueInDollars * PENNIES_PER_DOLLAR);
        }

        static string FormatMoney(int amountInCents)
        {
            double amountInDollars = Convert.ToDouble(amountInCents) / PENNIES_PER_DOLLAR;
            return amountInDollars.ToString("C2");
        }
        static void PersistTransactions()
        {
            var fileWriter = new StreamWriter("transactions.csv");
            var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(Transactions);
            fileWriter.Close();
        }

        static void LoadTransactions()
        {
            try
            {
                var fileReader = new StreamReader("transactions.csv");
                var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                Transactions = csvReader.GetRecords<Transaction>().ToList();
                fileReader.Close();
            }
            catch (System.IO.FileNotFoundException)
            {

            }
        }
    }
}