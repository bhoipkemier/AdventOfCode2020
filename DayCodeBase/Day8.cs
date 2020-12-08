using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day8: DayCodeBase
	{
		public override string Problem1()
		{ //1487
			var computer = new Computer(GetData());
			computer.ProcessCommandsOnce();
			return computer.Accumulator.ToString();
		}
		public override string Problem2()
		{
			var computer = new Computer(GetData());
			var result = computer.SwapJmpNoop();
			if (!result) throw new Exception("Error");
			return computer.Accumulator.ToString();
		}

		public class Computer
		{
			public int IP;
			public int Accumulator;
			private HashSet<int> _commandsRun;
			public List<string[]> _program = new List<string[]>();

			public Computer(IEnumerable<string> data)
			{
				_program = data.Select(d => d.Split(' ')).ToList();
			}

			public bool ProcessCommandsOnce()
			{
				IP = 0;
				Accumulator = 0;
				_commandsRun = new HashSet<int>();
				while (!_commandsRun.Contains(IP))
				{
					_commandsRun.Add(IP);
					Execute(_program[IP]);
					if (++IP == _program.Count()) return true;
				}
				return false;
			}

			private void Execute(string[] command)
			{
				switch(command[0])
				{
					case "nop": return;
					case "acc": ExecuteAcc(command); return;
					case "jmp": ExecuteJmp(command); return;
				}
			}

			private void ExecuteJmp(string[] command)
			{
				IP += (int.Parse(command[1]) - 1);
			}

			private void ExecuteAcc(string[] command)
			{
				Accumulator += int.Parse(command[1]);
			}

			internal bool SwapJmpNoop()
			{
				var saved = _program.ToArray();
				for(var i = 0; i < saved.Count(); ++i)
				{
					if (saved[i][0] == "jmp" || saved[i][0] == "nop")
					{
						_program = saved.ToList();

						if (_program[i][0] == "jmp")
						{
							_program[i] = new[] { "nop", _program[i][1] };
						}
						else
						{
							_program[i] = new[] { "jmp", _program[i][1] };
						}
						if (ProcessCommandsOnce()) { return true; }
					}
				}
				return false;
			}
		}
	}
}
