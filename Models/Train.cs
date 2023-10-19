using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportManagmentSystemAPI.Models
{
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TrainId { get; set; }
        public string TrainName { get; set; }

        public int NumberOfComponents { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsActive { get; set; }
        public List<Schedule> ScheduleList { get; set; }

    }
}
