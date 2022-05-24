using System;
using System.Collections.Generic;
using System.Text;

namespace XafDTW.Helpers
{
    public static class EnumHelper
    {
        public static Output GetEnumItem<Output, Input>(Input input)
        {
            //Output type checking...
            if (typeof(Output).BaseType != typeof(Enum))
                throw new Exception("Output is not a enum type");

            //Input type checking: string type
            if (typeof(Input) == typeof(string))
                return (Output)Enum.Parse(typeof(Output), (dynamic)input);

            //Input type checking: Integer type
            if (typeof(Input) == typeof(Int16) ||
                typeof(Input) == typeof(Int32) ||
                typeof(Input) == typeof(Int64))

                return (Output)(dynamic)input;

            throw new Exception("Enum value not found");
        }
        public static Output GetEnumItemNew<Output>(string input)
        {
            //Output type checking...
            if (typeof(Output).BaseType != typeof(Enum))
                throw new Exception("Output is not a enum type");

            int tempint = 0;

            if (int.TryParse(input, out tempint))
                return (Output)(dynamic)tempint;

            return (Output)Enum.Parse(typeof(Output), (dynamic)input);

            throw new Exception("Enum value not found");
        }

    }
}
