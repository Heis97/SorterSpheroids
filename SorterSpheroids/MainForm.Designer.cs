namespace SorterSpheroids
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.but_manual = new System.Windows.Forms.Button();
            this.but_auto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // but_manual
            // 
            resources.ApplyResources(this.but_manual, "but_manual");
            this.but_manual.Name = "but_manual";
            this.but_manual.UseVisualStyleBackColor = true;
            this.but_manual.Click += new System.EventHandler(this.but_manual_Click);
            // 
            // but_auto
            // 
            resources.ApplyResources(this.but_auto, "but_auto");
            this.but_auto.Name = "but_auto";
            this.but_auto.UseVisualStyleBackColor = true;
            this.but_auto.Click += new System.EventHandler(this.but_auto_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.but_auto);
            this.Controls.Add(this.but_manual);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button but_manual;
        private System.Windows.Forms.Button but_auto;
    }
}

