using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TreadSys.Common
{
    public partial class DlgNumPad : Form
    {
        #region 1.成员

        private int decimals = 0;          // 输入框格式
        //private Sound btnSound = new Sound();

        /// <summary>
        /// 采用格式
        /// </summary>
        public enum ModeDef { Quality, DrugVolume, Temperatrue, Volume, Velocity, Time, Pressure, Other };

        #endregion

        #region 2.属性

        /// <summary>
        /// 输入框名称
        /// </summary>
        public string Title
        {
            get { return Lab_标题.Text; }
            set { Lab_标题.Text = value; }
        }

        /// <summary>
        /// 输入框单位
        /// </summary>
        public string Unit
        {
            get { return Lab_单位.Text; }

            set { Lab_单位.Text = value; }
        }

        /// <summary>
        /// 输入框最大值
        /// </summary>
        public double MaxValue
        {
            get { return hkNumPad1.MaxValue; }

            set
            {
                hkNumPad1.MaxValue = value;

                UpdateRangeLable();
            }
        }

        /// <summary>
        /// 输入框最小值
        /// </summary>
        public double MinValue
        {
            get { return hkNumPad1.MinValue; }

            set
            {
                hkNumPad1.MinValue = value;

                UpdateRangeLable();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableMaxChecking
        {
            get { return hkNumPad1.EnableMaxChecking; }

            set
            {
                Btn_最大.Visible = value;

                hkNumPad1.EnableMaxChecking = value;

                UpdateRangeLable();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableMinChecking
        {
            get { return hkNumPad1.EnableMinChecking; }

            set
            {
                hkNumPad1.EnableMinChecking = value;

                UpdateRangeLable();
            }
        }

        /// <summary>
        /// 输入框格式
        /// </summary>
        public int Decimals
        {
            get { return decimals; }

            set
            {
                decimals = value;
                hkNumPad1.Decimals = value;

                UpdateRangeLable();
            }
        }

        /// <summary>
        /// 获取输入框文字
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            return TBox_输入.Text;
        }

        /// <summary>
        /// 获取输入框数值
        /// </summary>
        /// <returns></returns>
        public double GetValue()
        {
            return hkNumPad1.GetValue();
        }

        /// <summary>
        /// 设置输入框数值
        /// </summary>
        /// <param name="v"></param>
        public void SetValue(double v)
        {
            double n = Math.Pow(10, decimals);
            double v1 = Math.Round((v * n) / n, Decimals);

            TBox_输入.Text = v1.ToString();
            hkNumPad1.TextValue = v1.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public hkNumPad GetNumPad()
        {
            return hkNumPad1;
        }

        /// <summary>
        /// 显示负值按钮
        /// </summary>
        public bool EnableNegtiveButton
        {
            get { return hkNumPad1.EnableNegtiveButton; }

            set { hkNumPad1.EnableNegtiveButton = value; }
        }

        #endregion

        #region 3.方法

        /// <summary>
        /// 初始化
        /// </summary>
        public DlgNumPad()
        {
            InitializeComponent();

            EnableMaxChecking = false;

            hkNumPad1.EventBtnClick += new ComonEvent(hkNumPad1_EventBtnClick);
            hkNumPad1.EventOK += new ComonEvent(hkNumPad1_EventOK);
            hkNumPad1.EventCancel += new ComonEvent(hkNumPad1_EventCancel);
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        /// <returns></returns>
        public new DialogResult ShowDialog()
        {
            UpdateRangeLable();

            return base.ShowDialog();
        }

        /// <summary>
        /// 模式
        /// </summary>
        /// <param name="m"></param>
        public void Mode(ModeDef m)
        {
            if (m == ModeDef.Quality)
            {
                Text = "规格";
                Lab_标题.Text = "规格";
                Lab_单位.Text = "mg";

                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";

                // Max Checking
                Btn_最大.Visible = false;
                hkNumPad1.EnableMinChecking = false;

                // Min Checking
                hkNumPad1.EnableMinChecking = true;
                hkNumPad1.MinValue = 1;

                // 小数位
                Decimals = 0;
                hkNumPad1.EnableDecimalsChecking = true;
            }
            else if (m == ModeDef.DrugVolume)
            {
                Text = "体积";
                Lab_标题.Text = "体积";
                Lab_单位.Text = "ml";

                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";

                // Max Checking
                Btn_最大.Visible = true;
                hkNumPad1.EnableMaxChecking = true;

                // Min Checking
                hkNumPad1.EnableMinChecking = true;
                hkNumPad1.MinValue = 0;

                // Decimals
                Decimals = 0;
                hkNumPad1.EnableDecimalsChecking = true;
            }
            else if (m == ModeDef.Temperatrue)
            {
                Text = "温度";
                Lab_标题.Text = "温度";
                Lab_单位.Text = "℃";

                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";

                // Max Checking
                Btn_最大.Visible = true;
                hkNumPad1.EnableMaxChecking = true;
                hkNumPad1.MaxValue = 80;

                // Min Checking
                hkNumPad1.EnableMinChecking = true;
                hkNumPad1.MinValue = 10;

                // Decimals
                Decimals = 1;
                hkNumPad1.EnableDecimalsChecking = true;
            }
            else if (m == ModeDef.Volume)
            {
                Text = "体积";
                Lab_标题.Text = "体积";
                Lab_单位.Text = "ml";

                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";

                // Max Checking
                Btn_最大.Visible = true;
                hkNumPad1.EnableMaxChecking = true;

                // Min Checking
                hkNumPad1.EnableMinChecking = true;
                hkNumPad1.MinValue = 0;

                // Decimals
                Decimals = 1;
                hkNumPad1.EnableDecimalsChecking = true;
            }
            else if (m == ModeDef.Velocity)
            {
                Text = "流速";
                Lab_标题.Text = "流速";
                Lab_单位.Text = "ml/s";

                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";

                // Max Checking
                Btn_最大.Visible = true;
                hkNumPad1.EnableMaxChecking = true;
                hkNumPad1.MaxValue = 10;

                // Min Checking
                hkNumPad1.EnableMinChecking = true;
                hkNumPad1.MinValue = 0;

                // Decimals
                Decimals = 1;
                hkNumPad1.EnableDecimalsChecking = true;
            }
            else if (m == ModeDef.Time)
            {
                Text = "时间";
                Lab_标题.Text = "时间";
                Lab_单位.Text = "秒";

                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";

                // Max Checking
                Btn_最大.Visible = false;
                hkNumPad1.EnableMaxChecking = false;

                // Min Checking
                hkNumPad1.EnableMinChecking = true;
                hkNumPad1.MinValue = 0;

                // Decimals
                Decimals = 0;
                hkNumPad1.EnableDecimalsChecking = true;
            }
            else if (m == ModeDef.Pressure)
            {
                Text = "压力";
                Lab_标题.Text = " 压力";
                Lab_单位.Text = "kg/cm²";

                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";

                // Max Checking
                Btn_最大.Visible = true;
                hkNumPad1.EnableMaxChecking = true;
                hkNumPad1.MaxValue = 10;

                // Min Checking
                hkNumPad1.EnableMinChecking = true;
                hkNumPad1.MinValue = 0;

                // Decimals
                Decimals = 0;
                hkNumPad1.EnableDecimalsChecking = true;
            }
            else
            {
                TBox_输入.Text = "";
                hkNumPad1.TextValue = "";
            }
        }

        private void SoundPath()
        {
            //btnSound.SoundPlayOne(Application.StartupPath + @"\Sound_Btn.wav", 50);
        }

        private void UpdateRangeLable()
        {
            double min = Math.Round(MinValue, decimals);
            double max = Math.Round(MaxValue, decimals);

            if (EnableMaxChecking || EnableMinChecking)
            {
                Lab_范围.Visible = true;
            }
            else
            {
                Lab_范围.Visible = false;
            }

            if (EnableMinChecking && EnableMaxChecking)
            {
                Lab_范围.Text = "取值范围: (" + min.ToString("") + ", " + max.ToString("") + ")";
            }
            else if (EnableMinChecking)
            {
                Lab_范围.Text = "取值范围: ≥" + MinValue.ToString("");
            }
            else if (EnableMaxChecking)
            {
                Lab_范围.Text = "取值范围: ≤" + MaxValue.ToString("");
            }
        }

        /// <summary>
        /// 热灌注加热功率
        /// </summary>
        /// <param name="flag">true:保留，false:令6~9Disable</param>
        public void SetHeatPowerMode(bool flag)
        {
            hkNumPad1.SetHeatPowerMode(flag);
        }

        #endregion

        #region 4.事件

        private void hkNumPad1_EventOK()
        {
            DialogResult = DialogResult.OK;
        }

        private void hkNumPad1_EventCancel()
        {
            DialogResult = DialogResult.Cancel;
        }

        private void hkNumPad1_EventBtnClick()
        {
            TBox_输入.Text = hkNumPad1.TextValue;
        }

        private void Btn_最大_Click(object sender, EventArgs e)
        {
            TBox_输入.Focus();
            SoundPath();
            double n = Math.Pow(10, decimals);
            double v1 = Math.Floor(hkNumPad1.MaxValue * n) / n;

            TBox_输入.Text = v1.ToString();
            hkNumPad1.TextValue = TBox_输入.Text;

            hkNumPad1.UpdateState();
        }

        private void Btn_关闭_Click(object sender, EventArgs e)
        {
            TBox_输入.Focus();
            SoundPath();
            DialogResult = DialogResult.Cancel;
        }

        private void TBox_输入_Click(object sender, EventArgs e)
        {
            SoundPath();  //播放按键音效
            TBox_输入.SelectAll();
            hkNumPad1.TextSelected = true;
        }

        private void DlgNumPad_Shown(object sender, EventArgs e)
        {
            hkNumPad1.UpdateState();

            TBox_输入.Focus();
            TBox_输入.SelectAll();

            hkNumPad1.TextSelected = true;
        }


        #endregion


    }
}