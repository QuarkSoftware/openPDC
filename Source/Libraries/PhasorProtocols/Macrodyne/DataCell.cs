//******************************************************************************************************
//  DataCell.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  04/30/2009 - J. Ritchie Carroll
//       Generated original version of source code.
//  09/15/2009 - Stephen C. Wills
//       Added new header and license agreement.
//
//******************************************************************************************************

using GSF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PhasorProtocols.Macrodyne
{
    /// <summary>
    /// Represents the Macrodyne implementation of a <see cref="IDataCell"/> that can be sent or received.
    /// </summary>
    [Serializable()]
    public class DataCell : DataCellBase
    {
        #region [ Members ]

        // Fields
        private byte m_status2Flags;
        private ClockStatusFlags m_clockStatusFlags;
        private ushort m_sampleNumber;
        private ushort m_referenceSampleNumber;
        private PhasorValue m_referencePhasor;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="DataCell"/>.
        /// </summary>
        /// <param name="parent">The reference to parent <see cref="IDataFrame"/> of this <see cref="DataCell"/>.</param>
        /// <param name="configurationCell">The <see cref="IConfigurationCell"/> associated with this <see cref="DataCell"/>.</param>
        public DataCell(IDataFrame parent, IConfigurationCell configurationCell)
            : base(parent, configurationCell, 0x0000, Common.MaximumPhasorValues, Common.MaximumAnalogValues, Common.MaximumDigitalValues)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DataCell"/> from specified parameters.
        /// </summary>
        /// <param name="parent">The reference to parent <see cref="DataFrame"/> of this <see cref="DataCell"/>.</param>
        /// <param name="configurationCell">The <see cref="ConfigurationCell"/> associated with this <see cref="DataCell"/>.</param>
        /// <param name="addEmptyValues">If <c>true</c>, adds empty values for each defined configuration cell definition.</param>
        public DataCell(DataFrame parent, ConfigurationCell configurationCell, bool addEmptyValues)
            : this(parent, configurationCell)
        {
            if (addEmptyValues)
            {
                int x;

                // Define needed phasor values
                for (x = 0; x < configurationCell.PhasorDefinitions.Count; x++)
                {
                    PhasorValues.Add(new PhasorValue(this, configurationCell.PhasorDefinitions[x]));
                }

                // Define a frequency and df/dt
                FrequencyValue = new FrequencyValue(this, configurationCell.FrequencyDefinition);
            }
        }

        /// <summary>
        /// Creates a new <see cref="DataCell"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected DataCell(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Deserialize data cell
            m_status2Flags = info.GetByte("status2Flags");
            m_clockStatusFlags = (ClockStatusFlags)info.GetValue("clockStatusFlags", typeof(ClockStatusFlags));
            m_sampleNumber = info.GetUInt16("sampleNumber");
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the reference to parent <see cref="DataFrame"/> of this <see cref="DataCell"/>.
        /// </summary>
        public new DataFrame Parent
        {
            get
            {
                return base.Parent as DataFrame;
            }
            set
            {
                base.Parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ConfigurationCell"/> associated with this <see cref="DataCell"/>.
        /// </summary>
        public new ConfigurationCell ConfigurationCell
        {
            get
            {
                return base.ConfigurationCell as ConfigurationCell;
            }
            set
            {
                base.ConfigurationCell = value;
            }
        }

        /// <summary>
        /// Gets the numeric ID code for this <see cref="DataCell"/>.
        /// </summary>
        public new uint IDCode
        {
            get
            {
                return ConfigurationCell.IDCode;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Macrodyne.ClockStatusFlags"/> for this <see cref="DataCell"/>.
        /// </summary>
        public ClockStatusFlags ClockStatusFlags
        {
            get
            {
                return m_clockStatusFlags;
            }
            set
            {
                m_clockStatusFlags = value;
            }
        }

        /// <summary>
        /// Gets or sets status 1 flags for this <see cref="DataCell"/>.
        /// </summary>
        public StatusFlags Status1Flags
        {
            get
            {
                return Parent.CommonHeader.StatusFlags;
            }
            set
            {
                Parent.CommonHeader.StatusFlags = value;
                base.StatusFlags = Word.MakeWord((byte)value, m_status2Flags);
            }
        }

        /// <summary>
        /// Gets or sets status 2 flags for this <see cref="DataCell"/>.
        /// </summary>
        public byte Status2Flags
        {
            get
            {
                return m_status2Flags;
            }
            set
            {
                m_status2Flags = value;
                base.StatusFlags = Word.MakeWord((byte)Status1Flags, m_status2Flags);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Macrodyne.TriggerReason"/> for this <see cref="DataCell"/>.
        /// </summary>
        public TriggerReason TriggerReason
        {
            get
            {
                return (TriggerReason)(m_status2Flags & (byte)(Bits.Bit00 | Bits.Bit01 | Bits.Bit02));
            }
            set
            {
                m_status2Flags = (byte)((m_status2Flags & ~(byte)(Bits.Bit00 | Bits.Bit01 | Bits.Bit02)) | (byte)value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Macrodyne.GpsStatus"/> for this <see cref="DataCell"/>.
        /// </summary>
        public GpsStatus GpsStatus
        {
            get
            {
                return (GpsStatus)(m_status2Flags & (byte)(Bits.Bit03 | Bits.Bit04));
            }
            set
            {
                m_status2Flags = (byte)((m_status2Flags & ~(byte)(Bits.Bit03 | Bits.Bit04)) | (byte)value);
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if data of this <see cref="DataCell"/> is valid.
        /// </summary>
        public override bool DataIsValid
        {
            get
            {
                return !((Status1Flags & Macrodyne.StatusFlags.OperationalLimitReached) > 0);
            }
            set
            {
                if (value)
                    Status1Flags = Status1Flags & ~Macrodyne.StatusFlags.OperationalLimitReached;
                else
                    Status1Flags = Status1Flags | Macrodyne.StatusFlags.OperationalLimitReached;
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if timestamp of this <see cref="DataCell"/> is valid based on GPS lock.
        /// </summary>
        public override bool SynchronizationIsValid
        {
            get
            {
                return !((Status1Flags & Macrodyne.StatusFlags.TimeError) > 0);
            }
            set
            {
                if (value)
                    Status1Flags = Status1Flags & ~Macrodyne.StatusFlags.TimeError;
                else
                    Status1Flags = Status1Flags | Macrodyne.StatusFlags.TimeError;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="PhasorProtocols.DataSortingType"/> of this <see cref="DataCell"/>.
        /// </summary>
        public override DataSortingType DataSortingType
        {
            get
            {
                return (SynchronizationIsValid ? PhasorProtocols.DataSortingType.ByTimestamp : PhasorProtocols.DataSortingType.ByArrival);
            }
            set
            {
                // We just ignore this value as we have defined data sorting type as a derived value based on synchronization validity
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if source device of this <see cref="DataCell"/> is reporting an error.
        /// </summary>
        /// <remarks>Macrodyne doesn't define bits for device error.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool DeviceError
        {
            get
            {
                return false;
            }
            set
            {
                // We just ignore this value as Macrodyne defines no flags for data errors
            }
        }

        /// <summary>
        /// Gets or sets reference phasor sample number.
        /// </summary>
        public ushort ReferenceSampleNumber
        {
            get
            {
                return m_referenceSampleNumber;
            }
            set
            {
                m_referenceSampleNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets reference phasor value.
        /// </summary>
        public PhasorValue ReferencePhasor
        {
            get
            {
                return m_referencePhasor;
            }
            set
            {
                m_referencePhasor = value;
            }
        }

        /// <summary>
        /// Gets <see cref="AnalogValueCollection"/> of this <see cref="DataCell"/>.
        /// </summary>
        /// <remarks>
        /// Macrodyne doesn't define any analog values.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override AnalogValueCollection AnalogValues
        {
            get
            {
                return base.AnalogValues;
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of string based property names and values for the <see cref="DataCell"/> object.
        /// </summary>
        public override Dictionary<string, string> Attributes
        {
            get
            {
                Dictionary<string, string> baseAttributes = base.Attributes;

                baseAttributes.Add("Status 1 Flags", (int)Status1Flags + ": " + Status1Flags);
                baseAttributes.Add("Status 2 Flags", Status2Flags.ToString());
                baseAttributes.Add("Clock Status Flags", (int)ClockStatusFlags + ": " + ClockStatusFlags);
                baseAttributes.Add("GPS Status", (int)GpsStatus + ": " + GpsStatus);
                baseAttributes.Add("Trigger Reason", (int)TriggerReason + ": " + TriggerReason);

                return baseAttributes;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Parses the binary body image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        protected override int ParseBodyImage(byte[] buffer, int startIndex, int length)
        {
            ConfigurationCell configCell = ConfigurationCell;
            ConfigurationFrame configFrame = configCell.Parent;
            ProtocolVersion protocolVersion = configFrame.CommonHeader.ProtocolVersion;
            IPhasorValue phasorValue;
            IDigitalValue digitalValue;
            int parsedLength, index = startIndex;

            if (protocolVersion == ProtocolVersion.M)
            {
                // Parse out optional STATUS2 flags
                if (configFrame.Status2Included)
                {
                    m_status2Flags = buffer[index];
                    index++;
                }
                else
                {
                    m_status2Flags = 0;
                }

                // We interpret status bytes together as one word (matches other protocols this way)
                base.StatusFlags = Word.MakeWord((byte)Status1Flags, m_status2Flags);
            }
            else
            {
                // Read sample number for G protocol
                m_sampleNumber = EndianOrder.BigEndian.ToUInt16(buffer, index);
                index += 2;
            }

            // Parse out time tag
            if (configFrame.TimestampIncluded)
            {
                m_clockStatusFlags = (ClockStatusFlags)buffer[index];
                index += 1;

                ushort day = BinaryCodedDecimal.Decode(EndianOrder.BigEndian.ToUInt16(buffer, index));
                byte hours = BinaryCodedDecimal.Decode(buffer[index + 2]);
                byte minutes = BinaryCodedDecimal.Decode(buffer[index + 3]);
                byte seconds = BinaryCodedDecimal.Decode(buffer[index + 4]);
                double timebase = 2880.0D;
                index += 5;

                // Read sample number for M protocol
                if (protocolVersion == ProtocolVersion.M)
                {
                    m_sampleNumber = EndianOrder.BigEndian.ToUInt16(buffer, index + 5);
                    timebase = 719.0D;
                    index += 2;
                }

                // TODO: Think about how to handle year change with floating clock...
                // Calculate timestamp
                Parent.Timestamp = new DateTime(DateTime.UtcNow.Year, 1, 1).AddDays(day - 1).AddHours(hours).AddMinutes(minutes).AddSeconds(seconds + m_sampleNumber / timebase);
            }
            else
            {
                Parent.Timestamp = DateTime.UtcNow.Ticks;
                SynchronizationIsValid = false;
                m_sampleNumber = EndianOrder.BigEndian.ToUInt16(buffer, index);
                index += 2;
            }

            // Parse out first five phasor values (1 - 5)
            int phasorIndex = 0;

            // Phasor 1 (always present)
            phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
            PhasorValues.Add(phasorValue);
            index += parsedLength;

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor2Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor2Enabled)
            {
                // Phasor 2
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor3Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor3Enabled)
            {
                // Phasor 3
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor4Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor4Enabled)
            {
                // Phasor 4
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor5Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor5Enabled)
            {
                // Phasor 5
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            // For 1690M format the frequency, reference phasor, dF/dt and first digital follow phasors 1-5
            if (protocolVersion == ProtocolVersion.M)
            {
                // Parse out frequency value
                FrequencyValue = Macrodyne.FrequencyValue.CreateNewValue(this, configCell.FrequencyDefinition, buffer, index, out parsedLength);
                index += parsedLength;

                // Parse reference phasor information
                if (configFrame.ReferenceIncluded)
                {
                    m_referenceSampleNumber = EndianOrder.BigEndian.ToUInt16(buffer, index);
                    m_referencePhasor = Macrodyne.PhasorValue.CreateNewValue(this, new PhasorDefinition(null, "Reference Phasor", PhasorType.Voltage, null), buffer, index, out parsedLength) as Macrodyne.PhasorValue;
                    index += 6;
                }

                // Parse first digital value
                if (configFrame.Digital1Included)
                {
                    digitalValue = DigitalValue.CreateNewValue(this, configCell.DigitalDefinitions[0], buffer, index, out parsedLength);
                    DigitalValues.Add(digitalValue);
                    index += parsedLength;
                }
            }

            // Parse out next five phasor values (6 - 10)
            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor6Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor6Enabled)
            {
                // Phasor 6
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor7Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor7Enabled)
            {
                // Phasor 7
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor8Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor8Enabled)
            {
                // Phasor 8
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor9Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor9Enabled)
            {
                // Phasor 9
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            if ((configFrame.OnlineDataFormatFlags & Macrodyne.OnlineDataFormatFlags.Phasor10Enabled) == Macrodyne.OnlineDataFormatFlags.Phasor10Enabled)
            {
                // Phasor 10
                phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                PhasorValues.Add(phasorValue);
                index += parsedLength;
            }

            // For 1690G format the channel phasors, reference phasor, frequency, dF/dt and digitals follow phasors 1-10
            if (protocolVersion == ProtocolVersion.G)
            {
                // Technically 30 more possible channel phasors can be defined
                for (int i = phasorIndex; i < ConfigurationCell.PhasorDefinitions.Count; i++)
                {
                    phasorValue = Macrodyne.PhasorValue.CreateNewValue(this, configCell.PhasorDefinitions[phasorIndex++], buffer, index, out parsedLength);
                    PhasorValues.Add(phasorValue);
                    index += parsedLength;
                }

                // Parse reference phasor information
                if (configFrame.ReferenceIncluded)
                {
                    m_referencePhasor = Macrodyne.PhasorValue.CreateNewValue(this, new PhasorDefinition(null, "Reference Phasor", PhasorType.Voltage, null), buffer, index, out parsedLength) as Macrodyne.PhasorValue;
                    index += parsedLength;
                }

                // Parse out frequency value
                FrequencyValue = Macrodyne.FrequencyValue.CreateNewValue(this, configCell.FrequencyDefinition, buffer, index, out parsedLength);
                index += parsedLength;

                // Parse first digital value
                if (configFrame.Digital1Included)
                {
                    digitalValue = DigitalValue.CreateNewValue(this, configCell.DigitalDefinitions[0], buffer, index, out parsedLength);
                    DigitalValues.Add(digitalValue);
                    index += parsedLength;
                }
            }

            // Parse second digital value
            if (configFrame.Digital2Included)
            {
                digitalValue = DigitalValue.CreateNewValue(this, configCell.DigitalDefinitions[configCell.DigitalDefinitions.Count - 1], buffer, index, out parsedLength);
                DigitalValues.Add(digitalValue);
                index += parsedLength;
            }

            // Return total parsed length
            return (index - startIndex);
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination <see cref="StreamingContext"/> for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // Serialize data cell
            info.AddValue("status2Flags", m_status2Flags);
            info.AddValue("clockStatusFlags", m_clockStatusFlags, typeof(ClockStatusFlags));
            info.AddValue("sampleNumber", m_sampleNumber);
        }

        #endregion

        #region [ Static ]

        // Static Methods

        // Delegate handler to create a new Macrodyne data cell
        internal static IDataCell CreateNewCell(IChannelFrame parent, IChannelFrameParsingState<IDataCell> state, int index, byte[] buffer, int startIndex, out int parsedLength)
        {
            DataCell dataCell = new DataCell(parent as IDataFrame, (state as IDataFrameParsingState).ConfigurationFrame.Cells[index]);

            parsedLength = dataCell.ParseBinaryImage(buffer, startIndex, 0);

            return dataCell;
        }

        #endregion
    }
}