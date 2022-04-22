using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Leadshine.SMC.IDE.Motion;

namespace TreadSys.Common
{
    public class SMC606Helper
    {
        static public bool isFirstRun = true;               // 三轴综合运动会进行位置清零-初始定位-位置清零；此变量用来防止重复
        static public int timeFactor = 10;                  // 时间倍乘   放慢速度的时间倍乘     注： 10为默认值
        static public double lengthFactor = 1.0;            // 长度倍乘         注： 系数为1时按XYZ轴的原始数据运行，范围是0.1-1
        static public int cycleNum = 1;                     // 走多少个轨迹循环   注： 在没有倍乘的情况下，一个循环的时间是3.2秒

        static private ushort connectNo = SMC606.connectNo;                            // 运动控制器连接号
        static private ushort[] axisLeft = new ushort[] { 2, 1, 0 };                   // 左侧三个轴的轴号
        static private ushort[] axisRight = new ushort[] { 3, 4, 5 };                  // 右侧三个轴的轴号
   
        static private double speed = 20000;
        static double[] speed_init = new double[] { speed, speed * 2.5, speed };//x/y/z运行速度
        // 运行速度：回零
        #region 平地轨迹位置/时间数据

        // PVT原始位置数据
        // 前后轨迹位置 右脚
        static private double[] _PosX_R = new double[]{
        -0.957142857,-0.957142857,-0.957142857,-0.957142857,-0.926190476,0.926190476,0.957142857, 0.957142857,0.957142857,0.957142857,0.957142857,0.966666667,0.957142857,0.957142857,0.935714286,
-0.935714286,-0.957142857,-0.957142857,-0.966666667,-0.957142857
        };
        // PVT原始位置数据
        // 前后轨迹位置 左脚
        static private double[] _PosX_L = new double[]{
0.957142857,0.957142857,0.966666667,0.957142857,0.957142857,0.935714286,-0.935714286,-0.957142857,-0.957142857,-0.966666667,-0.957142857,-0.957142857,-0.957142857,-0.957142857,-0.957142857,-0.926190476,0.926190476,0.957142857,0.957142857,0.957142857,0.957142857
        };
        // 上下轨迹位置 右
        static private double[] _PosY_R = new double[]{   0,0,0,0,7.538461538,18.15384615,4.484615385,0.384615385,-1.261538462,-2.907692308,-2.507692308,-3.084615385,-3.992307692,-5.915384615,-10.89230769,0,0,0,0,0
        };
        // 上下轨迹位置 左
        static private double[] _PosY_L = new double[]{
            -2.907692308,-2.507692308,-3.084615385,-3.992307692,-5.915384615,-10.89230769,0,0,0,0,0,0,0,0,0,7.538461538,18.15384615,4.484615385,
0.384615385,-1.261538462
        };
        // 踝关节轨迹位置 右
        static private double[] _PosZ_R = new double[]{
            0,0,0,1.09,4.056,0.5325,0,0,-0.757,-2.5385,-1.761,-0.0765,-0.896,-0.089,-0.154,0,0.5935,0,0,0

        };
        // 踝关节轨迹位置 左
        static private double[] _PosZ_L = new double[]{
           -2.5385,-1.761,-0.0765,-0.896,-0.089,-0.154,0,0.5935,0,0,0,0,0,0,1.09,4.056,0.5325,0,0,-0.757
        };


        #endregion   轨迹位置/时间数据
        #region 上坡轨迹位置/时间数据

