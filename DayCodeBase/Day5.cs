using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day5: DayCodeBase
	{
		public override string Problem1()
		{
			return GetData()
				.Select(input => input.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1"))
				.Select(binStr => Convert.ToInt32(binStr, 2))
				.Max()
				.ToString();
		}
		public override string Problem2()
		{
			var seats = GetData()
				.Select(input => input.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1"))
				.Select(binStr => Convert.ToInt32(binStr, 2))
				.OrderBy(s => s)
				.ToList();
			for(var i = 0; i < seats.Count - 1; ++i)
			{
				if (seats[i] + 2 == seats[i + 1]) return (seats[i] + 1).ToString();
			}
			return "error";
		}
	}
}
