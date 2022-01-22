﻿using FusionTest.Models;
using Newtonsoft.Json;



namespace FusionCode
{

    class Program
    {
        //would normally go in a appsetting json file
        private const string _uri = @"https://6f5c9791-a282-482e-bbe9-2c1d1d3f4c9f.mock.pstmn.io/interview/part-list";
        private const string _httpMediaType = @"application/json";
        //--------------------------------------------------------

        private static readonly HttpClient _client = new();
        private const string _cpu = "CPU";
        private const string _currencyFormat = "{0:C}";
        private const decimal _budget = 1800;

        static void Main()
        {
            RunShowReportAsync().GetAwaiter().GetResult();
        }


        #region "private methods"
        static private async Task RunShowReportAsync()
        {
            try
            {
                ShowReport(GetParts());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }


        static private void ShowReport(CustomerParts custParts)
        {
            List<string> reportHeader = new() { "" }, reportDetail = new() { "" }, reportOutput = new() { "" };
            decimal totalCost = 0;
            int rows = 0;

            //broken out for readability
            var expensiveCpus = custParts.Parts
               .Distinct()
               .Where(x => x.CategoryType == _cpu)
               .OrderByDescending(x => x.Price)
               .Take(2)
               .ToList();

            var noCpus = custParts.Parts
                .Distinct()
                .Where(x => x.CategoryType != _cpu)
                .OrderBy(x => x.Price)
                .ToList();

            var reportData = expensiveCpus.Concat(noCpus)
                .OrderBy(x => x.Price)
                .ToList();

            foreach (var report in reportData)
            {
                totalCost += report.Price;
                if (totalCost <= _budget)
                {
                    rows++;
                    reportDetail.Add($"{report.Name}: { string.Format(_currencyFormat, report.Price) }");
                }
                else
                    break;
            }
            foreach (var lineItem in CreateReportHeader(rows, totalCost).Concat(reportDetail).ToList())
                Console.WriteLine(lineItem);
        }

        static private List<string> CreateReportHeader(int count, decimal cost)
        {
            return new List<string>
            {
                "Customers Budget Report",
                $"Total Number of Parts: {count}",
                $"Total Parts Cost: { string.Format(_currencyFormat, cost)}",
                ""
            };
        }

        static private CustomerParts GetParts()
        {
            //client.BaseAddress = new Uri(_uri);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_httpMediaType));
            //return JsonConvert.DeserializeObject<CustomerParts>(client.SendAsync().);

            using (StreamReader r = new StreamReader("parts.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<CustomerParts>(json);
            }
        }
        #endregion
    }
}

