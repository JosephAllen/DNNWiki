using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DotNetNuke.Wiki.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Converts a ienumerable collection of T items to a Datatable
        /// </summary>
        /// <typeparam name="T">the generic type</typeparam>
        /// <param name="collection">the collection to convert</param>
        /// <param name="tableName">the table name</param>
        /// <returns>returns a Datatable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string tableName)
        {
            DataTable tbl = ToDataTable(collection);
            tbl.TableName = tableName;
            return tbl;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dt = new DataTable();
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();
            //Create the columns in the DataTable
            foreach (PropertyInfo pi in pia)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }
            //Populate the table
            foreach (T item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in pia)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}