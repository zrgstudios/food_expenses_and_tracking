using System;
using System.IO;

public static class DotEnv
{
	public static void Load(string file_path)
	{	
		if (!File.Exists(file_path))
        {
			Console.WriteLine("Couldn't find file");
			return;
		}
			

		foreach (var line in File.ReadAllLines(file_path))
        {
			var parts = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
			
			if (parts.Length != 2)
				continue;

			Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
	}
}
