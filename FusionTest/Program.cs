using FusionTest.Models;
using System.Net.Http.Headers;

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
                _client.BaseAddress = new Uri(_uri);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_httpMediaType));

                ShowReport(await GetParts());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }


        static private void ShowReport(CustomerParts custParts)
        {
            List<string> reportHeader = new(), reportDetail = new(), reportOutput = new();
            decimal totalCost = 0;
            int rows = 0;

            //broken out for readability
            var reportData = custParts.Parts
               .Distinct()
               .Where(x => x.CategoryType == _cpu)
               .OrderByDescending(x => x.Price)
               .Take(2)
               .OrderBy(x => x.Price)
               .ToList()
               .Concat(
                    custParts.Parts
                    .Distinct()
                    .Where(x => x.CategoryType != _cpu)
                    .OrderBy(x => x.Price)
                    .ToList()
                );

            foreach (var report in reportData)
            {
                if ((totalCost + report.Price) <= _budget)
                {
                    totalCost += report.Price;
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

        //normally in a data layer
        static private async Task<CustomerParts> GetParts()
        {
            CustomerParts? customerParts = null;
            var response = await _client.GetAsync(_uri);

            if (response.IsSuccessStatusCode)
                customerParts = await response.Content.ReadAsAsync<CustomerParts>();

            return customerParts;
        }
        #endregion
    }
}


