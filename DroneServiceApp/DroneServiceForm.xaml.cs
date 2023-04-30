﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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

        private const int RegularCost = 200;

        #region Buttons and Events

        // Q6.11 Custom method to increment service tag control - ASK LECTURER
        // Q6.8 Custom method to display regular service queue in ListView - ASK LECTURER
        // Q6.9 Custom method to display express service queue in ListView - ASK LECTURER
        // TOOLTIPS

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

            // CHECK REFACTOR THIS
            if (!CheckTextBoxEmpty(clientName)) return;
            if (!CheckTextBoxEmpty(droneModel)) return;
            if (!CheckTextBoxEmpty(serviceProblem)) return;
            if (!CheckTextBoxEmpty(serviceCost.ToString("C",CultureInfo.CurrentCulture))) return;
            if (!CheckTextBoxEmpty(serviceTag.ToString())) return;
            if (!CheckTextBoxEmpty(servicePriority)) return;
            if (serviceCost < 0) return;

            AddNewItem(clientName, droneModel, serviceProblem, serviceCost, serviceTag, servicePriority);
            ClearTextBoxes();
        }

        //CHECK THIS ISNT WORKING NOW
        /// <summary>
        ///  Q6.10 Create a custom keypress method to ensure the Service Cost textbox can only accept a double value with one decimal point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = CheckNumeric(e.Text);
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
        /// Q6.13 Mouse click method to populate textbox from express service ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewServiceExpress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectItemListView(ListViewServiceExpress);
        }

        /// <summary>
        /// Q6.14 Button method to dequeue regular data structure and add item to list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRegularDequeue_Click(object sender, RoutedEventArgs e)
        {
            if (CheckQueueEmpty(RegularQueue)) return;
            ClearTextBoxes();
            RemoveAndDequeue(RegularQueue, ListViewServiceRegular);
        }

        /// <summary>
        /// Q6.15 Button method to dequeue express data structure and add item to list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExpressDequeue_Click(object sender, RoutedEventArgs e)
        {
            if (CheckQueueEmpty(ExpressQueue)) return;
            ClearTextBoxes();
            RemoveAndDequeue(ExpressQueue, ListViewServiceExpress);
        }
        
        /// <summary>
        /// Q6.16 Create a double mouse click method that will delete a service item from the finished listbox and remove the same item from the List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewFinishedItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RemoveFinishedItems();
        }

        /// <summary>
        /// Button to remove items from completed items list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPaid_Click(object sender, RoutedEventArgs e)
        {
            RemoveFinishedItems();
        }

        /// <summary>
        /// Q6.6 Increase express service by 15% 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonExpress_Checked(object sender, RoutedEventArgs e)
        {
                const double expressCost = RegularCost * 1.15;
                TextBoxCost.Text = expressCost.ToString("C", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Set default cost for a regular service when radio button checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonRegular_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxCost.Text = RegularCost.ToString("C", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Set default cost for a regular service on window load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxCost.Text = RegularCost.ToString("C", CultureInfo.CurrentCulture);
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
                    UpdateStatusStrip("Drone added to regular service queue");
                    break;
                case "Express":
                    ExpressQueue.Enqueue(newDrone);
                    ListViewServiceExpress.Items.Add(newDrone);
                    UpdateStatusStrip("Drone added to express service queue");
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
        /// Q6.16 Create a double mouse click method that will delete a service item from the finished listbox and remove the same item from the List
        /// </summary>
        private void RemoveFinishedItems()
        {
            var selectedItem = (Drone)ListViewFinishedItems.SelectedItem;
            FinishedList.Remove(selectedItem);
            ListViewFinishedItems.Items.Remove(selectedItem);
            UpdateStatusStrip("Drone removed from finished items");
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
        private void SelectItemListView(Selector listView)
        {
            var selectedItem = (Drone)listView.SelectedItem;
            TextBoxClientName.Text = selectedItem.ClientName;
            TextBoxModel.Text = selectedItem.DroneModel;
            TextBoxProblem.Text = selectedItem.ServiceProblem;
            TextBoxCost.Text = selectedItem.ServiceCost.ToString("C", CultureInfo.CurrentCulture);
            TextBoxServiceTag.Text = selectedItem.ServiceTag.ToString();
            SetRadioButton(selectedItem.ServicePriority);
        }

        /// <summary>
        /// Update status strip
        /// </summary>
        /// <param name="message"></param>
        private void UpdateStatusStrip(string message)
        {
            StatusBarMessage.Text = message;
        }

        /// <summary>
        /// Check radio button 
        /// </summary>
        /// <param name="servicePriority"></param>
        private void SetRadioButton(string servicePriority)
        {
            switch (servicePriority)
            {
                case "Express":
                    RadioButtonExpress.IsChecked = true;
                    break;
                case "Regular":
                    RadioButtonRegular.IsChecked = true;
                    break;
            }
        }

        #endregion

        #region Booleans for errors

        /// <summary>
        /// Check if the queue is empty, if empty, do nothing
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static bool CheckQueueEmpty(ICollection? queue)
        {
            return queue != null && queue.Count == 0;

        }

        /// <summary>
        /// Check if text is numeric using regular expressions
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool CheckNumeric(string text)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, "[^0-9]+[.]");
        }

        /// <summary>
        /// Check if text box is empty
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static bool CheckTextBoxEmpty(string content)
        {
            return !string.IsNullOrEmpty(content);
        }

        #endregion

     
    }
    
}