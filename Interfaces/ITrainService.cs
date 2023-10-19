using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.Models;

namespace TransportManagmentSystemAPI.Interfaces
{
    public interface ITrainService
    {
        Train CreateTrainWithSchedule(Train train);
        List<Train> getAllActiveTrainswithSchedules();
        string cancellingTrain(string id , Train train);

        Train AddNewScheduleForExisitingTrain(string trainId, Schedule schedule);

    }
}
