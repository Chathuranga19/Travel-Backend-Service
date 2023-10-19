using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportManagmentSystemAPI.DBconfig
{
    public interface IScheam
    {
        public string UsersScheama { get; set; }
        public string TravellerScheama { get; set; }
        public string TrainScheam { get; set; }
        public string ReservationScheam { get; set; }
        public string ScheduleScheam { get; set; }


    }
}
