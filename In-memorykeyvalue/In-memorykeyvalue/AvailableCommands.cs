using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace In_memorykeyvalue
{
    public sealed class AvailableCommands
    {
        #region Containers
        public Stack<Queue<KVPair>> TransStack = new Stack<Queue<KVPair>>();
        //public Queue<KVPair> TransKeyValuePairs = new Queue<KVPair>();
        public Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        #endregion
        #region local properties
        private int begin { get; set; }
        //bool commit { get; set; }
        //private bool rollback { get; set; }
        #endregion
        #region Helper functions
        private bool ReturnPair(string TextPair, out KVPair pair)
        {
            char[] sep = { ' ' };
            var strPairArray = TextPair.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            if (strPairArray.Length == 0)
            {
                //throw new CtrlException(CtrlException.errmsg.none2process);
                pair = new KVPair("", "");
                return false;
            }
            if (strPairArray.Length != 2)
            {
                //throw new CtrlException( CtrlException.errmsg.notkeyvalue);
                pair = new KVPair("", "");
                return false;
            }
            pair = new KVPair(strPairArray[0], strPairArray[1]);
            return true;
        }
        #endregion
        #region commands section
        public void Put()
        {
#if DEBUG
            while (true)
            {
#endif
                try
                {
                    Console.WriteLine("{0}Enter <key> and <single word Value>  to save.", begin > 0 ? "ACTIVE TRANSACTION level: " + begin.ToString("# ") : "");
                    var stringPair = Console.ReadLine().Trim();
                    if (begin > 0)
                    {
                        KVPair console2kvpair = null;
                        if (!ReturnPair(stringPair, out console2kvpair)) return;
                        //var console2kvpair = ReturnPair(stringPair);
                        KVPair kvPair = new KVPair(console2kvpair.Key, console2kvpair.Value, KVPair.actiontype.upsert);
                        //TransKeyValuePairs.Enqueue(kvPair);
                        TransStack.Peek().Enqueue(kvPair);
                    }
                    else
                    {
                        KVPair console2kvpair = null;
                        if (!ReturnPair(stringPair, out console2kvpair)) return;
                        //var console2kvpair = ReturnPair(stringPair);
                        KVPair kvPair = new KVPair(console2kvpair.Key, console2kvpair.Value, KVPair.actiontype.upsert);
                        if (keyValuePairs.ContainsKey(kvPair.Key))
                            keyValuePairs[kvPair.Key] = kvPair.Value;
                        else
                            keyValuePairs.Add(kvPair.Key, kvPair.Value);
                    }
                }
                catch (CtrlException ex) { Console.WriteLine(ex.Message); }
#if DEBUG
            }
#endif
        }
        public void Delete()
        {
            Console.WriteLine("{0}Enter <key> of entry to delete.", begin > 0 ? "ACTIVE TRANSACTION level: " + begin.ToString() : "");
            var Refkey = Console.ReadLine().Trim();
            //if (Refkey.Trim() == "") return;
            if (begin > 0)
            {
                KVPair kvPair = new KVPair(Refkey, KVPair.actiontype.delete);
                TransStack.Peek().Enqueue(kvPair);
            }
            else
            if (keyValuePairs.ContainsKey(Refkey))
                keyValuePairs.Remove(Refkey);
        }
        public void Putref()
        {
            try
            {
                Console.WriteLine("{0}Enter <key> and <value as an existing key> to save a reference", begin > 0 ? "ACTIVE TRANSACTION level: " + begin.ToString() : "");
                var keyValue = Console.ReadLine().Trim();
                KVPair console2kvpair = null;
                if (!ReturnPair(keyValue, out console2kvpair)) return; ;
                if (begin > 0)
                {
                    KVPair kvPair = new KVPair(console2kvpair.Key, console2kvpair.Value, KVPair.actiontype.upsertref);
                    TransStack.Peek().Enqueue(kvPair);
                }
                else
                {
                    if (keyValuePairs.ContainsKey(console2kvpair.Key))
                    {
                        var vkey = keyValuePairs[console2kvpair.Key];
                        if (keyValuePairs.ContainsKey(vkey))
                        {
                            keyValuePairs[vkey] = console2kvpair.Value;
                        }
                        else
                            //throw new CtrlException(CtrlException.errmsg.refnoexist/*string.Format("Reference {0} doesn't exist", vkey)*/, CtrlException.disposition.error);
                            throw new CtrlException(CtrlException.errmsg.refnoexist);
                    }
                    else
                        throw new CtrlException(CtrlException.errmsg.refnoexist, console2kvpair.Key);
                }
            }
            catch (CtrlException cx)
            {
                Console.WriteLine(cx.Message);
            }


        }
        public void Get()
        {
            try
            {

                Console.WriteLine("enter <key> to display <value>. ");
                var keyValue = Console.ReadLine().Trim();
#if DEBUG
                if (keyValue == "")
                    foreach (var item in keyValuePairs) { Console.WriteLine(item.Key + " " + item.Value); }
                else
#endif
                {
                    var KeyValue = keyValuePairs.FirstOrDefault(x => x.Key == keyValue);
                    if (KeyValue.Key == null)
                        throw new CtrlException(CtrlException.errmsg.noentryfound, keyValue);
                    Console.WriteLine("{0}\r\n", KeyValue);
                }
            }
            catch (CtrlException cx)
            {
                Console.WriteLine("{0}\r\n", cx.Message);

            }
        }
        public void Getref()
        {
            try
            {
                string valueref = null;
                string refvalue = null;
                Console.WriteLine("Enter <key> to get a value from a referencing key.");
                var refkey = Console.ReadLine().Trim();
                if (keyValuePairs.ContainsKey(refkey))
                {
                    valueref = keyValuePairs[refkey];
                    if (keyValuePairs.ContainsKey(valueref))
                        refvalue = keyValuePairs[valueref];
                    else
                        throw new CtrlException(CtrlException.errmsg.norefentyfound, valueref);
                }
                else
                {
                    throw new CtrlException(CtrlException.errmsg.norefentyfound, refkey);
                }
                Console.WriteLine("[{0}, {1}]\r\n", refkey, refvalue);

            }
            catch (CtrlException cx)
            {
                Console.WriteLine(cx.Message);
            }
        }
        public void Begin()
        {
            var queueitem = new Queue<KVPair>();
            TransStack.Push(queueitem);
            begin = TransStack.Count;
            Console.WriteLine("In Active Transaction");
        }
        public void Commit()
        {
            try
            {
                if (begin <= 0) throw new CtrlException(CtrlException.errmsg.acommit);
#if DEBUG
                Console.WriteLine("Commiting Transaction");
#endif
                var TransKeyValuePairs = TransStack.Pop();
                if (TransStack.Count > 0)
                {
                    foreach (var kv in TransKeyValuePairs)
                        TransStack.Peek().Enqueue(kv);
                    begin = TransStack.Count;
                    return;
                }
                begin = TransStack.Count;
                //commit = true;
                foreach (var kvPair in TransKeyValuePairs)
                {
                    if (kvPair.ActionType == KVPair.actiontype.upsert)
                        if (keyValuePairs.ContainsKey(kvPair.Key))
                            keyValuePairs[kvPair.Key] = kvPair.Value;
                        else
                            keyValuePairs.Add(kvPair.Key, kvPair.Value);
                    if (kvPair.ActionType == KVPair.actiontype.delete)
                        if (keyValuePairs.ContainsKey(kvPair.Key))
                            keyValuePairs.Remove(kvPair.Key);
                        else
                            Console.WriteLine("{0} was not found in the database", kvPair.Key);
                    if (kvPair.ActionType == KVPair.actiontype.upsertref)

                        if (keyValuePairs.ContainsKey(kvPair.Key))
                        {
                            var valueAsKeyRef = keyValuePairs[kvPair.Key];
                            if (keyValuePairs.ContainsKey(valueAsKeyRef))
                                keyValuePairs[valueAsKeyRef] = kvPair.Value;
                        }
                }

            }
            catch (CtrlException cx)
            {
                Console.WriteLine(cx.Message);
            }
        }
        public void Rollback()
        {
            Queue<KVPair> TransKeyValuePairs = null;
#if DEBUG
            Console.WriteLine("Rolling back Transaction");
#endif
            begin = TransStack.Count;
            if (begin > 0)
                TransKeyValuePairs = TransStack.Pop();
            //rollback = false;
        }
        #endregion
    }
}
