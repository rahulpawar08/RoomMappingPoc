using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Tool.HBRoomCodesExtractor.Models
{
    public class RootObject
    {
        public int from { get; set; }
        public int to { get; set; }
        public int total { get; set; }
        public AuditData auditData { get; set; }
        public List<Room> rooms { get; set; }
    }

    public class AuditData
    {
        public string processTime { get; set; }
        public string timestamp { get; set; }
        public string requestHost { get; set; }
        public string serverId { get; set; }
        public string environment { get; set; }
        public string release { get; set; }
    }

    public class TypeDescription
    {
        public string content { get; set; }
    }

    public class CharacteristicDescription
    {
        public string content { get; set; }
    }

    public class Room
    {
        public string code { get; set; }
        public string type { get; set; }
        public string characteristic { get; set; }
        public int minPax { get; set; }
        public int maxPax { get; set; }
        public int maxAdults { get; set; }
        public int maxChildren { get; set; }
        public int minAdults { get; set; }
        public string description { get; set; }
        public TypeDescription typeDescription { get; set; }
        public CharacteristicDescription characteristicDescription { get; set; }
    }
}
