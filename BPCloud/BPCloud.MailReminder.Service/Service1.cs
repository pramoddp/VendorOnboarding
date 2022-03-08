using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPCloud.MailReminder.Service
{
    public partial class Service1 : ServiceBase
    {
        private Timer Schedular;
        private static bool Starter = false;
        private static bool Flag = true;
        private static int scheduledTime = 1;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            scheduledTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduleTime"]);
            ErrorLog.WriteLog("OBD Mail Reminder Service started");
            this.ScheduleService();
        }
        protected override void OnStop()
        {
            ErrorLog.WriteLog("OBD Mail Reminder Service stopped");
        }
        public void ScheduleService() //schdule timing
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                if (Starter)
                {
                    if (dateTime.Hour == scheduledTime)
                    {
                        if (Flag)
                        {
                            Flag = false;
                            ErrorLog.WriteLog("OBD Mail Reminder Service started to send mail");
                            ReminderMailSender.StartReminder();
                        }
                    }
                    else
                    {
                        Flag = true;
                    }
                }

                Schedular = new Timer(new TimerCallback(SchedularCallback));
                DateTime schedulerTime = DateTime.MinValue;

                int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);
                schedulerTime = DateTime.Now.AddMinutes(intervalMinutes);
                if (DateTime.Now > schedulerTime)
                {
                    //If Scheduled Time is passed set Schedule for the next Interval.
                    schedulerTime = schedulerTime.AddMinutes(intervalMinutes);
                }

                TimeSpan timeSpan = schedulerTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                ErrorLog.WriteLog("OBD Mail Reminder Service scheduled to run after: " + schedule);
                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Schedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }
        private void SchedularCallback(object e)
        {
            Starter = true;
            this.ScheduleService();
        }

        //private Timer Schedular;
        //private static bool Starter = false;
        //public Service1()
        //{
        //    InitializeComponent();
        //}

        //protected override void OnStart(string[] args)
        //{
        //    ErrorLog.WriteLog("OBD Mail Reminder Service started");
        //    this.ScheduleService();
        //}

        //protected override void OnStop()
        //{
        //    ErrorLog.WriteLog("OBD Mail Reminder Service stopped");
        //}
        //public void ScheduleService() //schdule timing
        //{
        //    try
        //    {

        //        if (Starter)
        //        {
        //            ErrorLog.WriteLog("OBD Mail Reminder Service started to send mail");
        //            ReminderMailSender.StartReminder();
        //        }

        //        Schedular = new Timer(new TimerCallback(SchedularCallback));

        //        //Set the Default Time.
        //        DateTime scheduledTime = DateTime.MinValue;

        //        int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

        //        //Set the Scheduled Time by adding the Interval to Current Time.
        //        scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
        //        if (DateTime.Now > scheduledTime)
        //        {
        //            //If Scheduled Time is passed set Schedule for the next Interval.
        //            scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
        //        }

        //        TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
        //        string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        //        ErrorLog.WriteLog("OBD Mail Reminder Service scheduled to run after: " + schedule);
        //        //Get the difference in Minutes between the Scheduled and Current Time.
        //        int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

        //        //Change the Timer's Due Time.
        //        Schedular.Change(dueTime, Timeout.Infinite);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteLog(ex.Message);

        //        //Stop the Windows Service.
        //        using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
        //        {
        //            serviceController.Stop();
        //        }
        //    }
        //}
        //private void SchedularCallback(object e)
        //{
        //    Starter = true;
        //    this.ScheduleService();
        //}
    }
}
