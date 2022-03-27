using System;
using System.IO;
using System.Collections.Generic;

public static class DotEnv
{
    public static Dictionary<string, string> Load(string file_path)
    {	
        if (!File.Exists(file_path))
        {
            Console.WriteLine("Couldn't find file");
            return null;
        }

        Dictionary<string, string> sql_settings = new Dictionary<string, string>();

        foreach (var line in File.ReadAllLines(file_path))
        {
            var parts = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                continue;

            sql_settings[parts[0]]= parts[1];
        }

        return sql_settings;
    }
}
