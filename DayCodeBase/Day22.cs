using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day22 : DayCodeBase
	{
		public override string Problem1()
		{
			var (player1, player2) = GetDecks(GetData());
			while (player1.Any() && player2.Any())
			{
				if (player1.Peek() > player2.Peek())
				{
					player1.Enqueue(player1.Dequeue());
					player1.Enqueue(player2.Dequeue());
				}
				else
				{
					player2.Enqueue(player2.Dequeue());
					player2.Enqueue(player1.Dequeue());
				}
			}

			return GetWinningScore(player1, player2);
		}

		public override string Problem2()
		{
			var (player1, player2) = GetDecks(GetData());
			var player1IsTheWinner = PlayGame(player1, player2, new Dictionary<string, bool>());
			return (player1IsTheWinner ? player1 : player2)
				.Reverse()
				.Select((cardNum, index) => cardNum * (index + 1))
				.Sum()
				.ToString();
		}

		private bool PlayGame(Queue<int> player1, Queue<int> player2, Dictionary<string, bool> prevWinner)
		{
			var previousConfigurations = new HashSet<string>();
			while (player1.Any() && player2.Any())
			{
				var currentConfig = GetConfiguration(player1, player2);
				if (previousConfigurations.Contains(currentConfig))
				{
					return true;
				}
				previousConfigurations.Add(currentConfig);
				
				var p1Card = player1.Dequeue();
				var p2Card = player2.Dequeue();
				bool p1WinsRound;

				if (p1Card <= player1.Count && p2Card <= player2.Count)
				{
					p1WinsRound = PlayGame(new Queue<int>(player1.Take(p1Card)), new Queue<int>(player2.Take(p2Card)), prevWinner);
				}
				else
				{
					p1WinsRound = p1Card > p2Card;
				}
				
				if (p1WinsRound)
				{
					player1.Enqueue(p1Card);
					player1.Enqueue(p2Card);
				}
				else
				{
					player2.Enqueue(p2Card);
					player2.Enqueue(p1Card);
				}
			}

			var toReturn = player1.Any();
			return toReturn;
		}

		private string GetConfiguration(Queue<int> player1, Queue<int> player2)
		{
			return $"{string.Join(',', player1)}:{string.Join(',', player2)}";
		}

		private string GetWinningScore(Queue<int> player1, Queue<int> player2)
		{
			return player1
				.Union(player2)
				.Reverse()
				.Select((cardNum, index) => cardNum * (index + 1))
				.Sum()
				.ToString();
		}

		private (Queue<int>, Queue<int>) GetDecks(string[] data)
		{
			var player1 = new Queue<int>();
			var player2 = new Queue<int>();
			var activeQueue = player1;
			foreach (var line in data)
			{
				if (line == string.Empty)
				{
					activeQueue = player2;
				}else if (!line.StartsWith("Player"))
				{
					activeQueue.Enqueue(int.Parse(line));
				}
			}
			return (player1, player2);
		}
	}
}
