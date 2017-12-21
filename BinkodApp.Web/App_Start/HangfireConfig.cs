using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Hangfire.Dashboard;
using Owin;
using Hangfire.SqlServer;
using Hangfire.Annotations;

namespace BinkodApp.Web.App_Start
{
    public class HangfireConfig
    {
        public static void ConfigureHangfire(IAppBuilder app)
        {
            try
            {
                string connectionString = Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["HangfireConnection"]);

                // Default value == 15 seconds, QueuePollInterval=15sec
                var sqlServerStorageOptions = new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(300) };
                GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, sqlServerStorageOptions);

                var dashboardOptions = new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } };
                app.UseHangfireDashboard("/hangfire", dashboardOptions);// app.UseHangfireDashboard();

                var backgroundJobServerOptions = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * 1 }; //instead of * 5
                app.UseHangfireServer(backgroundJobServerOptions); //app.UseHangfireServer();
            }
            catch(Exception ex) { }
        }

        public static void InitializeJobs()
        {
            try
            {
                //RecurringJob.AddOrUpdate<BinkodApp.Web.Helper.ScheduledJobs>("SendEmail", job => job.SendEmail(), Cron.MinuteInterval(240));
                //Delete three month older files
                string CronExpression = "* * */30 * *";
                RecurringJob.AddOrUpdate<BinkodApp.Web.Helper.ScheduledJobs>("DeleteOlderFiles", job => job.DeleteOlderFiles(), CronExpression);
            }
            catch { }
        }
    }

    internal class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //check user Authorization
            return true;
        }
    }
}