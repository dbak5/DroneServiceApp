using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

// Author: DaHye Baker
// Student ID: 30063368
// Organisation: South Metropolitan TAFE
// Description: Drone Service Application to log drones for service and repair

//CHECK CLOSE TRACE LISTENER ON FORM CLOSE
//CHECK MESSAGE BOXES TO APPEAR WHEN DEQUEUE OR PAYING
//CHECK ADD COMMENTS TO XAML


namespace DroneServiceApp
{
    /// <summary>
    /// Interaction logic for DroneServiceForm.xaml
    /// </summary>
    public partial class DroneServiceForm
    {
        public DroneServiceForm()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Q6.2 Create a global List<T> of type Drone called “FinishedList”
        public List<Drone> FinishedList = new();

        // Q6.3 Create a global Queue<T> of type Drone called “RegularService”
        public Queue<Drone> RegularQueue = new();

        // Q6.4 Create a global Queue<T> of type Drone called “ExpressService”
        public Queue<Drone> ExpressQueue = new();

        #region Buttons and Events

        /// <summary>
        /// Q6.5 Create a button method called “AddNewItem” that will add a new service item to a Queue based on the priority.
        /// Use TextBoxes for the Client Name, Drone Model, Service Problem and Service Cost.
        /// Use a numeric up/down control for the Service Tag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("\nAdd New item method started");
            var text = TextBoxCost.Text;

            Trace.Indent();
            Trace.WriteLine("\nChecking if any inputs are empty");
            Trace.Indent();
            Trace.WriteLine("\nClient Name: " + TextBoxClientName.Text);
            Trace.WriteLine("Drone Model: " + TextBoxModel.Text);
            Trace.WriteLine("Service Cost: " + TextBoxCost.Text);
            Trace.WriteLine("Service Priority: " + GetServicePriority());
            Trace.WriteLine("Service Tag: " + TextBoxServiceTag.Text);
            Trace.WriteLine("Service Problem: " + TextBoxProblem.Text);
            Trace.Unindent();
            if (CheckInputEmpty(TextBoxClientName.Text, "Please enter a name", TextBoxClientName)) return;
            if (CheckInputEmpty(TextBoxModel.Text, "Please enter a drone model", TextBoxModel)) return;
            if (CheckInputEmpty(TextBoxCost.Text.ToString(CultureInfo.CurrentCulture),
                    "Please enter a service cost", TextBoxCost)) return;
            if (CheckInputEmpty(GetServicePriority(), "Please specify service priority", RadioButtonRegular)) return;
            if (CheckInputEmpty(TextBoxServiceTag.Text, "Please tag service", TextBoxServiceTag)) return;
            if (CheckInputEmpty(TextBoxProblem.Text, "Please enter a service or repair issue", TextBoxProblem)) return;

            double.TryParse(TextBoxCost.Text.TrimStart('$'), out var serviceCost);
            int.TryParse(TextBoxServiceTag.Text, out var serviceTag);

            AddNewItem(TextBoxClientName.Text, TextBoxModel.Text, TextBoxProblem.Text, serviceCost, serviceTag, GetServicePriority());
            Trace.WriteLine("\nNew item added");
            Trace.WriteLine("\nAdd New item method ended");
            Trace.Unindent();
        }

        /// <summary>
        ///  Q6.10 Create a custom keypress method to ensure the Service Cost textbox can only accept a double value with one decimal point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Trace.WriteLine("\nService cost textbox validation started\n");
            var text = TextBoxCost.Text;
            var textBox = (TextBox)sender;

