namespace IOF.TradeAPI.Test
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabPage_gateWayAPITest = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBox_gateWayApiResult = new System.Windows.Forms.ListBox();
            this.tab_tradeApiTest = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.lab_port = new System.Windows.Forms.Label();
            this.textBox_baseAddress = new System.Windows.Forms.TextBox();
            this.lab_baseAddress = new System.Windows.Forms.Label();
            this.btn_setparameter = new System.Windows.Forms.Button();
            this.bun_login = new System.Windows.Forms.Button();
            this.btn_allcall = new System.Windows.Forms.Button();
            this.btn_singlecall = new System.Windows.Forms.Button();
            this.listBox_tradeApiResult = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MethodUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_gateWayAPITest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tab_tradeApiTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage_gateWayAPITest
            // 
            this.tabPage_gateWayAPITest.Controls.Add(this.splitContainer2);
            this.tabPage_gateWayAPITest.Location = new System.Drawing.Point(4, 22);
            this.tabPage_gateWayAPITest.Name = "tabPage_gateWayAPITest";
            this.tabPage_gateWayAPITest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_gateWayAPITest.Size = new System.Drawing.Size(1479, 818);
            this.tabPage_gateWayAPITest.TabIndex = 1;
            this.tabPage_gateWayAPITest.Text = "网关接口测试";
            this.tabPage_gateWayAPITest.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBox_gateWayApiResult);
            this.splitContainer2.Size = new System.Drawing.Size(1473, 812);
            this.splitContainer2.SplitterDistance = 490;
            this.splitContainer2.TabIndex = 0;
            // 
            // listBox_gateWayApiResult
            // 
            this.listBox_gateWayApiResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_gateWayApiResult.FormattingEnabled = true;
            this.listBox_gateWayApiResult.ItemHeight = 12;
            this.listBox_gateWayApiResult.Location = new System.Drawing.Point(0, 0);
            this.listBox_gateWayApiResult.Name = "listBox_gateWayApiResult";
            this.listBox_gateWayApiResult.Size = new System.Drawing.Size(979, 812);
            this.listBox_gateWayApiResult.TabIndex = 0;
            // 
            // tab_tradeApiTest
            // 
            this.tab_tradeApiTest.Controls.Add(this.splitContainer1);
            this.tab_tradeApiTest.Location = new System.Drawing.Point(4, 22);
            this.tab_tradeApiTest.Name = "tab_tradeApiTest";
            this.tab_tradeApiTest.Padding = new System.Windows.Forms.Padding(3);
            this.tab_tradeApiTest.Size = new System.Drawing.Size(1479, 818);
            this.tab_tradeApiTest.TabIndex = 0;
            this.tab_tradeApiTest.Text = "交易接口测试";
            this.tab_tradeApiTest.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox_tradeApiResult);
            this.splitContainer1.Size = new System.Drawing.Size(1473, 812);
            this.splitContainer1.SplitterDistance = 1024;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.textBox_port);
            this.splitContainer3.Panel2.Controls.Add(this.lab_port);
            this.splitContainer3.Panel2.Controls.Add(this.textBox_baseAddress);
            this.splitContainer3.Panel2.Controls.Add(this.lab_baseAddress);
            this.splitContainer3.Panel2.Controls.Add(this.btn_setparameter);
            this.splitContainer3.Panel2.Controls.Add(this.bun_login);
            this.splitContainer3.Panel2.Controls.Add(this.btn_allcall);
            this.splitContainer3.Panel2.Controls.Add(this.btn_singlecall);
            this.splitContainer3.Size = new System.Drawing.Size(1024, 812);
            this.splitContainer3.SplitterDistance = 635;
            this.splitContainer3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MethodUrl,
            this.Parameter});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1024, 635);
            this.dataGridView1.TabIndex = 0;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(384, 19);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(100, 21);
            this.textBox_port.TabIndex = 8;
            // 
            // lab_port
            // 
            this.lab_port.AutoSize = true;
            this.lab_port.Location = new System.Drawing.Point(346, 22);
            this.lab_port.Name = "lab_port";
            this.lab_port.Size = new System.Drawing.Size(41, 12);
            this.lab_port.TabIndex = 7;
            this.lab_port.Text = "端口：";
            // 
            // textBox_baseAddress
            // 
            this.textBox_baseAddress.Location = new System.Drawing.Point(86, 18);
            this.textBox_baseAddress.Name = "textBox_baseAddress";
            this.textBox_baseAddress.Size = new System.Drawing.Size(253, 21);
            this.textBox_baseAddress.TabIndex = 6;
            this.textBox_baseAddress.Text = "127.0.0.1";
            // 
            // lab_baseAddress
            // 
            this.lab_baseAddress.AutoSize = true;
            this.lab_baseAddress.Location = new System.Drawing.Point(14, 22);
            this.lab_baseAddress.Name = "lab_baseAddress";
            this.lab_baseAddress.Size = new System.Drawing.Size(77, 12);
            this.lab_baseAddress.TabIndex = 5;
            this.lab_baseAddress.Text = "接口基地址：";
            // 
            // btn_setparameter
            // 
            this.btn_setparameter.Location = new System.Drawing.Point(326, 55);
            this.btn_setparameter.Name = "btn_setparameter";
            this.btn_setparameter.Size = new System.Drawing.Size(133, 23);
            this.btn_setparameter.TabIndex = 4;
            this.btn_setparameter.Text = "设置所选接口参数";
            this.btn_setparameter.UseVisualStyleBackColor = true;
            // 
            // bun_login
            // 
            this.bun_login.Location = new System.Drawing.Point(16, 55);
            this.bun_login.Name = "bun_login";
            this.bun_login.Size = new System.Drawing.Size(75, 23);
            this.bun_login.TabIndex = 3;
            this.bun_login.Text = "登陆";
            this.bun_login.UseVisualStyleBackColor = true;
            // 
            // btn_allcall
            // 
            this.btn_allcall.Location = new System.Drawing.Point(223, 55);
            this.btn_allcall.Name = "btn_allcall";
            this.btn_allcall.Size = new System.Drawing.Size(75, 23);
            this.btn_allcall.TabIndex = 2;
            this.btn_allcall.Text = "全部调用";
            this.btn_allcall.UseVisualStyleBackColor = true;
            // 
            // btn_singlecall
            // 
            this.btn_singlecall.Location = new System.Drawing.Point(110, 55);
            this.btn_singlecall.Name = "btn_singlecall";
            this.btn_singlecall.Size = new System.Drawing.Size(93, 23);
            this.btn_singlecall.TabIndex = 0;
            this.btn_singlecall.Text = "单个调用";
            this.btn_singlecall.UseVisualStyleBackColor = true;
            // 
            // listBox_tradeApiResult
            // 
            this.listBox_tradeApiResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_tradeApiResult.FormattingEnabled = true;
            this.listBox_tradeApiResult.ItemHeight = 12;
            this.listBox_tradeApiResult.Location = new System.Drawing.Point(0, 0);
            this.listBox_tradeApiResult.Name = "listBox_tradeApiResult";
            this.listBox_tradeApiResult.Size = new System.Drawing.Size(445, 812);
            this.listBox_tradeApiResult.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_tradeApiTest);
            this.tabControl1.Controls.Add(this.tabPage_gateWayAPITest);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1487, 844);
            this.tabControl1.TabIndex = 0;
            // 
            // MethodUrl
            // 
            this.MethodUrl.HeaderText = "接口地址";
            this.MethodUrl.Name = "MethodUrl";
            this.MethodUrl.ReadOnly = true;
            this.MethodUrl.Width = 200;
            // 
            // Parameter
            // 
            this.Parameter.HeaderText = "参数";
            this.Parameter.Name = "Parameter";
            this.Parameter.ReadOnly = true;
            this.Parameter.Width = 700;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1487, 844);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "交易服务Api测试工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabPage_gateWayAPITest.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tab_tradeApiTest.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage_gateWayAPITest;
        private System.Windows.Forms.TabPage tab_tradeApiTest;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listBox_gateWayApiResult;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox_tradeApiResult;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_setparameter;
        private System.Windows.Forms.Button bun_login;
        private System.Windows.Forms.Button btn_allcall;
        private System.Windows.Forms.Button btn_singlecall;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label lab_port;
        private System.Windows.Forms.TextBox textBox_baseAddress;
        private System.Windows.Forms.Label lab_baseAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn MethodUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
    }
}

