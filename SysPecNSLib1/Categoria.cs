using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SysPecNSLib
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Sigla { get; set; }

        //Metodos construtores para inserção e consulta
        public Categoria()
        {

        }
        public Categoria(string? nome, string? sigla)
        {
            Nome = nome;
            Sigla = sigla;
        }

        public Categoria(int id, string? nome, string? sigla = null)
        {
            Id = id;
            Nome = nome;
            Sigla = sigla;
        }

        //para inserir categorias usando procedure
        public void Inserir()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_categoria_insert";
            cmd.Parameters.AddWithValue("spnome", Nome);
            cmd.Parameters.AddWithValue("spsigla", Sigla);
            Id = Convert.ToInt32(cmd.ExecuteScalar());
        }
        //Obter por id as categorias
        public static Categoria ObterPorId(int id)
        {
            Categoria categoria = new();
            var cmd = Banco.Abrir();
            cmd.CommandText = $"select * from categorias where id = {id}";
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                categoria = new(dr.GetInt32(0), dr.GetString(1), null);
            }
            return categoria;
        }
        //listar as categorias 
        public static List<Categoria> ObterLista()
        {
            List<Categoria> categorias = new();
            var cmd = Banco.Abrir();
            cmd.CommandText = $"select * from categorias";
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                categorias.Add(new(dr.GetInt32(0), dr.GetString(1), null));
            }
            return categorias;
        }
        //atualizar as categorias usando procedure
        public void Atualizar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_categoria_update";
            cmd.Parameters.AddWithValue("spid", Id);
            cmd.Parameters.AddWithValue("spnome", Nome);
            cmd.Parameters.AddWithValue("spsigla", Sigla);
            cmd.ExecuteNonQuery();
        }
        //deletar categorias
        public void Deletar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = $"delete from categorias where id = {Id}";
            cmd.ExecuteNonQuery();
        }


    }
}
