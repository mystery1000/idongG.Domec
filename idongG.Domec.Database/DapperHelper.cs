using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SQLite;

namespace idongG.DataBase;

/// <summary>
/// 此代码由DeepSeek生成
///
/// // 在应用程序启动时初始化 mysql
///var connectionString = "server=localhost;database=mydb;user=root;password=123456;";
/// DapperHelper.Initialize(connectionString);

//Sqlserver
///  string DB_STR = $"Data Source={ip};Initial Catalog=CQ_Hanbo_IJPTranslator_Database;User ID={dbusername};Password={dbpassword};";
///
// 连接字符串，指定 SQLite 数据库文件路径
//   string connectionString = "Data Source=example.db;Version=3;";

/// // 同步查询
///  var users = DapperHelper.Query<User>("SELECT * FROM Users WHERE Age > @Age", new { Age = 18 });
/// 异步查询
/// var product = await DapperHelper.QueryFirstOrDefaultAsync<Product>(
///    "SELECT * FROM Products WHERE Id = @Id",
///    new { Id = 123 });
/// 执行插入
///var result = await DapperHelper.ExecuteAsync(
///    "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)",
///    new { Name = "John", Email = "john@example.com" });
/// 使用事务
///  DapperHelper.ExecuteTransaction((conn, transaction) =>
///{
///    conn.Execute("UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1", transaction: transaction);
///    conn.Execute("UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2", transaction: transaction);
///});
/// </summary>
public class DapperHelper
{
    public DapperHelper(string connectionString, EnumDBType enumDBType = EnumDBType.Sqlite)
    {
        _connectionString = connectionString;
        _DBType = enumDBType;
    }

    private string _connectionString;

    public enum EnumDBType
    {
        Mysql = 0,
        Sqlite = 1,
        SqlServer = 3
    }

    private EnumDBType _DBType;
    private IDbConnection conn;

    /// <summary>
    /// 获取数据库连接（自动管理开关）
    /// </summary>
    private async Task<IDbConnection> CreateConnectionAsync()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new ArgumentNullException(nameof(_connectionString), "Connection string not initialized");

        switch (_DBType)
        {
            case EnumDBType.Mysql:
                {
                    if (conn == null) conn = new MySqlConnection(_connectionString);
                    if (conn.State != ConnectionState.Open)
                        await (conn as MySqlConnection).OpenAsync();
                    return conn;
                }

            case EnumDBType.Sqlite:
                {
                    if (conn == null) conn = new SQLiteConnection(_connectionString);
                    if (conn.State != ConnectionState.Open)
                        await (conn as SQLiteConnection).OpenAsync();
                    return conn;
                }

            case EnumDBType.SqlServer:
                {
                    if (conn == null) conn = new SqlConnection(_connectionString);
                    if (conn.State != ConnectionState.Open)
                        await (conn as SqlConnection).OpenAsync();
                    return conn;
                }
        }
        return null;
    }

    private IDbConnection CreateConnection()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new ArgumentNullException(nameof(_connectionString), "Connection string not initialized");

        switch (_DBType)
        {
            case EnumDBType.Mysql:
                {
                    if (conn == null) conn = new MySqlConnection(_connectionString);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return conn;
                }

            case EnumDBType.Sqlite:
                {
                    if (conn == null) conn = new SQLiteConnection(_connectionString);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return conn;
                }

            case EnumDBType.SqlServer:
                {
                    if (conn == null) conn = new SqlConnection(_connectionString);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return conn;
                }
        }
        return null;
    }

    #region 同步方法

    /// <summary>
    /// 执行非查询操作
    /// </summary>
    public int Execute(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        conn = CreateConnection();
        return conn.Execute(sql, parameters, commandType: commandType);
    }

    /// <summary>
    /// 查询单个值
    /// </summary>
    public T ExecuteScalar<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        conn = CreateConnection();
        return conn.ExecuteScalar<T>(sql, parameters, commandType: commandType);
    }

    /// <summary>
    /// 查询数据列表
    /// </summary>
    public IEnumerable<T> Query<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        conn = CreateConnection();
        return conn.Query<T>(sql, parameters, commandType: commandType);
    }

    /// <summary>
    /// 查询单条记录
    /// </summary>
    public T QueryFirstOrDefault<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        conn = CreateConnection();
        return conn.QueryFirstOrDefault<T>(sql, parameters, commandType: commandType);
    }

    /// <summary>
    /// 检查表是否存在
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public bool CheckTableExist(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName)) return false;
        conn = CreateConnection();
        var str = "";
        switch (_DBType)
        {
            case EnumDBType.Mysql:
                str = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = @TableName";
                break;

            case EnumDBType.Sqlite:
                str = $" SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @TableName";
                break;

            case EnumDBType.SqlServer:
                str = $" SELECT COUNT(*) FROM sys.tables WHERE name = @TableName AND schema_id = SCHEMA_ID('dbo')";
                break;
        }

        int tableCount = conn.ExecuteScalar<int>(str, new { TableName = tableName });

        return tableCount > 0;
    }

    #endregion

    #region 异步方法

    /// <summary>
    /// 异步执行非查询操作
    /// </summary>
    public async Task<int> ExecuteAsync(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        using var conn = CreateConnectionAsync();
        return await conn.Result.ExecuteAsync(sql, parameters, commandType: commandType);
    }

    /// <summary>
    /// 异步查询单个值
    /// </summary>
    public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        using var conn = CreateConnectionAsync();
        return await conn.Result.ExecuteScalarAsync<T>(sql, parameters, commandType: commandType);
    }

    /// <summary>
    /// 异步查询数据列表
    /// </summary>
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        using var conn = CreateConnectionAsync();
        return await conn.Result.QueryAsync<T>(sql, parameters, commandType: commandType);
    }

    /// <summary>
    /// 异步查询单条记录
    /// </summary>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
    {
        using var conn = CreateConnectionAsync();
        return await conn.Result.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType);
    }

    #endregion

    #region 事务支持

    /// <summary>
    /// 执行事务操作
    /// </summary>
    public bool ExecuteTransaction(Action<IDbConnection, IDbTransaction> transactionActions)
    {
        using var conn = CreateConnection();
        using var transaction = conn.BeginTransaction();

        try
        {
            transactionActions(conn, transaction);
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    #endregion

    public void Dispose()
    {
        conn?.Close();
    }
}