using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;

using Leadshine;
using Leadshine.SMC.IDE.Motion;

//namespace SMC606
namespace TreadSys.Common
{
    public class SMC606
    {             
        public static ushort connectNo = 0;             // 连接号为public，因有可能会有多个控制器 
        static ushort connectType = 2;                  // 连接类型：网口连接
        static string connectString = "192.168.5.11";   // 连接字串，SMC606的默认IP是192.168.5.11
        static uint baud   = 0;                         // 波特率网口连接方式下为0

        public static ushort axis0 = 0;                 // 设置逻辑轴号对应的物理轴                      
        public static ushort axis1 = 1;
        public static ushort axis2 = 2;
        public static ushort axis3 = 3;                                      
        public static ushort axis4 = 4;
        public static ushort axis5 = 5;



        /// <summary>连接运动控制器
        /// 
        /// </summary>
        /// <param name="ConnectNo"></param>
        /// <returns></returns>
        public static short connect(ushort ConnectNo)
        {
            short iret = LTSMC.smc_board_init(ConnectNo, connectType, connectString, baud);

            return iret;
        }

        /// <summary>关闭运动控制器连接
        /// 
        /// </summary>
        /// <param name="ConnectNo"></param>
        /// <returns></returns>
        public static short disconnect(ushort ConnectNo)
        {
            short iret = LTSMC.smc_board_close(ConnectNo);
            
            return iret;
        }

        /// <summary>打开/关闭伺服
        /// 
        /// </summary>
        /// <param name="ConnectNo"></param>
        /// <param name="axis"></param>
        /// <param name="on_off">0为开，1为关</param>
        /// <returns></returns>
        public static short servOnOff(ushort ConnectNo, ushort axis, ushort on_off)
        {
            short iret = LTSMC.smc_write_sevon_pin(ConnectNo, axis, on_off);
            
            return iret;
        }

        
        #region 单轴运动函数

        /// <summary>单轴-回零运动
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static ushort oneAxisReturnZero(ushort axis, double run_speed, ushort ConnectNo)
        {
            ushort iret = 0;
            try
            {
                double start_speed = 1000;
                double acc_time = 0.1;
                double dec_time = 0.1;
                double offset = 0.0;                    //回零偏移设置为0

                //设置起始速度、运行速度、加速时间、减速时间
                LTSMC.smc_set_home_profile_unit(ConnectNo, axis, start_speed, run_speed, acc_time, dec_time);
                //设置回零方向为正向，回零模式1(一次回零加回找)
                LTSMC.smc_set_homemode(ConnectNo, axis, 1, 1, 1, 0);
                //设置偏移模式为0
                LTSMC.smc_set_home_position_unit(ConnectNo, axis, 0, offset);
                //启动回零
                LTSMC.smc_home_move(ConnectNo, axis);

                iret = 0;

            }
            catch (Exception)
            {

                iret = 1;
            }
            return iret;

        }

        /// <summary>单轴-位置清零
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void oneAxisClearToZero(ushort connectNo, ushort axis)
        {
            //把轴的位置清零
            LTSMC.smc_set_position_unit(connectNo, axis, 0);
        }

        /// <summary>单轴-定长运动
        /// 
        /// </summary>
        public static void oneAxisMotionDistance(ushort connectNo, ushort axis, double speed, double distance)
        {
            double start = 1000;

            double stop = 1000;
            double acc = 0.1;
            double dec = 0.1;
            //double dis = 10000;
            double s = 0.01;

            //
            LTSMC.smc_set_pulse_outmode(connectNo, axis, 2);                //设置脉冲模式
            LTSMC.smc_set_equiv(connectNo, axis, 1);                        //设置脉冲当量
            LTSMC.smc_set_alm_mode(connectNo, axis, 1, 1, 0);             //设置报警使能，关闭报警
            LTSMC.smc_write_sevon_pin(connectNo, axis, 0);                //打开伺服使能
            LTSMC.smc_set_s_profile(connectNo, axis, 0, s);                 //设置S段时间（0-1s)
            LTSMC.smc_set_profile_unit(connectNo, axis, start, speed, acc, dec, stop);//设置起始速度、运行速度、停止速度、加速时间、减速时间
            LTSMC.smc_set_dec_stop_time(connectNo, axis, dec);              //设置减速停止时间
            LTSMC.smc_pmove_unit(connectNo, axis, distance, 0);                  //定长运动
        }


