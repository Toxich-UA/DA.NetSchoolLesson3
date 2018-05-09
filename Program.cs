using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lesson3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var clients = JsonConvert.DeserializeObject<List<BankClient>>(File.ReadAllText(@"bankClients.json"));
            
            //First
            var sumAmountForApril = clients.SelectMany(operation => operation.Operations)
                .Where(date => date.Date.Month == 4).Sum(a => a.Amount);
            Console.WriteLine($"1. The amount of debit and credit operations for April equal {sumAmountForApril}");
            Utility.SerializeObject<BankClient>(sumAmountForApril, "1.sumAmountForApril", "Json");
            Console.WriteLine("Saved to 1.sumAmountForApril.json");
            ContinueMessage();

            //Second
            var customersWhoDidNotWithdrawMoney = clients.Where(o => (o.Operations.FindAll(type => type.OperationType == "Credit").Count != 0) && (o.Operations.Any(date => date.Date.Month == 4)));
            Console.WriteLine("1. Customers who did not withdraw money in april ");
            PrintClientInformation(customersWhoDidNotWithdrawMoney);
            Utility.SerializeObject<BankClient>(customersWhoDidNotWithdrawMoney, "2.customersWhoDidNotWithdrawMoney", "json");
            Console.WriteLine("Saved to 2.customersWhoDidNotWithdrawMoney.json");
            ContinueMessage();

            //Thirt
            var debitMaxValue = clients.Max(o => o.Operations.Where(type => type.OperationType == "Debit").Sum(a => a.Amount));
            var maxDebitOperation = clients.Where(o => o.Operations.Where(type => type.OperationType == "Debit").Sum(a => a.Amount).Equals(debitMaxValue));
            Console.WriteLine("3.Max debit operation:");
            PrintClientInformation(maxDebitOperation);
            Utility.SerializeObject<BankClient>(maxDebitOperation, "3.Max debit operation", "json");
            Console.WriteLine("Saved to 3.Max debit operation.json");
            ContinueMessage();

            //Fourth
            var creditMaxValue = clients.Max(o => o.Operations.Where(type => type.OperationType == "Credit").Sum(a => a.Amount));
            var maxCreditOperation = clients.Where(o => o.Operations.Where(type => type.OperationType == "Credit").Sum(a => a.Amount).Equals(creditMaxValue));
            Console.WriteLine("4.Max credit operation:");
            PrintClientInformation(maxCreditOperation);
            Utility.SerializeObject<BankClient>(maxCreditOperation, "4.Max credit operation", "json");
            Console.WriteLine("Saved to 4.Max credit operation.json");
            ContinueMessage();

            //Fifth
            var filteredOperations = clients.Where(d => d.Operations.All(date => date.Date.CompareTo(new DateTime(2018, 5, 1, 0, 0, 0)) == -1)).ToList();
            var clientWithMaxBalance = Utility.GetClientWithMaxBalance(filteredOperations);
            Console.WriteLine("5.Сustomer with the maximum balance as of May 1 00:00:");
            PrintSingleClientInformation(clientWithMaxBalance);
            Utility.SerializeObject<BankClient>(maxCreditOperation, "5.сustomerWithTheMaximumBalance", "json");
            Console.WriteLine("Saved to 5.сustomerWithTheMaximumBalance");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }

        public static void PrintClientInformation(IEnumerable<BankClient> clients)
        {
            foreach (var customer in clients)
            {
                PrintSingleClientInformation(customer);
            }
        }

        public static void PrintSingleClientInformation(BankClient client)
        {
            Console.WriteLine("=======================");
            Console.WriteLine($"First Name: {client.FirstName}");
            Console.WriteLine($"Last Name: {client.LastName}");
            Console.WriteLine($"Middle Name: {client.MiddleName}");
            Console.WriteLine($"First deposit: {client.Operations.First().Date}");
            Console.WriteLine("=======================");
        }

        private static void ContinueMessage()
        {
            Console.WriteLine("Press any key to continue(screen will be cleaned)...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
