using System;
using System.Runtime.Serialization;

namespace In_memorykeyvalue
{
    public class CtrlException : Exception
    {
        public CtrlException(CtrlException.errmsg msgid) : base(ErrorMsg[(int)msgid]) { }
        public CtrlException(CtrlException.errmsg msgid, string param1) : base(string.Format(ErrorMsg[(int)msgid], param1)) { }
        public CtrlException(CtrlException.errmsg msgid, string param1, string param2) : base(string.Format(ErrorMsg[(int)msgid], param1, param2)) { }
        public CtrlException(string message) : base(message) { }
        public CtrlException()
        {
        }
        public CtrlException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected CtrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        static string[] ErrorMsg =
        {
            "nothing to process\r\n",
            "Not a keyvalue pair\r\n",
            "Reference {0} doesn't exist\r\n",
            "No entry found for {0}\r\n",
            "No reference entry found for {0}\r\n",
            "Can NOT commit because no begin\r\n",
            "{0} is not a command\r\n"
        };
        public enum errmsg { none2process = 0, notkeyvalue = 1, refnoexist = 2, noentryfound = 3, norefentyfound = 4, acommit = 5, notacommand = 6 }
    }
}
