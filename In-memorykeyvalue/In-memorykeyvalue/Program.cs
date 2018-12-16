using System;
using System.Collections.Generic;
using System.Linq;

namespace In_memorykeyvalue
{
    class Program
    {
        private static readonly AvailableCommands _commands = new AvailableCommands();
        static void Main(string[] args)
        {
#if DEBUG
            TestLoad();
#endif
            while (true)
            {
                var commands = GetAllCommands();
                Console.Write("Available commands: {0}", string.Join("; ", commands));
                Console.WriteLine("; Quit");
                Console.WriteLine("Input command (only), then press Enter:");
                var inputcmd = Console.ReadLine().Trim();
                if (inputcmd.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    break;
                TryCommandRun(inputcmd);
            }
        }
        private static IEnumerable<string> GetAllCommands()
        {
            var commandsType = _commands.GetType();
            var allMethods = commandsType.GetMethods();
            return allMethods
                .Where(m => m.DeclaringType.Equals(commandsType))
                .Select(m => m.Name);
        }
        private static void TryCommandRun(string inputcmd)
        {
            var matchMethod = _commands.GetType().GetMethods()
                .Where(m => m.Name.Equals(inputcmd, StringComparison.OrdinalIgnoreCase))
                .SingleOrDefault();
            try
            {
                if (matchMethod != null)
                    matchMethod.Invoke(_commands, null);
                else
                    throw new CtrlException(CtrlException.errmsg.notacommand, inputcmd);
            }
            catch (CtrlException cx)
            {
                Console.WriteLine(cx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
#if DEBUG
        static void TestLoad()
        {
            _commands.keyValuePairs.Add("A", "0 ");
            _commands.keyValuePairs.Add("B", "1 ");
            _commands.keyValuePairs.Add("C", "2 ");
            _commands.keyValuePairs.Add("D", "3 ");
            _commands.keyValuePairs.Add("E", "4 ");
            _commands.keyValuePairs.Add("F", "5 ");
            _commands.keyValuePairs.Add("G", "6 ");
            _commands.keyValuePairs.Add("H", "7 ");
            _commands.keyValuePairs.Add("I", "8 ");
            _commands.keyValuePairs.Add("J", "9 ");
            _commands.keyValuePairs.Add("K", "10");
            _commands.keyValuePairs.Add("L", "11");
            _commands.keyValuePairs.Add("M", "12");
            _commands.keyValuePairs.Add("N", "13");
            _commands.keyValuePairs.Add("O", "14");
            _commands.keyValuePairs.Add("P", "15");
            _commands.keyValuePairs.Add("Q", "16");
            _commands.keyValuePairs.Add("R", "17");
            _commands.keyValuePairs.Add("S", "18");
            _commands.keyValuePairs.Add("T", "19");
            _commands.keyValuePairs.Add("U", "20");
        }
#endif
    }
}