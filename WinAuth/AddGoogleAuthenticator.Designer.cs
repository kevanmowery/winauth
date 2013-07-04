namespace WinAuth
{
	partial class AddGoogleAuthenticator
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
			this.newAuthenticatorGroup = new System.Windows.Forms.GroupBox();
			this.summaryInfoLabel = new System.Windows.Forms.Label();
			this.showQrCodeButton = new System.Windows.Forms.Button();
			this.secretCodeField = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.icon3RadioButton = new WinAuth.GroupRadioButton();
			this.icon2RadioButton = new WinAuth.GroupRadioButton();
			this.icon1RadioButton = new WinAuth.GroupRadioButton();
			this.label10 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.nameField = new System.Windows.Forms.TextBox();
			this.icon3 = new System.Windows.Forms.PictureBox();
			this.icon2 = new System.Windows.Forms.PictureBox();
			this.icon1 = new System.Windows.Forms.PictureBox();
			this.qrImage = new System.Windows.Forms.PictureBox();
			this.newAuthenticatorGroup.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.icon3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).BeginInit();
			this.SuspendLayout();
			// 
			// newAuthenticatorGroup
			// 
			this.newAuthenticatorGroup.Controls.Add(this.summaryInfoLabel);
			this.newAuthenticatorGroup.Controls.Add(this.showQrCodeButton);
			this.newAuthenticatorGroup.Controls.Add(this.qrImage);
			this.newAuthenticatorGroup.Controls.Add(this.secretCodeField);
			this.newAuthenticatorGroup.Controls.Add(this.label4);
			this.newAuthenticatorGroup.Controls.Add(this.label2);
			this.newAuthenticatorGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newAuthenticatorGroup.Location = new System.Drawing.Point(12, 123);
			this.newAuthenticatorGroup.Name = "newAuthenticatorGroup";
			this.newAuthenticatorGroup.Size = new System.Drawing.Size(458, 229);
			this.newAuthenticatorGroup.TabIndex = 1;
			this.newAuthenticatorGroup.TabStop = false;
			// 
			// summaryInfoLabel
			// 
			this.summaryInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.summaryInfoLabel.Location = new System.Drawing.Point(240, 122);
			this.summaryInfoLabel.Name = "summaryInfoLabel";
			this.summaryInfoLabel.Size = new System.Drawing.Size(165, 94);
			this.summaryInfoLabel.TabIndex = 8;
			// 
			// showQrCodeButton
			// 
			this.showQrCodeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.showQrCodeButton.Location = new System.Drawing.Point(240, 96);
			this.showQrCodeButton.Name = "showQrCodeButton";
			this.showQrCodeButton.Size = new System.Drawing.Size(107, 23);
			this.showQrCodeButton.TabIndex = 1;
			this.showQrCodeButton.Text = "Show QR Code";
			this.showQrCodeButton.UseVisualStyleBackColor = true;
			this.showQrCodeButton.Click += new System.EventHandler(this.showQrCodeButton_Click);
			// 
			// secretCodeField
			// 
			this.secretCodeField.AllowDrop = true;
			this.secretCodeField.CausesValidation = false;
			this.secretCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.secretCodeField.Location = new System.Drawing.Point(115, 68);
			this.secretCodeField.Multiline = true;
			this.secretCodeField.Name = "secretCodeField";
			this.secretCodeField.Size = new System.Drawing.Size(325, 22);
			this.secretCodeField.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(23, 71);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(86, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Secret Code:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(14, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(438, 40);
			this.label2.TabIndex = 1;
			this.label2.Text = "Enter the code for your authenticator. Or if have a QR code, you can copy and pas" +
    "te the URL or image into the Secret Code field below.\r\n";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.okButton.Location = new System.Drawing.Point(315, 361);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(396, 361);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.icon3RadioButton);
			this.groupBox1.Controls.Add(this.icon2RadioButton);
			this.groupBox1.Controls.Add(this.icon1RadioButton);
			this.groupBox1.Controls.Add(this.icon3);
			this.groupBox1.Controls.Add(this.icon2);
			this.groupBox1.Controls.Add(this.icon1);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.nameField);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(456, 105);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// icon3RadioButton
			// 
			this.icon3RadioButton.AutoSize = true;
			this.icon3RadioButton.Group = "ICON";
			this.icon3RadioButton.Location = new System.Drawing.Point(220, 59);
			this.icon3RadioButton.Name = "icon3RadioButton";
			this.icon3RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon3RadioButton.TabIndex = 3;
			this.icon3RadioButton.Tag = "ChromeIcon.png";
			this.icon3RadioButton.UseVisualStyleBackColor = true;
			this.icon3RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon2RadioButton
			// 
			this.icon2RadioButton.AutoSize = true;
			this.icon2RadioButton.Group = "ICON";
			this.icon2RadioButton.Location = new System.Drawing.Point(144, 59);
			this.icon2RadioButton.Name = "icon2RadioButton";
			this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon2RadioButton.TabIndex = 2;
			this.icon2RadioButton.Tag = "GoogleIcon.png";
			this.icon2RadioButton.UseVisualStyleBackColor = true;
			this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon1RadioButton
			// 
			this.icon1RadioButton.AutoSize = true;
			this.icon1RadioButton.Checked = true;
			this.icon1RadioButton.Group = "ICON";
			this.icon1RadioButton.Location = new System.Drawing.Point(68, 59);
			this.icon1RadioButton.Name = "icon1RadioButton";
			this.icon1RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon1RadioButton.TabIndex = 1;
			this.icon1RadioButton.TabStop = true;
			this.icon1RadioButton.Tag = "GoogleAuthenticatorIcon.png";
			this.icon1RadioButton.UseVisualStyleBackColor = true;
			this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(11, 57);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(36, 16);
			this.label10.TabIndex = 3;
			this.label10.Text = "Icon:";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(11, 19);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(48, 16);
			this.label12.TabIndex = 3;
			this.label12.Text = "Name:";
			// 
			// nameField
			// 
			this.nameField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameField.Location = new System.Drawing.Point(68, 18);
			this.nameField.Name = "nameField";
			this.nameField.Size = new System.Drawing.Size(371, 22);
			this.nameField.TabIndex = 0;
			// 
			// icon3
			// 
			this.icon3.Image = global::WinAuth.Properties.Resources.ChromeIcon;
			this.icon3.Location = new System.Drawing.Point(240, 44);
			this.icon3.Name = "icon3";
			this.icon3.Size = new System.Drawing.Size(48, 48);
			this.icon3.TabIndex = 4;
			this.icon3.TabStop = false;
			this.icon3.Tag = "";
			this.icon3.Click += new System.EventHandler(this.icon3_Click);
			// 
			// icon2
			// 
			this.icon2.Image = global::WinAuth.Properties.Resources.GoogleIcon;
			this.icon2.Location = new System.Drawing.Point(164, 44);
			this.icon2.Name = "icon2";
			this.icon2.Size = new System.Drawing.Size(48, 48);
			this.icon2.TabIndex = 4;
			this.icon2.TabStop = false;
			this.icon2.Tag = "";
			this.icon2.Click += new System.EventHandler(this.icon2_Click);
			// 
			// icon1
			// 
			this.icon1.Image = global::WinAuth.Properties.Resources.GoogleAuthenticatorIcon;
			this.icon1.Location = new System.Drawing.Point(88, 44);
			this.icon1.Name = "icon1";
			this.icon1.Size = new System.Drawing.Size(48, 48);
			this.icon1.TabIndex = 4;
			this.icon1.TabStop = false;
			this.icon1.Tag = "";
			this.icon1.Click += new System.EventHandler(this.icon1_Click);
			// 
			// qrImage
			// 
			this.qrImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.qrImage.Location = new System.Drawing.Point(114, 96);
			this.qrImage.Name = "qrImage";
			this.qrImage.Size = new System.Drawing.Size(120, 120);
			this.qrImage.TabIndex = 6;
			this.qrImage.TabStop = false;
			// 
			// AddGoogleAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(483, 396);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.newAuthenticatorGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AddGoogleAuthenticator";
			this.ShowIcon = false;
			this.Text = "Add Google Authenticator";
			this.Load += new System.EventHandler(this.AddGoogleAuthenticator_Load);
			this.newAuthenticatorGroup.ResumeLayout(false);
			this.newAuthenticatorGroup.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.icon3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox newAuthenticatorGroup;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.PictureBox qrImage;
		private System.Windows.Forms.TextBox secretCodeField;
		private System.Windows.Forms.Button showQrCodeButton;
		private System.Windows.Forms.Label summaryInfoLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private GroupRadioButton icon3RadioButton;
		private GroupRadioButton icon2RadioButton;
		private GroupRadioButton icon1RadioButton;
		private System.Windows.Forms.PictureBox icon3;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox nameField;
	}
}