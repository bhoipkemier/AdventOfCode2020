using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day2: DayCodeBase
	{
		private Regex _regex = new Regex(@"(?<min>\d+)-(?<max>\d+)\s*(?<char>.):\s*(?<password>.+)");

		public override string Problem1()
		{
			return GetData()
				.Select(p => _regex.Match(p))
				.Where(IsValid)
				.Count()
				.ToString();
		}

		private bool IsValid(Match match)
		{
			var min = int.Parse(match.Groups["min"].Value);
			var max = int.Parse(match.Groups["max"].Value);
			var charToFind = match.Groups["char"].Value;
			var password = match.Groups["password"].Value;
			var count = password.ToCharArray().Where(c => c == charToFind[0]).Count();
			return min <= count && max >= count;
		}


		public override string Problem2()
		{
			return GetData()
				.Select(p => _regex.Match(p))
				.Where(IsValid2)
				.Count()
				.ToString();
		}

		private bool IsValid2(Match match)
		{
			var min = int.Parse(match.Groups["min"].Value);
			var max = int.Parse(match.Groups["max"].Value);
			var charToFind = match.Groups["char"].Value[0];
			var password = match.Groups["password"].Value;
			var char1 = password.ToCharArray()[min-1];
			var char2 = password.ToCharArray()[max-1];
			return (charToFind == char1 && charToFind != char2) ||
				(charToFind == char2 && charToFind != char1);
		}
	}
}
