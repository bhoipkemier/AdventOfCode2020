using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public abstract class DayCodeBase
	{
		public string[] GetData(int fileCount = 0, string splitChars = "\n")
		{
			var filename = $"Data/{GetType().Name}_{fileCount}.txt";
			return File
				.ReadAllText(filename)
				.Split(splitChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
				.Select(s => s.Replace("\r", string.Empty))
				.ToArray();
		}

		public Dictionary<long, long> GetProgram(int file = 0)
		{
			var data = GetData(file, ",");
			var toReturn = new Dictionary<long, long>();
			for (var l = 0L; l < data.Length; ++l)
			{
				toReturn[l] = long.Parse(data[(int)l]);
			}
			return toReturn;
		}

		public virtual string Problem1() => string.Empty;

		public virtual string Problem2() => string.Empty;
	}
}
