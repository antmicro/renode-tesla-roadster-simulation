//
// Copyright (c) 2010-2024 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Collections.Generic;
using Antmicro.Renode.Core;
using Antmicro.Renode.Core.CAN;
using Antmicro.Renode.Logging;
using Antmicro.Renode.Peripherals.UART;

namespace Antmicro.Renode.Peripherals.CAN
{
    public class CANDecoder : ICAN
    {
        public CANDecoder(IMachine machine)
        {
        }

        public void Reset()
        {
        }

        public void OnFrameReceived(CANMessageFrame message)
        {
            this.DebugLog("Received {0} bytes [{1}] on id 0x{2:x}", message.Data.Length, message.DataAsHex, message.Id);
            switch((MessageId)message.Id)
            {
                case MessageId.SwpSetVariable:
                    this.InfoLog("SWP variable {0} set to 0x{1:x2}", TranslateName(SwpVariable, message.Data[0]), message.Data[1]);
                    break;
                case MessageId.ShfDimmer:
                    this.InfoLog("SHF dimmer set to {0}%", message.Data[0]);
                    break;
                case MessageId.BsmRequest:
                    this.InfoLog("BSM request: {0}", TranslateName(BsmRequestType, message.Data[0]));
                    break;
                case MessageId.CsbRequest:
                    this.InfoLog("CSB request: {0}", TranslateName(CsbRequestType, message.Data[0]));
                    break;
            }
        }

        public event Action<CANMessageFrame> FrameSent;

        private static string TranslateName(Dictionary<int, string> names, int key)
        {
            return names.GetValueOrDefault(key, $"0x{key:x} (unknown)");
        }

        private enum MessageId
        {
            SwpSetVariable = 0x76,
            ShfDimmer = 0x82,
            BsmRequest = 0x200,
            CsbRequest = 0x208,
        }

        private readonly Dictionary<int, string> SwpVariable = new Dictionary<int, string>
        {
            [0] = "debug mode",
            [1] = "interior fade duration",
            [2] = "illumination level",
            [3] = "abs shed duration",
            [4] = "turn signal off duration",
            [5] = "turn signal on duration",
            [6] = "lamp out turn signal off time",
            [7] = "lamp out turn signal on time",
            [8] = "lock/unlock flash off time",
            [9] = "lock/unlock flash on time",
            [10] = "window autodown hold time",
            [11] = "window event threshold",
            [12] = "window event startup defeat",
            [13] = "debounce limit",
            [14] = "lo beam shed duration",
            [15] = "hi beam shed duration",
            [16] = "click period",
            [17] = "click on pulses",
            [18] = "hazard sw sample rate",
            [19] = "voltage report interval",
            [20] = "window event debounce limit",
            [21] = "device state report interval",
            [22] = "open/short report interval",
        };

        private readonly Dictionary<int, string> BsmRequestType = new Dictionary<int, string>
        {
            [0] = "Version",
            [1] = "Reset",
            [3] = "Set State",
            [4] = "Get State",
            [5] = "Clear Non-perm Faults",
            [6] = "Get Humidity and Temp",
            [7] = "Clear All Faults",
            [15] = "Get Isolation Resistance",
            [16] = "Do Isolation Test",
            [17] = "Report Intervals",
            [18] = "En/Disable IsolationTest",
            [19] = "Get Contactor Open Count",
            [20] = "Get ADC Chan 0-4",
            [21] = "Get ADC Chan 5-9",
            [22] = "Disable Low V Checks",
            [31] = "Self Test",
            [32] = "Get Odometer",
            [33] = "Set Odometer",
            [34] = "Query Precharge Data",
            [36] = "Open Contactor Cmd",
        };

        private readonly Dictionary<int, string> CsbRequestType = new Dictionary<int, string>
        {
            [0] = "Version",
            [1] = "Reset",
            [3] = "Clear Faults",
            [4] = "Energy",
            [5] = "Report Intervals",
            [6] = "iBat",
            [7] = "Serial Number",
            [8] = "Fault",
            [10] = "History",
            [11] = "Mean iBat Buffers",
            [20] = "ADC Chan 0-4",
            [21] = "ADC Chan 5-9",
            [31] = "Self Test",
        };
    }
}
