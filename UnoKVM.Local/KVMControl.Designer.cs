namespace UnoKVM.Local
{
    partial class KVMControl
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cbVideoSources = new ComboBox();
            label1 = new Label();
            cbInputDevices = new ComboBox();
            label2 = new Label();
            pbVideo = new PictureBox();
            btConnect = new Button();
            btRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)pbVideo).BeginInit();
            SuspendLayout();
            // 
            // cbVideoSources
            // 
            cbVideoSources.FormattingEnabled = true;
            cbVideoSources.Location = new Point(211, 35);
            cbVideoSources.Name = "cbVideoSources";
            cbVideoSources.Size = new Size(151, 28);
            cbVideoSources.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(211, 12);
            label1.Name = "label1";
            label1.Size = new Size(97, 20);
            label1.TabIndex = 1;
            label1.Text = "Video Source";
            // 
            // cbInputDevices
            // 
            cbInputDevices.FormattingEnabled = true;
            cbInputDevices.Location = new Point(14, 35);
            cbInputDevices.Name = "cbInputDevices";
            cbInputDevices.Size = new Size(151, 28);
            cbInputDevices.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 12);
            label2.Name = "label2";
            label2.Size = new Size(92, 20);
            label2.TabIndex = 1;
            label2.Text = "Input Device";
            // 
            // pbVideo
            // 
            pbVideo.Location = new Point(0, 80);
            pbVideo.Name = "pbVideo";
            pbVideo.Size = new Size(1000, 618);
            pbVideo.TabIndex = 2;
            pbVideo.TabStop = false;
            // 
            // btConnect
            // 
            btConnect.Location = new Point(624, 33);
            btConnect.Name = "btConnect";
            btConnect.Size = new Size(94, 29);
            btConnect.TabIndex = 3;
            btConnect.Text = "Connect";
            btConnect.UseVisualStyleBackColor = true;
            btConnect.Click += Connect;
            // 
            // btRefresh
            // 
            btRefresh.Location = new Point(755, 33);
            btRefresh.Name = "btRefresh";
            btRefresh.Size = new Size(94, 29);
            btRefresh.TabIndex = 3;
            btRefresh.Text = "Refresh";
            btRefresh.UseVisualStyleBackColor = true;
            btRefresh.Click += RefreshDevices;
            // 
            // KVMControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1006, 721);
            Controls.Add(btRefresh);
            Controls.Add(btConnect);
            Controls.Add(pbVideo);
            Controls.Add(label2);
            Controls.Add(cbInputDevices);
            Controls.Add(label1);
            Controls.Add(cbVideoSources);
            Name = "KVMControl";
            Text = "UnoKVM";
            FormClosing += KVMControl_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pbVideo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbVideoSources;
        private Label label1;
        private ComboBox cbInputDevices;
        private Label label2;
        private PictureBox pbVideo;
        private Button btConnect;
        public Button btRefresh;
    }
}
