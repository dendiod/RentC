using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    internal class ReadFromConsole
    {
        internal int ReadInt(string message)
        {
            Console.Write(message);
            int num;
            int.TryParse(Console.ReadLine().Trim(), out num);
            return num;
        }

        internal string ReadString(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        internal DateTime ReadDate(string message)
        {
            DateTime date;
            Console.Write(message);
            DateTime.TryParseExact(Console.ReadLine().Trim(),
                "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }

        internal int? ReadIntOptional(string message)
        {
            int num;
            Console.Write(message);
            string line = Console.ReadLine().Trim();
            if (line == "")
            {
                return null;
            }

            int.TryParse(line, out num);

            return num;
        }

        internal string ReadStringOptional(string message)
        {
            string str = ReadString(message);
            if (str.Trim() == "")
            {
                str = null;
            }

            return str;
        }

        internal DateTime? ReadDateOptional(string message)
        {
            DateTime date;
            Console.Write(message);
            string line = Console.ReadLine().Trim();
            if (line == "")
            {
                return null;
            }

            DateTime.TryParseExact(line,
                "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            return date;
        }
    }
}
