using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day6 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData();
			var toReturn = 0;
			var set = new HashSet<char>();
			foreach (var line in data)
			{
				if (line.Trim().Length == 0)
				{
					toReturn += set.Count;
					set.Clear();
				}
				else
				{
					foreach (var c in line)
					{
						set.Add(c);
					}
				}
			};
			toReturn += set.Count;
			return toReturn.ToString();
		}

		public override string Problem2()
		{
			var toReturn = 0;
			var groups = GetGroups();
			foreach(var group in groups)
			{
				var set = new HashSet<char>(group[0].ToCharArray());
				foreach(var line in group)
				{
					set = new HashSet<char>(set.Intersect(line.ToCharArray()));
				}
				toReturn += set.Count();
			}
			return toReturn.ToString();
		}

		public List<List<string>> GetGroups()
		{
			var data = GetData();
			var toReturn = new List<List<string>>();
			var curGroup = new List<string>();
			foreach (var line in data)
			{
				if (line.Trim().Length == 0)
				{
					toReturn.Add(curGroup);
					curGroup = new List<string>();
				}
				else
				{
					curGroup.Add(line);
				}
			};
			toReturn.Add(curGroup);
			return toReturn;
		}
	}
}
