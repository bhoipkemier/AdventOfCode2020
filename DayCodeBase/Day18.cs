using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day18 : DayCodeBase
	{
		Regex _regex = new Regex(@"(?<token>[\d]+|\+|\*|\(|\))");
		
		public override string Problem1()
		{
			var toReturn = 0ul;
			foreach (var line in GetData())
			{
				var tokens = _regex
					.Matches(line)
					.Select(match => match.Groups["token"].Value)
					.ToList();
				toReturn += Solve(tokens);
			}
			return toReturn.ToString();
		}
		public override string Problem2()
		{
			var toReturn = 0ul;
			foreach (var line in GetData())
			{
				var tokens = _regex
					.Matches(line)
					.Select(match => match.Groups["token"].Value)
					.ToList();
				toReturn += Solve2(tokens);
			}
			return toReturn.ToString();
		}

		private ulong Solve2(List<string> tokens)
		{
			var result = new List<string>();
			var operStack = new Stack<string>();
			ulong value;
			for (var i = 0; i < tokens.Count; ++i)
			{
				var token = tokens[i];
				if (token == "(")
				{
					var tokensInParen = GetTokensInParen(tokens.Skip(i + 1));
					value = Solve2(tokens.Skip(i+1).Take(tokensInParen).ToList());
					result.Add(value.ToString());
					i += (tokensInParen + 1);
				}
				else if (token == "+")
				{
					operStack.Push(token);
				}
				else if(token == "*")
				{
					while (operStack.Any())
					{
						result.Add(operStack.Pop());
					}
					operStack.Push(token);
				}
				else
				{
					result.Add(token);
				}
			}
			while (operStack.Any())
			{
				result.Add(operStack.Pop());
			}
			return EvaluatePostFix(result);
		}

		private ulong EvaluatePostFix(List<string> result)
		{
			var evalStack = new Stack<ulong>();
			foreach (var item in result)
			{
				if (item == "+")
				{
					evalStack.Push(evalStack.Pop() + evalStack.Pop());
				} else if (item == "*")
				{
					evalStack.Push(evalStack.Pop() * evalStack.Pop());
				}
				else
				{
					evalStack.Push(ulong.Parse(item));
				}
			}
			return evalStack.Peek();
		}

		private ulong Solve(List<string> tokens)
		{
			var toReturn = 0ul;
			var oper = "+";
			var value = 0ul;
			for (var i = 0; i < tokens.Count; ++i)
			{
				var token = tokens[i];
				switch (token)
				{
					case "+":
					case "*":
						oper = token;
						break;
					case ")": throw new ArgumentException();
					case "(":
						var tokensInParen = GetTokensInParen(tokens.Skip(i + 1));
						value = Solve(tokens.Skip(i+1).Take(tokensInParen).ToList());
						toReturn = oper == "+" ? (toReturn + value) : (toReturn * value);
						i += (tokensInParen + 1);
						break;
					default:
						value = ulong.Parse(token);
						toReturn = oper == "+" ? (toReturn + value) : (toReturn * value);
						break;
				}
			}
			return toReturn;
		}

		private int GetTokensInParen(IEnumerable<string> tokens)
		{
			var parenCount = 0;
			var toReturn = 0;
			foreach (var token in tokens)
			{
				if (token == ")")
					--parenCount;
				if (token == "(")
					++parenCount;
				if (parenCount < 0) return toReturn;
				++toReturn;
			}
			throw new Exception("paren in-balance");
		}
	}
}
