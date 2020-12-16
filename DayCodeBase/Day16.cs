using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day16 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = new Data(GetData().ToList());
			return data.GetErrorRate().ToString();
		}

		public override string Problem2()
		{
			var data = new Data(GetData().ToList());
			var tickets = data.GetValidTickets();
			var sets = GetSets(tickets, data.Rules);
			ReduceValidRules(sets);
			var ruleOrder = GetRuleOrder(data, sets, new List<Rule>());
			var toReturn = 1ul;
			for (var i = 0; i < ruleOrder.Count; ++i)
			{
				if (ruleOrder[i].FieldName.Contains("departure"))
				{
					toReturn *= (ulong)(data.YourTicket.FieldSizes[i]);
				}
			}
			return toReturn.ToString();
		}

		private void ReduceValidRules(PropertySet[] sets)
		{
			var ruleRemoved = true;
			while (ruleRemoved) 
			{
				ruleRemoved = false;
				foreach (var singleSet in sets.Where(s => s.ValidRules.Count == 1))
				{
					foreach (var toRemoveSet in sets.Where(s => s != singleSet && s.ValidRules.Contains(singleSet.ValidRules.First())))
					{
						toRemoveSet.ValidRules.Remove(singleSet.ValidRules.First());
						ruleRemoved = true;
					}
				}	
			}
		}

		private PropertySet[] GetSets(List<Ticket> tickets, List<Rule> rules)
		{
			var toReturn = new PropertySet[tickets[0].FieldSizes.Count];
			for (var i = 0; i < tickets[0].FieldSizes.Count; ++i)
			{
				toReturn[i] = new PropertySet()
				{
					TicketIndex = i,
					FieldSizes = new HashSet<int>(tickets.Select(t => t.FieldSizes[i]))
				};
				toReturn[i].ValidRules = rules.Where(r => toReturn[i].FieldSizes.All(fs => r.IsMatch(fs))).ToList();
			}
			return toReturn;
		}

		private List<Rule> GetRuleOrder(Data data, PropertySet[] sets, List<Rule> rules)
		{
			if (rules.Count == data.Rules.Count)
			{
				return rules;
			}

			var currentSet = sets[rules.Count];

			foreach (var nextRule in currentSet.ValidRules.Except(rules))
			{
				rules.Add(nextRule);
				var result = GetRuleOrder(data, sets, rules);
				if (result != null) return result;
				rules.RemoveAt(rules.Count - 1);
			}
			return null;
		}

		public class PropertySet
		{
			public int TicketIndex { get; set; }
			public HashSet<int> FieldSizes { get; set; }
			public List<Rule> ValidRules { get; set; }
		}

		public class Data
		{
			public List<Rule> Rules { get; set; }
			public Ticket YourTicket { get; set; }
			public List<Ticket> NearbyTickets { get; set; }

			public Data(List<string> data)
			{
				Rules = new List<Rule>();
				NearbyTickets = new List<Ticket>();
				int state = 0;
				foreach (var line in data)
				{
					if (line.Trim().Length == 0)
					{
						state++;
					}else if (state == 0)
					{
						Rules.Add(new Rule(line));
					}else if (state == 1 && !line.EndsWith(':'))
					{
						YourTicket = new Ticket(line);
					}else if (state == 2 && !line.EndsWith(':'))
					{
						NearbyTickets.Add(new Ticket(line));
					}
				}
			}

			public List<Ticket> GetValidTickets()
			{
				return NearbyTickets
					.Where(t => !GetInvalidFields(t).Any())
					.Union(new []{YourTicket})
					.ToList();
			}

			public int GetErrorRate()
			{
				return NearbyTickets.SelectMany(GetInvalidFields).Sum();
			}

			private List<int> GetInvalidFields(Ticket ticket)
			{
				return ticket.FieldSizes
					.Where(fs => Rules.All(r => !r.IsMatch(fs)))
					.ToList();
			}

			public bool ValidRule(int position, Rule rule, List<Ticket> ticketsToTest)
			{
				return ticketsToTest.All(t => rule.IsMatch(t.FieldSizes[position]));
			}
		}
		public class Rule
		{
			private static Regex _regex = new Regex(@"(?<fieldName>.+): (?<rangeStart1>\d+)-(?<rangeEnd1>\d+) or (?<rangeStart2>\d+)-(?<rangeEnd2>\d+)");

			public string FieldName { get; set; }
			public List<Tuple<int, int>> Ranges { get; set; }

			public Rule(string data)
			{
				Ranges = new List<Tuple<int, int>>();
				var match = _regex.Match(data);
				FieldName = match.Groups["fieldName"].Value;
				Ranges.Add(new Tuple<int, int>(int.Parse(match.Groups["rangeStart1"].Value), int.Parse(match.Groups["rangeEnd1"].Value)));
				Ranges.Add(new Tuple<int, int>(int.Parse(match.Groups["rangeStart2"].Value), int.Parse(match.Groups["rangeEnd2"].Value)));
			}

			public bool IsMatch(int toTest)
			{
				return Ranges.Any(r => r.Item1 <= toTest && toTest <= r.Item2);
			}
		}
		public class Ticket
		{
			public List<int> FieldSizes { get; set; }

			public Ticket(string line)
			{
				FieldSizes = line.Split(',').Select(int.Parse).ToList();
			}
		}
	}
}
