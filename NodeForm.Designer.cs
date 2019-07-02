namespace NodeInterface
{
    partial class NodeForm
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
            this.nodeRender1 = new NodeInterface.NodeRender();
            this.SuspendLayout();
            // 
            // nodeRender1
            // 
            this.nodeRender1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.nodeRender1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeRender1.Grid = true;
            this.nodeRender1.GridColor = System.Drawing.SystemColors.ControlDark;
            this.nodeRender1.GridStep = 15;
            this.nodeRender1.GridWidth = 2;
            this.nodeRender1.Location = new System.Drawing.Point(0, 0);
            this.nodeRender1.Name = "nodeRender1";
            this.nodeRender1.Size = new System.Drawing.Size(804, 452);
            this.nodeRender1.TabIndex = 0;
            this.nodeRender1.Text = "nodeRender1";
            this.nodeRender1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.nodeRender1_MouseClick);
            this.nodeRender1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.nodeRender1_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 452);
            this.Controls.Add(this.nodeRender1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private NodeInterface.NodeRender nodeRender1;
    }
}