        // PVT原始位置数据
        // 前后轨迹位置 右脚
        static private double[] UP_PosX_R = new double[]{
        -0.947619048,-0.95,-0.952380952,-0.95,-0.919047619,0.84047619,0.914285714,0.926190476,0.930952381,0.938095238,0.94047619,0.911904762,0.985714286,0.964285714,0.973809524,-0.838095238,-0.778571429,-1.09047619,-0.95,-0.95
        };
        // PVT原始位置数据
        // 前后轨迹位置 左脚
        static private double[] UP_PosX_L = new double[]{
0.938095238,0.94047619,0.911904762,0.985714286,0.964285714,0.973809524,-0.838095238,-0.778571429,-1.09047619,-0.95,-0.95,-0.947619048,-0.95,-0.952380952,-0.95,-0.919047619,0.84047619,0.914285714,0.926190476,0.930952381
        };
        // 上下轨迹位置 右
        static private double[] UP_PosY_R = new double[]{   -2.169230769,-2.046153846,-2.046153846,-1.838461538,-1.569230769,13.33076923,8.5,7.646153846,6.984615385,6.453846154,5.638461538,4.492307692,3.630769231,0.392307692,-10.66153846,-21.17692308,-7.5,-3.453846154,-2.353846154,-2.253846154
        };
        // 上下轨迹位置 左
        static private double[] UP_PosY_L = new double[]{
    6.453846154,5.638461538,4.492307692,3.630769231,0.392307692,-10.66153846,-21.17692308,-7.5,-3.453846154,-2.353846154,-2.253846154,-2.169230769,-2.046153846,-2.046153846,-1.838461538,-1.569230769,13.33076923,8.5,7.646153846,6.984615385

        };
        // 踝关节轨迹位置 右
        static private double[] UP_PosZ_R = new double[]{
         -0.041,-0.0385,-0.038,-0.034,2.294,-0.767,-1.134,0.14,0.1375,0.1255,0.111,0.088,0.0715,-0.3545,-0.332,-0.5515,0.466,-0.0555,-0.045,-0.0425
        };
        // 踝关节轨迹位置 左
        static private double[] UP_PosZ_L = new double[]{
0.1255,0.111,0.088,0.0715,-0.3545,-0.332,-0.5515,0.466,-0.0555,-0.045,-0.0425,-0.041,-0.0385,-0.038,-0.034,2.294,-0.767,-1.134,0.14,0.1375
        };


        #endregion   轨迹位置/时间数据
        #region 下坡坡轨迹位置/时间数据

        // PVT原始位置数据
        // 前后轨迹位置 右脚
        static private double[] DOWN_PosX_R = new double[]{
        -0.957142857,-0.957142857,-0.957142857,-0.957142857,-0.926190476,0.926190476,0.957142857, 0.957142857,0.957142857,0.957142857,0.957142857,0.966666667,0.957142857,0.957142857,0.935714286,
-0.935714286,-0.957142857,-0.957142857,-0.966666667,-0.957142857
        };
        // PVT原始位置数据
        // 前后轨迹位置 左脚
        static private double[] DOWN_PosX_L = new double[]{
0.957142857,0.957142857,0.966666667,0.957142857,0.957142857,0.935714286,-0.935714286,-0.957142857,-0.957142857,-0.966666667,-0.957142857,-0.957142857,-0.957142857,-0.957142857,-0.957142857,-0.926190476,0.926190476,0.957142857,0.957142857,0.957142857,0.957142857
        };
        // 上下轨迹位置 右
        static private double[] DOWN_PosY_R = new double[]{   2.061538462,2.184615385,2.276923077,2.461538462,4.376923077,11.49230769,3.246153846,2.253846154,0.923076923,-0.269230769,-1.3,-2.246153846,-3.461538462,-5.623076923,-12.13846154,-10.03846154,-1.984615385,1.8,1.930769231,2.053846154
        };
        // 上下轨迹位置 左
        static private double[] DOWN_PosY_L = new double[]{
-0.269230769,-1.3,-2.246153846,-3.461538462,-5.623076923,-12.13846154,-10.03846154,-1.984615385,1.8,1.930769231,2.053846154,2.061538462,2.184615385,2.276923077,2.461538462,4.376923077,11.49230769,3.246153846,2.253846154,0.923076923
        };
        // 踝关节轨迹位置 右
        static private double[] DOWN_PosZ_R = new double[]{
        0.067,0.0695,0.0715,0.076,3.4725,-1.0945,-1.819,0.063,0.0255,-0.0075,-0.036,-0.0625,-0.098,-0.593,-0.512,-0.624,0.7985,0.071,0.0645,0.0675
        };
        // 踝关节轨迹位置 左
        static private double[] DOWN_PosZ_L = new double[]{
         -0.0075,-0.036,-0.0625,-0.098,-0.593,-0.512,-0.624,0.7985,0.071,0.0645,0.0675,0.067,0.0695,0.0715,0.076,3.4725,-1.0945,-1.819,0.063,0.0255
        };


