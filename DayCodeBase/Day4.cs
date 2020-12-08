using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day4 : DayCodeBase
	{
		public override string Problem1()
		{
			return GetData()
				.CollapseInput()
				.Select(GetDictionary)
				.Where(d => HasFields(d, "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"))
				.Count()
				.ToString();
		}
		public override string Problem2()
		{
			return GetData()
				.CollapseInput()
				.Select(GetDictionary)
				.Where(d => HasFields(d, "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"))
				.Where(MeetsCriteria)
				.Count()
				.ToString();
		}

		private bool MeetsCriteria(Dictionary<string, string> data)
		{
			var byr = int.Parse(data["byr"]);
			var iyr = int.Parse(data["iyr"]);
			var eyr = int.Parse(data["eyr"]);
			var hcl = data["hcl"].Trim();
			var pid = data["pid"].Trim();
			var ecl = data["ecl"].Trim();
			if (data["hgt"].Trim().Length < 3) return false;
			if (!int.TryParse(data["hgt"].Trim().Substring(0, data["hgt"].Trim().Length - 2), out var hgt)) return false;
			if (data["hgt"].Trim().EndsWith("cm"))
			{
				if (hgt < 150 || hgt > 193)
					return false;
			} else if (data["hgt"].Trim().EndsWith("in"))
			{
				if (hgt < 59 || hgt > 76)
					return false;
			} else
			{
				return false;
			}

			return byr >= 1920 && byr <= 2002 &&
				iyr >= 2010 && iyr <= 2020 &&
				eyr >= 2020 && eyr <= 2030 &&
				Regex.IsMatch(hcl, "#[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]") && hcl.Length == 7 &&
				new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(ecl) &&
				Regex.IsMatch(pid, @"\d\d\d\d\d\d\d\d\d") && pid.Length == 9;
		}

		private Dictionary<string, string> GetDictionary(string input)
		{
			var pairs = input.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
			return pairs.ToDictionary(p => p.Substring(0, 3), p => p.Substring(4));
		}

		private bool HasFields(Dictionary<string, string> dict, params string[] keys)
		{
			return keys.All(k => dict.ContainsKey(k));
		}
	}
	public static class ExtensionsMethods
	{
		public static IEnumerable<string> CollapseInput(this IEnumerable<string> input)
		{
			string toYield = "";
			foreach (var line in input)
			{
				if (line.Trim().Length == 0)
				{
					yield return toYield.Trim();
					toYield = "";
				}
				else
				{
					toYield += " " + line;
				}
			}
			yield return toYield.Trim();
		}
	}
}
