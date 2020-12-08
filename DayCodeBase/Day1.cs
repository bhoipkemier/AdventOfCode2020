using System;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day1: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.Select(int.Parse)
				.ToArray();
			for (var i = 0; i < data.Length; ++i)
			{
				for (var j = 0; j < data.Length; ++j)
				{
					if (i != j && data[i] + data[j] == 2020)
					{
						return (data[i] * data[j]).ToString();
					}
				}
			}
			return "error";
		}


		public override string Problem2()
		{
			var data = GetData()
				.Select(int.Parse)
				.ToArray();
			for (var i = 0; i < data.Length; ++i)
			{
				for (var j = 0; j < data.Length; ++j)
				{
					for (var k = 0; k < data.Length; ++k)
					{
						if (i != j && i != k && j != k && data[i] + data[j] + data[k] == 2020)
						{
							return (data[i] * data[j] * data[k]).ToString();
						}
					}
				}
			}
			return "error";
		}
	}
}
