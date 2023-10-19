using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.DBconfig;
using TransportManagmentSystemAPI.Interfaces;
using TransportManagmentSystemAPI.Models;
//Core Service -03 - Schedule Managment Service
namespace TransportManagmentSystemAPI.Services
{
    public class ScheduleServiceManagment : IScheduleService
    {
        private readonly IMongoCollection<Schedule> _schedueList;
        private readonly IMongoCollection<Train> _trainList;
        public ScheduleServiceManagment(IDatabaseSettings _databaseSettings, IScheam _scheam)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _schedueList = database.GetCollection<Schedule>(_scheam.ScheduleScheam);
            _trainList = database.GetCollection<Train>(_scheam.TrainScheam);
        }

        public Schedule AddNewScheduleForExisitingTrain(string trainId, Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public Schedule UpdateSchedule(string id, Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
