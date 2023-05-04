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
    public class Drone
    {
        // CHECK CANT GET DISPLAY IN LIST VIEW IF PROPERTIES ARE PRIVATE
        #region Properties
        // ReSharper disable InconsistentNaming
        public string ClientName { get; private set; }
        public string DroneModel { get; private set; }
        public string ServiceProblem { get; private set; }
        public string ServicePriority { get; private set; }
        public double ServiceCost { get; private set; }
        public int ServiceTag { get; private set; }
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

        // Use separate getter and setter methods, ensure the attributes are private and the accessor methods are public. 
        #region Getters and Setters

        // Add suitable code to the Client Name accessor method so the data is formatted as Title case
        public void SetClientName(string newClientName)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            ClientName =  textInfo.ToTitleCase(newClientName.ToLower());
        }

        public void SetDroneModel(string newDroneModel)
        {
            DroneModel = newDroneModel;
        }

        // Add suitable code to the Service Problem accessor method so the data is formatted as sentence case
        public void SetServiceProblem(string serviceProblem)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            ServiceProblem = textInfo.ToTitleCase(serviceProblem[0].ToString()) + serviceProblem[1..].ToLower();
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
