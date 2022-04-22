using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TreadSys.Common
{
    public delegate void ComonEvent();

    /// <summary>
    /// Numerical keyboard
    /// </summary>
    public partial class hkNumPad : UserControl
    {
        #region 1.成员

        //private Sound btnSound = new Sound();
        private string text = "";
        private bool bTextSelected = false;

        private double maxc = 100;
        private bool bEnableMaxChecking = false;

        private double minValue = 0.1;
        private double maxValue = 10;
        private bool bEnableMinChecking = false;

        private bool bEnableNegtiveButton = false;

        private int decimals = 1;     // 小数位
        private bool bEnableDecimalsChecking = false;

        private string lastKey = "";    // 双击处理
        private DateTime lastTime = DateTime.Now;

        public event ComonEvent EventOK = null;
        public event ComonEvent EventBtnClick = null;
        public event ComonEvent EventCancel = null;

        #endregion

        #region 2.属性

        /// <summary>
        /// 文字选中状态
        /// 文字被选中时, 点击按键自动清空原来的text内容
        /// </summary>
        public bool TextSelected
        {
            get
            {
                return bTextSelected;
            }

            set
            {
                bTextSelected = value;
            }
        }

        /// <summary>
        /// 输入值
        /// </summary>
        public string TextValue
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get
            {
                return maxValue;
            }

            set
            {
                maxValue = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableMaxChecking
        {
            get
            {
                return bEnableMaxChecking;
            }

            set
            {
                bEnableMaxChecking = value;
            }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue
        {
            get
            {
                return minValue;
            }

            set
            {
                minValue = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableMinChecking
        {
            get
            {
                return bEnableMinChecking;
            }

            set
            {
                bEnableMinChecking = value;
            }
        }

        /// <summary>
        /// 格式（小数点）
        /// </summary>
        public int Decimals
        {
            get
            {
                return decimals;
            }

            set
            {
                decimals = value;
            }
        }

        /// <summary>
        /// 小数输入
        /// </summary>
        public bool EnableDecimalsChecking
        {
            get
            {
                return bEnableDecimalsChecking;
            }

            set
            {
                bEnableDecimalsChecking = value;
            }
        }

        /// <summary>
        /// 显示负值按钮
        /// </summary>
        public bool EnableNegtiveButton
        {
            get
            {
                return bEnableNegtiveButton;
            }

            set
            {
                bEnableNegtiveButton = value;

                if (bEnableNegtiveButton == true)
                {
                    btnOK2.Visible = false;
                    btnNegtive.Enabled = true;
                }
                else
                {
                    btnOK2.Visible = true;
                    btnNegtive.Enabled = false;
                }
            }
        }

        #endregion

        #region 3.方法

        private void SoundPath()
        {
            //btnSound.SoundPlayOne(Application.StartupPath + @"\Sound_Btn.wav", 50);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public hkNumPad()
        {
            InitializeComponent();

            tbFocus.Focus();
        }

        /// <summary>
        /// 添加处理
        /// </summary>
        public void UpdateState()
        {
            btnOK.Enabled = true;
            btnOK2.Enabled = true;

            try
            {
                double v = Convert.ToDouble(text.ToString());
                if (bEnableMaxChecking && v > maxValue)
                {
                    btnOK.Enabled = false;
                    btnOK2.Enabled = false;
                }

                if (bEnableMinChecking && v < minValue)
                {
                    btnOK.Enabled = false;
                    btnOK2.Enabled = false;
                }

                if (bEnableDecimalsChecking)
                {
                    if (text.IndexOf('.') > -1)       //存在小数点
                    {
                        string i1 = text.Substring(0, text.IndexOf('.'));                              //小数点左边值
                        string i2 = text.Substring(text.IndexOf('.'), text.Length - text.IndexOf('.'));//小数点右边值

                        if (i2.Length > decimals + 1)
                            i2 = i2.Substring(0, decimals + 1);
                        string v1 = i1 + i2;
                        if (v.ToString() != v1)
                            text = v1.ToString();
                    }
                    else
                    {
                        double n = Math.Pow(10, decimals);
                        double v1 = Math.Floor(v * n) / n;
                        if (v != v1)
                            text = v1.ToString();
                    }
                }
            }
            catch
            {
                btnOK.Enabled = false;
                btnOK2.Enabled = false;

                if (EventBtnClick != null)
                    EventBtnClick();

                return;
            }

            if (EventBtnClick != null)
                EventBtnClick();
        }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public double GetValue()
        {
            double v;

            try
            {
                v = Convert.ToDouble(text.ToString());
            }
            catch
            {
                if (EnableMinChecking)
                {
                    v = minValue;
                }
                else
                {
                    v = 0;
                }
            }

            return v;
        }

        /// <summary>
        /// 热灌注加热功率
        /// </summary>
        /// <param name="flag">true:保留，false:令6~9Disable</param>
        public void SetHeatPowerMode(bool flag)
        {
            btn0.Enabled = flag;
            btn6.Enabled = flag;
            btn7.Enabled = flag;
            btn8.Enabled = flag;
            btn9.Enabled = flag;
            btnDot.Enabled = flag;
            btnNegtive.Enabled = flag;
        }

        // 选中处理
        private void SelectedCheck()
        {
            if (bTextSelected)
            {
                bTextSelected = false;
                text = "";
            }
        }

        #endregion

        #region 4.事件

        private void btnNegtive_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "±" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "±";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "-";
            else
            {
                int index = text.IndexOf('-');

                if (index == -1)
                {
                    text = "-" + text;
                }
                else
                {
                    text = text.Remove(index, 1);
                }
            }

            UpdateState();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "7" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "7";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "7";
            else
                text += "7";

            UpdateState();
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "8" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "8";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "8";
            else
                text += "8";

            UpdateState();
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "9" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "9";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "9";
            else
                text += "9";

            UpdateState();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "4" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "4";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "4";
            else
                text += "4";

            UpdateState();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "5" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "5";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "5";
            else
                text += "5";

            UpdateState();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "6" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "6";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "6";
            else
                text += "6";

            UpdateState();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "1" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "1";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "1";
            else
                text += "1";

            UpdateState();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "2" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "2";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "2";
            else
                text += "2";

            UpdateState();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "3" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "3";
            lastTime = DateTime.Now;

            if (text == "0")
                text = "3";
            else
                text += "3";

            UpdateState();
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "0" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "0";
            lastTime = DateTime.Now;

            if (GetValue() != 0 || text.Length == 0 || text.IndexOf('.') != -1)
            {
                text += "0";
            }

            UpdateState();
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "." && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = ".";
            lastTime = DateTime.Now;

            int i = text.IndexOf('.');

            if (i == -1)
            {
                text += ".";
            }

            UpdateState();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            // 双击处理
            TimeSpan ts = DateTime.Now - lastTime;
            if (lastKey == "b" && ts.TotalMilliseconds < 100)
            {
                return;
            }

            // 选中处理
            SelectedCheck();

            lastKey = "b";
            lastTime = DateTime.Now;

            int n = text.Length;

            if (n > 0)
            {
                text = text.Substring(0, n - 1);
            }

            UpdateState();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            text = "";

            UpdateState();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            if (EventBtnClick != null)
                EventBtnClick();

            if (EventOK != null)
            {
                EventOK();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tbFocus.Focus();
            SoundPath();
            if (EventCancel != null)
            {
                EventCancel();
            }
        }

        #endregion
    }
}