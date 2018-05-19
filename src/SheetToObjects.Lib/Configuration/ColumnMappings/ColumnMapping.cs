﻿using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration.ColumnMappings
{
    public abstract class ColumnMapping
    {
        public abstract string DisplayName { get; }

        public bool IsRequired
        {
            get { return Rules.Exists(r => r is RequiredRule); }
        }

        public int ColumnIndex { get; protected set; }
        public string PropertyName { get; }
        public string Format { get; }
        public List<IRule> Rules { get; }

        protected ColumnMapping(string propertyName, string format, List<IRule> rules)
        {
            PropertyName = propertyName;
            Format = format;
            Rules = rules ?? new List<IRule>();
        }
    }
}
