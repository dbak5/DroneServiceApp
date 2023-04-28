using System.Collections.Generic;
using System.Windows;

// Author: DaHye Baker
// Student ID: 30063368
// Organisation: South Metropolitan TAFE
// Description: Drone Service Application to log drones for service and repair

namespace DroneServiceApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Drone> FinishedList = new();
        private Queue<Drone> ExpressQueue = new();
        private Queue<Drone> RegularQueue = new();

        #region Buttons and Events
        // Q6.10 Keypress method for service cost

        // Q6.12 Mouse click method to populate textbox from regular service ListView

        // Q6.13 Mouse click method to populate textbox from express service ListView

        // Q6.14 Button method to dequeue regular data structure and add item to list

        // Q6.15 Button method to dequeue express data structure and add item to list

        // Q6.16 Double click method to remove item from listbox and list data structure

        #endregion

        #region Methods
        // Q6.3 Queue of class (regular service)

        // Q6.4 Queue of class (express service)

        // Q6.5 Add new service item

        // Q6.6 Increase express service by 15% 

        // Q6.7 Custom method to return radio button priority

        // Q6.8 Custom method to display regular service queue in ListView

        // Q6.9 Custom method to display express service queue in ListView

        // Q6.11 Custom method to increment service tag control

        // Q6.17 Custom method to clear textboxes

        #endregion
    }
}
