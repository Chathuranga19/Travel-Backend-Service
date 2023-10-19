using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.DBconfig;
using TransportManagmentSystemAPI.Interfaces;
using TransportManagmentSystemAPI.Models;
//Core Service -04 -Reservation managment 
namespace TransportManagmentSystemAPI.Services
{
    public class ReservationService : IReservation
    {
        private readonly IMongoCollection<Reservation> _reservationList;
        private readonly IMongoCollection<Train> _trainList;
        public ReservationService(IDatabaseSettings _databaseSettings, IScheam _scheam)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _reservationList = database.GetCollection<Reservation>(_scheam.ReservationScheam);
            _trainList = database.GetCollection<Train>(_scheam.TrainScheam);

        }
        public Dictionary<int,string> CreateReservation(Reservation reservation)
        {
            Dictionary<int, string> returnCode = new Dictionary<int, string>();
            var DateToday = DateTime.Now;
            var ReservationDate = reservation.ReservationDate;

            TimeSpan timeDifferenceWhenReserve = ReservationDate - DateToday;

            if (reservation.Id != null)
            {
                Reservation existingReservationforUpdate = _reservationList.Find(exres => exres.Id == reservation.Id).FirstOrDefault();
                TimeSpan timeDiff = existingReservationforUpdate.ReservationDate - DateToday;

                if (timeDiff.TotalDays < 5)
                {
                    returnCode.Add(400, "Your Reservation confirmed and this cannot be updated/Cancelled");
                    return returnCode;
                }
                else if (timeDifferenceWhenReserve.TotalDays < 0)
                {
                    returnCode.Add(400, "Invalid reservation date. Please choose a valid date.");
                    return returnCode;
                }
                else
                {
                    reservation.BookingCreatedDate = DateToday;
                    _reservationList.ReplaceOne(res => res.Id == reservation.Id, reservation);
                    returnCode.Add(200, "Reservation Updated successfully" + reservation.Id);
                    return returnCode;
                }
            }
            else
            {
                if (timeDifferenceWhenReserve.TotalDays >= 30 || timeDifferenceWhenReserve.TotalDays < 0)
                {
                    returnCode.Add(400, "Reservations must be made within 30 days from the booking date");
                    return returnCode;
                }
                else
                {
                    var existingReservationB = _reservationList
                    .Find(res => res.ReferenceId == reservation.ReferenceId && !res.IsCancelled)
                    .ToList();
                    var validCount = 0;
                    foreach(Reservation ex in existingReservationB)
                    {
                        TimeSpan timediffEx = ex.ReservationDate - DateToday;
                        if (timediffEx.TotalDays > 0)
                        {
                            validCount++;
                        }
                    }

                    if (validCount >= 4)
                    {
                        returnCode.Add(400, "Maximum 4 reservation per reference ID");
                        return returnCode;
                    }
                    else 
                    {
                        reservation.BookingCreatedDate = DateToday;
                        _reservationList.InsertOne(reservation);
                        returnCode.Add(200, "Reservation successfully created " + reservation.Id);
                        return returnCode;

                    }
                }
            }
        }

        public List<Reservation> DisplayAllReservation(string travallerId)
        {
            var listofBookings = travallerId != null ? _reservationList.Find(res => res.ReferenceId == travallerId).ToList() : _reservationList.Find(res => true).ToList();
            int index = 0;
            foreach (Reservation res in listofBookings)
            {
                index = index + 1;
                Train gottrain = _trainList.Find(tra => tra.Id == res.Train).ToList().FirstOrDefault();
                listofBookings[index - 1].BookedTrain = gottrain;
            }
            if (listofBookings.Count > 0)
            {
                return listofBookings;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<int, string> CancelledReservation(string id , Reservation reservation)
        {
            Dictionary<int, string> cancellingStat = new Dictionary<int, string>();
            if (reservation != null)
            {
               Reservation existingReservation = _reservationList.Find(exres => exres.Id == id).FirstOrDefault();
               TimeSpan timeDiff = existingReservation.ReservationDate - DateTime.Now;

                if (timeDiff.TotalDays >= 5)
                {
                    var Cancelled = Builders<Reservation>.Update.
                          Set(res => res.IsCancelled, reservation.IsCancelled);

                    var updatedProfile = _reservationList.UpdateOne(reser => reser.Id == id, Cancelled);
                    cancellingStat.Add(100, "Reservation cancelled REF" + existingReservation.ReferenceId);
                    return cancellingStat;
                }
                else
                {
                    cancellingStat.Add(500, "Your Reservation is confirmed and this cannot be Cancelled");
                    return cancellingStat;
                }
            }
            else 
            {
                return null;
            }
        
        }

        // Method to delete a reservation.
        public Dictionary<int, string> DeleteReservation(string id)
        {
            Dictionary<int, string> deletionStatus = new Dictionary<int, string>();

            Reservation existingReservation = _reservationList
                .Find(exres => exres.Id == id)
                .FirstOrDefault();
            if (existingReservation != null)
            {
                // Check if the reservation can be deleted (at least 5 days before the reservation date)
                TimeSpan timeDiff = existingReservation.ReservationDate - DateTime.Now;
                if (timeDiff.TotalDays >= 5)
                {
                    try
                    {
                        var deletionResult = _reservationList.DeleteOne(reser => reser.Id == id);
                        deletionStatus.Add(200, "Reservation deleted successfully");
                    }
                    catch (Exception)
                    {
                        deletionStatus.Add(500, "Failed to delete");
                    }
                }
                else
                {
                    deletionStatus.Add(
                        400,
                        "Cannot delete the reservation as it is confirmed or less than 5 days before the reservation date."
                    );
                }
            }
            else
            {
                deletionStatus.Add(404, "Reservation not found");
            }

            return deletionStatus;
        }
    }
}
