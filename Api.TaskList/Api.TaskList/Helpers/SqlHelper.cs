using System;
using System.Globalization;

namespace Api.TaskList.Helpers
{
    public static class SqlHelper
    {
        public static string FormatarCampoSql(object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }

            var typeName = obj.GetType().Name.ToUpper();

            switch (typeName)
            {
                case "INT64":
                case "INT32":
                    return obj.ToString();

                case "STRING":
                    return string.Format("'{0}'", obj.ToString());

                case "DATETIME":
                    DateTime dt = DateTime.Parse(obj.ToString());

                    return $"'{dt.ToString("yyyy-MM-dd hh:MM:ss")}'";

                case "DECIMAL":
                    double valorNovo = 0;

                    if (double.TryParse(obj.ToString(), out valorNovo))
                    {
                        return valorNovo.ToString(new CultureInfo("en-US"));
                    }

                    return "0";
            }

            return "";
        }
    }
}
