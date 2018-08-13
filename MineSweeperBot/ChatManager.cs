using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MineSweeperBot
{
    public static class ChatManager
    {
        public static List<string> commands;

        public static string COMMAND_OPEN_SMALL = "!o";
        public static string COMMAND_OPEN_CAPITAL = "!O";
        public static string COMMAND_FLAG_SMALL = "!f";
        public static string COMMAND_FLAG_CAPITAL = "!F";
        public static string COMMAND_CLEAR_AROUND_SMALL = "!c";
        public static string COMMAND_CLEAR_AROUND_CAPITAL = "!C";

        static ChatManager()
        {
            commands = new List<string>();
            commands.Add(ChatManager.COMMAND_OPEN_SMALL);
            commands.Add(ChatManager.COMMAND_OPEN_CAPITAL);
            commands.Add(ChatManager.COMMAND_FLAG_SMALL);
            commands.Add(ChatManager.COMMAND_FLAG_CAPITAL);
            commands.Add(ChatManager.COMMAND_CLEAR_AROUND_SMALL);
            commands.Add(ChatManager.COMMAND_CLEAR_AROUND_CAPITAL);
        }

        public static bool IsCommand (string message)
        {
            foreach (string x in commands)
            {
                if (message.StartsWith(x))
                    return true;
            }
            return false;
        }

        public static Point TranslateCommand (string message)
        {
            Point pt;
            int p1 = -1, p2 = -1;
            message = message.Replace(COMMAND_OPEN_SMALL, "");
            message = message.Replace(COMMAND_OPEN_CAPITAL, "");
            message = message.Replace(COMMAND_FLAG_SMALL, "");
            message = message.Replace(COMMAND_FLAG_CAPITAL, "");
            message = message.Replace(COMMAND_CLEAR_AROUND_SMALL, "");
            message = message.Replace(COMMAND_CLEAR_AROUND_CAPITAL, "");
            message = message.Replace(" ", "");

            try
            {
                string[] str = message.Split(',');
                Int32.TryParse(str[0], out p1);
                Int32.TryParse(str[1], out p2);

                pt = new Point(p1, p2);
            }
            catch { return new Point(-1,-1); }

            return pt;
        }

        public static int CommandType(string message)
        {
            int result = -1;
            foreach (string x in commands)
            {
                if (message.StartsWith(x))
                {
                    if (x == ChatManager.COMMAND_OPEN_SMALL || x == ChatManager.COMMAND_OPEN_CAPITAL)
                        result = 1;
                    if (x == ChatManager.COMMAND_FLAG_SMALL || x == ChatManager.COMMAND_FLAG_CAPITAL)
                        result = 2;
                    if (x == ChatManager.COMMAND_CLEAR_AROUND_SMALL || x == ChatManager.COMMAND_CLEAR_AROUND_CAPITAL)
                        result = 3;
                }
            }
            return result;
        }
    }
}
