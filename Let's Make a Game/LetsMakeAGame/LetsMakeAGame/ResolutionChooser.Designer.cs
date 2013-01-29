namespace LetsMakeAGame
{
    partial class ResolutionChooser
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
            this.resSelect = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // resSelect
            // 
            this.resSelect.FormattingEnabled = true;
            this.resSelect.Items.AddRange(new object[] {
            "640 x 480",
            "720 x 480",
            "720 x 576",
            "800 x 600",
            "1024 x 786",
            "1152 x 864",
            "1280 x 720",
            "1280 x 768",
            "1280 x 800",
            "1360 x 768",
            "1366 x 768",
            "1440 x 900",
            "1600 x 900"});
            this.resSelect.Location = new System.Drawing.Point(84, 121);
            this.resSelect.Name = "resSelect";
            this.resSelect.Size = new System.Drawing.Size(144, 21);
            this.resSelect.TabIndex = 0;
            // 
            // ResolutionChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 411);
            this.Controls.Add(this.resSelect);
            this.Name = "ResolutionChooser";
            this.Text = "ResolutionChooser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox resSelect;
    }
}