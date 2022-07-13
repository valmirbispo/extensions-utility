using ExtensionsUtility.System.Strings;
using System.Text;

namespace ExtensionsUtility.System.Conversions
{
    public static class ConversionsExtensions
    {
        public static long TryParse(this string value, long defaultValue)
        {
            long convertedValue;

            bool success = long.TryParse(value, out convertedValue);
            if (success)
            {
                return convertedValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime TryParse(this string value, DateTime defaultValue)
        {
            DateTime convertedValue;

            bool success = DateTime.TryParse(value, out convertedValue);
            if (success)
            {
                return convertedValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static Guid TryParse(this string value, Guid defaultValue)
        {
            Guid convertedValue;

            bool success = Guid.TryParse(value, out convertedValue);
            if (success)
            {
                return convertedValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static long ToInt64(this string valor)
        {

            return Convert.ToInt64(valor);
        }

        public static long? ToNullInt64(this string valor)
        {

            return string.IsNullOrEmpty(valor) ? null : (int?)Convert.ToInt64(valor);
        }

        public static int ToInt32(this string valor)
        {

            return Convert.ToInt32(valor);
        }

        public static int? ToNullInt32(this string valor)
        {

            return string.IsNullOrEmpty(valor) ? null : (int?)Convert.ToInt32(valor);
        }

        public static DateTime ToDateTime(this string valor)
        {

            return Convert.ToDateTime(valor);
        }

        public static string ToStringBr(this DateTime value, string divisorHora = ":")
        {
            return value.ToString("dd/MM/yyyy HH:mm").Replace(":", divisorHora);
        }

        public static DateTime? ToNullDateTime(this string valor)
        {

            return string.IsNullOrEmpty(valor) ? null : (DateTime?)Convert.ToDateTime(valor);
        }

        public static decimal ToDecimal(this string valor)
        {
            if (!valor.IsNullOrEmpty())
            {
                valor = valor.Replace(".", ",");

                if (!valor.Contains(","))
                    valor = string.Format("{0},0", valor);
            }

            return Convert.ToDecimal(valor);
        }

        public static decimal? ToNullDecimal(this string valor)
        {
            if (!valor.IsNullOrEmpty())
                valor = valor.Replace(".", ",");

            return string.IsNullOrEmpty(valor) ? null : (decimal?)Convert.ToDecimal(valor);
        }
    }
}
