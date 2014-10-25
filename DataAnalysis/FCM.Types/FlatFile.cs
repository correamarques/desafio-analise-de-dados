using System.Collections.Generic;

namespace FCM.Types
{
	public class FlatFile
	{
		public string FileName { get; set; }
		public List<Salesman> Salesmen { get; set; }
		public List<Customer> Customers { get; set; }
		public List<Sale> Sales { get; set; }
	}
}
