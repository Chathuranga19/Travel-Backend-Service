using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.DBconfig;
using TransportManagmentSystemAPI.Interfaces;
using TransportManagmentSystemAPI.Models;
//Core Service -02 - Train Service managment 
namespace TransportManagmentSystemAPI.Services
{
    public class TrainScheduleManagementService : ITrainService
    {
        private readonly IMongoCollection<Train> _trainList;
        private readonly IMongoCollection<Schedule> _schedueList;
        private readonly IMongoCollection<Reservation> _reservationList;
        public TrainScheduleManagementService(IDatabaseSettings _databaseSettings, IScheam _scheam)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _trainList = database.GetCollection<Train>(_scheam.TrainScheam);
            _schedueList = database.GetCollection<Schedule>(_scheam.ScheduleScheam);
            _reservationList = database.GetCollection<Reservation>(_scheam.ReservationScheam);
        }
        public Train CreateTrainWithSchedule(Train train)
        {
            var ListoftheSchedules = train.ScheduleList;
            if (train.Id == null)
            {
                if (ListoftheSchedules != null && ListoftheSchedules.Count > 0)
                {
                    foreach (Schedule schedule in ListoftheSchedules)
                    {
                         _schedueList.InsertOne(schedule);
                    }
                }
                _trainList.InsertOne(train);
                return train;
            }
            else
            {
                if (ListoftheSchedules != null && ListoftheSchedules.Count > 0) {
                    foreach (Schedule schedule in ListoftheSchedules)
                    {
                        _schedueList.ReplaceOne(sch => sch.Id == schedule.Id, schedule);
                    }
                }
                if (train.IsCancelled)
                {
                    var trinReservationCount = _reservationList.Find(res => res.Id == train.Id).ToList().Count;
                    if (trinReservationCount > 0)
                    {
                        return null;
                    }
                }
                _trainList.ReplaceOne(tra => tra.Id == train.Id, train);
                return train;
            }
        }

        public List<Train> getAllActiveTrainswithSchedules()
        {
           return _trainList.Find(tra => tra.IsActive).ToList();
        }

        public string cancellingTrain(string id , Train train)
        {
            var trinReservationCount = _reservationList.Find(res => res.Train == id).ToList().Count;
            if (trinReservationCount > 0)
            {
                return "Already Reserved train";
            }
            else 
            {
                train.IsCancelled = true;
                _trainList.ReplaceOne(tra => tra.Id == train.Id, train);
                return "Train Cancelled";
            }
             
        }

        public Train AddNewScheduleForExisitingTrain(string trainId, Schedule schedule)
        {
            var Selectedtrain = _trainList.Find(tra => tra.Id == trainId).ToList().FirstOrDefault();

            if (Selectedtrain != null)
            {
                var extingList = Selectedtrain.ScheduleList;
                _schedueList.InsertOne(schedule);
                extingList.Add(schedule);
                _trainList.ReplaceOne(tra => tra.Id == trainId, Selectedtrain);

                return Selectedtrain;
            }
            else 
            {
                return null;
            }
        }
    }
}
