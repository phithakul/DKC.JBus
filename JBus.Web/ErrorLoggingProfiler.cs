using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace JBus.Web
{
    public class ErrorLoggingProfiler : StackExchange.Profiling.Data.IDbProfiler
    {
        private StackExchange.Profiling.Data.IDbProfiler wrapped;

        public ErrorLoggingProfiler(StackExchange.Profiling.Data.IDbProfiler wrapped)
        {
            this.wrapped = wrapped;
        }

        public void ExecuteFinish(IDbCommand profiledDbCommand, StackExchange.Profiling.Data.SqlExecuteType executeType, DbDataReader reader)
        {
            this.wrapped.ExecuteFinish(profiledDbCommand, executeType, reader);
        }

        public void ExecuteStart(IDbCommand profiledDbCommand, StackExchange.Profiling.Data.SqlExecuteType executeType)
        {
            this.wrapped.ExecuteStart(profiledDbCommand, executeType);
        }

        public bool IsActive
        {
            get { return this.wrapped.IsActive; }
        }

        public void OnError(IDbCommand profiledDbCommand, StackExchange.Profiling.Data.SqlExecuteType executeType, Exception exception)
        {
            var formatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
            exception.Data["SQL"] = formatter.FormatSql(profiledDbCommand.CommandText, SqlTiming.GetCommandParameters(profiledDbCommand));
            this.wrapped.OnError(profiledDbCommand, executeType, exception);
        }

        public void ReaderFinish(IDataReader reader)
        {
            this.wrapped.ReaderFinish(reader);
        }
    }
}