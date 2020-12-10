using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day10 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData(2)
				.Select(ulong.Parse)
				.OrderBy(l => l)
				.ToList();
			data.Insert(0, 0);
			var groups = data
				.Select((num, index) => index == data.Count - 1 ? 3 : data[index + 1] - num)
				.GroupBy(l => l)
				.ToList();
			return (groups.First(g => g.Key == 1).Count() * groups.First(g => g.Key == 3).Count()).ToString();
		}

		public override string Problem2()
		{
			var data = GetData(2)
				.Select(ulong.Parse)
				.OrderBy(l => l)
				.Select(num => (num, possibilities: (ulong?)null))
				.ToList();
			data.Insert(0, (0, 1));
			for (var i = 1; i < data.Count; ++i)
			{
				var possibilities =
					((data[i - 1].num + 3 >= data[i].num) ? data[i - 1].possibilities : 0) +
					((i - 2 >= 0) && (data[i - 2].num + 3 >= data[i].num) ? data[i - 2].possibilities : 0) +
					((i - 3 >= 0) && (data[i - 3].num + 3 >= data[i].num) ? data[i - 3].possibilities : 0);
				data[i] = (data[i].num, possibilities);
			}
			return data.Last().possibilities.ToString();
		}
	}
}
