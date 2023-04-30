﻿using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

// Author: DaHye Baker
// Student ID: 30063368
// Organisation: South Metropolitan TAFE
// Description: Drone Service Application to log drones for service and repair

namespace DroneServiceApp
{
    /// <summary>
    /// Interaction logic for DroneServiceForm.xaml
    /// </summary>
    public partial class DroneServiceForm : Window
    {
        public DroneServiceForm()
        {
            InitializeComponent();
            DataContext = this;
        }

        public List<Drone> FinishedList = new();

        // Q6.4 Queue of drone class (express service)
        public Queue<Drone> ExpressQueue = new();

        // Q6.3 Queue of drone class (regular service)
        public Queue<Drone> RegularQueue = new();

        #region Buttons and Events

        // Q6.10 Keypress method for service cost
        // Q6.16 Double click method to remove item from listbox and list data structure
        // Q6.6 Increase express service by 15% 
        // Q6.11 Custom method to increment service tag control

        // Q6.8 Custom method to display regular service queue in ListView
        // Q6.9 Custom method to display express service queue in ListView

        /// <summary>
        /// Q6.13 Mouse click method to populate textbox from express service ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewServiceExpress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectItemListView(ListViewServiceExpress);
        }

        /// <summary>
        /// Q6.12 Mouse click method to populate textbox from regular service ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewServiceRegular_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectItemListView(ListViewServiceRegular);
        }

        /// <summary>
        /// 6.5 Create a button method called “AddNewItem”
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
            var servicePriority = GetServicePriority().ToString();

            AddNewItem(clientName, droneModel, serviceProblem, serviceCost, serviceTag, servicePriority);
            ClearTextBoxes();
        }

        /// <summary>
        /// Q6.14 Button method to dequeue regular data structure and add item to list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRegularDequeue_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfQueueEmpty(RegularQueue)) return;
            RemoveAndDequeue(RegularQueue, ListViewServiceRegular);
        }

        /// <summary>
        /// Q6.15 Button method to dequeue express data structure and add item to list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExpressDequeue_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfQueueEmpty(ExpressQueue)) return;
            RemoveAndDequeue(ExpressQueue, ListViewServiceExpress);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Q6.5 Add new method
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="droneModel"></param>
        /// <param name="serviceProblem"></param>
        /// <param name="serviceCost"></param>
        /// <param name="serviceTag"></param>
        /// <param name="servicePriority"></param>
        private void AddNewItem(string clientName, string droneModel, string serviceProblem, double serviceCost,
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
        /// Q6.7 Create a custom method called “GetServicePriority”
        /// </summary>
        /// <returns></returns>
        private object GetServicePriority()
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

        /// <summary>
        /// Q6.17 Custom method to clear textboxes
        /// </summary>
        private void ClearTextBoxes()
        {
            TextBoxClientName.Clear();
            TextBoxModel.Clear();
            TextBoxProblem.Clear();
            TextBoxCost.Clear();
            TextBoxServiceTag.Text = string.Empty;
            RadioButtonExpress.IsChecked = false;
            RadioButtonRegular.IsChecked = false;
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

        /// <summary>
        /// Dequeue items, remove from list view and add to completed list and completed list view
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="listView"></param>
        private void RemoveAndDequeue(Queue<Drone> queue, ItemsControl listView)
        {
            var selectedItem = queue.Peek();
            FinishedList.Add(selectedItem);
            ListViewFinishedItems.Items.Add(selectedItem);
            listView.Items.Remove(selectedItem);
            queue.Dequeue();
        }

        /// <summary>
        /// Select item from list view and display in text boxes
        /// </summary>
        /// <param name="listView"></param>
        private void SelectItemListView(ListView listView)
        {
            var selectedItem = (Drone)listView.SelectedItem;
            TextBoxClientName.Text = selectedItem.ClientName;
            TextBoxModel.Text = selectedItem.DroneModel;
            TextBoxProblem.Text = selectedItem.ServiceProblem;
            TextBoxCost.Text = selectedItem.ServiceCost.ToString();
            TextBoxServiceTag.Text = selectedItem.ServiceTag.ToString();
        }

        #endregion

        #region Booleans for errors

        /// <summary>
        /// Check if the queue is empty, if empty, do nothing
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static bool CheckIfQueueEmpty(ICollection? queue)
        {
            return queue != null && queue.Count == 0;

        }

        #endregion

    }

    //public class ObservableQueue<T> : INotifyCollectionChanged, IEnumerable<T>
    //{
    //    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    //    private readonly Queue<T> _queue = new Queue<T>();

    //    public void Enqueue(T item)
    //    {
    //        _queue.Enqueue(item);
    //        if (CollectionChanged != null)
    //            CollectionChanged(this,
    //                new NotifyCollectionChangedEventArgs(
    //                    NotifyCollectionChangedAction.Add, item));
    //    }

    //    public T Dequeue()
    //    {
    //        var item = _queue.Dequeue();
    //        if (CollectionChanged != null)
    //            CollectionChanged(this,
    //                new NotifyCollectionChangedEventArgs(
    //                    NotifyCollectionChangedAction.Remove, item));
    //        return item;
    //    }

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        return _queue.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}

}
