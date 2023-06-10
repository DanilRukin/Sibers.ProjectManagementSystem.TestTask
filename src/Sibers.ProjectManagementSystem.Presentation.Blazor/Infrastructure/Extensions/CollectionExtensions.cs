using System.Text;

namespace Sibers.ProjectManagementSystem.Presentation.Blazor.Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static string AsOneString(this IEnumerable<string> collection, string delimeter = " ; ")
        {
            int count = collection.Count();
            StringBuilder builder = new StringBuilder(count * 10 * delimeter.Length);
            foreach (var message in collection)
            {
                builder.Append($"{message}{delimeter}");
            }
            return builder.ToString();
        }

        public static bool RemoveWithCriterion<TEntity>(
            this ICollection<TEntity> collection,
            Func<TEntity, bool> criterion)
            where TEntity : class
        {
            TEntity? toRemove = default(TEntity);
            foreach (var item in collection)
            {
                if (criterion(item))
                {
                    toRemove = item;
                    break;
                }
            }
            if (toRemove != null)
            {
                collection.Remove(toRemove);
                return true;
            }
            return false;
        }

        public static bool RemoveRangeWithCriterion<TEntity>(
            this ICollection<TEntity> collection,
            Func<TEntity, bool> criterion)
        {
            List<TEntity> toRemove = new List<TEntity>();
            foreach (var item in collection)
            {
                if (criterion(item))
                {
                    toRemove.Add(item);
                }
            }
            if (toRemove.Count > 0)
            {
                foreach (var item in toRemove)
                {
                    collection.Remove(item);
                }
                return true;
            }
            return false;
        }
    }
}
