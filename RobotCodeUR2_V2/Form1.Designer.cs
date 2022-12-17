namespace RobotCodeUR2_V2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.shapeDataLabel = new System.Windows.Forms.Label();
            this.lockStateLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.contoursLabel = new System.Windows.Forms.Label();
            this.bgrLabel = new System.Windows.Forms.Label();
            this.blurLabel = new System.Windows.Forms.Label();
            this.gaussianUp = new System.Windows.Forms.Button();
            this.gaussianDown = new System.Windows.Forms.Button();
            this.bgrSlider = new System.Windows.Forms.TrackBar();
            this.triAreaSlider = new System.Windows.Forms.TrackBar();
            this.triAreaLabel = new System.Windows.Forms.Label();
            this.rawFrame = new System.Windows.Forms.PictureBox();
            this.decoFrame = new System.Windows.Forms.PictureBox();
            this.blurFrame = new System.Windows.Forms.PictureBox();
            this.bgrFrame = new System.Windows.Forms.PictureBox();
            this.squareAreaLabel = new System.Windows.Forms.Label();
            this.squareAreaSlider = new System.Windows.Forms.TrackBar();
            this.borderAreaLabel = new System.Windows.Forms.Label();
            this.borderAreaSlider = new System.Windows.Forms.TrackBar();
            this.openCommsButton = new System.Windows.Forms.Button();
            this.serialCommState = new System.Windows.Forms.Label();
            this.arduinoDataLabel = new System.Windows.Forms.TextBox();
            this.comPortTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bgrSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triAreaSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rawFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decoFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blurFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgrFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.squareAreaSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderAreaSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // shapeDataLabel
            // 
            this.shapeDataLabel.AutoSize = true;
            this.shapeDataLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.shapeDataLabel.ForeColor = System.Drawing.Color.Cyan;
            this.shapeDataLabel.Location = new System.Drawing.Point(2020, 10);
            this.shapeDataLabel.Margin = new System.Windows.Forms.Padding(0);
            this.shapeDataLabel.Name = "shapeDataLabel";
            this.shapeDataLabel.Size = new System.Drawing.Size(138, 29);
            this.shapeDataLabel.TabIndex = 2;
            this.shapeDataLabel.Text = "Shape Data";
            // 
            // lockStateLabel
            // 
            this.lockStateLabel.AutoSize = true;
            this.lockStateLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lockStateLabel.ForeColor = System.Drawing.Color.Cyan;
            this.lockStateLabel.Location = new System.Drawing.Point(2020, 950);
            this.lockStateLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lockStateLabel.Name = "lockStateLabel";
            this.lockStateLabel.Size = new System.Drawing.Size(125, 29);
            this.lockStateLabel.TabIndex = 5;
            this.lockStateLabel.Text = "Lock State";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(2020, 1850);
            this.startButton.Margin = new System.Windows.Forms.Padding(2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(301, 82);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Press to begin shape accquisition";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // contoursLabel
            // 
            this.contoursLabel.AutoSize = true;
            this.contoursLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.contoursLabel.ForeColor = System.Drawing.Color.Cyan;
            this.contoursLabel.Location = new System.Drawing.Point(2200, 950);
            this.contoursLabel.Margin = new System.Windows.Forms.Padding(0);
            this.contoursLabel.Name = "contoursLabel";
            this.contoursLabel.Size = new System.Drawing.Size(110, 29);
            this.contoursLabel.TabIndex = 9;
            this.contoursLabel.Text = "Contours";
            // 
            // bgrLabel
            // 
            this.bgrLabel.AutoSize = true;
            this.bgrLabel.Location = new System.Drawing.Point(2020, 1150);
            this.bgrLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.bgrLabel.Name = "bgrLabel";
            this.bgrLabel.Size = new System.Drawing.Size(185, 29);
            this.bgrLabel.TabIndex = 12;
            this.bgrLabel.Text = "BGR filter value:";
            // 
            // blurLabel
            // 
            this.blurLabel.AutoSize = true;
            this.blurLabel.Location = new System.Drawing.Point(2020, 1017);
            this.blurLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.blurLabel.Name = "blurLabel";
            this.blurLabel.Size = new System.Drawing.Size(229, 29);
            this.blurLabel.TabIndex = 13;
            this.blurLabel.Text = "Gaussian blur value:";
            // 
            // gaussianUp
            // 
            this.gaussianUp.Location = new System.Drawing.Point(2020, 1050);
            this.gaussianUp.Name = "gaussianUp";
            this.gaussianUp.Size = new System.Drawing.Size(200, 100);
            this.gaussianUp.TabIndex = 16;
            this.gaussianUp.Text = "Increase Blur";
            this.gaussianUp.UseVisualStyleBackColor = true;
            this.gaussianUp.Click += new System.EventHandler(this.gaussianUp_Click);
            // 
            // gaussianDown
            // 
            this.gaussianDown.Location = new System.Drawing.Point(2250, 1050);
            this.gaussianDown.Name = "gaussianDown";
            this.gaussianDown.Size = new System.Drawing.Size(200, 100);
            this.gaussianDown.TabIndex = 17;
            this.gaussianDown.Text = "Decrease Blur";
            this.gaussianDown.UseVisualStyleBackColor = true;
            this.gaussianDown.Click += new System.EventHandler(this.gaussianDown_Click);
            // 
            // bgrSlider
            // 
            this.bgrSlider.AutoSize = false;
            this.bgrSlider.LargeChange = 10;
            this.bgrSlider.Location = new System.Drawing.Point(2020, 1200);
            this.bgrSlider.Margin = new System.Windows.Forms.Padding(2);
            this.bgrSlider.Maximum = 255;
            this.bgrSlider.Name = "bgrSlider";
            this.bgrSlider.Size = new System.Drawing.Size(366, 100);
            this.bgrSlider.SmallChange = 2;
            this.bgrSlider.TabIndex = 14;
            this.bgrSlider.Scroll += new System.EventHandler(this.bgrSlider_Scroll);
            // 
            // triAreaSlider
            // 
            this.triAreaSlider.Location = new System.Drawing.Point(2020, 1350);
            this.triAreaSlider.Maximum = 10000;
            this.triAreaSlider.Minimum = 1100;
            this.triAreaSlider.Name = "triAreaSlider";
            this.triAreaSlider.Size = new System.Drawing.Size(366, 101);
            this.triAreaSlider.TabIndex = 18;
            this.triAreaSlider.Value = 1100;
            this.triAreaSlider.Scroll += new System.EventHandler(this.triAreaSlider_Scroll);
            // 
            // triAreaLabel
            // 
            this.triAreaLabel.AutoSize = true;
            this.triAreaLabel.Location = new System.Drawing.Point(2020, 1300);
            this.triAreaLabel.Name = "triAreaLabel";
            this.triAreaLabel.Size = new System.Drawing.Size(251, 29);
            this.triAreaLabel.TabIndex = 21;
            this.triAreaLabel.Text = "Triangle area modifier";
            // 
            // rawFrame
            // 
            this.rawFrame.Location = new System.Drawing.Point(12, 11);
            this.rawFrame.Name = "rawFrame";
            this.rawFrame.Size = new System.Drawing.Size(1000, 1000);
            this.rawFrame.TabIndex = 26;
            this.rawFrame.TabStop = false;
            // 
            // decoFrame
            // 
            this.decoFrame.Location = new System.Drawing.Point(1018, 11);
            this.decoFrame.Name = "decoFrame";
            this.decoFrame.Size = new System.Drawing.Size(1000, 1000);
            this.decoFrame.TabIndex = 27;
            this.decoFrame.TabStop = false;
            // 
            // blurFrame
            // 
            this.blurFrame.Location = new System.Drawing.Point(12, 1017);
            this.blurFrame.Name = "blurFrame";
            this.blurFrame.Size = new System.Drawing.Size(1000, 1000);
            this.blurFrame.TabIndex = 28;
            this.blurFrame.TabStop = false;
            // 
            // bgrFrame
            // 
            this.bgrFrame.Location = new System.Drawing.Point(1018, 1017);
            this.bgrFrame.Name = "bgrFrame";
            this.bgrFrame.Size = new System.Drawing.Size(1000, 1000);
            this.bgrFrame.TabIndex = 29;
            this.bgrFrame.TabStop = false;
            // 
            // squareAreaLabel
            // 
            this.squareAreaLabel.AutoSize = true;
            this.squareAreaLabel.Location = new System.Drawing.Point(2020, 1450);
            this.squareAreaLabel.Name = "squareAreaLabel";
            this.squareAreaLabel.Size = new System.Drawing.Size(239, 29);
            this.squareAreaLabel.TabIndex = 30;
            this.squareAreaLabel.Text = "Square area modifier";
            // 
            // squareAreaSlider
            // 
            this.squareAreaSlider.Location = new System.Drawing.Point(2020, 1500);
            this.squareAreaSlider.Maximum = 20000;
            this.squareAreaSlider.Minimum = 1000;
            this.squareAreaSlider.Name = "squareAreaSlider";
            this.squareAreaSlider.Size = new System.Drawing.Size(366, 101);
            this.squareAreaSlider.TabIndex = 31;
            this.squareAreaSlider.Value = 1000;
            this.squareAreaSlider.Scroll += new System.EventHandler(this.squareAreaSlider_Scroll);
            // 
            // borderAreaLabel
            // 
            this.borderAreaLabel.AutoSize = true;
            this.borderAreaLabel.Location = new System.Drawing.Point(2020, 1600);
            this.borderAreaLabel.Name = "borderAreaLabel";
            this.borderAreaLabel.Size = new System.Drawing.Size(235, 29);
            this.borderAreaLabel.TabIndex = 32;
            this.borderAreaLabel.Text = "Border area modifier";
            // 
            // borderAreaSlider
            // 
            this.borderAreaSlider.Location = new System.Drawing.Point(2020, 1650);
            this.borderAreaSlider.Maximum = 60000;
            this.borderAreaSlider.Name = "borderAreaSlider";
            this.borderAreaSlider.Size = new System.Drawing.Size(366, 101);
            this.borderAreaSlider.TabIndex = 33;
            this.borderAreaSlider.Value = 45000;
            this.borderAreaSlider.Scroll += new System.EventHandler(this.borderAreaSlider_Scroll);
            // 
            // openCommsButton
            // 
            this.openCommsButton.Location = new System.Drawing.Point(2020, 1780);
            this.openCommsButton.Name = "openCommsButton";
            this.openCommsButton.Size = new System.Drawing.Size(302, 75);
            this.openCommsButton.TabIndex = 34;
            this.openCommsButton.Text = "Press to open serial comms with Arduino";
            this.openCommsButton.UseVisualStyleBackColor = true;
            this.openCommsButton.Click += new System.EventHandler(this.openCommsButton_Click);
            // 
            // serialCommState
            // 
            this.serialCommState.AutoSize = true;
            this.serialCommState.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.serialCommState.ForeColor = System.Drawing.Color.Cyan;
            this.serialCommState.Location = new System.Drawing.Point(2350, 1780);
            this.serialCommState.Name = "serialCommState";
            this.serialCommState.Size = new System.Drawing.Size(197, 29);
            this.serialCommState.TabIndex = 35;
            this.serialCommState.Text = "Arduino is offline.";
            // 
            // arduinoDataLabel
            // 
            this.arduinoDataLabel.BackColor = System.Drawing.SystemColors.WindowText;
            this.arduinoDataLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.arduinoDataLabel.Location = new System.Drawing.Point(2025, 481);
            this.arduinoDataLabel.Name = "arduinoDataLabel";
            this.arduinoDataLabel.Size = new System.Drawing.Size(291, 35);
            this.arduinoDataLabel.TabIndex = 36;
            this.arduinoDataLabel.Text = "Returned coordinate data";
            this.arduinoDataLabel.TextChanged += new System.EventHandler(this.arduinoDataLabel_TextChanged);
            // 
            // comPortTextBox
            // 
            this.comPortTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.comPortTextBox.ForeColor = System.Drawing.Color.Cyan;
            this.comPortTextBox.Location = new System.Drawing.Point(2020, 1720);
            this.comPortTextBox.Name = "comPortTextBox";
            this.comPortTextBox.Size = new System.Drawing.Size(471, 35);
            this.comPortTextBox.TabIndex = 37;
            this.comPortTextBox.Text = "Please replace text with Arduino COM port:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3456, 2113);
            this.Controls.Add(this.comPortTextBox);
            this.Controls.Add(this.arduinoDataLabel);
            this.Controls.Add(this.serialCommState);
            this.Controls.Add(this.openCommsButton);
            this.Controls.Add(this.borderAreaSlider);
            this.Controls.Add(this.borderAreaLabel);
            this.Controls.Add(this.squareAreaSlider);
            this.Controls.Add(this.squareAreaLabel);
            this.Controls.Add(this.bgrFrame);
            this.Controls.Add(this.blurFrame);
            this.Controls.Add(this.decoFrame);
            this.Controls.Add(this.rawFrame);
            this.Controls.Add(this.triAreaLabel);
            this.Controls.Add(this.triAreaSlider);
            this.Controls.Add(this.gaussianDown);
            this.Controls.Add(this.gaussianUp);
            this.Controls.Add(this.bgrSlider);
            this.Controls.Add(this.blurLabel);
            this.Controls.Add(this.bgrLabel);
            this.Controls.Add(this.contoursLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.lockStateLabel);
            this.Controls.Add(this.shapeDataLabel);
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bgrSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triAreaSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rawFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decoFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blurFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgrFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.squareAreaSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderAreaSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label shapeDataLabel;
        private System.Windows.Forms.Label lockStateLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label contoursLabel;
        private System.Windows.Forms.Label bgrLabel;
        private System.Windows.Forms.Label blurLabel;
        private System.Windows.Forms.Button gaussianUp;
        private System.Windows.Forms.Button gaussianDown;
        private System.Windows.Forms.TrackBar bgrSlider;
        private System.Windows.Forms.TrackBar triAreaSlider;
        private System.Windows.Forms.Label triAreaLabel;
        private System.Windows.Forms.PictureBox rawFrame;
        private System.Windows.Forms.PictureBox decoFrame;
        private System.Windows.Forms.PictureBox blurFrame;
        private System.Windows.Forms.PictureBox bgrFrame;
        private System.Windows.Forms.Label squareAreaLabel;
        private System.Windows.Forms.TrackBar squareAreaSlider;
        private System.Windows.Forms.Label borderAreaLabel;
        private System.Windows.Forms.TrackBar borderAreaSlider;
        private System.Windows.Forms.Button openCommsButton;
        private System.Windows.Forms.Label serialCommState;
        private System.Windows.Forms.TextBox arduinoDataLabel;
        private System.Windows.Forms.TextBox comPortTextBox;
    }
}

