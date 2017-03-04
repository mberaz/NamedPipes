using System;

namespace Common
{
    [Serializable]
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return $"[{Id}: {Body}]";
        }
    }
}
