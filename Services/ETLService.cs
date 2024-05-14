using ETLMegaStore.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ETLMegaStore.Services
{

    public class ETLService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ETLService> logger; // Added for logging

        public ETLService(IConfiguration config, ILogger<ETLService> logger)
        {
            configuration = config;
            this.logger = logger;
        }

        public async Task ExtractData()
        {
            var filePath = configuration.GetSection("FilePath").Value;

            if (string.IsNullOrEmpty(filePath))
            {
                logger.LogError("Missing 'FilePath' in configuration. Please provide a valid path to the Excel file.");
                throw new Exception("Missing file path");
            }

            if (!File.Exists(filePath))
            {
                logger.LogError($"File not found: {filePath}");
                return;
            }

            var processedFolderPath = configuration.GetSection("ProcessedFolderPath").Value;
            FileInfo fileInfo = new FileInfo(filePath);

            List<SalesOrder> salesOrders = new List<SalesOrder>();

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Skip header
                {
                    SalesOrder order = new SalesOrder
                    {
                        Id = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                        OrderId = worksheet.Cells[row, 2].Value.ToString(),
                        OrderDate = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString()),
                        ShipDate = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString()),
                        ShipMode = worksheet.Cells[row, 5].Value.ToString(),
                        CustomerId = worksheet.Cells[row, 6].Value.ToString(),
                        CustomerName = worksheet.Cells[row, 7].Value.ToString(),
                        Segment = worksheet.Cells[row, 8].Value.ToString(),
                        Country = worksheet.Cells[row, 9].Value.ToString(),
                        City = worksheet.Cells[row, 10].Value.ToString(),
                        State = worksheet.Cells[row, 11].Value.ToString(),
                        PostalCode = worksheet.Cells[row, 12].Value.ToString(),
                        Region = worksheet.Cells[row, 13].Value.ToString(),
                        ProductId = int.Parse(worksheet.Cells[row, 14].Value.ToString()),
                        Category = worksheet.Cells[row, 15].Value.ToString(),
                        ProductName = worksheet.Cells[row, 17].Value.ToString(),
                        Sales = decimal.Parse(worksheet.Cells[row, 18].Value.ToString()),
                        Quantity = int.Parse(worksheet.Cells[row, 19].Value.ToString()),
                        Discount = decimal.Parse(worksheet.Cells[row, 20].Value.ToString()),
                        Profit = decimal.Parse(worksheet.Cells[row, 21].Value.ToString())
                    };

                    salesOrders.Add(order);
                }
            }

        }


        public async Task TransformData(List<object> extractedData) 
        {

            var transformedData = new List<object>();
            foreach (var item in extractedData)
            {
                var transformedItem = TransformItem(item); 
            }

        }

        public async Task LoadData(List<SalesOrder> transformedData) // Accepts transformed data
        {
            var destinyConnectionString = configuration.GetSection("DestinyConnection").Value;

            using (var context = new DestinyDBContext(new DbContextOptionsBuilder<DestinyDBContext>()
                                                .UseSqlServer(destinyConnectionString)
                                                .Options))
            {
                await context.SalesOrders.AddRangeAsync(transformedData);
                await context.SaveChangesAsync();

                logger.LogInformation($"Loaded {transformedData.Count} records to destination.");
            }
        }

        private object TransformItem(object item)
        {

            if (item is SalesOrder salesOrder)
            {

                salesOrder.Country = salesOrder.Country;

            }
            return item;
        }
    }

}
