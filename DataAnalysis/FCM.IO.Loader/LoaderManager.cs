using System.Collections.Generic;
using System.IO;

namespace FCM.IO.Loader
{

	public class ImportedFile
	{
	}

	public class LoaderManager
	{
		private string sourcePath;
		public List<ImportedFile> ArquivosImportados { get; set; }


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


		public void LoadFiles()
		{
			foreach (string file in FindFileList())
			{

			}
		}

	}
}