        /// <summary>单轴连续运动
        /// 
        /// </summary>
        /// <param name="axis"></param>
        public static void oneAxisMotionContinues(ushort connectNo, ushort axis, double run_speed, ushort dir)
        {
            double start_speed = 1000;
            double stop_speed = 1000;
            double acc_time = 0.1;
            double dec_time = 0.1;

            LTSMC.smc_set_s_profile(connectNo, axis, 0, 0.01);                          //设置S段时间（0-0.05s)
            LTSMC.smc_set_profile_unit(connectNo, axis, start_speed, run_speed, acc_time, dec_time, stop_speed);//设置起始速度、运行速度、加速时间、减速时间、停止速度
            LTSMC.smc_set_dec_stop_time(connectNo, axis, dec_time);                     //设置减速停止时间
            LTSMC.smc_vmove(connectNo, axis, dir);   //连续运动
        }

        /// <summary>单轴-回原点(0)
        /// 
        /// </summary>
        public static void oneAxisLocationZero(ushort connectNo, ushort axis, double speed)
        {
            double distance = 0;
            LTSMC.smc_get_position_unit(connectNo, axis, ref distance);       // 获取偏离0点值

            double start = 1000;
            double stop = 1000;
            double acc = 0.1;
            double dec = 0.1;
            double s = 0.01;

            LTSMC.smc_set_s_profile(connectNo, axis, 0, s);                 //设置S段时间（0-1s)
            LTSMC.smc_set_profile_unit(connectNo, axis, start, speed, acc, dec, stop);//设置起始速度、运行速度、停止速度、加速时间、减速时间
            LTSMC.smc_set_dec_stop_time(connectNo, axis, dec);              //设置减速停止时间
            LTSMC.smc_pmove_unit(connectNo, axis, -distance, 0);                  //定长运动
        }

        #endregion 单轴运动函数

        #region 多轴运动函数

        // 多轴函数
        // 多轴轨迹运行的基本逻辑： 多轴回零 - 多轴位置清零 - 定位到起始点 - 多轴位置清零 - 多轴轨迹运动 - 停止

        /// <summary>初始化多轴，并打开伺服
        /// 
        /// </summary>
        /// <returns></returns>
        public static ushort initAxis(ushort[] axis, int array_size)
        {
            ushort iret = 0;

            try
            {
                for (ushort i = 0; i < array_size; i++)
                {
                    LTSMC.smc_set_pulse_outmode(connectNo, axis[i], 2);        //设置脉冲模式   = 2
                    LTSMC.smc_set_equiv(connectNo, axis[i], 1);                //设置脉冲当量   = 1
                    LTSMC.smc_set_alm_mode(connectNo, axis[i], 1, 1, 0);       //设置报警使能，高电平有效
                    LTSMC.smc_write_sevon_pin(connectNo, axis[i], 0);          //打开伺服使能
                    LTSMC.smc_set_home_pin_logic(connectNo, axis[i], 1, 0);    //设置原点低电平有效
                }
                iret = 0;
            }
            catch (Exception)
            {
                iret = 1;
            }

            return iret;
        }

