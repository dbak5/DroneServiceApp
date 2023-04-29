using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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
            this.DataContext = this;
        }

        public List<Drone> FinishedList = new();

        // Q6.4 Queue of class (express service)
        public ObservableQueue<Drone> ExpressQueue = new();

        // Q6.3 Queue of class (regular service)
        public ObservableQueue<Drone> RegularQueue = new();



        #region Buttons and Events

        // Q6.10 Keypress method for service cost

        /// <summary>
        /// Q6.12 Mouse click method to populate textbox from regular service ListView
        /// Q6.13 Mouse click method to populate textbox from express service ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            var clientName = TextBoxClientName.Text;
            var droneModel = TextBoxModel.Text;
            var serviceProblem = TextBoxProblem.Text;
            var serviceCost = double.Parse(TextBoxCost.Text);
            var serviceTag = int.Parse(TextBoxServiceTag.Text);
            var servicePriority = GetRadioButton().ToString();

            AddNewDrone(clientName, droneModel, serviceProblem, serviceCost, serviceTag, servicePriority);
        }

        // Q6.14 Button method to dequeue regular data structure and add item to list
        // Q6.15 Button method to dequeue express data structure and add item to list
        private void ButtonDequeue_Click(object sender, RoutedEventArgs e)
        {

        }

        // Q6.16 Double click method to remove item from listbox and list data structure

        #endregion

        #region Methods


        /// <summary>
        /// Q6.5 Add new service item
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="droneModel"></param>
        /// <param name="serviceProblem"></param>
        /// <param name="serviceCost"></param>
        /// <param name="serviceTag"></param>
        /// <param name="servicePriority"></param>
        private void AddNewDrone(string clientName, string droneModel, string serviceProblem, double serviceCost,
            int serviceTag, string servicePriority)
        {
            var newDrone = new Drone();
            {
                SetDroneProperties(newDrone, clientName, droneModel, serviceProblem, serviceCost, serviceTag,
                    servicePriority);
            }
            switch (servicePriority)
            {
                case "Regular":
                    RegularQueue.Enqueue(newDrone);
                    ListViewServiceRegular.Items.Add(newDrone);
                    break;
                case "Express":
                    ExpressQueue.Enqueue(newDrone);
                    ListViewServiceExpress.Items.Add(newDrone);
                    break;
            }
        }

        /// <summary>
        /// Set properties for a drone
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="clientName"></param>
        /// <param name="droneModel"></param>
        /// <param name="serviceProblem"></param>
        /// <param name="serviceCost"></param>
        /// <param name="serviceTag"></param>
        /// <param name="servicePriority"></param>
        private static void SetDroneProperties(Drone newDrone, string clientName, string droneModel,
            string serviceProblem, double serviceCost, int serviceTag, string servicePriority)
        {
            newDrone.SetClientName(clientName);
            newDrone.SetDroneModel(droneModel);
            newDrone.SetServiceProblem(serviceProblem);
            newDrone.SetServiceCost(serviceCost);
            newDrone.SetServiceTag(serviceTag);
            newDrone.SetServicePriority(servicePriority);
        }

        // Q6.6 Increase express service by 15% 

        // Q6.7 Custom method to return radio button priority

        // Q6.8 Custom method to display regular service queue in ListView

        // Q6.9 Custom method to display express service queue in ListView

        // Q6.11 Custom method to increment service tag control

        // Q6.17 Custom method to clear textboxes

        /// <summary>
        /// RadioButton find value
        /// </summary>
        /// <returns></returns>
        private object GetRadioButton()
        {
            if (RadioButtonExpress.IsChecked == true)
            {
                return RadioButtonExpress.Content;
            }
            if (RadioButtonRegular.IsChecked == true)
            {
                return RadioButtonRegular.Content;
            }
            return "";
        }

        #endregion

    }

    public class ObservableQueue<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly Queue<T> queue = new Queue<T>();

        public void Enqueue(T item)
        {
            queue.Enqueue(item);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Add, item));
        }

        public T Dequeue()
        {
            var item = queue.Dequeue();
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Remove, item));
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
