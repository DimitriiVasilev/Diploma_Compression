namespace CompressV2
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
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
            this.components = new System.ComponentModel.Container();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.btCompress = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btDraw = new System.Windows.Forms.Button();
            this.btDrawDifference = new System.Windows.Forms.Button();
            this.btScale = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraph
            // 
            this.zedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraph.Location = new System.Drawing.Point(82, 0);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(719, 330);
            this.zedGraph.TabIndex = 0;
            // 
            // btCompress
            // 
            this.btCompress.Location = new System.Drawing.Point(3, 3);
            this.btCompress.Name = "btCompress";
            this.btCompress.Size = new System.Drawing.Size(75, 23);
            this.btCompress.TabIndex = 1;
            this.btCompress.Text = "Compress";
            this.btCompress.UseVisualStyleBackColor = true;
            this.btCompress.Click += new System.EventHandler(this.btCompress_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btCompress);
            this.flowLayoutPanel1.Controls.Add(this.btDraw);
            this.flowLayoutPanel1.Controls.Add(this.btDrawDifference);
            this.flowLayoutPanel1.Controls.Add(this.btScale);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(82, 330);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btDraw
            // 
            this.btDraw.Location = new System.Drawing.Point(3, 32);
            this.btDraw.Name = "btDraw";
            this.btDraw.Size = new System.Drawing.Size(75, 23);
            this.btDraw.TabIndex = 2;
            this.btDraw.Text = "Draw Axes";
            this.btDraw.UseVisualStyleBackColor = true;
            this.btDraw.Click += new System.EventHandler(this.btDraw_Click);
            // 
            // btDrawDifference
            // 
            this.btDrawDifference.Location = new System.Drawing.Point(3, 61);
            this.btDrawDifference.Name = "btDrawDifference";
            this.btDrawDifference.Size = new System.Drawing.Size(75, 34);
            this.btDrawDifference.TabIndex = 4;
            this.btDrawDifference.Text = "Draw Difference";
            this.btDrawDifference.UseVisualStyleBackColor = true;
            this.btDrawDifference.Click += new System.EventHandler(this.btDrawDifference_Click);
            // 
            // btScale
            // 
            this.btScale.Location = new System.Drawing.Point(3, 101);
            this.btScale.Name = "btScale";
            this.btScale.Size = new System.Drawing.Size(75, 23);
            this.btScale.TabIndex = 3;
            this.btScale.Text = "Reset Scale";
            this.btScale.UseVisualStyleBackColor = true;
            this.btScale.Click += new System.EventHandler(this.btScale_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 330);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Graduate Work";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.Button btCompress;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btDraw;
        private System.Windows.Forms.Button btScale;
        private System.Windows.Forms.Button btDrawDifference;
    }
}

