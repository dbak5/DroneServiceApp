// Author: DaHye Baker
// Student ID: 30063368
// Organisation: South Metropolitan TAFE
// Description: Drone Service Application to log drones for service and repair

// Q6.1 Separate class file

namespace DroneServiceApp
{
    public class Drone
    {
        // Q6.2 List of Class

        // Declare variables
        // ReSharper disable InconsistentNaming
        private string ClientName;
        private string DroneModel;
        private string ServiceProblem;
        private string ServicePriority;
        private double ServiceCost;
        private int ServiceTag;

        public Drone(string clientName, string droneModel, string serviceProblem, string servicePriority)
        {
            ClientName = clientName;
            DroneModel = droneModel;
            ServiceProblem = serviceProblem;
            ServicePriority = servicePriority;
        }

        // Constructor with all properties
        public Drone(string newClientName, string newDroneModel, string newServiceProblem, double newServiceCost, int newServiceTag, string newServicePriority)
        {
            ClientName = newClientName;
            DroneModel = newDroneModel;
            ServiceProblem = newServiceProblem;
            ServiceCost = newServiceCost;
            ServiceTag = newServiceTag;
            ServicePriority = newServicePriority;
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

        public void SetServicePriority(int newServicePriority)
        {
            ServiceTag = newServicePriority;
        }

        public string GetServicePriority()
        {
            return ServicePriority;
        }
        #endregion

    }
}
