namespace FEFTHelper
{
    partial class frmKeypad
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtmobileno = new System.Windows.Forms.TextBox();
            this.btnok = new System.Windows.Forms.Button();
            this.TransList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mobile No";
            // 
            // txtmobileno
            // 
            this.txtmobileno.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmobileno.Location = new System.Drawing.Point(251, 30);
            this.txtmobileno.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtmobileno.Name = "txtmobileno";
            this.txtmobileno.Size = new System.Drawing.Size(459, 46);
            this.txtmobileno.TabIndex = 1;
            // 
            // btnok
            // 
            this.btnok.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnok.Location = new System.Drawing.Point(606, 85);
            this.btnok.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(104, 42);
            this.btnok.TabIndex = 2;
            this.btnok.Text = "OK";
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // TransList
            // 
            this.TransList.FormattingEnabled = true;
            this.TransList.ItemHeight = 16;
            this.TransList.Location = new System.Drawing.Point(66, 186);
            this.TransList.Name = "TransList";
            this.TransList.Size = new System.Drawing.Size(709, 228);
            this.TransList.TabIndex = 3;
            // 
            // frmKeypad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 439);
            this.Controls.Add(this.TransList);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.txtmobileno);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmKeypad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Key In the Mobile No";
            this.Load += new System.EventHandler(this.frmKeypad_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtmobileno;
        private System.Windows.Forms.Button btnok;
        private System.Windows.Forms.ListBox TransList;
    }
}