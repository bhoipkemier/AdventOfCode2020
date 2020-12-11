using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day11 : DayCodeBase
	{
		public override string Problem1()
		{
			var grid = GetData()
				.Select(l => l.ToCharArray())
				.ToArray();
			var generation = 0;
			for (var changeMade = true; changeMade; ++generation)
			{
				(changeMade, grid) = Generate(grid);
			}
			return grid.Sum(r => r.Count(c => c == '#')).ToString();
		}

		private (bool changeMade, char[][] grid) Generate(char[][] reference)
		{
			var toReturn = reference.Select(r => r.Select(c => c).ToArray()).ToArray();
			var changeMade = false;
			for (var y = 0; y < toReturn.Length; ++y)
			{
				for (var x = 0; x < toReturn[y].Length; ++x)
				{
					changeMade = Generate(reference, toReturn, y, x) || changeMade;
				}
			}
			return (changeMade, toReturn);
		}

		private bool Generate(char[][] reference, char[][] newGrid, int y, int x)
		{
			var seat = reference[y][x];
			if (seat == '.') return false;
			var occupiedCount = GetOccupiedCount(reference, y, x);
			if (seat == 'L' && occupiedCount == 0) newGrid[y][x] = '#';
			if (seat == '#' && occupiedCount >= 4) newGrid[y][x] = 'L';
			return reference[y][x] != newGrid[y][x];
		}

		private int GetOccupiedCount(char[][] reference, int y, int x)
		{
			return
				(IsOccupied(reference, y - 1, x - 1) ? 1 : 0) +
				(IsOccupied(reference, y - 1, x) ? 1 : 0) +
				(IsOccupied(reference, y - 1, x + 1) ? 1 : 0) +
				(IsOccupied(reference, y, x - 1) ? 1 : 0) +
				(IsOccupied(reference, y, x + 1) ? 1 : 0) +
				(IsOccupied(reference, y + 1, x - 1) ? 1 : 0) +
				(IsOccupied(reference, y + 1, x) ? 1 : 0) +
				(IsOccupied(reference, y + 1, x + 1) ? 1 : 0);
		}

		private bool IsOccupied(char[][] reference, int y, int x)
		{
			if (y < 0 || x < 0 || y >= reference.Length || x >= reference[y].Length) return false;
			return reference[y][x] == '#';
		}

		public override string Problem2()
		{
			var grid = GetData()
				.Select(l => l.ToCharArray())
				.ToArray();
			var generation = 0;
			for (var changeMade = true; changeMade; ++generation)
			{
				(changeMade, grid) = Generate2(grid);
			}
			return grid.Sum(r => r.Count(c => c == '#')).ToString();
		}

		private (bool changeMade, char[][] grid) Generate2(char[][] reference)
		{
			var toReturn = reference.Select(r => r.Select(c => c).ToArray()).ToArray();
			var changeMade = false;
			for (var y = 0; y < toReturn.Length; ++y)
			{
				for (var x = 0; x < toReturn[y].Length; ++x)
				{
					changeMade = Generate2(reference, toReturn, y, x) || changeMade;
				}
			}
			return (changeMade, toReturn);
		}

		private bool Generate2(char[][] reference, char[][] newGrid, int y, int x)
		{
			var seat = reference[y][x];
			if (seat == '.') return false;
			var occupiedCount = GetOccupiedCount2(reference, y, x);
			if (seat == 'L' && occupiedCount == 0) newGrid[y][x] = '#';
			if (seat == '#' && occupiedCount >= 5) newGrid[y][x] = 'L';
			return reference[y][x] != newGrid[y][x];
		}

		private int GetOccupiedCount2(char[][] reference, int y, int x)
		{
			return
				(IsOccupied2(reference, y, x,  -1,  -1) ? 1 : 0) +
				(IsOccupied2(reference, y, x,  -1, 0) ? 1 : 0) +
				(IsOccupied2(reference, y, x,  -1, +1) ? 1 : 0) +
				(IsOccupied2(reference, y, x, 0, -1) ? 1 : 0) +
				(IsOccupied2(reference, y, x, 0, 1) ? 1 : 0) +
				(IsOccupied2(reference, y, x, 1, -1) ? 1 : 0) +
				(IsOccupied2(reference, y, x, 1, 0) ? 1 : 0) +
				(IsOccupied2(reference, y, x, 1, 1) ? 1 : 0);
		}

		private bool IsOccupied2(char[][] reference, int y, int x, int offsetY, int offsetX)
		{
			if (y + offsetY < 0 || x + offsetX < 0 || y + offsetY >= reference.Length || x + offsetX >= reference[y + offsetY].Length) return false;
			if (reference[y + offsetY][x + offsetX] == '.') return IsOccupied2(reference, y + offsetY, x + offsetX, offsetY, offsetX);
			return reference[y + offsetY][x + offsetX] == '#';
		}
	}
}