        /// <summary>多轴回零运动
        /// 
        /// </summary>
        /// <returns></returns>
        public static ushort returnAxisHome(ushort ConnectNo, ushort[] axis, int array_size, double[] run_speed)
        {
            ushort iret = 0;

            double start_speed = 1000;
            double acc_time = 0.1;
            double dec_time = 0.1;
            double offset = 0.0;                    //回零偏移设置为0  //原来为0.0

            try
            {
                for (ushort i = 0; i < array_size; i++)
                {
                    //设置起始速度、运行速度、加速时间、减速时间
                    LTSMC.smc_set_home_profile_unit(ConnectNo, axis[i], start_speed, run_speed[i], acc_time, dec_time);
                    //设置回零方向为正向，回零模式1(一次回零加回找)
                    LTSMC.smc_set_homemode(ConnectNo, axis[i], 1, 1, 1, 0);
                    //设置偏移模式为0
                    LTSMC.smc_set_home_position_unit(ConnectNo, axis[i], 1, offset);
                    //启动回零
                    LTSMC.smc_home_move(ConnectNo, axis[i]);
                }
                iret = 0;
            }
            catch (Exception)
            {
                iret = 1;
            }

            return iret;
        }

        /// <summary>多轴位置清为0
        /// 
        /// </summary>
        /// <returns></returns>       
        public static ushort clearAxisToZero(ushort ConnectNo, ushort[] axis, int array_size)
        {
            ushort iret = 0;

            try
            {
                for (int i = 0; i < array_size; i++)
                {
                    LTSMC.smc_set_position_unit(ConnectNo, axis[i], 0);
                }

                iret = 0;
            }
            catch (Exception)
            {
                iret = 1;
            }
            return iret;
        }

        /// <summary>多轴回逻辑0点 - 回零运动之后确定的逻辑0点
        /// 
        /// </summary>
        /// <returns></returns>
        public static ushort returnAxisLogicZero(ushort ConnectNo, ushort[] axis, int array_size, double run_speed)
        {
            ushort iret = 0;

            double[] distances = new double[array_size];

            double start_speed = 500;
            double stop_speed = 500;
            double acc_time = 0.1;
            double dec_time = 0.1;
            double s = 0.01;

            try
            {
                for (ushort i = 0; i < array_size; i++)          // 轴345
                {
                    LTSMC.smc_set_s_profile(ConnectNo, axis[i], 0, s);                 //设置S段时间（0-1s)
                    LTSMC.smc_set_profile_unit(ConnectNo, axis[i], start_speed, run_speed, acc_time, dec_time, stop_speed);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                    LTSMC.smc_set_dec_stop_time(ConnectNo, axis[i], dec_time);              //设置减速停止时间

                    LTSMC.smc_get_position_unit(ConnectNo, axis[i], ref distances[i]);       // 获取偏离0点值
                    LTSMC.smc_pmove_unit(ConnectNo, axis[i], -distances[i], 0);               // 定长运动
                }
                iret = 0;
            }
            catch (Exception)
            {
                iret = 1;
            }
            return iret;
        }

