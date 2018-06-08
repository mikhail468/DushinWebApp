using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DushinWebApp.Services
{
    public static class FileNameHelper
    {
        public static string GetNameFormated(string name)
        {
            var tokens = name.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = Char.ToUpper(token[0]) + token.Substring(1).ToLower();

            }
            return string.Join("", tokens);
        }
    }
}
