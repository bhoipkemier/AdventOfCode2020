using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Location = System.Tuple<int, int>;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day24 : DayCodeBase
	{
		public override string Problem1()
		{
			var blackTiles = GetBlackTiles();
			return blackTiles.Count.ToString();
		}

		public HashSet<Location> GetBlackTiles()
		{
			var regex = new Regex(@"(?<direction>e|se|sw|w|nw|ne)+");
			var blackTiles = new HashSet<Location>();
			foreach (var directions in GetData().Select(l => regex.Match(l)))
			{
				var location = new Location(0, 0);
				foreach (var direction in directions.Groups["direction"].Captures.Select(c => c.Value))
				{
					location = GetNewLocation(location, direction);
				}

				if (blackTiles.Contains(location))
				{
					blackTiles.Remove(location);
				}
				else
				{
					blackTiles.Add(location);
				}
			}
			return blackTiles;
		}

		public override string Problem2()
		{
			var blackTiles = GetBlackTiles();
			for (var i = 0; i < 100; ++i)
			{
				blackTiles = ApplyDay(blackTiles);
			}
			return blackTiles.Count.ToString();
		}

		private HashSet<Location> ApplyDay(HashSet<Location> blackTiles)
		{
			var toReturn = new HashSet<Location>();
			foreach (var blackTile in blackTiles)
			{
				var numAdjacentBlack = GetAdjacent(blackTile).Count(blackTiles.Contains);
				if (numAdjacentBlack == 1 || numAdjacentBlack == 2) toReturn.Add(blackTile);
			}

			foreach (var whiteTile in blackTiles.SelectMany(bt => GetAdjacent(bt).Where(adj => !blackTiles.Contains(adj))))
			{
				var numAdjacentBlack = GetAdjacent(whiteTile).Count(blackTiles.Contains);
				if (numAdjacentBlack == 2) toReturn.Add(whiteTile);
			}

			return toReturn;
		}

		private Location GetNewLocation(Location location, string direction)
		{
			var isOdd = Math.Abs(location.Item2) % 2 == 1;
			switch (direction) 
			{
				case "e": return new Location(location.Item1 + 1, location.Item2);
				case "se": return new Location(isOdd ? location.Item1: location.Item1 + 1, location.Item2 - 1);
				case "sw": return new Location(isOdd ? location.Item1 - 1: location.Item1, location.Item2 - 1);
				case "w": return new Location(location.Item1 - 1, location.Item2);
				case "ne": return new Location(isOdd ? location.Item1: location.Item1 + 1, location.Item2 + 1);
				case "nw": return new Location(isOdd ? location.Item1 - 1: location.Item1, location.Item2 + 1);
			}
			throw new NotImplementedException();
		}

		private Location[] GetAdjacent(Location location)
		{
			return new[]
			{
				GetNewLocation(location, "e"),
				GetNewLocation(location, "se"),
				GetNewLocation(location, "sw"),
				GetNewLocation(location, "w"),
				GetNewLocation(location, "nw"),
				GetNewLocation(location, "ne")
			};
		}
	}
}
