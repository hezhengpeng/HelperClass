namespace TreadSys.Common
{
    partial class DlgNumPad
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgNumPad));
            this.panel3 = new System.Windows.Forms.Panel();
            this.Btn_最大 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Btn_关闭 = new System.Windows.Forms.Button();
            this.Lab_范围 = new System.Windows.Forms.Label();
            this.hkNumPad1 = new hkNumPad();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Lab_标题 = new System.Windows.Forms.Label();
            this.TBox_输入 = new System.Windows.Forms.TextBox();
            this.Lab_单位 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.Lab_单位);
            this.panel3.Controls.Add(this.Btn_最大);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.hkNumPad1);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.Lab_标题);
            this.panel3.Controls.Add(this.TBox_输入);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(489, 572);
            this.panel3.TabIndex = 13;
            // 
            // Btn_最大
            // 
            this.Btn_最大.BackColor = System.Drawing.Color.Transparent;
            this.Btn_最大.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Btn_最大.BackgroundImage")));
            this.Btn_最大.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Btn_最大.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.Btn_最大.FlatAppearance.BorderSize = 0;
            this.Btn_最大.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_最大.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_最大.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_最大.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_最大.ForeColor = System.Drawing.Color.Black;
            this.Btn_最大.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_最大.Location = new System.Drawing.Point(360, 50);
            this.Btn_最大.Margin = new System.Windows.Forms.Padding(10);
            this.Btn_最大.Name = "Btn_最大";
            this.Btn_最大.Size = new System.Drawing.Size(96, 46);
            this.Btn_最大.TabIndex = 112;
            this.Btn_最大.TabStop = false;
            this.Btn_最大.Text = "最大";
            this.Btn_最大.UseVisualStyleBackColor = false;
            this.Btn_最大.Click += new System.EventHandler(this.Btn_最大_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.Btn_关闭);
            this.panel2.Controls.Add(this.Lab_范围);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(488, 37);
            this.panel2.TabIndex = 13;
            // 
            // Btn_关闭
            // 
            this.Btn_关闭.BackColor = System.Drawing.Color.Transparent;
            this.Btn_关闭.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Btn_关闭.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.Btn_关闭.FlatAppearance.BorderSize = 0;
            this.Btn_关闭.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.Btn_关闭.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.Btn_关闭.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_关闭.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_关闭.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(219)))), ((int)(((byte)(235)))));
            this.Btn_关闭.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_关闭.Location = new System.Drawing.Point(453, 3);
            this.Btn_关闭.Margin = new System.Windows.Forms.Padding(10);
            this.Btn_关闭.Name = "Btn_关闭";
            this.Btn_关闭.Size = new System.Drawing.Size(30, 30);
            this.Btn_关闭.TabIndex = 112;
            this.Btn_关闭.TabStop = false;
            this.Btn_关闭.UseVisualStyleBackColor = false;
            this.Btn_关闭.Click += new System.EventHandler(this.Btn_关闭_Click);
            // 
            // Lab_范围
            // 
            this.Lab_范围.AutoSize = true;
            this.Lab_范围.BackColor = System.Drawing.Color.Transparent;
            this.Lab_范围.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.Lab_范围.ForeColor = System.Drawing.Color.Black;
            this.Lab_范围.Location = new System.Drawing.Point(8, 7);
            this.Lab_范围.Margin = new System.Windows.Forms.Padding(0);
            this.Lab_范围.Name = "Lab_范围";
            this.Lab_范围.Size = new System.Drawing.Size(82, 24);
            this.Lab_范围.TabIndex = 9;
            this.Lab_范围.Text = "数值输入";
            // 
            // hkNumPad1
            // 
            this.hkNumPad1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.hkNumPad1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.hkNumPad1.Decimals = 1;
            this.hkNumPad1.EnableDecimalsChecking = false;
            this.hkNumPad1.EnableMaxChecking = false;
            this.hkNumPad1.EnableMinChecking = false;
            this.hkNumPad1.EnableNegtiveButton = false;
            this.hkNumPad1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hkNumPad1.Location = new System.Drawing.Point(25, 110);
            this.hkNumPad1.Margin = new System.Windows.Forms.Padding(4);
            this.hkNumPad1.MaxValue = 100D;
            this.hkNumPad1.MinValue = 0.1D;
            this.hkNumPad1.Name = "hkNumPad1";
            this.hkNumPad1.Size = new System.Drawing.Size(440, 456);
            this.hkNumPad1.TabIndex = 2;
            this.hkNumPad1.TextSelected = false;
            this.hkNumPad1.TextValue = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(-15, 102);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 2);
            this.panel1.TabIndex = 11;
            // 
            // Lab_标题
            // 
            this.Lab_标题.AutoSize = true;
            this.Lab_标题.BackColor = System.Drawing.Color.Transparent;
            this.Lab_标题.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.Lab_标题.ForeColor = System.Drawing.Color.White;
            this.Lab_标题.Location = new System.Drawing.Point(42, 61);
            this.Lab_标题.Margin = new System.Windows.Forms.Padding(0);
            this.Lab_标题.Name = "Lab_标题";
            this.Lab_标题.Size = new System.Drawing.Size(69, 25);
            this.Lab_标题.TabIndex = 9;
            this.Lab_标题.Text = "请输入";
            // 
            // TBox_输入
            // 
            this.TBox_输入.BackColor = System.Drawing.SystemColors.Window;
            this.TBox_输入.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBox_输入.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.TBox_输入.Location = new System.Drawing.Point(133, 59);
            this.TBox_输入.Margin = new System.Windows.Forms.Padding(0);
            this.TBox_输入.Name = "TBox_输入";
            this.TBox_输入.ReadOnly = true;
            this.TBox_输入.Size = new System.Drawing.Size(127, 32);
            this.TBox_输入.TabIndex = 8;
            this.TBox_输入.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TBox_输入.Click += new System.EventHandler(this.TBox_输入_Click);
            // 
            // Lab_单位
            // 
            this.Lab_单位.AutoSize = true;
            this.Lab_单位.BackColor = System.Drawing.Color.Transparent;
            this.Lab_单位.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.Lab_单位.ForeColor = System.Drawing.Color.White;
            this.Lab_单位.Location = new System.Drawing.Point(271, 61);
            this.Lab_单位.Margin = new System.Windows.Forms.Padding(0);
            this.Lab_单位.Name = "Lab_单位";
            this.Lab_单位.Size = new System.Drawing.Size(0, 25);
            this.Lab_单位.TabIndex = 113;
            // 
            // DlgNumPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(489, 572);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("黑体", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DlgNumPad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "药物规格";
            this.Shown += new System.EventHandler(this.DlgNumPad_Shown);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox TBox_输入;
        private System.Windows.Forms.Label Lab_标题;
        public hkNumPad hkNumPad1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Lab_范围;
        private System.Windows.Forms.Button Btn_最大;
        private System.Windows.Forms.Button Btn_关闭;
        private System.Windows.Forms.Label Lab_单位;

    }
}