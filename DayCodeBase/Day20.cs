using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day20 : DayCodeBase
	{

		private static string[] _monster = new []
		{
			"                  # ",
			"#    ##    ##    ###",
			" #  #  #  #  #  #   "
		};
		
		public override string Problem1()
		{
			var pieces = LoadData(GetData());
			var grid = GetGrid(pieces);
			return (
				grid.First().First().PuzzlePiece.Id *
				grid.First().Last().PuzzlePiece.Id *
				grid.Last().First().PuzzlePiece.Id *
				grid.Last().Last().PuzzlePiece.Id
			).ToString();
		}
		
		public override string Problem2()
		{
			var pieces = LoadData(GetData());
			var grid = GetGrid(pieces);
			var image = GetRenderedImage(grid).ToArray();
			for (var orientation = 0; orientation < 8; ++orientation)
			{
				var translatedImage = PieceOrientation.TransformImage(image, orientation);
				var monsterLocations = GetMonsterLocations(translatedImage, _monster);
				if (monsterLocations.Any())
					return (translatedImage.Sum(line => line.ToCharArray().Count(c => c == '#')) - monsterLocations.Count)
						.ToString();
			}
			return string.Empty;
		}

		private HashSet<Tuple<int, int>> GetMonsterLocations(string[] translatedImage, string[] monster)
		{
			var toReturn = new HashSet<Tuple<int, int>>();
			for (var y = 0; y < translatedImage.Length - (monster.Length - 1); ++y)
			{
				for(var x = 0; x < translatedImage[0].Length - (monster[0].Length - 1); ++x)
				{
					MonsterAt(translatedImage, monster, y, x, toReturn);
				}
			}
			return toReturn;
		}

		private void MonsterAt(string[] translatedImage, string[] monster, int offsetY, int offsetX, HashSet<Tuple<int, int>> monsterLocations)
		{
			for (var y = 0; y < monster.Length; ++y)
			{
				for (var x = 0; x < monster[y].Length; ++x)
				{
					if (monster[y][x] == '#' && translatedImage[offsetY + y][offsetX + x] != '#')
						return;
				}
			}
			
			for (var y = 0; y < monster.Length; ++y)
			{
				for (var x = 0; x < monster[y].Length; ++x)
				{
					if (monster[y][x] == '#')
						monsterLocations.Add(new Tuple<int, int>(offsetY + y, offsetX + x));
				}
			}

		}

		private List<string> GetRenderedImage(List<List<PieceOrientation>> grid)
		{
			var toReturn = new List<string>();
			foreach (var imageRow in grid)
			{
				var imagesInRow = imageRow.Select(po => po.GetImage()).ToList();
				for (var y = 0; y < imagesInRow[0].Length; ++y)
				{
					toReturn.Add(string.Concat(imagesInRow.Select(image => image[y])));
				}
			}
			return toReturn;
		}

		private List<List<PieceOrientation>> GetGrid(List<PuzzlePiece> pieces)
		{
			var lookup = GetLookup(pieces);
			var lengthOfGrid = (int)Math.Sqrt(pieces.Count);
			var piecesUsed = new HashSet<PuzzlePiece>();
			var configuration = Enumerable.Range(0, lengthOfGrid).Select(row =>
					Enumerable.Range(0, lengthOfGrid)
						.Select(col => (PieceOrientation)null)
						.ToList())
				.ToList();
			var allSides = pieces.SelectMany(p => p.AllSides).ToArray();
			foreach (var topRestriction in allSides)
			{
				var solved = Solve(lookup, piecesUsed, configuration, topRestriction);
				if (solved)
				{
					return configuration;
				}
			}
			return null;
		}

		private bool Solve(Dictionary<int, List<PuzzlePiece>> lookup, HashSet<PuzzlePiece> piecesUsed, List<List<PieceOrientation>> grid, int? topRestriction, int? leftRestriction = null)
		{
			var possiblePieces =
				topRestriction == null ? lookup[(int)leftRestriction].Except(piecesUsed).Distinct() :
				leftRestriction == null ? lookup[(int)topRestriction].Except(piecesUsed).Distinct() :
				lookup[(int)topRestriction].Intersect(lookup[(int)leftRestriction]).Except(piecesUsed).Distinct();
			foreach (var possiblePiece in possiblePieces)
			{
				foreach (var permutation in possiblePiece.Permutations.Where(p => (topRestriction == null || p[0] == topRestriction) && (leftRestriction == null || p[3] == leftRestriction)))
				{
					var (x, y) = GetNextLocation(grid);
					piecesUsed.Add(possiblePiece);
					grid[y][x] = new PieceOrientation
					{
						Orientation = permutation[4],
						PuzzlePiece = possiblePiece
					};
					var (nextX, nextY) = GetNextLocation(grid);
					if (nextX == int.MinValue && nextY == int.MinValue) return true;
					var pieceAboveNext = nextY == 0 ? null : grid[nextY - 1][nextX];
					var pieceToLeft = nextX == 0 ? null : grid[nextY][nextX-1];
					var nextSolved = Solve(lookup, piecesUsed, grid, pieceAboveNext?.GetBottom(), pieceToLeft?.GetRight());
					if (nextSolved)
					{
						return true;
					}
					else
					{
						piecesUsed.Remove(possiblePiece);
						grid[y][x] = null;
					}
				}
			}
			return false;
		}

		private (int x, int y) GetNextLocation(List<List<PieceOrientation>> configuration)
		{
			for(var y = 0; y < configuration.Count; ++y)
			{
				for (var x = 0; x < configuration[y].Count; ++x)
				{
					if (configuration[y][x] == null) return (x, y);
				}
			}
			return (int.MinValue, int.MinValue);
		}

		private Dictionary<int, List<PuzzlePiece>> GetLookup(List<PuzzlePiece> pieces)
		{
			var toReturn = new Dictionary<int, List<PuzzlePiece>>();
			foreach (var puzzlePiece in pieces)
			{
				foreach (var side in puzzlePiece.AllSides.Distinct())
				{
					if(!toReturn.ContainsKey(side)) toReturn[side] = new List<PuzzlePiece>();
					toReturn[side].Add(puzzlePiece);
				}
			}
			return toReturn;
		}

		private List<PuzzlePiece> LoadData(string[] data)
		{
			var toReturn = new List<PuzzlePiece>();
			var pieceData = data;
			for (var i = 0; pieceData.Any(); ++i)
			{
				pieceData = data
					.Skip(i * 12)
					.Take(11)
					.ToArray();
				if(pieceData.Length > 0) toReturn.Add(new PuzzlePiece(pieceData));
			}
			return toReturn;
		}

		public class PieceOrientation
		{
			public PuzzlePiece PuzzlePiece { get; set; }
			public int Orientation { get; set; }

			public int GetBottom()
			{
				return PuzzlePiece.Permutations[Orientation][2];
			}

			public int GetRight()
			{
				return PuzzlePiece.Permutations[Orientation][1];
			}

			public static string[] TransformImage(string[] image, int orientation)
			{
				if (orientation == 0) return image;
				if (orientation == 1) return Rotate(image);
				if (orientation == 2) return Rotate(Rotate(image));
				if (orientation == 3) return Rotate(Rotate(Rotate(image)));
				if (orientation == 4) return Flip(image);
				if (orientation == 5) return Rotate(Flip(image));
				if (orientation == 6) return Rotate(Rotate(Flip(image)));
				if (orientation == 7) return Rotate(Rotate(Rotate(Flip(image))));
				throw new NotImplementedException();
			}

			public string[] GetImage() => TransformImage(PuzzlePiece.Image, Orientation);

			private static string[] Flip(string[] data)
			{
				return data.Reverse().ToArray();
			}

			public static string[] Rotate(string[] data)
			{
				var toReturn = new List<string>();
				for (var x = 0; x < data.Length; ++x)
				{
					var newRow = new StringBuilder();
					for (var y = data.Length - 1; y >= 0; --y)
					{
						newRow.Append(data[y][x]);
					}
					toReturn.Add(newRow.ToString());
				}
				return toReturn.ToArray();
			}
		}


		public class PuzzlePiece
		{
			public ulong Id { get; set; }
			public int Top { get; set; }
			public int Right { get; set; }
			public int Bottom { get; set; }
			public int Left { get; set; }
			public int InvTop { get; set; }
			public int InvRight { get; set; }
			public int InvBottom { get; set; }
			public int InvLeft { get; set; }

			public string[] Image { get; }


			public IEnumerable<int> AllSides
			{
				get
				{
					return new[]
					{
						Top,
						Right,
						Bottom,
						Left,
						InvTop,
						InvRight,
						InvBottom,
						InvLeft
					};
				}
			}

			public int[][] Permutations
			{
				get
				{
					return new[]
					{
						new[] { Top, Right, Bottom, Left, 0 }, // Starting Pos
						new[] { InvLeft, Top, InvRight, Bottom, 1 }, // Rotate Clockwise Once
						new[] { InvBottom, InvLeft, InvTop, InvRight, 2 }, // Rotate Clockwise Twice
						new[] { Right, InvBottom, Left, InvTop, 3 }, // Rotate Clockwise Three Times
						new[] { Bottom, InvRight, Top, InvLeft, 4 }, // Flip Vert
						new[] { Left, Bottom, Right, Top, 5 }, // Flip Vert Rotate Clockwise Once
						new[] { InvTop, Left, InvBottom, Right, 6 }, // Flip Vert Rotate Clockwise Twice
						new[] { InvRight, InvTop, InvLeft, InvBottom, 7 }, // Flip Vert Rotate Clockwise Three Times
					};
				}
			}

			public PuzzlePiece(string[] data)
			{
				Debug.Assert(data.Length == 11);
				Id = ulong.Parse(data[0].Split(' ')[1].Trim(' ', ':'));
				Top = GetInt(data[1]);
				InvTop = GetInt(data[1].Reverse());
				Right = GetInt(data.Skip(1).Select(line => line.Last()));
				InvRight = GetInt(data.Skip(1).Select(line => line.Last()).Reverse());
				Bottom = GetInt(data[10]);
				InvBottom = GetInt(data[10].Reverse());
				Left = GetInt(data.Skip(1).Select(line => line.First()));
				InvLeft = GetInt(data.Skip(1).Select(line => line.First()).Reverse());
				Image = data.Skip(2).Take(8).Select(l => l.Substring(1, 8)).ToArray();
			}

			private int GetInt(IEnumerable<char> pixels)
			{
				var str = new string(pixels.ToArray())
					.Replace('#', '1')
					.Replace('.', '0');
				return Convert.ToInt32(str, 2);
			}
		}
	}
}
