
namespace FCM.Types
{
	public class Customer : IRecordType
	{
		public string CNPJ { get; set; }
		public string Name { get; set; }
		public string BusinessArea { get; set; }
	}
}
