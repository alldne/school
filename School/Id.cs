using System;
using System.Collections.Generic;

namespace School
{
    public class Id
    {
        private static Dictionary<string, Id> dict = new Dictionary<string, Id>();

        private string idString;

        // FIXME: Perform validation here.
        public static Id id(string idString)
        {
            Id id = null;
            dict.TryGetValue(idString, out id);
            if (id != null)
                return id;

            id = new Id(idString);
            dict.Add(idString, id);
            return id;
        }

        private Id(string idString)
        {
            this.idString = idString;
        }

        public override string ToString()
        {
            return idString;
        }
    }
}
