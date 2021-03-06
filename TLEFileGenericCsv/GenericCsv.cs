﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using ITLEFileSpec;

namespace TLEFileGenericCsv
{
    public class GenericCsv : IFileSpec
    {
        public GenericCsv()
        {
            TaggedLines = new List<int>();

            DataList = new BindingList<dynamic>();

            ExpectedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Author => "Eric Zimmerman";
        public string FileDescription => "Generic CSV/TSV plugin. Does not add Tag or Line number columns";
        public HashSet<string> ExpectedHeaders { get; }

        public IBindingList DataList { get; private set; }
        public List<int> TaggedLines { get; set; }
        public string InternalGuid => "40dd7405-16cf-4612-a480-9a050d0a9952";

        public void ProcessFile(string filename)
        {
            using (var fileReader = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                using (var ff = new StreamReader(fileReader))
                {
                    var csv = new CsvReader(ff, CultureInfo.InvariantCulture);
                    csv.Configuration.BadDataFound = null;


                    if (filename.ToUpperInvariant().EndsWith(".TSV"))
                    {
                        csv.Configuration.Delimiter = "\t";
                    }

                    var records = csv.GetRecords<dynamic>().ToList();

                    DataList = new BindingList<dynamic>(records);
                }
            }
        }
    }
}