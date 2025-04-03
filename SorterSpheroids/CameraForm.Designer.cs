namespace SorterSpheroids
{
    partial class CameraForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraForm));
            this.imageBox_main = new Emgu.CV.UI.ImageBox();
            this.but_start_recording = new System.Windows.Forms.Button();
            this.checkBox_centr_object = new System.Windows.Forms.CheckBox();
            this.checkBox_boarder_object = new System.Windows.Forms.CheckBox();
            this.checkBox_focal_area = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.but_start_video = new System.Windows.Forms.Button();
            this.but_stop_recording = new System.Windows.Forms.Button();
            this.textBox_video_name = new System.Windows.Forms.TextBox();
            this.textBox_camera_number = new System.Windows.Forms.TextBox();
            this.but_con_cam = new System.Windows.Forms.Button();
            this.textBox_set_exposit = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_main)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox_main
            // 
            this.imageBox_main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.imageBox_main, "imageBox_main");
            this.imageBox_main.Name = "imageBox_main";
            this.imageBox_main.TabStop = false;
            // 
            // but_start_recording
            // 
            resources.ApplyResources(this.but_start_recording, "but_start_recording");
            this.but_start_recording.Name = "but_start_recording";
            this.but_start_recording.UseVisualStyleBackColor = true;
            this.but_start_recording.Click += new System.EventHandler(this.but_start_recording_Click);
            // 
            // checkBox_centr_object
            // 
            resources.ApplyResources(this.checkBox_centr_object, "checkBox_centr_object");
            this.checkBox_centr_object.Name = "checkBox_centr_object";
            this.checkBox_centr_object.UseVisualStyleBackColor = true;
            // 
            // checkBox_boarder_object
            // 
            resources.ApplyResources(this.checkBox_boarder_object, "checkBox_boarder_object");
            this.checkBox_boarder_object.Name = "checkBox_boarder_object";
            this.checkBox_boarder_object.UseVisualStyleBackColor = true;
            // 
            // checkBox_focal_area
            // 
            resources.ApplyResources(this.checkBox_focal_area, "checkBox_focal_area");
            this.checkBox_focal_area.Name = "checkBox_focal_area";
            this.checkBox_focal_area.UseVisualStyleBackColor = true;
            this.checkBox_focal_area.CheckedChanged += new System.EventHandler(this.checkBox_focal_area_CheckedChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // but_start_video
            // 
            resources.ApplyResources(this.but_start_video, "but_start_video");
            this.but_start_video.Name = "but_start_video";
            this.but_start_video.UseVisualStyleBackColor = true;
            this.but_start_video.Click += new System.EventHandler(this.but_start_video_Click);
            // 
            // but_stop_recording
            // 
            resources.ApplyResources(this.but_stop_recording, "but_stop_recording");
            this.but_stop_recording.Name = "but_stop_recording";
            this.but_stop_recording.UseVisualStyleBackColor = true;
            this.but_stop_recording.Click += new System.EventHandler(this.but_stop_recording_Click);
            // 
            // textBox_video_name
            // 
            resources.ApplyResources(this.textBox_video_name, "textBox_video_name");
            this.textBox_video_name.Name = "textBox_video_name";
            // 
            // textBox_camera_number
            // 
            resources.ApplyResources(this.textBox_camera_number, "textBox_camera_number");
            this.textBox_camera_number.Name = "textBox_camera_number";
            // 
            // but_con_cam
            // 
            resources.ApplyResources(this.but_con_cam, "but_con_cam");
            this.but_con_cam.Name = "but_con_cam";
            this.but_con_cam.UseVisualStyleBackColor = true;
            this.but_con_cam.Click += new System.EventHandler(this.but_con_cam_Click);
            // 
            // textBox_set_exposit
            // 
            resources.ApplyResources(this.textBox_set_exposit, "textBox_set_exposit");
            this.textBox_set_exposit.Name = "textBox_set_exposit";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            resources.ApplyResources(this.button14, "button14");
            this.button14.Name = "button14";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // CameraForm
            // 
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.textBox_set_exposit);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.but_start_recording);
            this.Controls.Add(this.checkBox_centr_object);
            this.Controls.Add(this.checkBox_boarder_object);
            this.Controls.Add(this.checkBox_focal_area);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.but_start_video);
            this.Controls.Add(this.but_stop_recording);
            this.Controls.Add(this.textBox_video_name);
            this.Controls.Add(this.textBox_camera_number);
            this.Controls.Add(this.but_con_cam);
            this.Controls.Add(this.imageBox_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox_main;
        private System.Windows.Forms.Button but_start_recording;
        private System.Windows.Forms.CheckBox checkBox_centr_object;
        private System.Windows.Forms.CheckBox checkBox_boarder_object;
        private System.Windows.Forms.CheckBox checkBox_focal_area;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button but_start_video;
        private System.Windows.Forms.Button but_stop_recording;
        private System.Windows.Forms.TextBox textBox_video_name;
        private System.Windows.Forms.TextBox textBox_camera_number;
        private System.Windows.Forms.Button but_con_cam;
        private System.Windows.Forms.TextBox textBox_set_exposit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TextBox textBox1;
    }
}

