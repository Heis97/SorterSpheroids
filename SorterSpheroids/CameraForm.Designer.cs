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
            this.imageBox_main = new Emgu.CV.UI.ImageBox();
            this.but_start_recording = new System.Windows.Forms.Button();
            this.checkBox_centr_object = new System.Windows.Forms.CheckBox();
            this.but_auto_focus = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_main)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox_main
            // 
            this.imageBox_main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox_main.Location = new System.Drawing.Point(12, 12);
            this.imageBox_main.Name = "imageBox_main";
            this.imageBox_main.Size = new System.Drawing.Size(1920, 1080);
            this.imageBox_main.TabIndex = 2;
            this.imageBox_main.TabStop = false;
            // 
            // but_start_recording
            // 
            this.but_start_recording.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.but_start_recording.Location = new System.Drawing.Point(886, 1146);
            this.but_start_recording.Name = "but_start_recording";
            this.but_start_recording.Size = new System.Drawing.Size(96, 49);
            this.but_start_recording.TabIndex = 70;
            this.but_start_recording.Text = "Начать запись";
            this.but_start_recording.UseVisualStyleBackColor = true;
            this.but_start_recording.Click += new System.EventHandler(this.but_start_recording_Click);
            // 
            // checkBox_centr_object
            // 
            this.checkBox_centr_object.AutoSize = true;
            this.checkBox_centr_object.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_centr_object.Location = new System.Drawing.Point(186, 1249);
            this.checkBox_centr_object.Name = "checkBox_centr_object";
            this.checkBox_centr_object.Size = new System.Drawing.Size(102, 17);
            this.checkBox_centr_object.TabIndex = 77;
            this.checkBox_centr_object.Text = "Центр объекта";
            this.checkBox_centr_object.UseVisualStyleBackColor = true;
            // 
            // but_auto_focus
            // 
            this.but_auto_focus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.but_auto_focus.Location = new System.Drawing.Point(313, 1203);
            this.but_auto_focus.Name = "but_auto_focus";
            this.but_auto_focus.Size = new System.Drawing.Size(96, 49);
            this.but_auto_focus.TabIndex = 76;
            this.but_auto_focus.Text = "Автофокус";
            this.but_auto_focus.UseVisualStyleBackColor = true;
            // 
            // checkBox_boarder_object
            // 
            this.checkBox_boarder_object.AutoSize = true;
            this.checkBox_boarder_object.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_boarder_object.Location = new System.Drawing.Point(186, 1226);
            this.checkBox_boarder_object.Name = "checkBox_boarder_object";
            this.checkBox_boarder_object.Size = new System.Drawing.Size(115, 17);
            this.checkBox_boarder_object.TabIndex = 75;
            this.checkBox_boarder_object.Text = "Границы объекта";
            this.checkBox_boarder_object.UseVisualStyleBackColor = true;
            // 
            // checkBox_focal_area
            // 
            this.checkBox_focal_area.AutoSize = true;
            this.checkBox_focal_area.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_focal_area.Location = new System.Drawing.Point(186, 1203);
            this.checkBox_focal_area.Name = "checkBox_focal_area";
            this.checkBox_focal_area.Size = new System.Drawing.Size(60, 17);
            this.checkBox_focal_area.TabIndex = 74;
            this.checkBox_focal_area.Text = "Фокус";
            this.checkBox_focal_area.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(34, 1204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 73;
            this.label2.Text = "Показать на изображении";
            // 
            // but_start_video
            // 
            this.but_start_video.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.but_start_video.Location = new System.Drawing.Point(535, 1172);
            this.but_start_video.Name = "but_start_video";
            this.but_start_video.Size = new System.Drawing.Size(96, 49);
            this.but_start_video.TabIndex = 72;
            this.but_start_video.Text = "Запуск видео";
            this.but_start_video.UseVisualStyleBackColor = true;
            // 
            // but_stop_recording
            // 
            this.but_stop_recording.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.but_stop_recording.Location = new System.Drawing.Point(886, 1201);
            this.but_stop_recording.Name = "but_stop_recording";
            this.but_stop_recording.Size = new System.Drawing.Size(96, 49);
            this.but_stop_recording.TabIndex = 71;
            this.but_stop_recording.Text = "Остановить запись";
            this.but_stop_recording.UseVisualStyleBackColor = true;
            this.but_stop_recording.Click += new System.EventHandler(this.but_stop_recording_Click);
            // 
            // textBox_video_name
            // 
            this.textBox_video_name.Location = new System.Drawing.Point(530, 1146);
            this.textBox_video_name.Name = "textBox_video_name";
            this.textBox_video_name.Size = new System.Drawing.Size(342, 20);
            this.textBox_video_name.TabIndex = 69;
            // 
            // textBox_camera_number
            // 
            this.textBox_camera_number.Location = new System.Drawing.Point(32, 1146);
            this.textBox_camera_number.Name = "textBox_camera_number";
            this.textBox_camera_number.Size = new System.Drawing.Size(80, 20);
            this.textBox_camera_number.TabIndex = 68;
            // 
            // but_con_cam
            // 
            this.but_con_cam.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.but_con_cam.Location = new System.Drawing.Point(129, 1131);
            this.but_con_cam.Name = "but_con_cam";
            this.but_con_cam.Size = new System.Drawing.Size(96, 49);
            this.but_con_cam.TabIndex = 67;
            this.but_con_cam.Text = "Запуск камеры";
            this.but_con_cam.UseVisualStyleBackColor = true;
            this.but_con_cam.Click += new System.EventHandler(this.but_con_cam_Click);
            // 
            // textBox_set_exposit
            // 
            this.textBox_set_exposit.Location = new System.Drawing.Point(1091, 1216);
            this.textBox_set_exposit.Name = "textBox_set_exposit";
            this.textBox_set_exposit.Size = new System.Drawing.Size(80, 20);
            this.textBox_set_exposit.TabIndex = 79;
            // 
            // button1
            // 
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(1194, 1198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 58);
            this.button1.TabIndex = 78;
            this.button1.Text = "Установить экспозицию";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CameraForm
            // 
            this.ClientSize = new System.Drawing.Size(1964, 1280);
            this.Controls.Add(this.textBox_set_exposit);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.but_start_recording);
            this.Controls.Add(this.checkBox_centr_object);
            this.Controls.Add(this.but_auto_focus);
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
            this.Name = "CameraForm";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox_main;
        private System.Windows.Forms.Button but_start_recording;
        private System.Windows.Forms.CheckBox checkBox_centr_object;
        private System.Windows.Forms.Button but_auto_focus;
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
    }
}

