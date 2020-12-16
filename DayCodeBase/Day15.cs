using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day15 : DayCodeBase
	{
		public override string Problem1()
		{
			var answer = GetAnswer(2020);
			return answer.ToString();
		}

		public override string Problem2()
		{
			return GetAnswer(30000000).ToString();
		}

		public ulong GetAnswer(ulong index)
		{
			var dict = new Dictionary<ulong, ulong>
			{
				{1, 1},
				{12, 2},
				{0, 3},
				{20, 4},
				{8, 5}
			};
			var toAdd = 16ul;
			for (var i = (ulong)(dict.Count + 1); i < index; ++i)
			{
				if (dict.ContainsKey(toAdd))
				{
					var lastIndex = dict[toAdd];
					dict[toAdd] = i;
					toAdd = i - lastIndex;
				}
				else
				{
					dict[toAdd] = i;
					toAdd = 0;
				}
			}
			return toAdd;
		}
	}
}
