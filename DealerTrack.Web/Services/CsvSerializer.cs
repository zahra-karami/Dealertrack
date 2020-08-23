using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DealerTrack.Web.Services.Interface;
using DealerTrack.Web.Utilities;

namespace DealerTrack.Web.Services
{
    public class CsvSerializer<T> : ICsvSerializer<T> where T : class, new()
    {
        private readonly List<PropertyInfo> _properties;

        public bool IgnoreEmptyLines { get; set; } = true;
        public bool IgnoreReferenceTypesExceptString { get; set; } = true;
        public string NewlineReplacement { get; set; } = ((char)0x254).ToString();
        public string Replacement { get; set; } = ((char)0x255).ToString();
        public string RowNumberColumnTitle { get; set; } = "RowNumber";
        public char Separator { get; set; } = ',';
        public string SplitRex { get; set; } = "(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        public bool UseEofLiteral { get; set; } = false;
        public bool UseLineNumbers { get; set; } = true;
        public bool UseTextQualifier { get; set; } = false;


        public CsvSerializer()
        {
            var type = typeof(T);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance
                | BindingFlags.GetProperty | BindingFlags.SetProperty);


            var q = properties.AsQueryable();

            if (IgnoreReferenceTypesExceptString)
            {
                q = q.Where(a => a.PropertyType.IsValueType || a.PropertyType.Name == "String");
            }

            var r = from a in q
                    where a.GetCustomAttribute<CsvIgnoreAttribute>() == null
                    orderby a.Name
                    select a;

            _properties = r.ToList();
        }

        public async Task<IList<T>> DeserializeAsync(Stream stream)
        {
            string[] columns;
            string[] rows;

            try
            {
                var encoding = await GetEncodingAsync(stream);
                stream.Position = 0;
                using (var sr = new StreamReader(stream, encoding))
                {
                    var header = await sr.ReadLineAsync();
                    columns = string.IsNullOrEmpty(header) ? new string[0] : header.Split(Separator);
                    var records = (await sr.ReadToEndAsync()).Replace(Environment.NewLine, "\n");
                    rows = records.Split(new[] { "\n" }, StringSplitOptions.None);
                }

            }
            catch (Exception ex)
            {
                throw new InvalidCsvFormatException("The CSV File is Invalid. See Inner Exception for more inoformation.", ex);
            }

            var data = new List<T>();

            for (int row = 0; row < rows.Length; row++)
            {
                var line = rows[row];

                if (IgnoreEmptyLines && string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (!IgnoreEmptyLines && string.IsNullOrWhiteSpace(line))
                {
                    throw new InvalidCsvFormatException($@"Error: Empty line at line number: {row}");
                }

                var reg = $"{Separator}{SplitRex}";
                var parts = Regex.Split(line, reg);

                var firstColumnIndex = UseLineNumbers ? 2 : 1;
                if (parts.Length == firstColumnIndex && parts[firstColumnIndex - 1] != null && parts[firstColumnIndex - 1] == "EOF")
                {
                    break;
                }

                var datum = new T();

                var start = UseLineNumbers ? 1 : 0;



                for (int i = start; i < parts.Length; i++)
                {
                    var value = parts[i];
                    var column = columns[i];

                    // continue of deviant RowNumber column condition
                    // this allows for the deserializer to implicitly ignore the RowNumber column
                    if (column.Equals(RowNumberColumnTitle) && !_properties.Any(a => a.Name.Equals(RowNumberColumnTitle)))
                    {
                        continue;
                    }

                    value = value
                        .Replace(Replacement, Separator.ToString())
                        .Replace(NewlineReplacement, Environment.NewLine).Trim();

                    var p = _properties.FirstOrDefault(a => a.Name.Equals(column, StringComparison.InvariantCultureIgnoreCase));

                    // ignore property csv column, Property not found on targing type
                    if (p == null)
                    {
                        continue;
                    }

                    if (UseTextQualifier)
                    {
                        if (value.IndexOf("\"", StringComparison.Ordinal) == 0)
                            value = value.Substring(1);

                        if (value[value.Length - 1].ToString() == "\"")
                            value = value.Substring(0, value.Length - 1);

                        if (p.PropertyType.IsNumericType())
                            if (value.IndexOf(",", StringComparison.Ordinal) > 0)
                            {
                                value = value.Replace(",", "");
                            }
                    }
                    var converter = TypeDescriptor.GetConverter(p.PropertyType);
                    var convertedValue = converter.ConvertFrom(value);

                    p.SetValue(datum, convertedValue);
                }

                data.Add(datum);
            }

            return data;
        }

        private async Task<Encoding> GetEncodingAsync(Stream stream)
        {
            // Read the BOM
            var bom = new byte[4];
            await stream.ReadAsync(bom, 0, 4);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.ASCII;
        }
    }

    public class CsvIgnoreAttribute : Attribute { }

    public class InvalidCsvFormatException : Exception
    {
        public InvalidCsvFormatException(string message)
            : base(message)
        {
        }

        public InvalidCsvFormatException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
