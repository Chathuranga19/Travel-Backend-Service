using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.DBconfig;
using TransportManagmentSystemAPI.Models;

namespace TransportManagmentSystemAPI.Services
{
    interface ITravallerService
    {
        TravallerProfile CreateUpdateTravellerProfile(TravallerProfile travallerProfile);
        List<TravallerProfile> DisplayAllActiveProfile(bool isActive);
        String DeletedTravellerProfile(String _Nic);
        TravallerProfile ManageActivationTravellerProfile(string nic ,TravallerProfile travallerProfile);
        TravallerProfile GetTravallerProfileByNic(string _Nic);
    }
}
