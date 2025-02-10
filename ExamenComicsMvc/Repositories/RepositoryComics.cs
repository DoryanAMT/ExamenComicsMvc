using ExamenComicsMvc.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;


namespace ExamenComicsMvc.Repositories
{
    #region
    //create or alter procedure SP_INSERT_COMIC
    //(@nombre as nvarchar(50), @imagen as nvarchar(50), @descripcion as nvarchar(50))
    //as
    //declare @maxId as int;
    //select @maxId = max(IDCOMIC) + 1 from COMICS
    //insert into COMICS values(@maxId, @nombre, @imagen, @descripcion)
    //go
    #endregion
    public class RepositoryComics
    {
        DataTable tablaComics;
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        public RepositoryComics()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=COMICS;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from COMICS";
            SqlDataAdapter adComics = new SqlDataAdapter(sql,connectionString);
            this.tablaComics = new DataTable();
            adComics.Fill(this.tablaComics);

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic();
                comic.IdComic = row.Field<int>("IDCOMIC");
                comic.Nombre = row.Field<string>("NOMBRE");
                comic.Imagen = row.Field<string>("IMAGEN");
                comic.Descripcion = row.Field<string>("DESCRIPCION");
                comics.Add(comic);
            }
            return comics;
        }
        public void CreateComic
            (Comic comic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                            select datos;
            comic.IdComic = consulta.Max(x =>  x.Field<int>("IDCOMIC"))+1;
            string sql = "insert into COMICS values(@maxId, @nombre, @imagen, @descripcion)";
            this.com.Parameters.AddWithValue("@maxId", comic.IdComic);
            this.com.Parameters.AddWithValue("@nombre", comic.Nombre);
            this.com.Parameters.AddWithValue("@imagen", comic.Imagen);
            this.com.Parameters.AddWithValue("@descripcion", comic.Descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
        public Comic FindComic
            (int idComic)
        {
            string sql = "select * from COMICS where IDCOMIC=@idcomic";
            this.com.Parameters.AddWithValue("@idcomic", idComic);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            Comic comic = new Comic();
            comic.IdComic = int.Parse(this.reader["IDCOMIC"].ToString());
            comic.Nombre = this.reader["NOMBRE"].ToString();
            comic.Imagen = this.reader["IMAGEN"].ToString();
            comic.Descripcion = this.reader["DESCRIPCION"].ToString();
            this.cn.Close();
            this.com.Parameters.Clear();
            this.reader.Close();
            return comic;
        }
    }
}
