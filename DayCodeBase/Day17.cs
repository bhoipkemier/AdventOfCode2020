using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day17 : DayCodeBase
	{
		public override string Problem1()
		{
			var grid = ParseData(GetData());
			for (var i = 0; i < 6; ++i)
			{
				grid = DoCycle(grid);
			}
			return grid.Count.ToString();
		}
		public override string Problem2()
		{
			var grid = ParseData2(GetData());
			for (var i = 0; i < 6; ++i)
			{
				grid = DoCycle2(grid);
			}
			return grid.Count.ToString();
		}

		private void Print(HashSet<Tuple<int, int, int>> grid)
		{
			var ranges = GetRange(grid);
			for (var z = ranges.Item3.Item1; z <= ranges.Item3.Item2; ++z)
			{
				Console.WriteLine($"\r\nz={z}");
				for (var y = ranges.Item2.Item1; y <= ranges.Item2.Item2; ++y)
				{
					for (var x = ranges.Item1.Item1; x <= ranges.Item1.Item2; ++x)
					{
						Console.Write(grid.Contains(new Tuple<int, int, int>(x, y, z)) ? "#" : ".");
					}
					Console.WriteLine();
				}
			}
		}

		private HashSet<Tuple<int, int, int>> DoCycle(HashSet<Tuple<int, int, int>> grid)
		{
			var ranges = GetRange(grid);
			var toReturn = new HashSet<Tuple<int, int, int>>();
			for (var z = ranges.Item3.Item1 - 1; z <= ranges.Item3.Item2 + 1; ++z)
			{
				for (var y = ranges.Item2.Item1 - 1; y <= ranges.Item2.Item2 + 1; ++y)
				{
					for (var x = ranges.Item1.Item1 - 1; x <= ranges.Item1.Item2 + 1; ++x)
					{
						if (NextGenActive(grid, x, y, z)) toReturn.Add(new Tuple<int, int, int>(x, y, z));
					}
				}
			}
			return toReturn;
		}

		private HashSet<Tuple<int, int, int, int>> DoCycle2(HashSet<Tuple<int, int, int, int>> grid)
		{
			var ranges = GetRange2(grid);
			var toReturn = new HashSet<Tuple<int, int, int, int>>();

			for (var w = ranges.Item4.Item1 - 1; w <= ranges.Item4.Item2 + 1; ++w)
			{
				for (var z = ranges.Item3.Item1 - 1; z <= ranges.Item3.Item2 + 1; ++z)
				{
					for (var y = ranges.Item2.Item1 - 1; y <= ranges.Item2.Item2 + 1; ++y)
					{
						for (var x = ranges.Item1.Item1 - 1; x <= ranges.Item1.Item2 + 1; ++x)
						{
							if (NextGenActive2(grid, x, y, z, w)) toReturn.Add(new Tuple<int, int, int, int>(x, y, z, w));
						}
					}
				}
			}

			return toReturn;
		}

		private bool NextGenActive(HashSet<Tuple<int, int, int>> grid, int curX, int curY, int curZ)
		{
			var count = GetCount(grid, curX, curY, curZ);
			var curActive = grid.Contains(new Tuple<int, int, int>(curX, curY, curZ));
			return (curActive && (count == 2 || count == 3)) ||
				   (!curActive && count == 3);
		}

		private bool NextGenActive2(HashSet<Tuple<int, int, int, int>> grid, int curX, int curY, int curZ, int curW)
		{
			var count = GetCount2(grid, curX, curY, curZ, curW);
			var curActive = grid.Contains(new Tuple<int, int, int, int>(curX, curY, curZ, curW));
			return (curActive && (count == 2 || count == 3)) ||
				   (!curActive && count == 3);
		}

		private int GetCount(HashSet<Tuple<int, int, int>> grid, int curX, int curY, int curZ)
		{
			var toReturn = 0;
			for (var z = curZ - 1; z <= curZ + 1; ++z)
			{
				for (var y = curY - 1; y <= curY + 1; ++y)
				{
					for (var x = curX - 1; x <= curX + 1; ++x)
					{

						if ((x != curX || y != curY || z != curZ) && grid.Contains(new Tuple<int, int, int>(x, y, z))) toReturn++;
					}
				}
			}
			return toReturn;
		}

		private int GetCount2(HashSet<Tuple<int, int, int, int>> grid, int curX, int curY, int curZ, int curW)
		{
			var toReturn = 0;
			for (var w = curW - 1; w <= curW + 1; ++w)
			{
				for (var z = curZ - 1; z <= curZ + 1; ++z)
				{
					for (var y = curY - 1; y <= curY + 1; ++y)
					{
						for (var x = curX - 1; x <= curX + 1; ++x)
						{

							if ((x != curX || y != curY || z != curZ || w != curW) &&
							    grid.Contains(new Tuple<int, int, int, int>(x, y, z, w))) toReturn++;
						}
					}
				}
			}
			return toReturn;
		}

		private Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>> GetRange(HashSet<Tuple<int, int, int>> grid)
		{
			var minX = grid.Select(item => item.Item1).Min();
			var maxX = grid.Select(item => item.Item1).Max();
			var minY = grid.Select(item => item.Item2).Min();
			var maxY = grid.Select(item => item.Item2).Max();
			var minZ = grid.Select(item => item.Item3).Min();
			var maxZ = grid.Select(item => item.Item3).Max();
			return new Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(minX, maxX), new Tuple<int, int>(minY, maxY), new Tuple<int, int>(minZ, maxZ));
		}

		private Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>> GetRange2(HashSet<Tuple<int, int, int, int>> grid)
		{
			var minX = grid.Select(item => item.Item1).Min();
			var maxX = grid.Select(item => item.Item1).Max();
			var minY = grid.Select(item => item.Item2).Min();
			var maxY = grid.Select(item => item.Item2).Max();
			var minZ = grid.Select(item => item.Item3).Min();
			var maxZ = grid.Select(item => item.Item3).Max();
			var minW = grid.Select(item => item.Item4).Min();
			var maxW = grid.Select(item => item.Item4).Max();
			return new Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(minX, maxX), new Tuple<int, int>(minY, maxY), new Tuple<int, int>(minZ, maxZ), new Tuple<int, int>(minW, maxW));
		}

		private HashSet<Tuple<int, int, int>> ParseData(string[] data)
		{
			var toReturn = new HashSet<Tuple<int, int, int>>();
			var y = 0;
			foreach (var line in data)
			{
				for (var x = 0; x < line.Length; ++x)
				{
					if (line[x] == '#')
						toReturn.Add(new Tuple<int, int, int>(x, y, 0));
				}
				++y;
			}
			return toReturn;
		}

		private HashSet<Tuple<int, int, int, int>> ParseData2(string[] data)
		{
			var toReturn = new HashSet<Tuple<int, int, int, int>>();
			var y = 0;
			foreach (var line in data)
			{
				for (var x = 0; x < line.Length; ++x)
				{
					if (line[x] == '#')
						toReturn.Add(new Tuple<int, int, int, int>(x, y, 0, 0));
				}
				++y;
			}
			return toReturn;
		}
	}
}
