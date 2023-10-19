using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportManagmentSystemAPI.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ReferenceId { get; set; }
        public string TravallerName { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TravallerProfile { get; set; }
        public string PhoneNumber { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Train { get; set; }
        public int NoOfPassenger { get; set; }
        public string EmailAddress { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime BookingCreatedDate { get; set; }
        public bool IsCancelled { get; set; }

        [BsonIgnore]
        public Train BookedTrain { get; set; }

        [BsonIgnore]
        public TravallerProfile BookedTravallerProfile { get; set; }
    }
}
