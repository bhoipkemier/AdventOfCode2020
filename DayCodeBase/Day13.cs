using System;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day13 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData();
			var departure = long.Parse(data[0]);
			var firstShuttle = data[1].Split(',')
				.Where(id => id != "x")
				.Select(long.Parse)
				.Select(id => new {id = id, untilDeparture = id - (departure % id)})
				.OrderBy(shuttle => shuttle.untilDeparture)
				.First();
			return (firstShuttle.id * firstShuttle.untilDeparture).ToString();
		}

		public override string Problem2()
		{
			var data = GetData();
			var departure = long.Parse(data[0]);
			var shuttles = data[1].Split(',')
				.Select((id, index) =>
					id == "x" ? null : new Shuttle {Id = ulong.Parse(id), TimeOffset = (ulong) index})
				.Where(s => s != null)
				.ToList();

			var checkTime = shuttles[0].Id;
			for (var shuttleCountToInclude = 2; shuttleCountToInclude <= shuttles.Count; ++shuttleCountToInclude)
			{
				var incrementAmount = shuttles.Take(shuttleCountToInclude - 1)
					.Aggregate((ulong)1, (acc, shuttle) => acc * shuttle.Id);
				var shuttlesToConsider = shuttles.Take(shuttleCountToInclude).ToList();
				var isValid = shuttlesToConsider.All(s => s.IsValidDeparture(checkTime));
				while (!isValid)
				{
					checkTime += incrementAmount;
					isValid = shuttlesToConsider.All(s => s.IsValidDeparture(checkTime));
				}
			}
			return checkTime.ToString();
		}

		public class Shuttle
		{
			public ulong Id { get; set; }
			public ulong TimeOffset { get; set; }

			public bool IsValidDeparture(ulong time)
			{
				return (time + TimeOffset) % Id == 0;
			}
		}
	}
}
