// Author: DaHye Baker
// Student ID: 30063368
// Organisation: South Metropolitan TAFE
// Description: Drone Service Application to log drones for service and repair

// Q6.1 Separate class file

using System.Collections.ObjectModel;

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

        public string GetClientName()
        {
            return ClientName;
        }

        public void SetDroneModel(string newDroneModel)
        {
            DroneModel = newDroneModel;
        }

        public string GetDroneModel()
        {
            return DroneModel;
        }

        public void SetServiceProblem(string newServiceProblem)
        {
            ServiceProblem = newServiceProblem;
        }

        public string GetServiceProblem()
        {
            return ServiceProblem;
        }

        public void SetServiceCost(double newServiceCost)
        {
            ServiceCost = newServiceCost;
            // validation for negatives can be done here
            // 15% extra cost here for express
        }

        public double GetServiceCost()
        {
            return ServiceCost;
        }

        public void SetServiceTag(int newServiceTag)
        {
            ServiceTag = newServiceTag;
        }

        public int GetServiceTag()
        {
            return ServiceTag;
        }

        public void SetServicePriority(string newServicePriority)
        {
            ServicePriority = newServicePriority;
        }

        public string GetServicePriority()
        {
            return ServicePriority;
        }
        #endregion

    }

}
