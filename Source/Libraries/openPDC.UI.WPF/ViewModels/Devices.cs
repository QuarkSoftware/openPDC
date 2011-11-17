﻿//******************************************************************************************************
//  Devices.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/06/2011 - Mehulbhai P Thakkar
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using openPDC.UI.DataModels;
using openPDC.UI.Modal;
using openPDC.UI.UserControls;
using TimeSeriesFramework.UI;
using TimeSeriesFramework.UI.Commands;
using TimeSeriesFramework.UI.DataModels;
using TimeSeriesFramework.UI.UserControls;

namespace openPDC.UI.ViewModels
{
    /// <summary>
    /// Class to hold bindable <see cref="Device"/> collection and current selection information for UI.
    /// </summary>
    internal class Devices : PagedViewModelBase<openPDC.UI.DataModels.Device, int>
    {
        #region [ Members ]

        // Fields
        private Dictionary<Guid, string> m_nodeLookupList;
        private Dictionary<int, string> m_concentratorDeviceLookupList;
        private Dictionary<int, string> m_companyLookupList;
        private Dictionary<int, string> m_historianLookupList;
        private Dictionary<int, string> m_interconnectionLookupList;
        private Dictionary<int, string> m_protocolLookupList;
        private Dictionary<int, string> m_vendorDeviceLookupList;
        private Dictionary<string, string> m_timezoneLookupList;
        private RelayCommand m_editCommand;
        private RelayCommand m_phasorCommand;
        private RelayCommand m_measurementCommand;
        private RelayCommand m_copyCommand;
        private RelayCommand m_initializeCommand;
        private RelayCommand m_buildConnectionStringCommand;
        private RelayCommand m_buildAlternateCommandChannelCommand;
        private RelayCommand m_updateConfigurationCommand;
        private RelayCommand m_configureConcentratorCommand;
        private string m_runtimeID;
        private ObservableCollection<Device> m_devices;
        private RelayCommand m_searchCommand;
        private RelayCommand m_showAllCommand;
        private bool m_stayOnConfigurationScreen;
        private ObservableCollection<Device> m_pdcDevices;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates an instance of <see cref="Devices"/> class.
        /// </summary>
        /// <param name="itemsPerPage"></param>
        /// <param name="autoSave"></param>
        /// <param name="device"><see cref="Device"/> to be edited.</param>
        public Devices(int itemsPerPage, bool autoSave = true, openPDC.UI.DataModels.Device device = null)
            : base(itemsPerPage, autoSave)
        {
            m_nodeLookupList = Node.GetLookupList(null);
            m_concentratorDeviceLookupList = openPDC.UI.DataModels.Device.GetLookupList(null, openPDC.UI.DataModels.DeviceType.Concentrator, true);
            m_companyLookupList = Company.GetLookupList(null, true);
            m_historianLookupList = Historian.GetLookupList(null, true, false);
            m_interconnectionLookupList = Interconnection.GetLookupList(null, true);
            m_protocolLookupList = Protocol.GetLookupList(null, true);
            m_vendorDeviceLookupList = VendorDevice.GetLookupList(null, true);
            m_timezoneLookupList = TimeSeriesFramework.UI.CommonFunctions.GetTimeZones(true);

            if (device != null)     // i.e. user wants to edit existing device's configuration. So we will load that by default.
            {
                CurrentItem = device;
                OnPropertyChanged("IsNewRecord");
                OnPropertyChanged("CanGoToPhasorOrMeasurement");
                if (device.IsConcentrator)
                    PdcDevices = Device.GetDevices(null, "WHERE ParentID = " + device.ID);
            }
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets <see cref="ICommand"/> to build connection string.
        /// </summary>
        public ICommand BuildConnectionStringCommand
        {
            get
            {
                if (m_buildConnectionStringCommand == null)
                    m_buildConnectionStringCommand = new RelayCommand(BuildConnectionString);

                return m_buildConnectionStringCommand;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ICommand"/> to build alternate command channel.
        /// </summary>
        public ICommand BuildAlternateCommandChannelCommand
        {
            get
            {
                if (m_buildAlternateCommandChannelCommand == null)
                    m_buildAlternateCommandChannelCommand = new RelayCommand(BuildAlternateCommandChannel);

                return m_buildAlternateCommandChannelCommand;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Device"/> RuntimeID.
        /// </summary>
        public string RuntimeID
        {
            get
            {
                return m_runtimeID;
            }
            set
            {
                m_runtimeID = value;
                OnPropertyChanged("RuntimeID");
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="Node"/> defined in the database.
        /// </summary>
        public Dictionary<Guid, string> NodeLookupList
        {
            get
            {
                return m_nodeLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of concentrator <see cref="Device"/> defined in the database.
        /// </summary>
        public Dictionary<int, string> ConcentratorDeviceLookupList
        {
            get
            {
                return m_concentratorDeviceLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="Company"/> defined in the database.
        /// </summary>
        public Dictionary<int, string> CompanyLookupList
        {
            get
            {
                return m_companyLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="Historian"/> defined in the database.
        /// </summary>
        public Dictionary<int, string> HistorianLookupList
        {
            get
            {
                return m_historianLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="Interconnection"/> defined in the database.
        /// </summary>
        public Dictionary<int, string> InterconnectionLookupList
        {
            get
            {
                return m_interconnectionLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="Protocol"/> defined in the database.
        /// </summary>
        public Dictionary<int, string> ProtocolLookupList
        {
            get
            {
                return m_protocolLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="VendorDevice"/> defined in the database.
        /// </summary>
        public Dictionary<int, string> VendorDeviceLookupList
        {
            get
            {
                return m_vendorDeviceLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of system time zones.
        /// </summary>
        public Dictionary<string, string> TimezoneLookupList
        {
            get
            {
                return m_timezoneLookupList;
            }
        }

        /// <summary>
        /// Gets flag that determines if <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/> is a new record.
        /// </summary>
        public override bool IsNewRecord
        {
            get
            {
                return CurrentItem.ID == 0;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> for edit operation.
        /// </summary>
        public ICommand EditCommand
        {
            get
            {
                if (m_editCommand == null)
                    m_editCommand = new RelayCommand(GoToEdit);

                return m_editCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> for copy operation.
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if (m_copyCommand == null)
                    m_copyCommand = new RelayCommand(MakeCopy);

                return m_copyCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> for updating configuration.
        /// </summary>
        public ICommand UpdateConfigurationCommand
        {
            get
            {
                if (m_updateConfigurationCommand == null)
                    m_updateConfigurationCommand = new RelayCommand(UpdateConfiguration);

                return m_updateConfigurationCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> to go to phasors configuration.
        /// </summary>
        public ICommand PhasorCommand
        {
            get
            {
                if (m_phasorCommand == null)
                    m_phasorCommand = new RelayCommand(GoToPhasors, () => CanGoToPhasorOrMeasurement);

                return m_phasorCommand;
            }
        }

        /// <summary>
        /// Gets a boolean flag indicating if Phasors and Measurements button are visible or not.
        /// </summary>
        public bool CanGoToPhasorOrMeasurement
        {
            get
            {
                return CurrentItem.ID > 0;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> to go to measurements configuration.
        /// </summary>
        public ICommand MeasurementCommand
        {
            get
            {
                if (m_measurementCommand == null)
                    m_measurementCommand = new RelayCommand(GoToMeasurements, () => CanGoToPhasorOrMeasurement);

                return m_measurementCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> to send Initialize command to backed service.
        /// </summary>
        public ICommand InitializeCommand
        {
            get
            {
                if (m_initializeCommand == null)
                    m_initializeCommand = new RelayCommand(Initialize, () => CanSave);

                return m_initializeCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> to search within measurements.
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                if (m_searchCommand == null)
                    m_searchCommand = new RelayCommand(Search, (param) => true);

                return m_searchCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> to show all measurements.
        /// </summary>
        public ICommand ShowAllCommand
        {
            get
            {
                if (m_showAllCommand == null)
                    m_showAllCommand = new RelayCommand(ShowAll);

                return m_showAllCommand;
            }
        }

        public ICommand ConfigureConcentratorCommand
        {
            get
            {
                if (m_configureConcentratorCommand == null)
                    m_configureConcentratorCommand = new RelayCommand(ConfigureConcentrator);

                return m_configureConcentratorCommand;
            }
        }

        public ObservableCollection<Device> PdcDevices
        {
            get
            {
                return m_pdcDevices;
            }
            set
            {
                m_pdcDevices = value;
                OnPropertyChanged("PdcDevices");
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the primary key value of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.
        /// </summary>
        /// <returns>The primary key value of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.</returns>
        public override int GetCurrentItemKey()
        {
            return CurrentItem.ID;
        }

        /// <summary>
        /// Gets the string based named identifier of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.
        /// </summary>
        /// <returns>The string based named identifier of the <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/>.</returns>
        public override string GetCurrentItemName()
        {
            return CurrentItem.Name;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Historian"/> and assigns it to CurrentItem.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            // Reset all the dropdown fields so that first item displays as selected.
            if (m_nodeLookupList.Count > 0)
                CurrentItem.NodeID = m_nodeLookupList.First().Key;

            if (m_concentratorDeviceLookupList.Count > 0)
                CurrentItem.ParentID = m_concentratorDeviceLookupList.First().Key;

            if (m_companyLookupList.Count > 0)
                CurrentItem.CompanyID = m_companyLookupList.First().Key;

            if (m_historianLookupList.Count > 0)
                CurrentItem.HistorianID = m_historianLookupList.First().Key;

            if (m_interconnectionLookupList.Count > 0)
                CurrentItem.InterconnectionID = m_interconnectionLookupList.First().Key;

            if (m_protocolLookupList.Count > 0)
                CurrentItem.ProtocolID = m_protocolLookupList.First().Key;

            if (m_vendorDeviceLookupList.Count > 0)
                CurrentItem.VendorDeviceID = m_vendorDeviceLookupList.First().Key;

            if (m_timezoneLookupList.Count > 0)
                CurrentItem.TimeZone = m_timezoneLookupList.First().Key;
        }

        /// <summary>
        /// Loads collection of <see cref="Device"/> information stored in the database.
        /// </summary>
        /// <remarks>This method is overridden because MethodInfo.Invoke in the base class did not like optional parameters.</remarks>
        public override void Load()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                m_devices = Device.Load(null);
                ItemsSource = m_devices;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Popup(ex.Message + Environment.NewLine + "Inner Exception: " + ex.InnerException.Message, "Load Devices Exception:", MessageBoxImage.Error);
                    CommonFunctions.LogException(null, "Load Devices", ex.InnerException);
                }
                else
                {
                    Popup(ex.Message, "Load Devices Exception:", MessageBoxImage.Error);
                    CommonFunctions.LogException(null, "Load Devices", ex);
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// Saves <see cref="Device"/> information into database.
        /// </summary>
        public override void Save()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                base.Save();

                if (ItemsPerPage == 0) // i.e. if user is on form page then go back to list page after save.
                {
                    if (!m_stayOnConfigurationScreen)
                    {
                        openPDC.UI.UserControls.DeviceListUserControl deviceListUserControl = new openPDC.UI.UserControls.DeviceListUserControl();
                        CommonFunctions.LoadUserControl(deviceListUserControl, "Browse Devices");
                    }
                    //m_stayOnConfigurationScreen = false;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Popup(ex.Message + Environment.NewLine + "Inner Exception: " + ex.InnerException.Message, "Save Device Exception:", MessageBoxImage.Error);
                    CommonFunctions.LogException(null, "Save Device", ex.InnerException);
                }
                else
                {
                    Popup(ex.Message, "Save Device Exception:", MessageBoxImage.Error);
                    CommonFunctions.LogException(null, "Save Device", ex);
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// Handles <see cref="EditCommand"/>.
        /// </summary>
        /// <param name="parameter">Parameter to use for the command provided by commandparameter from UI.</param>
        private void GoToEdit(object parameter)
        {
            openPDC.UI.DataModels.Device deviceToEdit = (openPDC.UI.DataModels.Device)parameter;
            openPDC.UI.UserControls.DeviceUserControl deviceUserControl = new openPDC.UI.UserControls.DeviceUserControl(deviceToEdit);
            CommonFunctions.LoadUserControl(deviceUserControl, "Manage Device Configuration");
        }

        /// <summary>
        /// Handles <see cref="CopyCommand"/>.
        /// </summary>
        /// <param name="parameter">Parameter to use for the command provided by commandparameter from UI.</param>
        private void MakeCopy(object parameter)
        {
            Device deviceToCopy = (openPDC.UI.DataModels.Device)parameter;
            string newAcronym; ;
            int i = 1;
            do  // Find unique acronym.
            {
                newAcronym = deviceToCopy.Acronym + i.ToString();
                i++;
            } while (DeviceExists(newAcronym));

            deviceToCopy.Acronym = newAcronym; // Change acronym of the device before going to edit screen.
            deviceToCopy.Name = "Copy of " + deviceToCopy.Name; // Change name of the device before going to edit screen.            
            deviceToCopy.ID = 0;    // Set id to zero so that it will be added as a new device.
            ItemsPerPage = 0; // Set this so that on Save() user will be sent back to list screen.
            deviceToCopy.Enabled = false; // Always set enabled to false for copied device.

            // Go to edit screen.
            DeviceUserControl deviceUserControl = new DeviceUserControl(deviceToCopy);
            CommonFunctions.LoadUserControl(deviceUserControl, "Manage Device Configuration");
        }

        private void UpdateConfiguration(object parameter)
        {
            Device device = (openPDC.UI.DataModels.Device)parameter;
            InputWizardUserControl inputWizardUserControl = new InputWizardUserControl(device);
            CommonFunctions.LoadUserControl(inputWizardUserControl, "Input Device Configuration Wizard");
        }

        private bool DeviceExists(string acronym)
        {
            bool deviceExists = false;
            foreach (openPDC.UI.DataModels.Device device in ItemsSource)
            {
                if (device.Acronym == acronym)
                    return true;
            }
            return deviceExists;
        }

        /// <summary>
        /// Handles <see cref="MeasurementCommand"/>.
        /// </summary>
        private void GoToMeasurements()
        {
            //openPDC.UI.DataModels.Device device = (openPDC.UI.DataModels.Device)parameter;
            MeasurementUserControl measurementUserControl = new MeasurementUserControl(CurrentItem.ID);
            CommonFunctions.LoadUserControl(measurementUserControl, "Manage Measurements for " + CurrentItem.Acronym);
        }

        /// <summary>
        /// Handles <see cref="PhasorCommand"/>.
        /// </summary>        
        private void GoToPhasors()
        {
            //openPDC.UI.DataModels.Device device = (openPDC.UI.DataModels.Device)parameter;
            openPDC.UI.UserControls.PhasorUserControl phasorUserControl = new openPDC.UI.UserControls.PhasorUserControl(CurrentItem.ID);
            CommonFunctions.LoadUserControl(phasorUserControl, "Manage Phasors for " + CurrentItem.Acronym);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "CurrentItem")
            {
                if (CurrentItem == null)
                    RuntimeID = string.Empty;
                else
                    RuntimeID = TimeSeriesFramework.UI.CommonFunctions.GetRuntimeID("Device", CurrentItem.ID);
            }
        }

        private void Initialize()
        {
            try
            {
                if (!string.IsNullOrEmpty(RuntimeID))
                {
                    if (Confirm("Do you want to send Initialize " + GetCurrentItemName() + "?", "Confirm Initialize"))
                    {
                        openPDC.UI.DataModels.Device.NotifyService(CurrentItem);

                        Popup("Successfully sent Initialize command.", "Initialize", MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Popup("ERROR: " + ex.Message, "Failed To Initialize", MessageBoxImage.Error);
            }
        }

        private void BuildConnectionString()
        {
            if (CurrentItem != null)
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder(ConnectionStringBuilder.ConnectionType.DeviceConnection);
                if (!string.IsNullOrEmpty(CurrentItem.ConnectionString))
                    csb.ConnectionString = CurrentItem.ConnectionString;

                csb.Closed += new EventHandler(delegate(object popupWindow, EventArgs eargs)
                {
                    if ((bool)csb.DialogResult)
                        CurrentItem.ConnectionString = csb.ConnectionString;
                });
                csb.Owner = System.Windows.Application.Current.MainWindow;
                csb.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                csb.ShowDialog();
            }
        }

        private void BuildAlternateCommandChannel()
        {
            if (CurrentItem != null)
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder(ConnectionStringBuilder.ConnectionType.AlternateCommandChannel);
                if (!string.IsNullOrEmpty(CurrentItem.AlternateCommandChannel))
                    csb.ConnectionString = CurrentItem.AlternateCommandChannel;

                csb.Closed += new EventHandler(delegate(object popupWindow, EventArgs eargs)
                {
                    if ((bool)csb.DialogResult)
                        CurrentItem.AlternateCommandChannel = csb.ConnectionString;
                });
                csb.Owner = System.Windows.Application.Current.MainWindow;
                csb.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                csb.ShowDialog();
            }
        }

        /// <summary>
        /// Deletes current item from the database.
        /// </summary>
        public override void Delete()
        {
            if (CurrentItem.IsConcentrator)
            {
                if (Confirm("Are you sure you want to delete concentrator device?" + Environment.NewLine + "Clicking yes will delete all the devices connected to: " + CurrentItem.Acronym, "Delete Device"))
                {
                    try
                    {
                        ObservableCollection<Device> deviceList = Device.Load(null, CurrentItem.ID);
                        foreach (Device device in deviceList)
                            Device.Delete(null, device.ID);

                        base.Delete();
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            Popup(ex.Message + Environment.NewLine + "Inner Exception: " + ex.InnerException.Message, "Delete Device Exception:", MessageBoxImage.Error);
                            CommonFunctions.LogException(null, "Delete Device", ex.InnerException);
                        }
                        else
                        {
                            Popup(ex.Message, "Delete Device Exception:", MessageBoxImage.Error);
                            CommonFunctions.LogException(null, "Delete Device", ex);
                        }
                    }
                }
            }
            else
            {
                base.Delete();
            }
        }

        /// <summary>
        /// Hanldes <see cref="SearchCommand"/>.
        /// </summary>
        /// <param name="paramter">string value to search for in measurement collection.</param>
        public void Search(object paramter)
        {
            if (paramter != null && !string.IsNullOrEmpty(paramter.ToString()))
            {
                string searchText = paramter.ToString().ToLower();
                ItemsSource = new ObservableCollection<Device>
                    (m_devices.Where(d => d.Acronym.ToLower().Contains(searchText) ||
                                     d.Name.ToLower().Contains(searchText) ||
                                     d.CompanyAcronym.ToLower().Contains(searchText) ||
                                     d.CompanyName.ToLower().Contains(searchText)));
            }
        }

        /// <summary>
        /// Handles <see cref="ShowAllCommand"/>.
        /// </summary>
        public void ShowAll()
        {
            ItemsSource = m_devices;
        }

        protected override void m_currentItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.m_currentItem_PropertyChanged(sender, e);
            if (ItemsPerPage > 0 && string.Compare(e.PropertyName, "Enabled", true) == 0)
                ProcessPropertyChange();
        }

        private void ConfigureConcentrator()
        {
            if (CurrentItem.ParentID != null && CurrentItem.ParentID > 0)
            {
                m_stayOnConfigurationScreen = true;
                Device device = Device.GetDevice(null, "WHERE ID = " + CurrentItem.ParentID);
                if (device != null)
                {
                    PdcDevices = Device.GetDevices(null, "WHERE ParentID = " + CurrentItem.ParentID);
                    CurrentItem = device;
                }
            }
        }

        #endregion
    }
}
