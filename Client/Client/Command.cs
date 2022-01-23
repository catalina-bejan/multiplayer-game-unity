using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class Command
    {
        public static string TYPE_GREETING = "hello";
        public static string DELIMITER = "<<!>>";
        public static string END_FLAG = "<<END>>";

        private static String from { get; set; }
        private String type { get; set; }
        private String text { get; set; }


        public static byte[] CreateMessage(string from, string type, string text)
        {
            string message = from + Command.DELIMITER + type + Command.DELIMITER + text + Command.END_FLAG;
            return Encoding
                .Unicode
                .GetBytes(message);
        }




        //public enum StatusType
        //{
        //    UsernameInvalid,
        //    Connecting,
        //    Connected
        //}

        //public string Username { get; set; }
        //public StatusType Status { get; set; }
        //public string IncompleteMessage { get; set; }

        //public User(string username)
        //{
        //    Username = username;
        //    Status = StatusType.Connecting;
        //    IncompleteMessage = null;
        //}

        //public User()
        //{
        //    Username = null;
        //    Status = StatusType.UsernameInvalid;
        //    IncompleteMessage = null;
        //}












        //public const string CommandDelim = "|";
        //public const string SubCommandDelim = ":";
        //public const string EndMessageDelim = "<##>";


        //public const string ValidateUsername = "ValUnam";
        //public const string Connect = "Cnct";
        //public const string Disconnect = "Discon";
        //public const string Logout = "Lgout";


        //public const string PublicMessage = "PubMsg";
        //public const string PrivateMessage = "PrivMsg";

        //public const string UserList = "UsLst";

        //public const string MalformedCommand = "MalComm";
        //public const string InvalidRequest = "IReq";

        //public const string Request = "Req";
        //public const string Deny = "Den";
        //public const string Accept = "Acc";
        //public const string Add = "Add";
        //public const string Remove = "Rem";
        //public const string None = "None";


        //public static byte[] CreateMessage(string command, string subcommand, string data)
        //{
        //    if (command == null) return null;
        //    if (subcommand == null) subcommand = None;
        //    if (data == null) data = None;

        //    return Encoding.Unicode.GetBytes(command + CommandDelim + subcommand + CommandDelim + data + EndMessageDelim);
        //}

        //public static Message DecodeMessage(string message)
        //{
        //    string[] parts = message.Split(new string[] { CommandDelim }, StringSplitOptions.None);

        //    if (parts.Length != 3) return new Message(MalformedCommand, None, None);
        //    return new Message(parts[0], parts[1], parts[2]);
        //}

        //public class Message
        //{
        //    public string Command { get; set; }
        //    public string Subcommand { get; set; }
        //    public string Data { get; set; }

        //    public Message(string command, string subcommand, string data)
        //    {
        //        this.Command = command;
        //        this.Subcommand = subcommand;
        //        this.Data = data;
        //    }
        //}


    }
}
