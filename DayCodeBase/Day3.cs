using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day3: DayCodeBase
	{
		public override string Problem1()
		{
			var input = GetData(1).Select(s => s.ToCharArray()).ToList();
			var trees = GeneralSolution(input, 3, 1);
			return trees.ToString();
		}
		public override string Problem2()
		{
			var input = GetData(1).Select(s => s.ToCharArray()).ToList();
			var trees = new long[]{
				GeneralSolution(input, 1, 1),
				GeneralSolution(input, 3, 1),
				GeneralSolution(input, 5, 1),
				GeneralSolution(input, 7, 1),
				GeneralSolution(input, 1, 2)
			};
			return trees.Aggregate((long)1, (acc, count) => acc * count).ToString();
		}

		private int GeneralSolution(List<char[]> input, int over, int down)
		{
			var location = 0;
			var trees = 0;
			for(var i = down; i < input.Count; i += down)
			{
				var row = input[i];
				location += over;
				if (location >= row.Length) location -= row.Length;
				if (row[location] == '#') trees++;
			}
			return trees;
		}
	}
}
