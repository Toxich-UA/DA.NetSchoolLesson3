using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lesson3;
using Newtonsoft.Json;

namespace Lesson2
{
    static class Utility
    {
        /// <summary>
        /// Serialize object into json/xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static bool SerializeObject<T>(Object obj, string fileName, string fileType)
        {
            StreamWriter writer;

            fileType = fileType.ToLower();
            if (TypeIsValid(fileType))
            {
                writer = new StreamWriter($"{fileName}.{fileType}");
                switch (fileType)
                {
                    case "json":
                        using (writer)
                        {
                            writer.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
                        }
                        return true;
                    case "xml":
                        using (writer)
                        {
                            new XmlSerializer(typeof(T)).Serialize(writer, obj);
                        }
                        return true;
                }
            }

            return false;
        }
        private static bool TypeIsValid(string type)
        {
            return type != null && (type.Equals("json", StringComparison.CurrentCultureIgnoreCase) || type.Equals("xml", StringComparison.CurrentCultureIgnoreCase));
            ;
        }


        public static BankClient GetClientWithMaxBalance(List<BankClient> clients)
        {
            var maxBalance = new Dictionary<double, BankClient>();
            foreach (var client in clients)
            {
                maxBalance.Add(GetSum(client.Operations), client);
            }
            return maxBalance[maxBalance.Keys.Max()];
        }

        public static double GetSum(List<Operation> operationsList)
        {
            double result = .0;
            foreach (var operation in operationsList)
            {
                if (operation.OperationType == "Debit")
                    result += operation.Amount;
                else
                    result -= operation.Amount;
            }

            return result;
        }
    }
}
