using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day7: DayCodeBase
	{
		private const string regexPattern = @"(?<curBag>.+)\sbags? contain\s((?<amount>\d)\s(?<pattern>(.)+?)\sbags?[,\.]\s?)+";
		private const string emptyPattern = @"(?<curBag>.+)\sbags? contain no other bags.";

		public override string Problem1()
		{
			var data = GetData().ToList();
			var tree = GetTree(data);
			var allParents = GetAllParents(tree["shiny gold"]).Distinct().ToList();
			return allParents.Count().ToString();
		}

		public override string Problem2()
		{
			var data = GetData().ToList();
			var tree = GetTree(data);
			var childrenCount = GetChildrenCount(tree["shiny gold"])-1;
			return childrenCount.ToString();
		}

		private int GetChildrenCount(Node node)
		{
			var toReturn = 0;
			foreach (var child in node.Children)
				toReturn += GetChildrenCount(child.Item2) * child.Item1;
			return toReturn + 1;
		}

		private IEnumerable<Node> GetAllParents(Node node)
		{
			if (node.Parents.Any())
			{
				foreach(var parent in node.Parents)
				{
					yield return parent;
					foreach(var parentResult in GetAllParents(parent))
					{
						yield return parentResult;
					}
				}
			}
			else
			{
				yield return node;
			}
		}

		private Dictionary<string, Node> GetTree(List<string> data)
		{
			var toReturn = new Dictionary<string, Node>();
			foreach(var line in data)
			{
				var pattern = line.Contains("contain no other bags.") ? emptyPattern : regexPattern;
				AddInfo(toReturn, Regex.Match(line, pattern));
			}
			return toReturn;
		}

		private void AddInfo(Dictionary<string, Node> toReturn, Match match)
		{
			var parent = EnsureNode(toReturn, match.Groups["curBag"].Value);
			var zip = match.Groups["pattern"].Captures
				.Select(c => c.Value)
				.Zip(match.Groups["amount"].Captures.Select(c => c.Value), (pattern, amount) => new Tuple<int, string>(int.Parse(amount), pattern));
			foreach (var amountPattern in zip)
			{
				var child = EnsureNode(toReturn, amountPattern.Item2);
				parent.Children.Add(new Tuple<int, Node>(amountPattern.Item1, child));
				child.Parents.Add(parent);
			}
		}

		private Node EnsureNode(Dictionary<string, Node> toReturn, string value)
		{
			if (!toReturn.ContainsKey(value.Trim())) toReturn.Add(value.Trim(), new Node { Name = value.Trim() });
			return toReturn[value.Trim()];
		}

		public class Node
		{
			public List<Node> Parents = new List<Node>();
			public string Name;
			public List<Tuple<int, Node>> Children = new List<Tuple<int, Node>>();
		}
	}
}
