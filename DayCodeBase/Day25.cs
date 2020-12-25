using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Location = System.Tuple<int, int>;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day25 : DayCodeBase
	{
		public override string Problem1()
		{
			var subjectNumber = 7ul;
			var data = GetData().Select(ulong.Parse).ToArray();
			var cardPublicKey = data[0];
			var doorPublicKey = data[1];
			var cardLoopSize = GetLoopSize(cardPublicKey, subjectNumber);
			var doorLoopSize = GetLoopSize(doorPublicKey, subjectNumber);
			//var encryptionKeyByCard = Transform(1, doorPublicKey, cardLoopSize);
			var encryptionKeyByDoor = Transform(1, cardPublicKey, doorLoopSize);
			return encryptionKeyByDoor.ToString();
		}

		private ulong Transform(ulong value, ulong subjectNumber, ulong loopSize)
		{
			for (var i = 0ul; i < loopSize; ++i)
			{
				value *= subjectNumber;
				value %= 20201227;
			}
			return value;
		}

		private ulong GetLoopSize(ulong publicKey, ulong subjectNumber)
		{
			var i = 0ul;
			var value = 1ul;
			for (;value != publicKey; ++i)
			{
				value *= subjectNumber;
				value %= 20201227;
			}

			return i;
		}
	}
}
