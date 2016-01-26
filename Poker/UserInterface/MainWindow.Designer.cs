namespace Poker
{
    using System.Windows.Forms;

    public partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Button buttonFold;
        private Button buttonCheck;
        private Button buttonCall;
        private Button buttonRaise;
        private Button buttonAdd;
        private Button buttonOptions;
        private Button buttonBigBlind;
        private Button buttonSmallBlind;

        private ProgressBar progressBarTimer;

        private TextBox textBoxBotOneChips;
        private TextBox textBoxBotTwoChips;
        private TextBox textBoxBotThreeChips;
        private TextBox textBoxBotFourChips;
        private TextBox textBoxBotFiveChips;
        private TextBox textBoxPlayerChips;
        private TextBox textBoxPot;

        private TextBox textBoxSmallBlind;
        private TextBox textBoxBigBlind;
        private TextBox textBoxAdd;
        private TextBox textBoxRaise;

        private Label botFiveStatus;
        private Label botFourStatus;
        private Label botThreeStatus;
        private Label botOneStatus;
        private Label playerStatus;
        private Label botTwoStatus;
        private Label potLabel;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.buttonFold = new Button();
            this.buttonCheck = new Button();
            this.buttonCall = new Button();
            this.buttonRaise = new Button();
            this.progressBarTimer = new ProgressBar();
            this.textBoxPlayerChips = new TextBox();
            this.buttonAdd = new Button();
            this.textBoxAdd = new TextBox();
            this.textBoxBotFiveChips = new TextBox();
            this.textBoxBotFourChips = new TextBox();
            this.textBoxBotThreeChips = new TextBox();
            this.textBoxBotTwoChips = new TextBox();
            this.textBoxBotOneChips = new TextBox();
            this.textBoxPot = new TextBox();
            this.buttonOptions = new Button();
            this.buttonBigBlind = new Button();
            this.textBoxSmallBlind = new TextBox();
            this.buttonSmallBlind = new Button();
            this.textBoxBigBlind = new TextBox();
            this.botFiveStatus = new Label();
            this.botFourStatus = new Label();
            this.botThreeStatus = new Label();
            this.botOneStatus = new Label();
            this.playerStatus = new Label();
            this.botTwoStatus = new Label();
            this.potLabel = new Label();
            this.textBoxRaise = new TextBox();
            this.SuspendLayout();
            // 
            // buttonFold
            // 
            this.buttonFold.Anchor = AnchorStyles.Bottom;
            this.buttonFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFold.Location = new System.Drawing.Point(335, 660);
            this.buttonFold.Name = "buttonFold";
            this.buttonFold.Size = new System.Drawing.Size(130, 62);
            this.buttonFold.TabIndex = 0;
            this.buttonFold.Text = "Fold";
            this.buttonFold.UseVisualStyleBackColor = true;
            this.buttonFold.Click += new System.EventHandler(this.bFold_Click);
            // 
            // buttonCheck
            // 
            this.buttonCheck.Anchor = AnchorStyles.Bottom;
            this.buttonCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCheck.Location = new System.Drawing.Point(494, 660);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(134, 62);
            this.buttonCheck.TabIndex = 2;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // buttonCall
            // 
            this.buttonCall.Anchor = AnchorStyles.Bottom;
            this.buttonCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCall.Location = new System.Drawing.Point(667, 661);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(126, 62);
            this.buttonCall.TabIndex = 3;
            this.buttonCall.Text = "Call";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.bCall_Click);
            // 
            // buttonRaise
            // 
            this.buttonRaise.Anchor = AnchorStyles.Bottom;
            this.buttonRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRaise.Location = new System.Drawing.Point(835, 661);
            this.buttonRaise.Name = "buttonRaise";
            this.buttonRaise.Size = new System.Drawing.Size(124, 62);
            this.buttonRaise.TabIndex = 4;
            this.buttonRaise.Text = "Raise";
            this.buttonRaise.UseVisualStyleBackColor = true;
            this.buttonRaise.Click += new System.EventHandler(this.bRaise_Click);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Anchor = AnchorStyles.Bottom;
            this.progressBarTimer.BackColor = System.Drawing.SystemColors.Control;
            this.progressBarTimer.Location = new System.Drawing.Point(335, 631);
            this.progressBarTimer.Maximum = 1000;
            this.progressBarTimer.Name = "progressBarTimer";
            this.progressBarTimer.Size = new System.Drawing.Size(667, 23);
            this.progressBarTimer.TabIndex = 5;
            this.progressBarTimer.Value = 1000;
            // 
            // textBoxPlayerChips
            // 
            this.textBoxPlayerChips.Anchor = AnchorStyles.Bottom;
            this.textBoxPlayerChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPlayerChips.Location = new System.Drawing.Point(755, 553);
            this.textBoxPlayerChips.Name = "textBoxPlayerChips";
            this.textBoxPlayerChips.Size = new System.Drawing.Size(163, 23);
            this.textBoxPlayerChips.TabIndex = 6;
            this.textBoxPlayerChips.Text = "Chips : 0";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((AnchorStyles)((AnchorStyles.Bottom |AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(12, 697);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 25);
            this.buttonAdd.TabIndex = 7;
            this.buttonAdd.Text = "AddChips";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // textBoxAdd
            // 
            this.textBoxAdd.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.textBoxAdd.Location = new System.Drawing.Point(93, 700);
            this.textBoxAdd.Name = "textBoxAdd";
            this.textBoxAdd.Size = new System.Drawing.Size(125, 20);
            this.textBoxAdd.TabIndex = 8;
            // 
            // textBoxBotFiveChips
            // 
            this.textBoxBotFiveChips.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.textBoxBotFiveChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBotFiveChips.Location = new System.Drawing.Point(1012, 553);
            this.textBoxBotFiveChips.Name = "textBoxBotFiveChips";
            this.textBoxBotFiveChips.Size = new System.Drawing.Size(152, 23);
            this.textBoxBotFiveChips.TabIndex = 9;
            this.textBoxBotFiveChips.Text = "Chips : 0";
            // 
            // textBoxBotFourChips
            // 
            this.textBoxBotFourChips.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.textBoxBotFourChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBotFourChips.Location = new System.Drawing.Point(970, 81);
            this.textBoxBotFourChips.Name = "textBoxBotFourChips";
            this.textBoxBotFourChips.Size = new System.Drawing.Size(123, 23);
            this.textBoxBotFourChips.TabIndex = 10;
            this.textBoxBotFourChips.Text = "Chips : 0";
            // 
            // textBoxBotThreeChips
            // 
            this.textBoxBotThreeChips.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.textBoxBotThreeChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBotThreeChips.Location = new System.Drawing.Point(755, 81);
            this.textBoxBotThreeChips.Name = "textBoxBotThreeChips";
            this.textBoxBotThreeChips.Size = new System.Drawing.Size(125, 23);
            this.textBoxBotThreeChips.TabIndex = 11;
            this.textBoxBotThreeChips.Text = "Chips : 0";
            // 
            // textBoxBotTwoChips
            // 
            this.textBoxBotTwoChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBotTwoChips.Location = new System.Drawing.Point(276, 81);
            this.textBoxBotTwoChips.Name = "textBoxBotTwoChips";
            this.textBoxBotTwoChips.Size = new System.Drawing.Size(133, 23);
            this.textBoxBotTwoChips.TabIndex = 12;
            this.textBoxBotTwoChips.Text = "Chips : 0";
            // 
            // textBoxBotOneChips
            // 
            this.textBoxBotOneChips.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.textBoxBotOneChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBotOneChips.Location = new System.Drawing.Point(181, 553);
            this.textBoxBotOneChips.Name = "textBoxBotOneChips";
            this.textBoxBotOneChips.Size = new System.Drawing.Size(142, 23);
            this.textBoxBotOneChips.TabIndex = 13;
            this.textBoxBotOneChips.Text = "Chips : 0";
            // 
            // textBoxPot
            // 
            this.textBoxPot.Anchor = AnchorStyles.None;
            this.textBoxPot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPot.Location = new System.Drawing.Point(606, 212);
            this.textBoxPot.Name = "textBoxPot";
            this.textBoxPot.Size = new System.Drawing.Size(125, 23);
            this.textBoxPot.TabIndex = 14;
            this.textBoxPot.Text = "0";
            // 
            // buttonOptions
            // 
            this.buttonOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOptions.Location = new System.Drawing.Point(12, 12);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(75, 36);
            this.buttonOptions.TabIndex = 15;
            this.buttonOptions.Text = "BB/SB";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // buttonBigBlind
            // 
            this.buttonBigBlind.Location = new System.Drawing.Point(12, 254);
            this.buttonBigBlind.Name = "buttonBigBlind";
            this.buttonBigBlind.Size = new System.Drawing.Size(75, 23);
            this.buttonBigBlind.TabIndex = 16;
            this.buttonBigBlind.Text = "Big Blind";
            this.buttonBigBlind.UseVisualStyleBackColor = true;
            this.buttonBigBlind.Click += new System.EventHandler(this.bBB_Click);
            // 
            // textBoxSmallBlind
            // 
            this.textBoxSmallBlind.Location = new System.Drawing.Point(12, 228);
            this.textBoxSmallBlind.Name = "textBoxSmallBlind";
            this.textBoxSmallBlind.Size = new System.Drawing.Size(75, 20);
            this.textBoxSmallBlind.TabIndex = 17;
            this.textBoxSmallBlind.Text = "250";
            // 
            // buttonSmallBlind
            // 
            this.buttonSmallBlind.Location = new System.Drawing.Point(12, 199);
            this.buttonSmallBlind.Name = "buttonSmallBlind";
            this.buttonSmallBlind.Size = new System.Drawing.Size(75, 23);
            this.buttonSmallBlind.TabIndex = 18;
            this.buttonSmallBlind.Text = "Small Blind";
            this.buttonSmallBlind.UseVisualStyleBackColor = true;
            this.buttonSmallBlind.Click += new System.EventHandler(this.bSB_Click);
            // 
            // textBoxBigBlind
            // 
            this.textBoxBigBlind.Location = new System.Drawing.Point(12, 283);
            this.textBoxBigBlind.Name = "textBoxBigBlind";
            this.textBoxBigBlind.Size = new System.Drawing.Size(75, 20);
            this.textBoxBigBlind.TabIndex = 19;
            this.textBoxBigBlind.Text = "500";
            // 
            // botFiveStatus
            // 
            this.botFiveStatus.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.botFiveStatus.Location = new System.Drawing.Point(1012, 579);
            this.botFiveStatus.Name = "botFiveStatus";
            this.botFiveStatus.Size = new System.Drawing.Size(152, 32);
            this.botFiveStatus.TabIndex = 26;
            // 
            // botFourStatus
            // 
            this.botFourStatus.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.botFourStatus.Location = new System.Drawing.Point(970, 107);
            this.botFourStatus.Name = "botFourStatus";
            this.botFourStatus.Size = new System.Drawing.Size(123, 32);
            this.botFourStatus.TabIndex = 27;
            // 
            // botThreeStatus
            // 
            this.botThreeStatus.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.botThreeStatus.Location = new System.Drawing.Point(755, 107);
            this.botThreeStatus.Name = "botThreeStatus";
            this.botThreeStatus.Size = new System.Drawing.Size(125, 32);
            this.botThreeStatus.TabIndex = 28;
            // 
            // botOneStatus
            // 
            this.botOneStatus.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.botOneStatus.Location = new System.Drawing.Point(181, 579);
            this.botOneStatus.Name = "botOneStatus";
            this.botOneStatus.Size = new System.Drawing.Size(142, 32);
            this.botOneStatus.TabIndex = 29;
            // 
            // playerStatus
            // 
            this.playerStatus.Anchor = AnchorStyles.Bottom;
            this.playerStatus.Location = new System.Drawing.Point(755, 579);
            this.playerStatus.Name = "playerStatus";
            this.playerStatus.Size = new System.Drawing.Size(163, 32);
            this.playerStatus.TabIndex = 30;
            // 
            // botTwoStatus
            // 
            this.botTwoStatus.Location = new System.Drawing.Point(276, 107);
            this.botTwoStatus.Name = "botTwoStatus";
            this.botTwoStatus.Size = new System.Drawing.Size(133, 32);
            this.botTwoStatus.TabIndex = 31;
            // 
            // potLabel
            // 
            this.potLabel.Anchor = AnchorStyles.None;
            this.potLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.potLabel.Location = new System.Drawing.Point(654, 188);
            this.potLabel.Name = "potLabel";
            this.potLabel.Size = new System.Drawing.Size(31, 21);
            this.potLabel.TabIndex = 0;
            this.potLabel.Text = "Pot";
            // 
            // textBoxRaise
            // 
            this.textBoxRaise.Anchor = AnchorStyles.Bottom;
            this.textBoxRaise.Location = new System.Drawing.Point(965, 703);
            this.textBoxRaise.Name = "textBoxRaise";
            this.textBoxRaise.Size = new System.Drawing.Size(108, 20);
            this.textBoxRaise.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.poker_table___Copy;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.textBoxRaise);
            this.Controls.Add(this.potLabel);
            this.Controls.Add(this.botTwoStatus);
            this.Controls.Add(this.playerStatus);
            this.Controls.Add(this.botOneStatus);
            this.Controls.Add(this.botThreeStatus);
            this.Controls.Add(this.botFourStatus);
            this.Controls.Add(this.botFiveStatus);
            this.Controls.Add(this.textBoxBigBlind);
            this.Controls.Add(this.buttonSmallBlind);
            this.Controls.Add(this.textBoxSmallBlind);
            this.Controls.Add(this.buttonBigBlind);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.textBoxPot);
            this.Controls.Add(this.textBoxBotOneChips);
            this.Controls.Add(this.textBoxBotTwoChips);
            this.Controls.Add(this.textBoxBotThreeChips);
            this.Controls.Add(this.textBoxBotFourChips);
            this.Controls.Add(this.textBoxBotFiveChips);
            this.Controls.Add(this.textBoxAdd);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxPlayerChips);
            this.Controls.Add(this.progressBarTimer);
            this.Controls.Add(this.buttonRaise);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.buttonFold);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "GLS Texas Poker";
            this.Layout += new LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}