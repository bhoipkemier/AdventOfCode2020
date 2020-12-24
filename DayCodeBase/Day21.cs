using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.DayCodeBase
{
	public class Day21: DayCodeBase
	{
		public override string Problem1()
		{
			var recipes = GetData().Select(line => new Recipe(line)).ToList();
			GetAllergenIngred(recipes);
			return recipes.Sum(r => r.NonMatched.Count).ToString();
		}
		public override string Problem2()
		{
			var recipes = GetData().Select(line => new Recipe(line)).ToList();
			var lookup = GetAllergenIngred(recipes);
			return string.Join(',', lookup.OrderBy(ai => ai.Key).Select(ai => ai.Value));
		}

		private static Dictionary<string, string> GetAllergenIngred(List<Recipe> recipes)
		{
			var allAllergens = new HashSet<string>(recipes.SelectMany(r => r.Allergens));
			var allIngredients = new HashSet<string>(recipes.SelectMany(r => r.AllIngredients));
			var allergenIgred = new Dictionary<string, string>();
			while (allAllergens.Count > allergenIgred.Count)
			{
				foreach (var allergen in allAllergens.Except(allergenIgred.Keys).ToArray())
				{
					IEnumerable<string> ingredientIntersection = allIngredients;
					foreach (var recipe in recipes.Where(r => r.Allergens.Contains(allergen)))
					{
						ingredientIntersection = ingredientIntersection.Intersect(recipe.NonMatched);
					}

					if (ingredientIntersection.Count() == 1)
					{
						var ingredToRemove = ingredientIntersection.First();
						foreach (var recipe in recipes.Where(r => r.NonMatched.Contains(ingredToRemove)).ToArray())
						{
							recipe.NonMatched.Remove(ingredToRemove);
						}

						allergenIgred[allergen] = ingredToRemove;
					}
				}
			}
			return allergenIgred;
		}

		public class Recipe
		{
			private Regex _regex = new Regex(@"((?<ingredient>[a-z]+)\s*)+\(contains\s*((?<allergen>[a-z]+),*\s*)+\)");
			public HashSet<string> AllIngredients { get; set; }
			public HashSet<string> NonMatched { get; set; }
			public HashSet<string> Allergens { get; set; }

			public Recipe(string line)
			{
				var match = _regex.Match(line);
				AllIngredients = new HashSet<string>(match.Groups["ingredient"].Captures.Select(c => c.Value));
				NonMatched = new HashSet<string>(AllIngredients);
				Allergens = new HashSet<string>(match.Groups["allergen"].Captures.Select(c => c.Value));
			}
		}
	}
}
