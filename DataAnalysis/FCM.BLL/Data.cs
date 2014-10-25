using FCM.Types;
using System.Collections.Generic;
using System.Linq;

namespace FCM.BLL
{
    public class Data
    {
		public List<FileReport> PerformAnalysis(List<FlatFile> flatFiles)
		{
			List<FileReport> reportList = new List<FileReport>();
			foreach (FlatFile flatFile in flatFiles)
			{
				FileReport report = new FileReport();
				report.FileName = flatFile.FileName;
				report.NumberOfCustomers = flatFile.Customers.Count;
				report.NumberOfSalesman = flatFile.Salesmen.Count;
				report.IdMostExpensiveSale = GetIdMostExpensiveSale(flatFile.Sales);
				report.WorstSalesmanEver = GetWorstSalesmanEver(flatFile.Sales);
				reportList.Add(report);
			}

			return reportList;
		}

		private int GetIdMostExpensiveSale(List<Sale> saleList)
		{
			decimal price = 0m;
			int id = 0;
			foreach (Sale sale in saleList)
			{
				foreach (Item item in sale.Items)
				{
					if (item.Price > price)
					{
						price = item.Price;
						id = item.Id;
					}
				}
			}
			
			return id;
		}
		private string GetWorstSalesmanEver(List<Sale> saleList)
		{
			string salesmanName = string.Empty;
			decimal? amount = null;
			foreach (Sale sale in saleList)
			{
				decimal prices = sale.Items.Sum(w => w.Price);
				if (!amount.HasValue || prices < amount)
				{
					amount = prices;
					salesmanName = sale.SalesmanName;
				}
			}
			return salesmanName;
		}
    }
}
