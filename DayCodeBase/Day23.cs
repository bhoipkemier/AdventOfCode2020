using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day23 : DayCodeBase
	{
		public override string Problem1()
		{
			return PlayGame("362981754", 100);
		}
		public override string Problem2()
		{
			return PlayGame2("362981754", 10000000, 1000000);
		}
		private string PlayGame2(string cupOrder, int numberOfRounds, int numberCups)
		{
			var linkedList = new uint[numberCups + 1];
			for (uint i = 0; i < linkedList.Length; ++i)
			{
				linkedList[i] = i + 1;
			}
			var iOrder = cupOrder.ToCharArray().Select(c => uint.Parse(c.ToString())).ToList();
			for (var i = 1; i < iOrder.Count; ++i)
			{
				linkedList[iOrder[i - 1]] = iOrder[i];
			}
			linkedList[iOrder[iOrder.Count - 1]] = (uint)iOrder.Count + 1;
			linkedList[numberCups] = iOrder[0];
			linkedList[0] = iOrder[0];

			for (var i = 0; i < numberOfRounds; ++i)
			{
				DoRound2(linkedList);
			}
			return ((ulong)linkedList[1] * (ulong)linkedList[linkedList[1]]).ToString();
		}

		private string PlayGame(string cupOrder, int numberOfRounds)
		{
			for (var i = 0; i < numberOfRounds; ++i)
			{
				cupOrder = DoRound(cupOrder);
			}

			var start = cupOrder.IndexOf('1');
			return (cupOrder + cupOrder).Substring(start + 1, cupOrder.Length - 1);
		}

		private void DoRound2(uint[] linkedList)
		{
			var currentVal = linkedList[0]; // 3
			var triplet = new uint[3];
			triplet[0] = linkedList[currentVal]; // 8
			triplet[1] = linkedList[triplet[0]]; // 9
			triplet[2] = linkedList[triplet[1]]; // 1
			var afterTriplet = linkedList[triplet[2]];
			linkedList[currentVal] = afterTriplet;
			var dest = currentVal == 1 ? (uint)linkedList.Length - 1 : currentVal - 1;
			while (dest == triplet[0] || dest == triplet[1] || dest == triplet[2])
			{
				dest = dest == 1 ? (uint)linkedList.Length - 1 : dest - 1;
			}
			var destOldNext = linkedList[dest];
			linkedList[dest] = triplet[0];
			linkedList[triplet[2]] = destOldNext;
			linkedList[0] = afterTriplet;
		}

		private string DoRound(string cupOrder)
		{
			var iOrder = cupOrder.ToCharArray().Select(c => int.Parse(c.ToString())).ToList();
			var destSet = iOrder.Where((cup, index) => index > 3).OrderBy(i => i).ToList();
			var destNum = GetDestNum(destSet, iOrder[0]);
			var destPos = cupOrder.IndexOf(destNum.ToString());
			var toReturn = new StringBuilder(cupOrder.Length);
			toReturn.Append(cupOrder.Substring(4, destPos - 3));
			toReturn.Append(cupOrder.Substring(1, 3));
			toReturn.Append(cupOrder.Substring(destPos + 1));
			toReturn.Append(cupOrder.Substring(0, 1));

			return toReturn.ToString();
		}

		private object GetDestNum(List<int> destSet, int current)
		{
			var toReturn = current - 1;
			while (!destSet.Contains(toReturn))
			{
				if (toReturn == 0)
				{
					return destSet.Max();
				}
				--toReturn;
			}
			return toReturn;
		}
	}
}
