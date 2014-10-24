using FCM.Types;
//using FCM.Types;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FCM.IO.Loader
{
		public class LoaderManager
		{
			private string sourcePath;

			public LoaderManager(string sourcePath)
			{
				this.sourcePath = sourcePath;
			}

			private string[] FindFileList()
			{
				if (Directory.Exists(sourcePath))
				{
					return Directory.GetFiles(sourcePath);
				}
				else
					throw new DirectoryNotFoundException();
			}

			/// <summary>
			/// Load all files to import
			/// </summary>
			public void LoadFiles()
			{
				foreach (string file in FindFileList())
				{
					foreach (string line in File.ReadAllLines(file, Encoding.UTF8))
					{
						switch (line.Substring(0, 3))
						{
							case "001": ProcessSalesmanData(line.Split(fieldSeparetor)); break;
							case "002": ProcessCustomerData(line.Split(fieldSeparetor)); break;
							case "003": ProcessSaleData(line.Split(fieldSeparetor)); break;
							default:
								break;
						}

					}
				}
			}

			private char fieldSeparetor = 'ç';

			private Salesman ProcessSalesmanData(string[] rawData)
			{
				Salesman resultData = new Salesman();
				resultData.CPF = rawData[1];
				resultData.Name = rawData[2];

				return resultData;
			}

			private Customer ProcessCustomerData(string[] rawData)
			{
				Customer resultData = new Customer();
				resultData.CNPJ = rawData[1];
				resultData.Name = rawData[2];
				resultData.BusinessArea = rawData[3];

				return resultData;
			}
			private Sale ProcessSaleData(string[] rawData)
			{
				Sale resultData = new Sale();
				resultData.ID = int.Parse(rawData[1]);
				resultData.Items = ProcessSaleItemData(rawData[2]);
				resultData.SalesmanName = rawData[3];

				return resultData;
			}

			private List<Item> ProcessSaleItemData(string rawData)
			{
				//003çSale IDç[Item ID-Item Quantity-Item Price]çSalesman name
				//[1-34-10,2-33-1.50,3-40-0.10]
				List<Item> resultData = new List<Item>();
				string teste = Regex.Replace(rawData, @"[^\-\,\.0-9]{1,}", string.Empty);
				rawData.Replace("[", "").Replace("]", "");



				return resultData;
			}
		}
	}
