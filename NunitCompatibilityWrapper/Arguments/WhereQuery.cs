namespace NunitCompatibilityWrapper.Parameters
{
    public static class WhereQuery
    {
        /// <summary>
        /// Boolean if the "where" query should be added.
        /// </summary>
        public static bool useWhere = false;

        /// <summary>
        /// The first exclude
        /// </summary>
        private static bool firstExclude = true;

        /// <summary>
        /// The first include
        /// </summary>
        private static bool firstInclude = true;

        /// <summary>
        /// The uses exclude
        /// </summary>
        private static bool usesExclude = false;

        /// <summary>
        /// The uses include
        /// </summary>
        private static bool usesInclude = false;

        static WhereQuery()
        {
            whereQuery = "--where=\"";
        }

        /// <summary>
        /// The where query
        /// </summary>
        public static string whereQuery { get; private set; }
        /// <summary>
        /// Adds the categories.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="include">If set to <c>true</c>, the categories should be included.</param>
        public static void AddCategories(string[] categories, bool include)
        {
            useWhere = true;
            AddAndToQuery(include);

            whereQuery = whereQuery + "(";
            foreach (var category in categories)
            {
                category.Replace("-", "");

                AddToWhereQuery(category, include);
            }
            whereQuery = whereQuery + ")";
        }

        /// <summary>
        /// Adds the and to query if it is not the first include/exclude in the query.
        /// </summary>
        /// <param name="include">if set to <c>true</c> the category should be included.</param>
        private static void AddAndToQuery(bool include)
        {
            if (include && !firstInclude)
            {
                whereQuery = whereQuery + " and ";
            }
            else if (include && firstInclude)
            {
                if (usesExclude)
                {
                    whereQuery = whereQuery + " and ";
                }
                firstInclude = false;
                usesInclude = true;
            }
            if (!include && !firstExclude)
            {
                whereQuery = whereQuery + " and ";
            }
            else if (!include && firstExclude)
            {
                firstExclude = false;
                usesExclude = true;
            }
        }

        /// <summary>
        /// Adds to the where query.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="include">If set to <c>true</c>, the categories should be included.</param>
        private static void AddToWhereQuery(string category, bool include)
        {
            if (!usesExclude && !usesInclude)
            {
                whereQuery = include ? whereQuery + " or " : whereQuery + " and ";
            }
            if (include)
            {
                whereQuery = whereQuery + "Category == " + category;
            }
            else
            {
                whereQuery = whereQuery + "Category != " + category;
            }
        }
    }
}