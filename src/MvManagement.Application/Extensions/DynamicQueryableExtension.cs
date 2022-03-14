using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MvManagement.Extensions
{
   public static class DynamicQueryableExtensions
    {
        #region OrderBy
        public static IEnumerable<TModel> OrderBy<TModel>(this IEnumerable<TModel> source, string ordering, string defaultValue = null)
        {
            var s = source.AsQueryable();
            return s.OrderBy(ordering, defaultValue);
            //if (!string.IsNullOrWhiteSpace(ordering))
            //    return System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(source, ordering);
            //else if (!string.IsNullOrWhiteSpace(defaultValue))
            //    return System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(source, defaultValue);
            //else
            //    return source;
        }
        public static IEnumerable<TModel> OrderBy<TModel>(this IEnumerable<TModel> source, string ordering, Func<IEnumerable<TModel>, IEnumerable<TModel>> defaultexpressions)
        {
            if (!string.IsNullOrWhiteSpace(ordering))
                return DynamicQueryableExtensions.OrderBy(source, ordering);
            else if (defaultexpressions != null)
            {
                return defaultexpressions(source);
            }
            return source;
        }
        public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> source, string ordering, string defaultValue = null)
        {
            if (!string.IsNullOrWhiteSpace(ordering))
#if  Kahanu_System_Linq_Dynamic && !(NET35 || SILVERLIGHT || NETFX_CORE || WINDOWS_APP || DOTNET5_1 || UAP10_0 || NETSTANDARD)
                return System.Linq.Dynamic.DynamicQueryable.OrderBy(source, ordering);
#endif
#if  !Kahanu_System_Linq_Dynamic || NETSTANDARD2_0
                return System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(source, ordering);
#endif
            else if (!string.IsNullOrWhiteSpace(defaultValue))
#if Kahanu_System_Linq_Dynamic && !(NET35 || SILVERLIGHT || NETFX_CORE || WINDOWS_APP || DOTNET5_1 || UAP10_0 || NETSTANDARD)
                return System.Linq.Dynamic.DynamicQueryable.OrderBy(source, defaultValue);
#endif
#if !Kahanu_System_Linq_Dynamic || NETSTANDARD2_0
                return System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(source, defaultValue);
#endif
            else
                return source;
        }
        public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> source, string ordering, Func<IQueryable<TModel>, IQueryable<TModel>> defaultexpressions)
        {
            if (!string.IsNullOrWhiteSpace(ordering))
                return DynamicQueryableExtensions.OrderBy(source, ordering);
            else if (defaultexpressions != null)
            {
                return defaultexpressions(source);
            }
            return source;
        }
        public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> source, string ordering, params Expression<Func<TModel, object>>[] defaultexpressions)
        {
            if (!string.IsNullOrWhiteSpace(ordering))
                return DynamicQueryableExtensions.OrderBy(source, ordering);
            else if (defaultexpressions != null)
            {
                var ss = source;
                foreach (var item in defaultexpressions)
                {
                    ss = ss.OrderBy(item);
                }
                return ss;
            }
            return source;
        }
        //public static IQueryable<TModel> OrderByDescending<TModel>(this IQueryable<TModel> source, string ordering, params Expression<Func<TModel, object>>[] defaultexpressions)
        //{
        //    if (!string.IsNullOrWhiteSpace(ordering))
        //        return DynamicQueryableExtension.OrderBy(source, ordering);
        //    else if (defaultexpressions != null)
        //    {
        //        var ss = source;
        //        foreach (var item in defaultexpressions)
        //        {
        //            ss = ss.OrderByDescending(item);
        //        }
        //        return ss;
        //    }
        //    return source;
        //}
        #endregion OrderBy

        #region Where
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, string predicate, string defaultValue = null)
        {
            var s = source.AsQueryable();
            return DynamicQueryableExtensions.Where(s, predicate, defaultValue);

            //if (!string.IsNullOrWhiteSpace(predicate))
            //    return DynamicQueryableExtension.Where(source, predicate);
            //else if (!string.IsNullOrWhiteSpace(defaultValue))
            //    return DynamicQueryableExtension.Where(source, defaultValue);
            //else
            //    return source;
        }
