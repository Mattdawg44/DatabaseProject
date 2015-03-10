using MenuManager.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuManager
{
    public static class Helpers
    {
        public const string TOO_LONG = "This field exceeds the maximum length.";

        public static string GetCancelUrl(HttpRequestBase request, UrlHelper url)
        {
            if (request.UrlReferrer != null)
                return request.UrlReferrer.AbsolutePath;
            else
                return url.Action("Index");
        }

        public static List<Ingredient> GetSelectedIngredients(HttpRequestBase request, IQueryable<Ingredient> allIngredients)
        {
            if (request["Ingredients"] != null)
            {
                List<int> selectedIDs = ParseIDs((string)request["Ingredients"]);
                return allIngredients.Where(i => selectedIDs.Contains(i.ID)).ToList();
            }
            return new List<Ingredient>();
        }

        public static List<int> ParseIDs(string listString)
        {
            return listString.Split(',').Select(int.Parse).ToList();
        }

        public static List<int> GetIngredientIDs(ICollection<Ingredient> ingredients)
        {
            List<int> IDs = new List<int>();
            foreach (Ingredient i in ingredients)
            {
                IDs.Add(i.ID);
            }
            return IDs;
        }
    }
}