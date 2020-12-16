using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day14 : DayCodeBase
	{
		private Dictionary<ulong, ulong> _memory;
		private string _mask;
		private Regex _regex = new Regex(@"mem\[(?<address>\d+)\] = (?<value>\d+)");
		public override string Problem1()
		{
			_memory = new Dictionary<ulong, ulong>();
			var data = GetData();
			foreach (var line in data)
				Process(line);
			return _memory
				.Aggregate((ulong)0, (acc, cur) => acc + cur.Value)
				.ToString();
		}

		private void Process(string line)
		{
			if (line.StartsWith("mask"))
				ProcessMask(line.Split('=')[1].Trim());
			else
				ProcessMemSet(line);
		}

		private void ProcessMemSet(string line)
		{
			var match = _regex.Match(line);
			var address = ulong.Parse(match.Groups["address"].Value);
			var value = ulong.Parse(match.Groups["value"].Value);
			_memory[address] = ApplyMask(value);
		}

		private ulong ApplyMask(ulong value)
		{
			var bitPattern = Convert.ToString((long)value, 2).PadLeft(36, '0');
			var bitPattern36 = bitPattern.Substring(bitPattern.Length - 36).ToCharArray();
			for (var i = 0; i < _mask.Length; ++i)
			{
				bitPattern36[i] = _mask[i] == 'X' ? bitPattern36[i] : _mask[i];
			}
			return Convert.ToUInt64(new string(bitPattern36), 2);
		}

		private void ProcessMask(string mask)
		{
			_mask = mask;
		}

		public override string Problem2()
		{
			_memory = new Dictionary<ulong, ulong>();
			var data = GetData();
			foreach (var line in data)
				Process2(line);
			return _memory
				.Aggregate((ulong)0, (acc, cur) => acc + cur.Value)
				.ToString();
		}

		private void Process2(string line)
		{
			if (line.StartsWith("mask"))
				ProcessMask(line.Split('=')[1].Trim());
			else
				ProcessMemSet2(line);
		}

		private void ProcessMemSet2(string line)
		{
			var match = _regex.Match(line);
			var address = ulong.Parse(match.Groups["address"].Value);
			var value = ulong.Parse(match.Groups["value"].Value);
			var addresses = ApplyMask2(address);
			foreach (var addr in addresses)
			{
				_memory[addr] = value;
			}
		}

		private List<ulong> ApplyMask2(ulong address)
		{
			List<string> toReturn = new List<string>();
			var bitPattern = Convert.ToString((long)address, 2).PadLeft(36, '0');
			var bitPattern36 = bitPattern.Substring(bitPattern.Length - 36);
			var addresses = GetAddresses("", bitPattern36, 0);
			return addresses
				.Select(a => Convert.ToUInt64(a, 2))
				.ToList();
		}

		private IEnumerable<string> GetAddresses(string buildup, string address, int i)
		{
			if (i >= 36)
			{
				yield return buildup;
				yield break;
			}

			IEnumerable<string> results;
			switch (_mask[i])
			{
				case '1': 
					results = GetAddresses(buildup + "1", address, i + 1);
					break;
				case '0': 
					results = GetAddresses(buildup + address[i], address, i + 1);
					break;
				case 'X': 
					results = GetAddresses(buildup + "1", address, i + 1).Union(GetAddresses(buildup + "0", address, i + 1));
					break;
				default: throw new Exception();
			}

			foreach (var result in results)
			{
				yield return result;
			}
		}
	}
}
