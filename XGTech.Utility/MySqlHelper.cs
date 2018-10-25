using System;
using System.Data;
using MySql.Data.MySqlClient;
/// <summary>
/// 作者：蔡延曦
/// 2017-10-30
/// </summary>
public class MysqlHelpercyx
{
    /// <summary>
    /// 利用MysqlDataReader曲线构造并填充DataTable
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static DataTable ExecuteDataTable(string connectionString, string sql)
    {
        DataTable dt = new DataTable();
        MySqlDataReader dr = MySqlHelper.ExecuteReader(connectionString, sql);


        try
        {
            int fieldCount = dr.FieldCount;


            //获取schema并填充第一行数据
            if (dr.Read())
            {
                for (int i = 0; i < fieldCount; i++)
                {
                    string colName = dr.GetName(i);
                    dt.Columns.Add(colName, dr[i].GetType());
                }


                DataRow newrow = dt.NewRow();
                for (int i = 0; i < fieldCount; i++)
                {
                    newrow[i] = dr[i];
                }
                dt.Rows.Add(newrow);
            }

            //填充后续数据
            while (dr.Read())
            {
                DataRow newrow = dt.NewRow();
                for (int i = 0; i < fieldCount; i++)
                {
                    newrow[i] = dr[i];
                }
                dt.Rows.Add(newrow);
            }
            dt.AcceptChanges();
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1.Message);
            //throw;
        }
        finally
        {
            dr.Close();
        }


        return dt;
    }

    public static DataTable GetDataTable(string connectionString, string sql)
    {
        DataTable dt = new DataTable();
        DataSet set = MySqlHelper.ExecuteDataset(connectionString, sql);
        if (null!=set)
        {
            dt = set.Tables[0];
        }
        return dt;
    }
}