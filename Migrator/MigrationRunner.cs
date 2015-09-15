using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Migrator
{
    public class MigrationRunner
    {
        private IEnumerable<Migration> migrations;
        private ConnectionInfo connection;
        private bool force;

        public MigrationRunner(ConnectionInfo connection, Config config)
        {
            this.connection = connection;
            this.force = config.Force;
            var migrationPath = config.MigrationPath.IsNullOrEmptyReturn(Environment.CurrentDirectory);
            this.migrations = GetPaths(migrationPath).Select(p => new Migration(p)).ToArray();
        }

        private static IEnumerable<string> GetPaths(string path)
        {
            var files = Directory.GetFiles(path, "*.sql").OrderBy(f => f);
            return files;
        }

        public void Migrate()
        {
            Bootstrap();

            foreach (var migration in migrations)
            {
                migration.Migrate(connection, force);
            }
        }

        private string LoadResource(string resource)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        private void Bootstrap()
        {
            var sql = LoadResource("Migrator.Bootstrap.sql");

            connection.Execute(sql);
        }

        public static List<ConnectionInfo> GetConnections(Config config)
        {
            var connections = new List<ConnectionInfo>();

            connections.Add
            (
                new ConnectionInfo("DB", config.ConnectionString, config.CommandTimeout)
            );

            return connections;
        }
    }
}