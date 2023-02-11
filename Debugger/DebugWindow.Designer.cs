namespace Debug.Window;

partial class DebugWindow
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
            this.ContinueBtn = new System.Windows.Forms.Button();
            this.FrameLb = new System.Windows.Forms.ListBox();
            this.StackLb = new System.Windows.Forms.ListBox();
            this.FramLbl = new System.Windows.Forms.Label();
            this.StackLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ContinueBtn
            // 
            this.ContinueBtn.Location = new System.Drawing.Point(11, 474);
            this.ContinueBtn.Name = "ContinueBtn";
            this.ContinueBtn.Size = new System.Drawing.Size(658, 42);
            this.ContinueBtn.TabIndex = 2;
            this.ContinueBtn.Text = "Continue";
            this.ContinueBtn.UseVisualStyleBackColor = true;
            // 
            // FrameLb
            // 
            this.FrameLb.FormattingEnabled = true;
            this.FrameLb.ItemHeight = 20;
            this.FrameLb.Location = new System.Drawing.Point(12, 46);
            this.FrameLb.Name = "FrameLb";
            this.FrameLb.Size = new System.Drawing.Size(320, 424);
            this.FrameLb.TabIndex = 3;
            // 
            // StackLb
            // 
            this.StackLb.FormattingEnabled = true;
            this.StackLb.ItemHeight = 20;
            this.StackLb.Location = new System.Drawing.Point(347, 46);
            this.StackLb.Name = "StackLb";
            this.StackLb.Size = new System.Drawing.Size(320, 424);
            this.StackLb.TabIndex = 4;
            // 
            // FramLbl
            // 
            this.FramLbl.AutoSize = true;
            this.FramLbl.Location = new System.Drawing.Point(112, 9);
            this.FramLbl.Name = "FramLbl";
            this.FramLbl.Size = new System.Drawing.Size(89, 20);
            this.FramLbl.TabIndex = 5;
            this.FramLbl.Text = "Code Frame";
            // 
            // StackLbl
            // 
            this.StackLbl.AutoSize = true;
            this.StackLbl.Location = new System.Drawing.Point(476, 9);
            this.StackLbl.Name = "StackLbl";
            this.StackLbl.Size = new System.Drawing.Size(44, 20);
            this.StackLbl.TabIndex = 6;
            this.StackLbl.Text = "Stack";
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 521);
            this.Controls.Add(this.StackLbl);
            this.Controls.Add(this.FramLbl);
            this.Controls.Add(this.StackLb);
            this.Controls.Add(this.FrameLb);
            this.Controls.Add(this.ContinueBtn);
            this.MaximizeBox = false;
            this.Name = "DebugWindow";
            this.Text = "Debug Window";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private Button ContinueBtn;
    private ListBox FrameLb;
    private ListBox StackLb;
    private Label FramLbl;
    private Label StackLbl;
}