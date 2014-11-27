using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;
using Microsoft.AspNet.SignalR.Hubs;

namespace SpeedTrapper
{
    [HubName("speedTrapperHub")]
    public class speedTrapperHub : Hub
    {
       static Dictionary<string,int> runningCarDetails = new Dictionary<string,int>();
       static string serverId;
        public void GetCarDetails(bool isServer)
        {
            List<string>objConnectionsId;
            if (!isServer)
            {
                string[] cars = new string[] { "Lincoln", "Jaguar", "Tesla", "Tata" };
                Random objRandom = new Random();
                int speed = objRandom.Next(10)*100+100;
                string car = cars[objRandom.Next(cars.Length)];
                objConnectionsId = new List<string>();
                objConnectionsId.Add(Context.ConnectionId);
                objConnectionsId.Add(serverId);
                runningCarDetails.Add(Context.ConnectionId, speed);
                Clients.Clients(objConnectionsId).ShowCarDetails(car, speed);
            }
            else
                serverId = Context.ConnectionId;
        }

        public void TrappSpeed(int speed)
        {
            var overSpeedCars = runningCarDetails.Where(o => o.Value >= speed).ToList();
            foreach (var detail in overSpeedCars)
            {
                    Clients.Client(detail.Key).TrapSpeed(speed);
                    runningCarDetails.Remove(detail.Key);
            }
            
            Clients.Client(serverId).TrapSpeed(speed);
            
        }
    }
}