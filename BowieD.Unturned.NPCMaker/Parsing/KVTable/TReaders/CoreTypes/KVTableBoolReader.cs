﻿using System;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableBoolReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            string text = reader.readValue();
            if (text == null)
            {
                return false;
            }
            if (text.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (text.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (text.Equals("0", StringComparison.OrdinalIgnoreCase) || text.Equals("no", StringComparison.OrdinalIgnoreCase) || text.Equals("n", StringComparison.OrdinalIgnoreCase) || text.Equals("f", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (text.Equals("1", StringComparison.OrdinalIgnoreCase) || text.Equals("yes", StringComparison.OrdinalIgnoreCase) || text.Equals("y", StringComparison.OrdinalIgnoreCase) || text.Equals("t", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