        /// <summary>检测多轴是否已经停止
        /// 
        /// </summary>
        /// <param name="ConnectNo"></param>
        /// <param name="axis1"></param>
        /// <param name="axis2"></param>
        /// <param name="axis3"></param>
        /// <returns></returns>
        public static bool checkAxisStop(ushort ConnectNo, ushort axisX, ushort axisY, ushort axisZ)
        {
            // smc_check_done为0运动，1停止
            try
            {
                //while (LTSMC.smc_check_done(ConnectNo, axisX) == 0 || LTSMC.smc_check_done(ConnectNo, axisY) == 0 || LTSMC.smc_check_done(ConnectNo, axisZ) == 0) ;
                while (true)
                {
                    if (LTSMC.smc_check_done(ConnectNo, axisX) == 0 || LTSMC.smc_check_done(ConnectNo, axisY) == 0 || LTSMC.smc_check_done(ConnectNo, axisZ) == 0)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch (Exception)
            {

                //throw;
            }
            return true;        //一直检测直到停止

        }

        /// <summary>多轴定长运动
        /// 
        /// </summary>
        public static void multiMotionDistance(ushort connectNo, ushort[] axis, int array_size, double[] speed, double[] distances)
        {

            double start = 1000;
            double stop = 1000;
            double acc = 0.1;
            double dec = 0.1;
            double s = 0.01;

            for (ushort i = 0; i < array_size; i++)                 //轴号为变量
            {
                LTSMC.smc_set_s_profile(connectNo, axis[i], 0, s);                 //设置S段时间（0-1s)
                LTSMC.smc_set_profile_unit(connectNo, axis[i], start, speed[i], acc, dec, stop);//设置起始速度、运行速度、停止速度、加速时间、减速时间
                LTSMC.smc_set_dec_stop_time(connectNo, axis[i], dec);              //设置减速停止时间
                LTSMC.smc_pmove_unit(connectNo, axis[i], distances[i], 1);                  //定长运动
            }
        }

        /// <summary>多轴直线插补
        /// 
        /// </summary>
        public static void multiSmcLineUnit(ushort connectNo, ushort[] axis, int array_size, double x_offset, double y_offset, double z_offset, double speed)
        {
            double[] pos = new double[] { x_offset, y_offset, z_offset };    // XYZ轴的偏移位置
            ushort num = 3;

            double acc = 0.1;
            double dec = 0.1;
            ushort crd = 0;                                             //坐标系

            LTSMC.smc_set_vector_profile_unit(connectNo, crd, 0, speed, acc, dec, 0);
            LTSMC.smc_line_unit(connectNo, crd, num, axis, pos, 1);     // posi_mode 0-相对 1-绝对
        }

        /// <summary>多轴PVTS运动
        /// 
        /// </summary>
        /// <param name="connectNo"></param>
        /// <param name="axisnum"></param>
        /// <param name="axis"></param>
        /// <param name="PosX"></param>
        /// <param name="PosY"></param>
        /// <param name="PosZ"></param>
        /// <param name="times"></param>
        /// <param name="dataLength"></param>
        public static void multiAxisPVTS(ushort connectNo, ushort axisnum, ushort[] axis, double[] PosX, double[] PosY, double[] PosZ, double[] times, uint dataLength)
        {
            LTSMC.smc_pvts_table_unit(connectNo, axis[2], dataLength, times, PosX, 0, 0);
            LTSMC.smc_pvts_table_unit(connectNo, axis[1], dataLength, times, PosY, 0, 0);
            LTSMC.smc_pvts_table_unit(connectNo, axis[0], dataLength, times, PosZ, 0, 0);

            LTSMC.smc_pvt_move(connectNo, axisnum, axis);
        }

        // 关闭所有的轴, 即 SMC606 0-5轴
        public static void multiAxisAllClose()
        {
            LTSMC.smc_stop(SMC606.connectNo, SMC606.axis0, 1);    //立即停止
            LTSMC.smc_stop(SMC606.connectNo, SMC606.axis1, 1);
            LTSMC.smc_stop(SMC606.connectNo, SMC606.axis2, 1);
            LTSMC.smc_stop(SMC606.connectNo, SMC606.axis3, 1);
            LTSMC.smc_stop(SMC606.connectNo, SMC606.axis4, 1);
            LTSMC.smc_stop(SMC606.connectNo, SMC606.axis5, 1);

            SMC606.servOnOff(SMC606.connectNo, SMC606.axis0, 1);
            SMC606.servOnOff(SMC606.connectNo, SMC606.axis1, 1);
            SMC606.servOnOff(SMC606.connectNo, SMC606.axis2, 1);
            SMC606.servOnOff(SMC606.connectNo, SMC606.axis3, 1);
            SMC606.servOnOff(SMC606.connectNo, SMC606.axis4, 1);
            SMC606.servOnOff(SMC606.connectNo, SMC606.axis5, 1);
        }


        #endregion 多轴运动函数


    }
}
