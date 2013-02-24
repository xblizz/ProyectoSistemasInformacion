using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SistemaSeguridad.Content
{
    public static class GridHelper
    {
        public static IList GetData(IList source)
        {
            var rows = new object[source.Count];
            var i = 0;
            
           
            foreach (var obj in source)
            {
               //string[] temp = obj.ToString().Split(',');

                rows[i] = new
                              {
                                  id = i,
                                  cell = obj
                              };
                i++;
            }


            return rows;
        }

        public static IEnumerable<MemberInfo> Members(this Type t)
        {
            foreach (FieldInfo fi in t.GetFields())
                yield return fi;
            foreach (PropertyInfo pi in t.GetProperties())
                yield return pi;
            foreach (MethodInfo mi in t.GetMethods())
                yield return mi;
            foreach (EventInfo ei in t.GetEvents())
                yield return ei;
        }

        //public static IEnumerable<object> ParseType(Type t)
        //{
        //    foreach (MemberInfo mi in t.Members())
        //    {
        //        yield return mi;
        //        foreach (object x in mi.Attributes<DbcPredicateAttribute>())
        //        {
        //            yield return x;
        //        }
        //    }
        //}
    }
}