//        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, string predicate, params object[] args)
//        {
//#if Kahanu_System_Linq_Dynamic && !(NET35 || SILVERLIGHT || NETFX_CORE || WINDOWS_APP || DOTNET5_1 || UAP10_0 || NETSTANDARD)
//            return System.Linq.Dynamic.DynamicQueryable.Where(source.AsQueryable(), predicate, args);
//#endif
//#if !Kahanu_System_Linq_Dynamic || NETSTANDARD2_0
//            return System.Linq.Dynamic.Core.DynamicQueryableExtensions.Where((IQueryable) source.AsQueryable(), predicate, args).AsEnumerable<T>();
//#endif
//        }
        public static IQueryable<T> Where<T>(this IQueryable<T> source, string predicate, string defaultValue = null)
        {
            if (!string.IsNullOrWhiteSpace(predicate))
#if Kahanu_System_Linq_Dynamic && !(NET35 || SILVERLIGHT || NETFX_CORE || WINDOWS_APP || DOTNET5_1 || UAP10_0 || NETSTANDARD)
                return System.Linq.Dynamic.DynamicQueryable.Where(source, predicate);
#endif
#if !Kahanu_System_Linq_Dynamic || NETSTANDARD2_0
                return System.Linq.Dynamic.Core.DynamicQueryableExtensions.Where(source, predicate);
#endif
            else if (!string.IsNullOrWhiteSpace(defaultValue))
#if Kahanu_System_Linq_Dynamic && !(NET35 || SILVERLIGHT || NETFX_CORE || WINDOWS_APP || DOTNET5_1 || UAP10_0 || NETSTANDARD)
                return System.Linq.Dynamic.DynamicQueryable.Where(source, defaultValue);
#endif
#if !Kahanu_System_Linq_Dynamic || NETSTANDARD2_0
                return System.Linq.Dynamic.Core.DynamicQueryableExtensions.Where(source, defaultValue);
#endif
            else
                return source;
        }
        public static IQueryable<T> Where<T>(this IQueryable<T> source, string predicate, params Expression<Func<T, bool>>[] defaultPredicate)
        {
            if (!string.IsNullOrWhiteSpace(predicate))
#if Kahanu_System_Linq_Dynamic && !(NET35 || SILVERLIGHT || NETFX_CORE || WINDOWS_APP || DOTNET5_1 || UAP10_0 || NETSTANDARD)
                return System.Linq.Dynamic.DynamicQueryable.Where(source, predicate);
#endif
#if !Kahanu_System_Linq_Dynamic || NETSTANDARD2_0
                return System.Linq.Dynamic.Core.DynamicQueryableExtensions.Where(source, predicate);
#endif
            else if (defaultPredicate != null)
            {
                var ss = source;
                foreach (var item in defaultPredicate)
                {
                    ss = ss.Where(item);
                }
                return ss;
            }
            else
                return source;
        }
        #endregion Where
        public static IEnumerable Select<T>(this IEnumerable<T> source, string predicate, params object[] args)
        {
#if Kahanu_System_Linq_Dynamic && !(NET35 || SILVERLIGHT || NETFX_CORE || WINDOWS_APP || DOTNET5_1 || UAP10_0 || NETSTANDARD)
            return System.Linq.Dynamic.DynamicQueryable.Select(source.AsQueryable(), predicate, args);
#endif
#if !Kahanu_System_Linq_Dynamic || NETSTANDARD2_0
            return System.Linq.Dynamic.Core.DynamicQueryableExtensions.Select(source.AsQueryable(), predicate, args);
#endif
        }
        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return (from outerItem in outer
                    join innerItem in inner
                      on outerKeySelector(outerItem)
                      equals innerKeySelector(innerItem)
                      into Books
                    from innerItem in Books.DefaultIfEmpty()
                    select resultSelector(outerItem, innerItem));
        }
        public static IEnumerable<TResult> RightJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return (from innerItem in inner
                    join outerItem in outer
                      on innerKeySelector(innerItem)
                      equals outerKeySelector(outerItem)
                      into Books
                    from outerItem in Books.DefaultIfEmpty()
                    select resultSelector(outerItem, innerItem));
        }
        public static IEnumerable<TResult> FullJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector).Union(outer.RightJoin(inner, outerKeySelector, innerKeySelector, resultSelector));
        }
        public static System.Collections.IEnumerable Cast(this System.Collections.IEnumerable source, Type type)
        {
            //return typeof(Enumerable).GetMethod(nameof(Enumerable.Cast)).MakeGenericMethod(type).Invoke(null, new object[] { source }) as System.Collections.IEnumerable;
            return typeof(DynamicQueryableExtensions).GetMethod(nameof(DynamicQueryableExtensions.Cast2)).MakeGenericMethod(type).Invoke(null, new object[] { source }) as System.Collections.IEnumerable;
        }
        public static IEnumerable<TResult> Cast2<TResult>(this System.Collections.IEnumerable source)
        {
            IEnumerable<TResult> typedSource = source as IEnumerable<TResult>;
            if (typedSource != null) return typedSource;
            if (source == null) throw new System.ArgumentNullException("source");//Error.ArgumentNull("source");
            return CastIterator<TResult>(source);
        }
        static IEnumerable<TResult> CastIterator<TResult>(System.Collections.IEnumerable source)
        {
            foreach (object obj in source) yield return (TResult)(obj ?? default(TResult));
        }
    }
}