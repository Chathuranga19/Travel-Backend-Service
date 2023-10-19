using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportManagmentSystemAPI.DBconfig;
using TransportManagmentSystemAPI.Models;

//Core Service -01 - travaller profile managment 
namespace TransportManagmentSystemAPI.Services
{
    public class TravallerProfileService : ITravallerService
    {
        private readonly IMongoCollection<TravallerProfile> _travalProfileList;
        private readonly IMongoCollection<User> _userList;
        private readonly IMongoDatabase _database;
        public TravallerProfileService(IDatabaseSettings _databaseSettings,IScheam _scheam)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _travalProfileList = database.GetCollection<TravallerProfile>(_scheam.TravellerScheama);
            _userList = database.GetCollection<User>(_scheam.UsersScheama);
        }
        public TravallerProfile CreateUpdateTravellerProfile(TravallerProfile travallerProfile)
        {
            try
            {
                //update process
                if (travallerProfile.Id != null)
                {
                    //profile update
                    var update = Builders<TravallerProfile>.Update.
                        Set(upf => upf.FirstName, travallerProfile.FirstName)
                        .Set(upf => upf.LastName, travallerProfile.LastName)
                        .Set(upf => upf.PhoneNumber, travallerProfile.PhoneNumber);

                    var updatedProfile = _travalProfileList.UpdateOne(trav => trav.Id == travallerProfile.Id, update);

                    if (travallerProfile?.UserInfo != null && travallerProfile.UserInfo.Password != null)
                    {
                        var updatePassword = Builders<User>.Update.
                            Set(upf => upf.Password, travallerProfile.UserInfo.Password);
                        var passwordReset = _userList.UpdateOne(up => up.Nic == travallerProfile.Nic, updatePassword);
                    }
                    
                    return travallerProfile;
                }
                else
                {
                    //creating process
                    var uniqueCounts = _travalProfileList.Find(trv => trv.Nic == travallerProfile.Nic).ToList().Count;
                    if (uniqueCounts == 0)
                    {
                        travallerProfile.CreatedDate = DateTime.Now;
                        travallerProfile.UserInfo.Nic = travallerProfile.Nic;
                        _userList.InsertOne(travallerProfile.UserInfo);
                        _travalProfileList.InsertOne(travallerProfile);
                        return travallerProfile;
                    }
                    else
                    {
                        return null;
                    }
                }
                
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong ERRLOGGED ! " + e.ToString());
            }
            
        }

        public String DeletedTravellerProfile(String _Nic)
        {
            try
            {
                 _travalProfileList.DeleteOne(trv => trv.Nic == _Nic);
                return _Nic;

            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong ERRLOGGED ! " + e.ToString());
            }
        }

        public List<TravallerProfile> DisplayAllActiveProfile(bool isActive)
        {
            try
            {
                var profileList = _travalProfileList.Find(trav => trav.AccStatus == isActive).ToList();

                List<TravallerProfile> secureProfileList = profileList.Select(item =>
                {
                    item.UserInfo = null;
                    return item;
                }
                ).ToList();

                return secureProfileList.Count > 0 ? secureProfileList : null; 
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong ERRLOGGED ! " + e.ToString());
            }
        }

        public TravallerProfile ManageActivationTravellerProfile(string nic , TravallerProfile travallerProfile)
        {
            try
            {
                if (nic != null)
                {
                    var updatedStatus = Builders<TravallerProfile>.Update.
                           Set(upf => upf.AccStatus, travallerProfile.AccStatus);
                    _travalProfileList.UpdateOne(trav => trav.Nic == nic, updatedStatus);
                    return travallerProfile;
                }
                else 
                {
                    return null;
                }
               
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong ERRLOGGED ! " + e.ToString());
            }
        }

        public TravallerProfile GetTravallerProfileByNic(string _Nic)
        {
            var profile = _travalProfileList.Find(pro => pro.Nic == _Nic).ToList().FirstOrDefault();
            if (profile != null)
            {
                return profile;
            }
            else 
            {
                return null;
            }
        }
    }
}