        #endregion   轨迹位置/时间数据
        // 运动控制器初始化： 回零并运动到循环步行的起始位置
        public static void init()
        {
            try
            {
                //float a = 1;
                //SMC606Helper.startStepCycle(a);
                // 首次运动，进行清零，定位，清零
                short iret = SMC606.connect(SMC606.connectNo);

                if (iret != 0)
                {
                    //labelStatus.Text = "未连接";
                    //MessageBox.Show("未连接");
                }
                else
                {

                    int ret1;
                    int ret2;

                    // 初始化并打开所有伺服
                    ret1 = SMC606.initAxis(axisLeft, axisLeft.Length);
                    ret2 = SMC606.initAxis(axisRight, axisRight.Length);
                    if ((ret1 != 0) | (ret2 != 0))
                    {
                        //MessageBox.Show("初始化设置错误！");
                        return;
                    }

                    // 0 3 端口写0 解除刹车锁定
                    LTSMC.smc_write_outbit(connectNo, 0, 0);
                    LTSMC.smc_write_outbit(connectNo, 3, 0);

                    // 解除刹车锁定需要响应时间，等待xxx毫秒
                    Thread.Sleep(100);

                    // 锁定读端口为1，解锁读端口为0
                    while (true)
                    {
                        short port0 = LTSMC.smc_read_outbit(connectNo, 0);
                        short port3 = LTSMC.smc_read_outbit(connectNo, 3);
                        if ((port0 == 0) && (port3 == 0))
                        {
                            break;
                        }
                        Thread.Sleep(10);
                    }

                    // 0 3 轴先向负方向运动一段距离再回零，减少回零时间
                    //SMC606Helper.setAxis03ToZero(); 
                    //等待6个轴停止运动，然后开始回零
                    bool SMC_Bool_CheckAxisStop_Left=SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
                    bool SMC_Bool_CheckAxisStop_Right=SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);
                    
                    
                    ret1 = SMC606.returnAxisHome(connectNo, axisLeft, axisLeft.Length, speed_init);
                    ret2 = SMC606.returnAxisHome(connectNo, axisRight, axisRight.Length,speed_init);
                    if ((ret1 != 0) | (ret2 != 0) )
                    {
                        //MessageBox.Show("多轴回零错误！");
                        return;
                    }

                    //等待6个轴停止运动，然后运动到循环起始点
                    SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
                    SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);

                    SMC606.clearAxisToZero(connectNo, axisLeft, axisLeft.Length); ;   // 回零后进行清零
                    SMC606.clearAxisToZero(connectNo, axisRight, axisRight.Length); ;   // 回零后进行清零      
             
                    //病人上去的位置
                    double[] distances_left = new double[] { -29000, -1000000, 0 };
                    double[] distances_right = new double[] { -29000, -1100000, 0 };
                    SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length, speed_init, distances_left);   // 多轴定长
                    SMC606.multiMotionDistance(SMC606.connectNo, axisRight, axisRight.Length, speed_init, distances_right);   // 多轴定长
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("初始化异常");
                //throw;
                LogClass.WriteExceptionLog(ex);
            }

        }

        // 运动控制器急停：emergency
        public static void stopEmergency()
        {
            try
            {
                LTSMC.smc_stop(SMC606.connectNo, SMC606.axis0, 1);    //立即停止
                LTSMC.smc_stop(SMC606.connectNo, SMC606.axis1, 1);
                LTSMC.smc_stop(SMC606.connectNo, SMC606.axis2, 1);
                LTSMC.smc_stop(SMC606.connectNo, SMC606.axis3, 1);
                LTSMC.smc_stop(SMC606.connectNo, SMC606.axis4, 1);
                LTSMC.smc_stop(SMC606.connectNo, SMC606.axis5, 1);
            }
            catch (Exception ex)
            {
                LogClass.WriteExceptionLog(ex);
            }
        }

        // 平地
        public static void closeController()
        {
            try
            {
                SMC606.multiAxisAllClose();
                // 刹车锁定 0 3 端口写1
                LTSMC.smc_write_outbit(connectNo, 0, 1);
                LTSMC.smc_write_outbit(connectNo, 3, 1);

                // 刹车锁定需要响应时间，等待500毫秒
                Thread.Sleep(500);

                SMC606.disconnect(SMC606.connectNo);

            }
            catch (Exception ex)
            {
                LogClass.WriteExceptionLog(ex);
            }
            
        }
        public static void positionSafe()
        {
            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.clearAxisToZero(connectNo, axisLeft, axisLeft.Length);  // 轴停止立即清零，将当前位置作为轨迹运动的起始点

            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);
            SMC606.clearAxisToZero(connectNo, axisRight, axisRight.Length);   // 轴停止立即清零，将当前位置作为轨迹运动的起始点
            //病人上去的位置
            double[] distances_left = new double[] { -29000, -1000000, 0 };
            double[] distances_right = new double[] { -29000, -1100000, 0 };
            SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length, speed_init, distances_left);   // 多轴定长
            SMC606.multiMotionDistance(SMC606.connectNo, axisRight, axisRight.Length, speed_init, distances_right);   // 多轴定长
        }
        // 单独回零
        public static void zeroEnter()
        { // 0 3 轴先向负方向运动一段距离再回零，减少回零时间
            //SMC606Helper.setAxis03ToZero(); 
            //等待6个轴停止运动，然后开始回零
            bool SMC_Bool_CheckAxisStop_Left = SMC606.checkAxisStop(SMC606Helper.connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            bool SMC_Bool_CheckAxisStop_Right = SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);

            double[] speed_init = new double[] { speed, speed * 2.5, speed };//x/y/z运行速度
            ushort ret1 = SMC606.returnAxisHome(connectNo, axisLeft, axisLeft.Length, speed_init);
            ushort ret2 = SMC606.returnAxisHome(connectNo, axisRight, axisRight.Length, speed_init);
            if ((ret1 != 0) | (ret2 != 0))
            {
                //MessageBox.Show("多轴回零错误！");
                return;
            }

            //等待6个轴停止运动，然后运动到循环起始点
            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);

            SMC606.clearAxisToZero(connectNo, axisLeft, axisLeft.Length); ;   // 回零后进行清零
            SMC606.clearAxisToZero(connectNo, axisRight, axisRight.Length); ;   // 回零后进行清零   

        }


        static private int getTimeDelay()
        {
            return Convert.ToInt32(14 * 0.123 * timeFactor * 10);  // 右脚的第14步左脚开始第1步，每步0.123秒，定时器间隔为0.1秒，
        }
        //平地运动
        static public void horienStepCycle(float miutes)
        {
            double[] distances_left = new double[] { -5170, -836076.923, 0 };
            double[] distances_right = new double[] { -31415, -1003307.692, 0 };

            SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length,speed_init, distances_left);   // 多轴定长
         
            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);

            SMC606Helper.startStepCycle(miutes, _PosX_R, _PosY_R, _PosZ_R, _PosX_L, _PosY_L, _PosZ_L);

            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);

            distances_left = new double[] { -29000, -1000000, 0 };
            distances_right = new double[] { -29000, -1100000, 0 };
            SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length, speed_init, distances_left);   // 多轴定长
            SMC606.multiMotionDistance(SMC606.connectNo, axisRight, axisRight.Length, speed_init, distances_right);   // 多轴定长

        }

        //上坡运动
        static public void upStepCycle(float miutes)
        {
            double[] distances_left = new double[] { -24970, -670846.1535, 0 };
            double[] distances_right = new double[] { -31415, -1003307.692, 0 };

            SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length, speed_init, distances_left);   // 多轴定长
            SMC606.multiMotionDistance(SMC606.connectNo, axisRight, axisRight.Length, speed_init, distances_right);   // 多轴定长

            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);
            SMC606Helper.startStepCycle(miutes, UP_PosX_R, UP_PosY_R, UP_PosZ_R, UP_PosX_L, UP_PosY_L, UP_PosZ_L);
            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);

            distances_left = new double[] { -29000, -1000000, 0 };
            distances_right = new double[] { -29000, -1100000, 0 };
            SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length, speed_init, distances_left);   // 多轴定长
            SMC606.multiMotionDistance(SMC606.connectNo, axisRight, axisRight.Length, speed_init, distances_right);   // 多轴定长
        }
        //下坡运动
        static public void downStepCycle(float miutes)
        {
            double[] distances_left = new double[] { -11880, -732076.923, 0 };
            double[] distances_right = new double[] { -21120, -1042153.846, 0 };
            SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length, speed_init, distances_left);   // 多轴定长
            SMC606.multiMotionDistance(SMC606.connectNo, axisRight, axisRight.Length, speed_init, distances_right);   // 多轴定长

            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);
            SMC606Helper.startStepCycle(miutes, DOWN_PosX_R, DOWN_PosY_R, DOWN_PosZ_R, DOWN_PosX_L, DOWN_PosY_L, DOWN_PosZ_L);
            SMC606.checkAxisStop(connectNo, axisLeft[0], axisLeft[1], axisLeft[2]);
            SMC606.checkAxisStop(connectNo, axisRight[0], axisRight[1], axisRight[2]);

            distances_left = new double[] { -29000, -1000000, 0 };
            distances_right = new double[] { -29000, -1100000, 0 };
            SMC606.multiMotionDistance(SMC606.connectNo, axisLeft, axisLeft.Length, speed_init, distances_left);   // 多轴定长
            SMC606.multiMotionDistance(SMC606.connectNo, axisRight, axisRight.Length, speed_init, distances_right);   // 多轴定长
        }
        // 开始步伐
        static public void startStepCycle(float minutes, double[] ING_PosX_R, double[] ING_PosY_R, double[] ING_PosZ_R,double[] ING_PosX_L,double[] ING_PosY_L,double[] ING_PosZ_L)
        {
            double[] _PVTPosX_R; 
            double[] _PVTPosY_R; 
            double[] _PVTPosZ_R; 
            double[] _PVTPosX_L; 
            double[] _PVTPosY_L; 
            double[] _PVTPosZ_L; 
            double[] _Time;
            double circle_time=10;
            //走了多少循环*(float)(minutes * 60.0 / 3.2)
            double circle_count = Math.Ceiling((minutes * 60 /circle_time ));//3.2为一个循环s数，多则进1步
            int PVT_Count = (ING_PosX_R.Length * (int)circle_count)+1;
            _PVTPosX_R = new double[PVT_Count];//根据循环数定义数组
            _PVTPosY_R = new double[PVT_Count];
            _PVTPosZ_R = new double[PVT_Count];
            _PVTPosX_L = new double[PVT_Count];
            _PVTPosY_L = new double[PVT_Count];
            _PVTPosZ_L = new double[PVT_Count];
            _Time = new double[PVT_Count];

            double fTime = circle_time / ING_PosX_R.Length;//一个循环中间小步多少秒
            //处理数据
            for (int i = 0; i < PVT_Count; i++)
            {
                if(i==0)
                {
                    _Time[i]=0;
                }
                else
                {
                    _Time[i] = fTime + _Time[i-1];
                }

                if (i % ING_PosX_R.Length == 0)
                {
                    _PVTPosX_R[i] = 0;
                    _PVTPosY_R[i] = 0;
                    _PVTPosZ_R[i] = 0;
                    _PVTPosX_L[i] = 0;
                    _PVTPosY_L[i] = 0;
                    _PVTPosZ_L[i] = 0;
                    continue;
                }

                int indexPos = i % ING_PosX_R.Length;
                _PVTPosX_R[i] = ING_PosX_R[indexPos - 1] * 10000 + _PVTPosX_R[i - 1];
                _PVTPosY_R[i] = ING_PosY_R[indexPos-1] * 10000 + _PVTPosY_R[i - 1];
                _PVTPosZ_R[i] = ING_PosZ_R[indexPos - 1] * 10000 + _PVTPosZ_R[i - 1];
                _PVTPosX_L[i] = ING_PosX_L[indexPos] * 10000 + _PVTPosX_L[i - 1];
                _PVTPosY_L[i] = ING_PosY_L[indexPos] * 10000 + _PVTPosY_L[i - 1];
                _PVTPosZ_L[i] = ING_PosZ_L[indexPos] * 10000 + _PVTPosZ_L[i - 1];
               
            }
            //需要取反的数据
            for (int i = 0; i < _PVTPosX_L.Length;i++ )
            {
                _PVTPosX_L[i] *= -1;
            }
                //开始走
            SMC606.multiAxisPVTS(connectNo, (ushort)axisRight.Length, axisRight, _PVTPosX_R, _PVTPosY_R, _PVTPosZ_R, _Time, (uint)_Time.Length);
            SMC606.multiAxisPVTS(connectNo, (ushort)axisLeft.Length, axisLeft, _PVTPosX_L, _PVTPosY_L, _PVTPosZ_L, _Time, (uint)_Time.Length);     
        }
    }
}
