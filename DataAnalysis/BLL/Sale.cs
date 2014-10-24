using System.Collections.Generic;

namespace FCM.Types
{
	public class Sale : IRecordType
	{
		public int ID { get; set; }
		public List<Item> Items { get; set; }
		
	}
}
