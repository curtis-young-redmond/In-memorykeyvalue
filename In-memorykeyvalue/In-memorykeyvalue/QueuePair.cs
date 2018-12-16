using System;

namespace In_memorykeyvalue
{
    public class KVPair
    {
        public enum actiontype { none, upsert, delete, upsertref };
        public KVPair(string key = "", string value = "")
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
        public KVPair(string key = "", actiontype actionType = actiontype.none)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ActionType = actionType;
        }
        public KVPair(string key = "", string value = "", actiontype actionType = actiontype.none) : this(key, value)
        {
            ActionType = actionType;
        }
        public string Key { get; set; }
        public string Value { get; set; }
        public actiontype ActionType { get; set; }
    }
}
