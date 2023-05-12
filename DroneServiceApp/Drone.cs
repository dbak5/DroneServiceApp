// Author: DaHye Baker
// Student ID: 30063368
// Organisation: South Metropolitan TAFE
// Description: Drone Service Application to log drones for service and repair

// Q6.1 Separate class file

using System;
using System.Globalization;

namespace DroneServiceApp
{
    // Q6.1 Create a separate class file to hold the data items of the Drone.
    // Use separate getter and setter methods, ensure the attributes are private and the accessor methods are public. 
    public class Drone
    {
        #region Properties
        // ReSharper disable InconsistentNaming
        private string ClientName;
        private string DroneModel;
        private string ServiceProblem;
        private string ServicePriority;
        private double ServiceCost;
        private int ServiceTag;
        #endregion

        #region Constructors
        public Drone()
        {
        }

        public Drone(string clientName, string droneModel, string serviceProblem, double serviceCost, int serviceTag, string servicePriority)
        {
            ClientName = clientName;
            DroneModel = droneModel;
            ServiceProblem = serviceProblem;
            ServiceCost = serviceCost;
            ServiceTag = serviceTag;
            ServicePriority = servicePriority;
        }

        #endregion

        #region Getters and Setters

        // Add suitable code to the Client Name accessor method so the data is formatted as Title case
        public void SetClientName(string newClientName)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            ClientName =  textInfo.ToTitleCase(newClientName.ToLower());
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

        // Add suitable code to the Service Problem accessor method so the data is formatted as sentence case
        public void SetServiceProblem(string serviceProblem)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            ServiceProblem = textInfo.ToTitleCase(serviceProblem[0].ToString()) + serviceProblem[1..].ToLower();
        }

        public string GetServiceProblem()
        {
            return ServiceProblem;
        }

        public void SetServiceCost(double serviceCost)
        {
            if (serviceCost < 0)
            {
                ServiceCost = 100;
            }
            ServiceCost = serviceCost;
        }

        public double GetServiceCost()
        {
            return ServiceCost;
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

        public int GetServiceTag()
        {
            return ServiceTag;

        }

        public void SetServicePriority(string servicePriority)
        {
          
            ServicePriority = servicePriority;
        }

        public string GetServicePriority()
        {
            return ServicePriority;

        }

        #endregion
    }
}
