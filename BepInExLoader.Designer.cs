namespace BepInExLoader
{
    partial class BepInExLoader
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
            components = new System.ComponentModel.Container();
            browser = new FolderBrowserDialog();
            browseButton = new Button();
            installButton = new Button();
            pathTextBox = new TextBox();
            errorProvider2 = new ErrorProvider(components);
            versionSelect = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)errorProvider2).BeginInit();
            SuspendLayout();
            // 
            // browseButton
            // 
            browseButton.Location = new Point(685, 12);
            browseButton.Name = "browseButton";
            browseButton.Size = new Size(103, 23);
            browseButton.TabIndex = 1;
            browseButton.Text = "browseButton";
            browseButton.UseVisualStyleBackColor = true;
            browseButton.Click += browseButton_Click;
            // 
            // installButton
            // 
            installButton.Location = new Point(222, 127);
            installButton.Name = "installButton";
            installButton.Size = new Size(389, 186);
            installButton.TabIndex = 2;
            installButton.Text = "installButton";
            installButton.UseVisualStyleBackColor = true;
            installButton.Click += installButton_Click;
            // 
            // pathTextBox
            // 
            pathTextBox.Location = new Point(579, 13);
            pathTextBox.Name = "pathTextBox";
            pathTextBox.Size = new Size(100, 23);
            pathTextBox.TabIndex = 4;
            // 
            // errorProvider2
            // 
            errorProvider2.ContainerControl = this;
            // 
            // versionSelect
            // 
            versionSelect.FormattingEnabled = true;
            versionSelect.Location = new Point(355, 62);
            versionSelect.Name = "versionSelect";
            versionSelect.Size = new Size(121, 23);
            versionSelect.TabIndex = 5;
            // 
            // BepInExLoader
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(versionSelect);
            Controls.Add(pathTextBox);
            Controls.Add(installButton);
            Controls.Add(browseButton);
            Name = "BepInExLoader";
            Text = "BepInExLoader";
            ((System.ComponentModel.ISupportInitialize)errorProvider2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FolderBrowserDialog browser;
        private Button browseButton;
        private ErrorProvider errorProvider1;
        private Button installButton;
        private Label label1;
        private TextBox pathTextBox;
        private ErrorProvider errorProvider2;
        private ComboBox versionSelect;
    }
}
