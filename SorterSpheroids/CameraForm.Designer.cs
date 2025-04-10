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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraForm));
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
            this.but_set_exposit = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_set_nozzle_centr = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.comboBox_images = new System.Windows.Forms.ComboBox();
            this.textBox_focus_binary = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
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
            this.checkBox_centr_object.CheckedChanged += new System.EventHandler(this.checkBox_centr_object_CheckedChanged);
            // 
            // checkBox_boarder_object
            // 
            resources.ApplyResources(this.checkBox_boarder_object, "checkBox_boarder_object");
            this.checkBox_boarder_object.Name = "checkBox_boarder_object";
            this.checkBox_boarder_object.UseVisualStyleBackColor = true;
            this.checkBox_boarder_object.CheckedChanged += new System.EventHandler(this.checkBox_boarder_object_CheckedChanged);
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
            this.textBox_video_name.DoubleClick += new System.EventHandler(this.textBox_video_name_DoubleClick);
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
            // but_set_exposit
            // 
            resources.ApplyResources(this.but_set_exposit, "but_set_exposit");
            this.but_set_exposit.Name = "but_set_exposit";
            this.but_set_exposit.UseVisualStyleBackColor = true;
            this.but_set_exposit.Click += new System.EventHandler(this.but_set_exposit_Click);
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
            // button_set_nozzle_centr
            // 
            resources.ApplyResources(this.button_set_nozzle_centr, "button_set_nozzle_centr");
            this.button_set_nozzle_centr.Name = "button_set_nozzle_centr";
            this.button_set_nozzle_centr.UseVisualStyleBackColor = true;
            this.button_set_nozzle_centr.Click += new System.EventHandler(this.button_set_nozzle_centr_Click);
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // comboBox_images
            // 
            this.comboBox_images.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_images, "comboBox_images");
            this.comboBox_images.Name = "comboBox_images";
            this.comboBox_images.SelectedIndexChanged += new System.EventHandler(this.comboBox_images_SelectedIndexChanged);
            // 
            // textBox_focus_binary
            // 
            resources.ApplyResources(this.textBox_focus_binary, "textBox_focus_binary");
            this.textBox_focus_binary.Name = "textBox_focus_binary";
            this.textBox_focus_binary.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_focus_binary_KeyDown);
            // 
            // CameraForm
            // 
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.textBox_focus_binary);
            this.Controls.Add(this.comboBox_images);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button_set_nozzle_centr);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.textBox_set_exposit);
            this.Controls.Add(this.but_set_exposit);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.Button but_set_exposit;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_set_nozzle_centr;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox comboBox_images;
        private System.Windows.Forms.TextBox textBox_focus_binary;
    }
}

