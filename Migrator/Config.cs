using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Migrator
{
    public class Config
    {
        private const int DEFAULT_COMMAND_TIMEOUT = 10 * 60;

        public string ConnectionString { get; set; }
        public int CommandTimeout { get; set; } // number of {seconds} that db commands will execute
        public bool Force { get; set; } // forces migration of all sql files that were previously migrated
        public string MigrationPath { get; set; } // path to where .sql migration files to be run are located

        public Config()
        {
            CommandTimeout = DEFAULT_COMMAND_TIMEOUT;
        }
    }
}