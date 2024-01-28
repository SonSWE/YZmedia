/*
 * Project: TPDN
 * Author : Navisoft.
 * Summary: Bản này đã sửa cho nhanh hơn
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	NVS         Created
 */

using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess
{
    public class OracleHelper
    {
        #region "private utility methods & constructors"

        // Since this class provides only static methods, make the default constructor private to prevent 
        // instances from being created with "new OracleHelper()".
        private OracleHelper()
        {
        }


        // This method is used to attach array of OracleParameters to a OracleCommand.
        // This method will assign a value of DbNull to any parameter with a direction of
        // InputOutput and a value of null. 
        // This behavior will prevent default values from being used, but
        // this will be the less common case than an intended pure output parameter (derived as InputOutput)
        // where the user provided no input value.
        // Parameters:
        // -command - The command to which the parameters will be added
        // -commandParameters - an array of OracleParameters to be added to command
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            if ((command == null)) throw new ArgumentNullException("command is null");
            if (((commandParameters != null)))
            {
                foreach (OracleParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && p.Value == null)
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }


        // This method assigns dataRow column values to an array of OracleParameters.
        // Parameters:
        // -commandParameters: Array of OracleParameters to be assigned values
        // -dataRow: the dataRow used to hold the stored procedure' s parameter values
        private static void AssignParameterValues(OracleParameter[] commandParameters, DataRow dataRow)
        {

            if (commandParameters == null || dataRow == null)
            {
                // Do nothing if we get no data 
                return;
            }

            int i = 0;
            foreach (OracleParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if ((commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1))
                {
                    throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", i, commandParameter.ParameterName));
                }
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                {
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                }
                i = i + 1;
            }
        }

        // This method assigns an array of values to an array of OracleParameters.
        // Parameters:
        // -commandParameters - array of OracleParameters to be assigned values
        // -array of objects holding the values to be assigned
        private static void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
        {

            int i = 0;
            int j = 0;

            if ((commandParameters == null) && (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Value array
            j = commandParameters.Length - 1;
            for (i = 0; i <= j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if ((paramInstance.Value == null))
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if ((parameterValues[i] == null))
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        // This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        // to the provided command.
        // Parameters:
        // -command - the OracleCommand to be prepared
        // -connection - a valid OracleConnection, on which to execute this command
        // -transaction - a valid OracleTransaction, or ' null' 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParameters to be associated with the command or ' null' if no parameters are required
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, ref bool mustCloseConnection)
        {

            if ((command == null)) throw new ArgumentNullException("command");
            if ((commandText == null || commandText.Length == 0)) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or Oracle statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it.
            if ((transaction != null))
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            //command.Transaction = transaction

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if ((commandParameters != null))
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        // PrepareCommand

        #endregion

        #region ExecuteBatchNonQuery

        public static int ExcuteBatchNonQuery(string connectionString, CommandType commandType, string commandText, int numItem, params OracleParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
            // Create & open a OracleConnection, and dispose of it after we are done
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExcuteBatchNonQuery(connection, commandType, commandText, numItem, commandParameters);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        public static int ExcuteBatchNonQuery(OracleConnection connection, CommandType commandType, string commandText, int numItem, params OracleParameter[] commandParameters)
        {

            if ((connection == null)) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();

            int retval = 0;
            bool mustCloseConnection = false;

            PrepareBatchCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, numItem, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            //SangVV ADD de giai phong param truoc do, ko giai phong connecttion
            //if (cmd != null) cmd.Dispose();

            if ((mustCloseConnection)) connection.Close();

            return retval;
        }

        public static int ExcuteBatchNonQuery(OracleTransaction transaction, CommandType commandType, string commandText, int numItem, params OracleParameter[] commandParameters)
        {

            if ((transaction == null)) throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();

            int retval = 0;
            bool mustCloseConnection = false;

            PrepareBatchCommand(cmd, transaction.Connection, transaction, commandType, commandText, numItem, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            return retval;
        }

        private static void PrepareBatchCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, int numItem, OracleParameter[] commandParameters, ref bool mustCloseConnection)
        {

            if ((command == null)) throw new ArgumentNullException("command");
            if ((commandText == null || commandText.Length == 0)) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or Oracle statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it.
            if ((transaction != null))
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            //command.Transaction = transaction

            // Set the command type
            command.CommandType = commandType;


            //Set Batch Item
            command.ArrayBindCount = numItem;

            // Attach the command parameters if they are provided
            if ((commandParameters != null))
            {
                AttachParameters(command, commandParameters);
            }

            return;
        }
        #endregion

        #region "ExecuteNonQuery"

        // Execute a OracleCommand (that returns no resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(connectionString, commandType, commandText, (OracleParameter[])null);
        }


        // Execute a OracleCommand (that returns no resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
            // Create & open a OracleConnection, and dispose of it after we are done
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }
        // ExecuteNonQuery

        // Execute a OracleCommand (that returns no resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -connection - a valid OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters

            return ExecuteNonQuery(connection, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteNonQuery

        // Execute a OracleCommand (that returns no resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {

            if ((connection == null)) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            int retval = 0;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            //SangVV ADD de giai phong param truoc do, ko giai phong connecttion
            //if (cmd != null) cmd.Dispose();

            if ((mustCloseConnection)) connection.Close();

            return retval;
        }
        // ExecuteNonQuery

        // Execute a OracleCommand (that returns no resultset and takes no parameters) against the provided OracleTransaction.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -transaction - a valid OracleTransaction associated with the connection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(transaction, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteNonQuery

        // Execute a OracleCommand (that returns no resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {

            if ((transaction == null)) throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            int retval = 0;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            //SangVV them de dong command
            //if (cmd != null) cmd.Dispose();
            return retval;
        }
        // ExecuteNonQuery

        #endregion

        #region "ExecuteDataset"

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim ds As DataSet = OracleHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connectionString, commandType, commandText, (OracleParameter[])null);
        }

        // Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {

            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            // Create & open a OracleConnection, and dispose of it after we are done
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connection - a valid OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText)
        {

            // Pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connection, commandType, commandText, (OracleParameter[])null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if ((connection == null)) throw new ArgumentNullException("connection");
            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            DataSet ds = new DataSet();
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            cmd.InitialLOBFetchSize = 2 * 1024; // 1~4KB
            cmd.ExecuteNonQuery();

            if (commandParameters != null && commandParameters.Length > 0)
            {
                foreach (var oracleParameter in commandParameters)
                {
                    if (oracleParameter.OracleDbType != OracleDbType.RefCursor) continue;
                    if (oracleParameter.Direction != ParameterDirection.Output && oracleParameter.Direction != ParameterDirection.InputOutput) continue;

                    var dataTable = new DataTable();

                    var refCursorValue = (OracleRefCursor)oracleParameter.Value;
                    if (refCursorValue.IsNull == false)
                    {
                        var reader = refCursorValue.GetDataReader();

                        // Create table columns
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dataTable.Columns.Add(new DataColumn(reader.GetName(i), reader.GetFieldType(i)));
                        }

                        // Create table rows
                        if (reader.HasRows)
                        {
                            reader.FetchSize = 2 * 1024 * 1024; // 2~4MB
                            while (reader.Read())
                            {
                                var dataRow = dataTable.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataRow[i] = reader.GetValue(i);
                                }
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }

                    // Add table to DataSet
                    ds.Tables.Add(dataTable);
                }
            }

            if (mustCloseConnection && connection != null) connection.Close();

            // Return the dataset
            return ds;
        }
        // ExecuteDataset

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction. 
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        // Parameters
        // -transaction - a valid OracleTransaction
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(transaction, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteDataset

        // Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if ((transaction == null)) throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            DataSet ds = new DataSet();
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            cmd.InitialLOBFetchSize = 2 * 1024; // 1~4KB
            cmd.ExecuteNonQuery();

            if (commandParameters != null && commandParameters.Length > 0)
            {
                foreach (var oracleParameter in commandParameters)
                {
                    if (oracleParameter.OracleDbType != OracleDbType.RefCursor) continue;
                    if (oracleParameter.Direction != ParameterDirection.Output && oracleParameter.Direction != ParameterDirection.InputOutput) continue;

                    var dataTable = new DataTable();

                    var refCursorValue = (OracleRefCursor)oracleParameter.Value;
                    if (refCursorValue.IsNull == false)
                    {
                        var reader = refCursorValue.GetDataReader();

                        // Create table columns
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dataTable.Columns.Add(new DataColumn(reader.GetName(i), reader.GetFieldType(i)));
                        }

                        // Create table rows
                        if (reader.HasRows)
                        {
                            reader.FetchSize = 2 * 1024 * 1024; // 2~4MB
                            while (reader.Read())
                            {
                                var dataRow = dataTable.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataRow[i] = reader.GetValue(i);
                                }
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }

                    // Add table to DataSet
                    ds.Tables.Add(dataTable);
                }
            }

            // Return the dataset
            return ds;
        }
        // ExecuteDataset

        #endregion

        #region "ExecuteReader"
        // this enum is used to indicate whether the connection was provided by the caller, or created by OracleHelper, so that
        // we can set the appropriate CommandBehavior when calling ExecuteReader()
        private enum OracleConnectionOwnership
        {
            // Connection is owned and managed by OracleHelper
            Internal,
            // Connection is owned and managed by the caller
            External
        }
        // OracleConnectionOwnership

        // Create and prepare a OracleCommand, and call ExecuteReader with the appropriate CommandBehavior.
        // If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        // If the caller provided the connection, we want to leave it to them to manage.
        // Parameters:
        // -connection - a valid OracleConnection, on which to execute this command 
        // -transaction - a valid OracleTransaction, or ' null' 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParameters to be associated with the command or ' null' if no parameters are required 
        // -connectionOwnership - indicates whether the connection parameter was provided by the caller, or created by OracleHelper 
        // Returns: OracleDataReader containing the results of the command 
        private static OracleDataReader ExecuteReader(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, OracleConnectionOwnership connectionOwnership)
        {

            if ((connection == null)) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            try
            {
                // Create a reader
                OracleDataReader dataReader = default(OracleDataReader);

                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

                // Call ExecuteReader with the appropriate CommandBehavior
                if (connectionOwnership == OracleConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                //SangVV ADD de giai phong param truoc do, ko giai phong connecttion
                //if (cmd != null) cmd.Dispose();

                return dataReader;
            }
            catch
            {
                if ((mustCloseConnection)) connection.Close();
                if (cmd != null) cmd.Dispose();
                throw;
            }
        }
        // ExecuteReader

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteReader(connectionString, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteReader

        // Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            // Create & open a OracleConnection
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();
                // Call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(connection, (OracleTransaction)null, commandType, commandText, commandParameters, OracleConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the OracleDatReader, we need to close the connection ourselves
                if ((connection != null)) connection.Dispose();
                throw;
            }
        }
        // ExecuteReader

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText)
        {


            return ExecuteReader(connection, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteReader

        // Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            // Pass through the call to private overload using a null transaction value

            return ExecuteReader(connection, (OracleTransaction)null, commandType, commandText, commandParameters, OracleConnectionOwnership.External);
        }
        // ExecuteReader

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteReader(transaction, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteReader

        // Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if ((transaction == null)) throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            // Pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, OracleConnectionOwnership.External);
        }
        // ExecuteReader

        #endregion

        #region "ExecuteScalar"

        // Execute a OracleCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(connectionString, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteScalar

        // Execute a OracleCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim orderCount As Integer = Cint(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
            // Create & open a OracleConnection, and dispose of it after we are done.
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }
        // ExecuteScalar

        // Execute a OracleCommand (that returns a 1x1 resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(connection, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteScalar

        // Execute a OracleCommand (that returns a 1x1 resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {

            if ((connection == null)) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            object retval = null;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Execute the command & return the results
            retval = cmd.ExecuteScalar();

            if ((mustCloseConnection)) connection.Close();

            //SangVV them de dong command
            //if (cmd != null) cmd.Dispose();

            return retval;
        }
        // ExecuteScalar

        // Execute a OracleCommand (that returns a 1x1 resultset and takes no parameters) against the provided OracleTransaction.
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(transaction, commandType, commandText, (OracleParameter[])null);
        }
        // ExecuteScalar

        // Execute a OracleCommand (that returns a 1x1 resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if ((transaction == null)) throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            OracleCommand cmd = new OracleCommand();
            object retval = null;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Execute the command & return the results
            retval = cmd.ExecuteScalar();

            //SangVV them de dong command
            //if (cmd != null) cmd.Dispose();

            return retval;
        }
        // ExecuteScalar

        #endregion

        #region "FillDataset"
        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        // Parameters: 
        // -connectionString: A valid connection string for a OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {

            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
            if ((dataSet == null)) throw new ArgumentNullException("dataSet");

            // Create & open a OracleConnection, and dispose of it after we are done
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);

                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() = {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters: 
        // -connectionString: A valid connection string for a OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {

            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
            if ((dataSet == null)) throw new ArgumentNullException("dataSet");

            // Create & open a OracleConnection, and dispose of it after we are done
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);

                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        // Parameters:
        // -connection: A valid OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        public static void FillDataset(OracleConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {


            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        public static void FillDataset(OracleConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {


            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction. 
        // e.g.: 
        // FillDataset (trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"})
        // Parameters:
        // -transaction: A valid OracleTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        public static void FillDataset(OracleTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {

            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters:
        // -transaction: A valid OracleTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        public static void FillDataset(OracleTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {

            if ((transaction == null)) throw new ArgumentNullException("transaction");
            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        // Private helper method that execute a OracleCommand (that returns a resultset) against the specified OracleTransaction and OracleConnection
        // using the provided parameters.
        // e.g.: 
        // FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid OracleConnection
        // -transaction: A valid OracleTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        private static void FillDataset(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {

            if ((connection == null)) throw new ArgumentNullException("connection");
            if ((dataSet == null)) throw new ArgumentNullException("dataSet");

            // Create a command and prepare it for execution
            OracleCommand command = new OracleCommand();

            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Create the DataAdapter & DataSet
            OracleDataAdapter dataAdapter = new OracleDataAdapter(command);

            try
            {
                // Add the table mappings specified by the user
                if ((tableNames != null) && tableNames.Length > 0)
                {

                    string tableName = "Table";
                    int index = 0;

                    for (index = 0; index <= tableNames.Length - 1; index++)
                    {
                        if ((tableNames[index] == null || tableNames[index].Length == 0)) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName = tableName + (index + 1).ToString();
                    }
                }

                // Fill the DataSet using default values for DataTable names, etc

                dataAdapter.Fill(dataSet);
            }
            finally
            {
                if (((dataAdapter != null))) dataAdapter.Dispose();
            }


            if ((mustCloseConnection)) connection.Close();
        }
        #endregion

        #region "UpdateDataset"
        // Executes the respective command for each inserted, updated, or deleted row in the DataSet.
        // e.g.: 
        // UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order")
        // Parameters:
        // -insertCommand: A valid transact-Oracle statement or stored procedure to insert new records into the data source
        // -deleteCommand: A valid transact-Oracle statement or stored procedure to delete records from the data source
        // -updateCommand: A valid transact-Oracle statement or stored procedure used to update records in the data source
        // -dataSet: the DataSet used to update the data source
        // -tableName: the DataTable used to update the data source
        public static void UpdateDataset(OracleCommand insertCommand, OracleCommand deleteCommand, OracleCommand updateCommand, DataSet dataSet, string tableName)
        {

            if ((insertCommand == null)) throw new ArgumentNullException("insertCommand");
            if ((deleteCommand == null)) throw new ArgumentNullException("deleteCommand");
            if ((updateCommand == null)) throw new ArgumentNullException("updateCommand");
            if ((dataSet == null)) throw new ArgumentNullException("dataSet");
            if ((tableName == null || tableName.Length == 0)) throw new ArgumentNullException("tableName");

            // Create a OracleDataAdapter, and dispose of it after we are done
            OracleDataAdapter dataAdapter = new OracleDataAdapter();
            try
            {
                // Set the data adapter commands
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // Update the dataset changes in the data source
                dataAdapter.Update(dataSet, tableName);

                // Commit all the changes made to the DataSet
                dataSet.AcceptChanges();
            }
            finally
            {
                if (((dataAdapter != null))) dataAdapter.Dispose();
            }
        }
        #endregion
    }
}
