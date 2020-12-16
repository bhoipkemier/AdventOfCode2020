using System;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day12 : DayCodeBase
	{
		private long _x;
		private long _y;
		private int _dir = 1;
		public override string Problem1()
		{
			_x = 0;
			_y = 0;
			var data = GetData();
			foreach (var dir in data)
			{
				ApplyDir(dir);
			}

			return (Math.Abs(_x) + Math.Abs(_y)).ToString();
		}

		private void ApplyDir(string dir)
		{
			var value = long.Parse(dir.Substring(1));
			switch (dir[0])
			{
				case 'N': _y -= value;
					break;
				case 'S': _y += value;
					break;
				case 'E': _x += value;
					break;
				case 'W': _x -= value;
					break;
				case 'R': _dir += (((int)(value/90)) % 4);
					break;
				case 'L': _dir -= (((int)(value/90)) % 4);
					break;
				case 'F':
					switch (_dir)
					{
						case 0: _y -= value;
							break;
						case 1: _x += value;
							break;
						case 2: _y += value;
							break;
						case 3: _x -= value;
							break;
						default: throw new Exception();
							break;
					}
					break;
				default: throw new Exception();
					break;
			}

			_dir %= 4;
			if (_dir < 0) _dir += 4;
		}

		
		public override string Problem2()
		{
			_x = 0;
			_y = 0;
			_waypointY = -1;
			_waypointX = 10;
			var data = GetData();
			foreach (var dir in data)
			{
				ApplyDir2(dir);
			}

			return (Math.Abs(_x) + Math.Abs(_y)).ToString();
		}
		
		private long _waypointY = -1;
		private long _waypointX = 10;

		private void ApplyDir2(string dir)
		{
			long temp;
			var value = long.Parse(dir.Substring(1));
			switch (dir[0])
			{
				case 'N': _waypointY -= value;
					break;
				case 'S': _waypointY += value;
					break;
				case 'E': _waypointX += value;
					break;
				case 'W': _waypointX -= value;
					break;
				case 'L':
					value /= 90;
					for (var i = 0; i < value; ++i)
					{
						temp = _waypointY;
						_waypointY = -_waypointX;
						_waypointX = temp;
					}
					break;
				case 'R':
					value /= 90;
					for (var i = 0; i < value; ++i)
					{
						temp = _waypointX;
						_waypointX = -_waypointY;
						_waypointY = temp;
					}
					break;
				case 'F':
					_x += _waypointX * value;
					_y += _waypointY * value;
					break;
				default: throw new Exception();
					break;
			}
		}
	}
}
