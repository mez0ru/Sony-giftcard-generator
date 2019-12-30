using System.Collections.Generic;
using System.IO;

namespace Sony_giftcard_generator
{
	public static class LogWriter
	{
		public static void WriteLog(string[] Lines)
		{
			StreamWriter streamWriter = new StreamWriter("log.txt", append: false);
			foreach (string value in Lines)
			{
				streamWriter.WriteLine(value);
			}
			streamWriter.Close();
			streamWriter.Dispose();
		}

		public static string[] ReadLogs()
		{
			List<string> list = new List<string>();
			if (File.Exists("log.txt"))
			{
				StreamReader streamReader = new StreamReader("log.txt");
				while (!streamReader.EndOfStream)
				{
					list.Add(streamReader.ReadLine());
				}
				streamReader.Close();
				streamReader.Dispose();
			}
			return list.ToArray();
		}
	}
}
