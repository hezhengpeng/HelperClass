using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading;
using System.Reflection;

using Modbus.Device;
using Modbus.Data;
using Modbus.Utility;

using System.Collections;

namespace TreadSys.Common
{
    class NModbusFunction
    {
        public static bool isFiveAddr = true;        // 地址为5位;也有为4位的情况，第一位为标识

        /// <summary>NModbus读函数封装-读0X 1X 3X 4X
        /// 
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="readNum"></param>
        /// <returns></returns>
        public static ushort[] NModbusRtuRead(SerialPort serialPort, byte slaveId, ushort startAddress, ushort readNum)
        {
            ushort[] values;
            bool[] bvalues;

            if ((startAddress >= 0) & (startAddress < 50000))
            {
                try
                {
                    if ((startAddress >= 0) & (startAddress < 10000))
                    {
                        bvalues = readCoils(serialPort, slaveId, startAddress, readNum);

                        ushort[] array = new ushort[bvalues.Length];

                        for (int i = 0; i < bvalues.Length; i++)
                        {
                            array[i] = Convert.ToUInt16(bvalues[i]);
                        }

                        return array;

                    }
                    else if ((startAddress >= 10000) & (startAddress < 20000))
                    {
                        if (!isFiveAddr) startAddress -= 10000;

                        bvalues = readInputs(serialPort, slaveId, startAddress, readNum);

                        ushort[] array = new ushort[bvalues.Length];

                        for (int i = 0; i < bvalues.Length; i++)
                        {
                            array[i] = Convert.ToUInt16(bvalues[i]);
                        }
                        return array;
                    }
                    else if ((startAddress >= 30000) & (startAddress < 40000))
                    {
                        if (!isFiveAddr) startAddress -= 30000;
                        values = readInputRegisters(serialPort, slaveId, startAddress, readNum);
                        return values;
                    }
                    //else if ((startAddress >= 40000) & (startAddress < 50000))
                    else
                    {
                        if (!isFiveAddr) startAddress -= 40000;
                        values = readHoldingRegisters(serialPort, slaveId, startAddress, readNum);
                        return values;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("请确认地址是否错误");
                    return null;
                }
            }
            else
            {
                MessageBox.Show("地址长度错误！");
                return null;
            }
        }

        /// <summary>NModbus写函数封装-写0X 4X
        /// 
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="values"></param>
        public static void NModbusRtuWrite(SerialPort serialPort, byte slaveId, ushort startAddress, object[] values)
        {
            if (((startAddress >= 0) & (startAddress < 10000)) || ((startAddress >= 40000) & (startAddress < 50000)))
            {
                if ((startAddress >= 0) & (startAddress < 10000))
                {
                    writeMultipleCoils(serialPort, slaveId, startAddress, (bool[])ArrayList.Adapter((Array)values).ToArray(typeof(bool)));
                }
                else if ((startAddress >= 40000) & (startAddress < 50000))
                {
                    if (!isFiveAddr) startAddress -= 40000;
                    writeMultipleRegisters(serialPort, slaveId, startAddress, (ushort[])ArrayList.Adapter((Array)values).ToArray(typeof(ushort)));
                }
            }
            else
            {
                MessageBox.Show("请确认地址是否错误");
            }
        }


        /// <summary>读保持寄存器 4X
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="readNum"></param>
        /// <returns></returns>
        private static ushort[] readHoldingRegisters(SerialPort sport, byte slaveId, ushort startAddress, ushort readNum)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            ushort[] regValues = master.ReadHoldingRegisters(slaveId, startAddress, readNum);

            return regValues;
        }

        /// <summary>读输入寄存器 3X
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="readNum"></param>
        /// <returns></returns>
        private static ushort[] readInputRegisters(SerialPort sport, byte slaveId, ushort startAddress, ushort readNum)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            ushort[] regValues = master.ReadInputRegisters(slaveId, startAddress, readNum);

            return regValues;
        }

        /// <summary>读线圈 0X
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="readNum"></param>
        /// <returns></returns>
        private static bool[] readCoils(SerialPort sport, byte slaveId, ushort startAddress, ushort readNum)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            bool[] regValues = master.ReadCoils(slaveId, startAddress, readNum);

            return regValues;
        }

        /// <summary>读离散量 1X
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="readNum"></param>
        /// <returns></returns>
        private static bool[] readInputs(SerialPort sport, byte slaveId, ushort startAddress, ushort readNum)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            bool[] regValues = master.ReadInputs(slaveId, startAddress, readNum);

            return regValues;
        }

        /// <summary>NModbus - 写单线圈
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="flag"></param>
        private static void writeSingleCoil(SerialPort sport, byte slaveId, ushort startAddress, bool flag)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            master.WriteSingleCoil(slaveId, startAddress, flag);     // 注： 此函数无返回值
        }

        /// <summary>NModbus - 写多线圈
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="flags"></param>
        private static void writeMultipleCoils(SerialPort sport, byte slaveId, ushort startAddress, bool[] flags)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            master.WriteMultipleCoils(slaveId, startAddress, flags);     // 注： 此函数无返回值
        }

        /// <summary>NModbus - 写多寄存器
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="values"></param>
        private static void writeMultipleRegisters(SerialPort sport, byte slaveId, ushort startAddress, ushort[] values)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            master.WriteMultipleRegisters(slaveId, startAddress, values);     // 注： 此函数无返回值
        }

        /// <summary>NModbus - 写单寄存器
        /// 
        /// </summary>
        /// <param name="sport"></param>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="value"></param>
        private static void writeSingleRegister(SerialPort sport, byte slaveId, ushort startAddress, ushort value)
        {
            IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(sport);

            master.WriteSingleRegister(slaveId, startAddress, value);     // 注： 此函数无返回值
        }

    }
}
