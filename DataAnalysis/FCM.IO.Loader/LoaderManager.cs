using FCM.Types;
//using FCM.Types;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FCM.IO.Loader
{
	public class LoaderManager
	{
		private string sourcePath;
		private char fieldSeparetor;
		private char itemSeparator;
		private char itemDataSeparator;

		public LoaderManager(string sourcePath, char fieldSeparator = 'ç', char itemSeparator = ',', char itemDataSeparator = '-')
		{
			this.sourcePath = sourcePath;
			this.fieldSeparetor = fieldSeparator;
			this.itemSeparator = itemSeparator;
			this.itemDataSeparator = itemDataSeparator;
		}

		private string[] FindFileList()
		{
			if (Directory.Exists(sourcePath))
				return Directory.GetFiles(sourcePath, "*.dat");
			else
				throw new DirectoryNotFoundException();
		}

		/// <summary>
		/// Load all files to import
		/// </summary>
		/// <returns>List of FlatFile</returns>
		public List<FlatFile> LoadFiles()
		{
			List<FlatFile> files = new List<FlatFile>();
			foreach (string file in FindFileList())
			{
				FlatFile flatFile = new FlatFile();
				flatFile.Salesmen = new List<Salesman>();
				flatFile.Customers = new List<Customer>();
				flatFile.Sales = new List<Sale>();
				flatFile.FileName = Path.GetFileNameWithoutExtension(file);

				foreach (string line in File.ReadAllLines(file, Encoding.UTF8))
				{
					switch (line.Substring(0, 3))
					{
						case "001":
							flatFile.Salesmen.Add(ProcessSalesmanData(line.Split(fieldSeparetor)));
							break;
						case "002":
							flatFile.Customers.Add(ProcessCustomerData(line.Split(fieldSeparetor)));
							break;
						case "003":
							flatFile.Sales.Add(ProcessSaleData(line.Split(fieldSeparetor)));
							break;
						default: break;
					}
				}
				files.Add(flatFile);
			}
			return files;
		}
		/// <summary>
		/// Process Salesman data information
		/// </summary>
		/// <param name="rawData">raw data of Salesman</param>
		/// <returns>Salesman data</returns>
		private Salesman ProcessSalesmanData(string[] rawData)
		{
			Salesman resultData = new Salesman();
			resultData.CPF = rawData[1];
			resultData.Name = rawData[2];

			return resultData;
		}
		/// <summary>
		/// Process Customer data information
		/// </summary>
		/// <param name="rawData">raw data of Customer</param>
		/// <returns>Customer data</returns>
		private Customer ProcessCustomerData(string[] rawData)
		{
			Customer resultData = new Customer();
			resultData.CNPJ = rawData[1];
			resultData.Name = rawData[2];
			resultData.BusinessArea = rawData[3];

			return resultData;
		}
		/// <summary>
		/// Process Customer data information
		/// </summary>
		/// <param name="rawData">raw data of Sale</param>
		/// <returns>Sale data</returns>
		private Sale ProcessSaleData(string[] rawData)
		{
			Sale resultData = new Sale();
			resultData.ID = int.Parse(rawData[1]);
			resultData.Items = ProcessSaleItemData(rawData[2]);
			resultData.SalesmanName = rawData[3];

			return resultData;
		}
		/// <summary>
		/// Process Item data information
		/// </summary>
		/// <param name="rawData">raw data of Item</param>
		/// <returns>Item data</returns>
		private List<Item> ProcessSaleItemData(string rawData)
		{
			//003çSale IDç[Item ID-Item Quantity-Item Price]çSalesman name
			//[1-34-10,2-33-1.50,3-40-0.10]

			string[] listItemData = Regex.Replace(rawData, @"[^\-\,\.0-9]{1,}", string.Empty).Split(this.itemSeparator);
			List<Item> resultData = new List<Item>();
			foreach (string itemData in listItemData)
			{
				string[] filteredItem = itemData.Split(this.itemDataSeparator);
				Item item = new Item();
				item.Id = int.Parse(filteredItem[0]);
				item.Quantity = float.Parse(filteredItem[1]);
				item.Price = decimal.Parse(filteredItem[2], NumberStyles.Any, CultureInfo.InvariantCulture);
				resultData.Add(item);
			}
			return resultData;
		}
	}
}
