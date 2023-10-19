using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.Models;

namespace TransportManagmentSystemAPI.Interfaces
{
    public interface IReservation
    {
        Dictionary<int, string> CreateReservation (Reservation reservation);
        List<Reservation> DisplayAllReservation(string travallerId);
        Dictionary<int, string> CancelledReservation(string id, Reservation reservation);

    }
}
