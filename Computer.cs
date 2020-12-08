using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2020
{

	public interface IInputOutputStream<T>
	{
		void Write(T obj);
		Task<T> Get();
	}

	public class Computer
	{
		public Task<int> RunProgram(Dictionary<long, long> codes, IInputOutputStream<long> input,
			IInputOutputStream<long> output) => RunProgram(codes, input, output, CancellationToken.None);

		public async Task<int> RunProgram(Dictionary<long, long> codes, IInputOutputStream<long> input, IInputOutputStream<long> output, CancellationToken cancellationToken)
		{
			var instructionSize = 4L;
			var relativeBase = 0L;
			for (var ip = 0L; codes[ip] != 99L; ip += instructionSize)
			{
				if (cancellationToken.IsCancellationRequested) return 2;
				instructionSize = 4;
				var opcode = codes[ip] % 100;
				switch (opcode)
				{
					case 1:
						var val = GetVal(codes, ip, 1, relativeBase) + GetVal(codes, ip, 2, relativeBase);
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 2:
						val = GetVal(codes, ip, 1, relativeBase) * GetVal(codes, ip, 2, relativeBase);
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 3:
						instructionSize = 2;
						SetVal(codes, ip, 1, relativeBase, await input.Get());
						break;
					case 4:
						instructionSize = 2;
						output.Write(GetVal(codes, ip, 1, relativeBase));
						break;
					case 5:
						var test = GetVal(codes, ip, 1, relativeBase) != 0;
						instructionSize = test ? 0 : 3;
						if (test)
						{
							ip = GetVal(codes, ip, 2, relativeBase);
						}
						break;
					case 6:
						test = GetVal(codes, ip, 1, relativeBase) == 0;
						instructionSize = test ? 0 : 3;
						if (test)
						{
							ip = GetVal(codes, ip, 2, relativeBase);
						}
						break;
					case 7:
						test = GetVal(codes, ip, 1, relativeBase) < GetVal(codes, ip, 2, relativeBase);
						val = test ? 1 : 0;
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 8:
						test = GetVal(codes, ip, 1, relativeBase) == GetVal(codes, ip, 2, relativeBase);
						val = test ? 1 : 0;
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 9:
						instructionSize = 2;
						relativeBase += GetVal(codes, ip, 1, relativeBase);
						break;
					case 99:
						return 1;
					default:
						throw new NotImplementedException(codes[ip].ToString());
				}
			}

			return 0;
		}

		private void SetVal(Dictionary<long, long> codes, long ip, int ipOffset, long relativeBase, long val)
		{
			var modeOpCode = codes[ip];
			var paramMode = ipOffset == 1 ? (modeOpCode / 100) % 10 :
				ipOffset == 2 ? (modeOpCode / 1000) % 10 :
				ipOffset == 3 ? (modeOpCode / 10000) % 10 :
				ipOffset == 4 ? (modeOpCode / 100000) % 10 : int.MinValue;
			long addr;
			switch (paramMode)
			{
				case 0L:
					addr = codes[ip + ipOffset];
					break;
				case 1L:
					throw new Exception();
				case 2L:
					addr = codes[ip + ipOffset] + relativeBase;
					break;
				default:
					throw new Exception();
			}
			codes[addr] = val;
		}

		private long GetVal(Dictionary<long, long> codes, long ip, long ipOffset, long relativeBase)
		{
			var modeOpCode = codes[ip];
			var paramMode = ipOffset == 1 ? (modeOpCode / 100) % 10 :
				ipOffset == 2 ? (modeOpCode / 1000) % 10 :
				ipOffset == 3 ? (modeOpCode / 10000) % 10 :
				ipOffset == 4 ? (modeOpCode / 100000) % 10 : int.MinValue;
			long addr;
			switch (paramMode)
			{
				case 0L:
					addr = codes[ip + ipOffset];
					break;
				case 1L:
					addr = ip + ipOffset;
					break;
				case 2L:
					addr = codes[ip + ipOffset] + relativeBase;
					break;
				default:
					throw new Exception();
			}

			return codes.ContainsKey(addr) ? codes[addr] : 0;
		}
	}
}
