using DKC.JBus.Constants;
using Migrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKC.JBus
{
    public class DatabaseMigrator
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Init()
        {
            var config = new Config
            {
                ConnectionString = Application.ConnectionStrings,
                MigrationPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Migrations"),
                Force = true,
                CommandTimeout = 5 * 60
            };

            logger.Info("--------------------------------------------------------------------");
            logger.Info("Start Migration");
            logger.Info("Migration Path: " + config.MigrationPath);

            List<ConnectionInfo> connections = null;
            try
            {
                connections = MigrationRunner.GetConnections(config);

                foreach (var c in connections)
                {
                    logger.Info("Connecting to database: " + c.Name);
                    c.SqlConnection.Open();
                    var runner = new MigrationRunner(c, config);
                    logger.Info("Running migrations");
                    runner.Migrate();
                    c.SqlConnection.Close();
                    logger.Info(c.Name + " is up to date");
                }
            }
            catch (Exception e)
            {
                logger.Error("ERROR OCCURRED WHEN RUNNING MIGRATIONS:");
                logger.Error(e);
            }
            finally
            {
                if (connections != null)
                {
                    foreach (var connection in connections)
                    {
                        try { connection.Dispose(); }
                        catch { logger.Error("ERROR: Failed to close connection"); }
                    }
                }
            }
        }
    }
}