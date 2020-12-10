using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day9: DayCodeBase
	{
		public override string Problem1()
		{
			var preambleSize = 25;
			var data = GetData(1).Select(d => ulong.Parse(d)).ToArray();
			return GetInvalidNumber(data, preambleSize)?.ToString() ?? "Error";
		}

		public override string Problem2()
		{
			var preambleSize = 25;
			var data = GetData(1).Select(d => ulong.Parse(d)).ToArray();
			var invalidNum = GetInvalidNumber(data, preambleSize);
			for(var i = 0; i < data.Length; ++i)
			{
				var count = GetSummationRange(data, i, (ulong)invalidNum);
				if(count != null)
				{
					return (data.Skip(i).Take((int)count).Min() + data.Skip(i).Take((int)count).Max()).ToString();
				}
			}
			return "Error";
		}

		private int? GetSummationRange(ulong[] data, int startIndex, ulong invalidNum)
		{
			ulong sum = 0;
			for(var count = 1; sum <= invalidNum; ++count)
			{
				sum = data.Skip(startIndex).Take(count).Aggregate((ulong)0, (acc, cur) => acc + cur);
				if (sum == invalidNum) return count;
			}
			return null;
		}

		private ulong? GetInvalidNumber(ulong[] data, int preambleSize) {
			for (var i = preambleSize; i < data.Length; ++i)
			{
				if (!IsValid(new Span<ulong>(data, i - preambleSize, preambleSize), data[i])) return data[i];
			}
			return null;
		}

		private bool IsValid(Span<ulong> span, ulong value)
		{
			var preamble = new HashSet<ulong>(span.ToArray());
			foreach(var num in preamble)
			{
				var otherNum = value - num;
				if (otherNum != num && preamble.Contains(otherNum)) return true;
			}
			return false;
		}
	}
}
