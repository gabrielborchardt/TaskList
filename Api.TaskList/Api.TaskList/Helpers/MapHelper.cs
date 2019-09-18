using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Api.TaskList.Helpers
{
    public static class MapHelper
    {
        public static bool SeDateTime(DbDataReader dr, string nomePropriedade)
        {
            try
            {
                var data = Convert.ToDateTime(dr[nomePropriedade].ToString());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr, bool dateTimeString = false)
        {
            try
            {
                List<T> list = new List<T>();
                T obj = default(T);
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();

                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (prop.GetCustomAttributes(typeof(MapIgnorarAtributo), true).Length == 0)
                        {

                            if (!object.Equals(dr[prop.Name], DBNull.Value))
                            {

                                if (dateTimeString)
                                {
                                    if (SeDateTime((DbDataReader)dr, prop.Name))
                                    {
                                        var data = Convert.ToDateTime(dr[prop.Name].ToString());
                                        prop.SetValue(obj, data.ToShortDateString());
                                    }
                                    else
                                    {
                                        prop.SetValue(obj, dr[prop.Name], null);
                                    }
                                }
                                else
                                {
                                    prop.SetValue(obj, dr[prop.Name], null);
                                }
                            }

                        }
                    }
                    list.Add(obj);
                }

                return list;
            }
            catch (Exception ex)
            {
                //TODO: adicionar log aqui    
                // TextoHelper.GerarArquivoLog(ex);
                return null;
            }
        }

        public static T Select<T>(IDataReader dr, bool datetimeString = false)
        {
            T obj = default(T);
            string propertyName = string.Empty;
            string propertyType = string.Empty;
            string drValue = string.Empty;

            try
            {
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();

                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!prop.IsDefined(typeof(MapIgnorarAtributo), false))
                        {
                            propertyName = prop.Name;
                            propertyType = prop.PropertyType.ToString();

                            var value = dr[prop.Name];
                            drValue = value.ToString();

                            if (!object.Equals(value, DBNull.Value))
                            {

                                if (datetimeString)
                                {
                                    if (SeDateTime((DbDataReader)dr, prop.Name))
                                    {
                                        var data = Convert.ToDateTime(dr[prop.Name].ToString());
                                        prop.SetValue(obj, data.ToShortDateString());
                                    }
                                    else
                                    {
                                        if (value.GetType() != prop.PropertyType && value.GetType() != typeof(DateTime))
                                        {
                                            value = Convert.ChangeType(dr[prop.Name], prop.PropertyType);
                                        }

                                        //prop.SetValue(obj, value, null);
                                        SetPropertyValue(obj, prop.Name, value);
                                    }
                                }
                                else
                                {
                                    if (value.GetType() != prop.PropertyType && value.GetType() != typeof(DateTime))
                                    {
                                        value = Convert.ChangeType(dr[prop.Name], prop.PropertyType);
                                    }

                                    //prop.SetValue(obj, value, null);
                                    SetPropertyValue(obj, prop.Name, value);
                                }

                            }
                        }
                    }
                }

                return obj;

            }
            catch (Exception exceptionMapSelect)
            {
                throw new Exception(string.Format(exceptionMapSelect.Message + "PropertyName: {0} PropertyType: {1} DataReaderValue: {3}", propertyName, propertyType, drValue));
            }
        }

        private static void SetPropertyValue(object parent, string propertyName, object value)
        {
            var inherType = parent.GetType();
            while (inherType != null)
            {
                PropertyInfo propToSet = inherType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (propToSet != null && propToSet.CanWrite)
                {
                    propToSet.SetValue(parent, value, null);
                    break;
                }

                inherType = inherType.BaseType;
            }
        }

        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public class MapIgnorarAtributo : Attribute
        {

        }
    }
}
