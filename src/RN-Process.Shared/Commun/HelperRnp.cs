using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RN_Process.Shared.Commun
{
    public static class HelperRnp
    {
        //public static string ToDescription<TEnum>(this TEnum EnumValue) where TEnum : struct
        //{
        //    return Enumerations.GetEnumDescription((Enum)(object)((TEnum)EnumValue));
        //}

        public static string DescriptionAttr<TEnum>(this TEnum source)
        {
            var fi = source.GetType().GetField(source.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }

        public static string RemoverAcentos(string texto)
        {
            if (texto == null) return string.Empty;

            const string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇçÑñ";
            const string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCcNn";

            for (var i = 0; i < comAcentos.Length; i++)
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());

            return texto;
        }

        public static string FormatarTextoParaUrl(string texto)
        {
            texto = RemoverAcentos(texto);

            var textoretorno = texto.Replace(" ", "");

            const string permitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmonopqrstuvwxyz0123456789-_";

            textoretorno = Textoretorno(texto, permitidos, textoretorno);

            return textoretorno;
        }

        public static string FormatarTextoApenasLetrasENumeros(string texto)
        {
            texto = RemoverAcentos(texto);

            var textoretorno = texto.Replace(" ", "");

            const string permitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmonopqrstuvwxyz0123456789";

            textoretorno = Textoretorno(texto, permitidos, textoretorno);

            return textoretorno;
        }

        public static string FormatarTextoApenasLetrasENumerosESpaco(string texto)
        {
            texto = RemoverAcentos(texto);

            const string permitidos = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmonopqrstuvwxyz0123456789";

            var textoretorno = "";

            Textoretorno(texto, permitidos, textoretorno);

            //single space
            var options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            textoretorno = regex.Replace(textoretorno, " ");

            return textoretorno;
        }

        private static string Textoretorno(string texto, string permitidos, string textoretorno)
        {
            for (var i = 0; i < texto.Length; i++)
                if (!permitidos.Contains(texto.Substring(i, 1)))
                    textoretorno = textoretorno.Replace(texto.Substring(i, 1), "");

            return textoretorno;
        }

        public static string GetNumeros(string texto)
        {
            return string.IsNullOrEmpty(texto) ? "" : new string(texto.Where(char.IsDigit).ToArray());
        }

        public static string AjustarTexto(string valor, int tamanho)
        {
            if (valor.Length > tamanho) valor = valor.Substring(1, tamanho);
            return valor;
        }

        /// <summary>
        ///     deixa as primeiras letras maiusculas
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToTitleCase(string texto)
        {
            return ToTitleCase(texto, false);
        }

        public static string ToTitleCase(string texto, bool manterOqueJaEstiverMaiusculo)
        {
            texto = texto.Trim();

            if (!manterOqueJaEstiverMaiusculo)
                texto = texto.ToLower();

            var textInfo = new CultureInfo("pt-BR", false).TextInfo;
            return textInfo.ToTitleCase(texto);
        }


        public static List<string> ListValidDocumentFormat(string[] stringData)
        {
            var listValidFiles = new List<string>();
            string[] format = { "xml", "xlsx", ".xls", ".txt", ".csv", ".data" };
            foreach (var item in stringData)
            {
                if (format.Any(x => item.EndsWith(x)))
                {
                    listValidFiles.Add(item);
                }
            }
            return listValidFiles;
        }        
    }
}