            // Check if the input is a digit or a decimal point, cancel input if it's not
            Trace.Indent();
            Trace.WriteLine("Checking if the input is a digit or a decimal point");
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ".")
            {
                Trace.Indent();
                Trace.WriteLine("Input value: " + e);
                Trace.WriteLine("Input cancelled");
                Trace.Unindent();
                e.Handled = true; 
                return;
            }
            Trace.Unindent();

            // Check if the input would result in a valid double, cancel the input if not
            Trace.Indent();
            Trace.WriteLine("Checking if the input would result in a valid double");
            var newText = text.Insert(textBox.SelectionStart, e.Text);
            if (!double.TryParse(newText, out _))
            {
                Trace.Indent();
                Trace.WriteLine("Code: !double.TryParse(newText, out _)");
                Trace.WriteLine("Result: " + !double.TryParse(newText, out _));
                Trace.WriteLine("Input cancelled");
                Trace.Unindent();
                e.Handled = true; 
                return;
            }
            Trace.Unindent();

            // Check if a decimal point already exists, cancel the input if it does
            Trace.Indent();
            Trace.WriteLine("Checking if a decimal point already exists");
            if (e.Text == "." && text.Contains("."))
            {
                Trace.Indent();
                Trace.WriteLine("Code: e.Text == \".\" && text.Contains(\".\")");
                Trace.WriteLine("Result: " + (e.Text == "." && text.Contains(".")));
                Trace.WriteLine("Input cancelled");
                Trace.Unindent();
                e.Handled = true; 
                return;
            }
            Trace.Unindent();

            // Check if the input would result in more than two decimal places after the decimal point
            // If decimalIndex = -1, no decimal has been entered yet
            // Split textbox text by decimal place, add values to an array
            // If the last value of the array is greater than 1 in length, cancel input
            Trace.Indent();
            Trace.WriteLine("Checking if the input would result in more than two decimal places after the decimal point");
            var decimalIndex = text.IndexOf('.');
            var words = text.Split('.');
            var lastValue = words.Last();
            if (decimalIndex >= 0 && lastValue.Length > 1)
            {
                Trace.Indent();
                Trace.WriteLine("Code: decimalIndex >= 0 && lastValue.Length > 1");
                Trace.WriteLine("Result: " + (decimalIndex >= 0 && lastValue.Length > 1));
                Trace.WriteLine("Input cancelled");
                Trace.Unindent();
                e.Handled = true; 
            }
            Trace.Unindent();

            Trace.WriteLine("\nService cost textbox validation ended");
        }

        /// <summary>
        /// Q6.12 Create a mouse click method for the regular service ListView that will display the Client Name and Service Problem in the related textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewServiceRegular_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectItemListView(ListViewServiceRegular);
        }

        /// <summary>
        /// Q6.13 Create a mouse click method for the express service ListView that will display the Client Name and Service Problem in the related textboxes
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
        /// Form loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxServiceTag.Text = "100";
            Stream myFile = File.Create("TestFile.txt");
            var myTextListener = new TextWriterTraceListener(myFile);
            Trace.Listeners.Add(myTextListener);
        }


        #endregion

        #region Methods

        /// <summary>
        /// Q6.5 Add new method
        /// Q6.6 Before a new service item is added to the Express Queue the service cost must be increased by 15%. 
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
            Trace.WriteLine("\nService priority is express, service cost calculation starting");
            if (servicePriority == "Express")
            {
                Trace.Indent();
                Trace.WriteLine("\nService cost: " + serviceCost);
                Trace.WriteLine("\nCalculation: serviceCost * 1.15, 2");
                Trace.WriteLine("\nExpress service cost result: " + (serviceCost * 1.15, 2));
                Trace.Unindent();
                serviceCost = Math.Round(serviceCost * 1.15, 2);
                Trace.WriteLine("\nService cost calculation ended");
            }

            var newDrone = new Drone();
            {
                SetDroneProperties(newDrone, clientName, droneModel, serviceProblem, serviceCost, serviceTag,
                    servicePriority);
            }

            // The new service item will be added to the appropriate Queue based on the Priority radio button. 
            switch (servicePriority)
            {
                case "Regular":
                    RegularQueue.Enqueue(newDrone);
                    ListViewServiceRegular.Items.Add(newDrone);
                    UpdateStatusStrip("Drone added to regular service queue");
                    ClearTextBoxes();
                    break;
                case "Express":
                    ExpressQueue.Enqueue(newDrone);
                    ListViewServiceExpress.Items.Add(newDrone);
                    UpdateStatusStrip("Drone added to express service queue, 15% added to cost");
                    ClearTextBoxes();
                    break;
            }
        }

        /// <summary>
        /// Q6.7 Create a custom method called “GetServicePriority” which returns the value of the priority radio group.
        /// </summary>
        /// <returns></returns>
        private string GetServicePriority()
        {
            if (RadioButtonExpress.IsChecked == true)
            {
                return RadioButtonExpress.Content.ToString();
            }

            if (RadioButtonRegular.IsChecked == true)
            {
                return RadioButtonRegular.Content.ToString();
            }

            return "";
        }

        /// <summary>
        /// Q6.16 Create a double mouse click method that will delete a service item from the finished listbox and remove the same item from the List
        /// </summary>
        private void RemoveFinishedItems()
        {
            Trace.WriteLine("\nRemove finished items starting\n");
            var selectedItem = (Drone)ListViewFinishedItems.SelectedItem;
            if (selectedItem == null)
            {
                UpdateStatusStrip("Please select item");
                return;
            }
            FinishedList.Remove(selectedItem);
            Trace.Indent();
            Trace.WriteLine("\nDrone removed from finished list");

            ListViewFinishedItems.Items.Remove(selectedItem);
            Trace.WriteLine("\nDrone removed from finished list view");
            Trace.Unindent();

            Trace.WriteLine("\nRemove finished items ended");
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
            Trace.WriteLine("\nDequeue service item started");
            Trace.Indent();
            var selectedItem = queue.Peek();

            FinishedList.Add(selectedItem);
            Trace.WriteLine("\nAdded item to finished list");

            ListViewFinishedItems.Items.Add(selectedItem);
            Trace.WriteLine("\nAdded item to finished list view");

            listView.Items.Remove(selectedItem);
            Trace.WriteLine("\nRemoved item from regular or express list view");

            queue.Dequeue();
            Trace.WriteLine("\nRemoved item from regular or express queue");
            Trace.Unindent();
            Trace.WriteLine("\nDequeue service item ended");
        }

        /// <summary>
        /// Select item from list view and display in text boxes
        /// </summary>
        /// <param name="listView"></param>
        private void SelectItemListView(Selector listView)
        {
            var selectedItem = (Drone)listView.SelectedItem;
            if (selectedItem == null) return;
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

        //CHECK CANT GET THIS TO WORK - PROPERTIES ARE PRIVATE
        /// <summary>
        /// Q6.8 Custom method to display regular service queue in ListView
        /// Q6.9 Custom method to display express service queue in ListView
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="listView"></param>
        private static void DisplayQueue(IEnumerable<Drone> queue, ItemsControl listView)
        {
            // Set the ItemsSource of the ListView
            listView.ItemsSource = queue;
        }

        // CHECK THESE WITH LECTURER - WHY CREATE NUMERIC CONTROL WHEN THIS IS INCREMENTED AUTOMATICALLY
        /// <summary>
        /// Q6.11 Create a custom method to increment the service tag control, this method must be called inside the “AddNewItem” method before the new service item is added to a queue
        /// </summary>
        private void IncrementServiceTag()
        {

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
        /// Check if text box is empty
        /// </summary>
        /// <param name="content"></param>
        /// <param name="message"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool CheckInputEmpty(string content, string message, IInputElement input)
        {
            if (string.IsNullOrEmpty(content))
            {
                UpdateStatusStrip(message);
                input.Focus();
                return true;
            }

            return false;
        }

        #endregion
    }
}
