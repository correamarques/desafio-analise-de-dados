using FCM.Types;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FCM.IO.Exporter
{
	public class ExporterManager
	{
		private string sourcePath;
		private char fieldSeparator;
		public ExporterManager(string sourcePath, char fieldSeparator = 'ç')
		{
			this.sourcePath = sourcePath;
			this.fieldSeparator = fieldSeparator;
		}
		public void Save(IList<FileReport> reportList)
		{
			foreach (FileReport report in reportList)
			{
				string path = Path.Combine(sourcePath, string.Format("{0}.done.dat", report.FileName));
				File.AppendAllText(path, GetContentOfReport(report), Encoding.UTF8);
			}
		}

		string GetContentOfReport(FileReport report)
		{
			StringBuilder reportContent = new StringBuilder();
			reportContent.AppendFormat("{0}{1}", report.NumberOfCustomers, this.fieldSeparator);
			reportContent.AppendFormat("{0}{1}", report.NumberOfSalesman, this.fieldSeparator);
			reportContent.AppendFormat("{0}{1}", report.IdMostExpensiveSale, this.fieldSeparator);
			reportContent.AppendFormat("{0}", report.WorstSalesmanEver);
			return reportContent.ToString();
		}
	}
}
