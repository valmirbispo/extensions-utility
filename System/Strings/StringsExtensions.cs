using ExtensionsUtility.System.Objects;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtensionsUtility.System.Strings
{
    public static class StringsExtensions
    {
        public static bool IsGuid(this string value)
        {
            return Guid.TryParse(value, out Guid guidOutput);
        }

        public static int? ToNullableInt(this string value)
        {
            int i;
            if (int.TryParse(value, out i))
                return i;

            return null;
        }

        public static string Truncate(this string value, int maxLength, bool removeDots)
        {
            var returnValue = Truncate(value, maxLength);
            if (!returnValue.IsNullOrEmpty() && removeDots == true)
                returnValue = returnValue.Replace(" (...)", string.Empty);

            return returnValue;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            // If text in shorter or equal to length, just return it
            if (value.Length <= maxLength)
            {
                return value;
            }

            // Text is longer, so try to find out where to cut
            char[] delimiters = new char[] { ' ', '.', ',', ':', ';' };
            int index = value.LastIndexOfAny(delimiters, maxLength - 3);

            if (index > (maxLength / 2))
            {
                return value.Substring(0, index) + " (...)";
            }
            else
            {
                return value.Substring(0, maxLength - 3) + " (...)";
            }
        }

        public static string NullableTrim(this string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            return valor.Trim();
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            value = value.Replace(" ", string.Empty).Replace("/", string.Empty).Replace(":", string.Empty);

            return value.Replace("-", string.Empty).Replace(".", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace(",", string.Empty);
        }

        public static string RemoveDiacritics(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string ToEncoding(this string value, string dstEncoding)
        {

            Encoding iso = Encoding.GetEncoding(dstEncoding);
            Encoding utf8 = Encoding.UTF8;

            byte[] utfBytes = utf8.GetBytes(value.ToString());
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);

            string returnValue = iso.GetString(isoBytes);

            return returnValue;
        }

        public static string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);

            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        public static string DecodeFrom64(this string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);

            string returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }

        public static string DigitsOnly(this string textNumber)
        {

            var onlyNumber = new Regex(@"[^\d]");

            return onlyNumber.Replace(textNumber, "");
        }

        public static bool IsValidEmail(this string email)
        {
            Regex rgxEmail = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                               @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                               @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            return rgxEmail.IsMatch(email);
        }

        public static string MaskEmail(this string email)
        {
            if (email.IsNullOrEmpty()) return string.Empty;

            string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
            string result = Regex.Replace(email, pattern, m => new string('*', m.Length));

            return result;
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsNotNullOrEmpty(this string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        public static bool IsDateTime(this string text)
        {
            DateTime tempDate;

            return DateTime.TryParse(text, out tempDate);
        }

        public static bool IsNumber(this string text)
        {
            return Regex.IsMatch(text, @"^\d+$");
        }

        public static void ToUpper(this object obj)
        {
            if (obj.IsNotNull())
            {
                foreach (var p in obj.GetType().GetProperties())
                {
                    if (p.PropertyType == typeof(string) && p.GetValue(obj) != null)
                        p.SetValue(obj, p.GetValue(obj).ToString().ToUpper());
                }
            }
        }

        public static bool IsValidCpf(this string cpf)
        {
            if (cpf == null)
                return false;

            cpf = cpf.Replace(".", String.Empty).Replace("-", String.Empty).Trim();

            if (cpf.Length != 11)
                return false;

            switch (cpf)
            {
                case "00000000000":
                case "11111111111":
                case "22222222222":
                case "33333333333":
                case "44444444444":
                case "55555555555":
                case "66666666666":
                case "77777777777":
                case "88888888888":
                case "99999999999":
                    return false;
            }

            int soma = 0;
            for (int i = 0, j = 10, d; i < 9; i++, j--)
            {
                if (!Int32.TryParse(cpf[i].ToString(), out d))
                    return false;
                soma += d * j;
            }

            int resto = soma % 11;

            string digito = (resto < 2 ? 0 : 11 - resto).ToString();
            string prefixo = cpf.Substring(0, 9) + digito;

            soma = 0;
            for (int i = 0, j = 11; i < 10; i++, j--)
                soma += Int32.Parse(prefixo[i].ToString()) * j;

            resto = soma % 11;
            digito += (resto < 2 ? 0 : 11 - resto).ToString();

            return cpf.EndsWith(digito);
        }

        public static bool IsValidCnpj(this string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static string FormatCpf(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return string.Empty;

            cpf = cpf.Trim();

            if (cpf.Length != 11)
                return string.Empty;

            return string.Format("{0}.{1}.{2}-{3}", cpf.Substring(0, 3), cpf.Substring(3, 3), cpf.Substring(6, 3), cpf.Substring(9));
        }
    }
}
