namespace BPCloud.MailReminder.Service
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BPCloudMailReminderServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.BPCloudMailReminderServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // BPCloudMailReminderServiceProcessInstaller
            // 
            this.BPCloudMailReminderServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.BPCloudMailReminderServiceProcessInstaller.Password = null;
            this.BPCloudMailReminderServiceProcessInstaller.Username = null;
            // 
            // BPCloudMailReminderServiceInstaller
            // 
            this.BPCloudMailReminderServiceInstaller.ServiceName = "BPCloudMailReminderService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.BPCloudMailReminderServiceProcessInstaller,
            this.BPCloudMailReminderServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller BPCloudMailReminderServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller BPCloudMailReminderServiceInstaller;
    }
}