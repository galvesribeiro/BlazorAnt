using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorAnt.Utils
{
    internal static class ClassNamesBuilder
    {
        private const string SPACE = " ";
        private const string SEPARATOR = "-";

        public static string From(string prefix = null, string[] classes = null, IDictionary<string, Func<bool>> options = null)
        {
            var sb = new StringBuilder();
            if (classes != null)
            {
                foreach (var @class in classes)
                {
                    if (!string.IsNullOrWhiteSpace(@class))
                    {
                        if (!string.IsNullOrWhiteSpace(prefix))
                        {
                            sb.Append(prefix);
                            sb.Append(SEPARATOR);
                        }

                        sb.Append(@class);
                        sb.Append(SPACE);
                    }
                }
            }

            if (options != null)
            {
                foreach (var opt in options)
                {
                    if (opt.Value == null) continue;

                    if (opt.Value())
                    {
                        if (!string.IsNullOrWhiteSpace(prefix))
                        {
                            sb.Append(prefix);
                            sb.Append(SEPARATOR);
                        }
                        sb.Append(opt.Key);
                        sb.Append(SPACE);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
