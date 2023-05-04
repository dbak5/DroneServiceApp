// Author: DaHye Baker
// Student ID: 30063368
// Organisation: South Metropolitan TAFE
// Description: Drone Service Application to log drones for service and repair

// Q6.1 Separate class file

using System;

namespace DroneServiceApp
{
    public class Drone
    {
        // Q6.2 List of Class

        // Declare variables
        // ReSharper disable InconsistentNaming
        public string ClientName { get; private set; }
        public string DroneModel { get; private set; }
        public string ServiceProblem { get; private set; }
        public string ServicePriority { get; private set; }
        public double ServiceCost { get; private set; }
        public int ServiceTag { get; private set; }

        public Drone()
        {
      
        }

        // Constructor with all properties
        public Drone(string clientName, string droneModel, string serviceProblem, double serviceCost, int serviceTag, string servicePriority)
        {
            ClientName = clientName;
            DroneModel = droneModel;
            ServiceProblem = serviceProblem;
            ServiceCost = serviceCost;
            ServiceTag = serviceTag;
            ServicePriority = servicePriority;
        }

        #region Getters and Setters

        public void SetClientName(string newClientName)
        {
            ClientName = newClientName;
            // capitalisaing can be done here
        }

        public void SetDroneModel(string newDroneModel)
        {
            DroneModel = newDroneModel;
        }

        public void SetServiceProblem(string serviceProblem)
        {
            ServiceProblem = serviceProblem;
        }

        public void SetServiceCost(double serviceCost)
        {
            ServiceCost = serviceCost;
        }

        public void SetServiceTag(int serviceTag)
        {
            switch (serviceTag)
            {
                case < 100: throw new Exception("Service tag cannot be less than 100");
                case > 900: throw new Exception("Service tag cannot be more than 900");
                default:
                    ServiceTag = serviceTag;
                    break;
            }
        }

        public void SetServicePriority(string servicePriority)
        {
          
            ServicePriority = servicePriority;
        }

        #endregion

    }

}
