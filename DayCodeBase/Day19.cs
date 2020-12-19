using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pattern = System.Collections.Generic.List<int>;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day19 : DayCodeBase
	{
		Regex _regex = new Regex(@"(?<token>[\d]+|\+|\*|\(|\))");

		public override string Problem1()
		{
			var dataSet = new DataSet(GetData());
			return dataSet.GetValidCount().ToString();
		}

		public override string Problem2()
		{
			var dataSet = new DataSet(GetData());
			dataSet.Rules[8] = new Rule("8: 42 | 42 8");
			dataSet.Rules[11] = new Rule("11: 42 31 | 42 11 31");
			return dataSet.GetValidCount().ToString();
		}

		public class DataSet
		{
			public Dictionary<int, Rule> Rules { get; set; }
			public List<string> Messages { get; set; }

			public DataSet(string[] data)
			{
				Messages = new List<string>();
				Rules = new Dictionary<int, Rule>();
				var rules = true;
				foreach (var line in data)
				{
					if (line == "")
					{
						rules = false;
					}else if (rules)
					{
						var rule = new Rule(line);
						Rules[rule.Id] = rule;
					}else if (!rules)
					{
						Messages.Add(line);
					}
				}
			}

			public int GetValidCount()
			{
				return Messages.Count(MessageIsValid);
			}

			public bool MessageIsValid(string message)
			{
				var remainingSubstrings = Rules[0].ApplyRule(message, Rules).ToList();
				return remainingSubstrings.Any(s => s == string.Empty);
			}
		}

		public class Rule
		{
			public int Id { get; set; }
			public List<Pattern> Patterns { get; set; }
			public char? Value { get; set; }

			public Rule(string data)
			{
				var colonSplit = data.Split(':');
				Id = int.Parse(colonSplit[0]);
				var pipeSplit = colonSplit[1].Split('|');
				Patterns = new List<Pattern>();
				foreach (var pattern in pipeSplit)
				{
					if (pattern.Contains("\""))
					{
						Patterns = null;
						Value = pattern.Trim("\" ".ToCharArray())[0];
					}
					else
					{
						Value = null;
						Patterns.Add(
							pattern.Split(' ', StringSplitOptions.RemoveEmptyEntries)
								.Select(int.Parse)
								.ToList()
							);
					}
				}
			}

			public IEnumerable<string> ApplyRule(string message, Dictionary<int, Rule> rules)
			{
				if (Value != null)
				{
					if (message.Length > 0 && message[0] == Value)
						yield return message.Substring(1);
				}
				else
				{
					foreach (var pattern in Patterns)
					{
						var toCheckSet = new List<string>(new[] {message});
						foreach (var segment in pattern)
						{
							var segmentRule = rules[segment];
							var newToCheckSet = new List<string>();
							foreach (var toCheck in toCheckSet)
							{
								newToCheckSet.AddRange(segmentRule.ApplyRule(toCheck, rules));
							}
							toCheckSet = newToCheckSet;
						}

						foreach (var result in toCheckSet)
						{
							yield return result;
						}
					}
				}
			}
		}
	}
}
