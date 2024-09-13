using Org.BouncyCastle.Asn1.Esf;
using SysPecNSLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPecNSLib
{
    public class Estoque
    {
        public Produto Produto { get; set; }
        public double Quantidade { get; set; }
        public DateTime DataUltimoMovimento { get; set; }

        public Estoque()
        {
            Produto = new();
        }

        public Estoque(Produto produto, double quantidade, DateTime dataUltimoMovimento)
        {
            Produto = produto;
            Quantidade = quantidade;
            DataUltimoMovimento = dataUltimoMovimento;
        }

        public void Inserir()
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = $"insert estoque (produto_id, quantidade, data_ultimo_movimento) values ('{Produto}','{Quantidade}','{DataUltimoMovimento}')";
            cmd.ExecuteNonQuery();
        }

        public bool Atualizar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"update estoque set quantidade = '{Quantidade}',data_ultimo_movimento = '{DataUltimoMovimento}' where produto = {Produto}";
            return cmd.ExecuteNonQuery() > 0 ? true : false;
        }
        public static Estoque ObterPorId(int produto_id)
        {
            Estoque estoque = new Estoque();
            // consulta no banco e retornar o Nivel
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"SELECT * FROM estoque where produto_id = {produto_id};";
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Produto.ObterPorId(dr.GetInt32(0));
                estoque.Quantidade = dr.GetDouble(1);
                estoque.DataUltimoMovimento = dr.GetDateTime(2);
            }
            return estoque;
        }
        public static List<Estoque> ObterLista()
        {
            List<Estoque> lista = new List<Estoque>();
            // consulta para retornar a lista de níveis
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from estoque";
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new(
                        Produto.ObterPorId(dr.GetInt32(0)),
                        dr.GetDouble(1),
                        dr.GetDateTime(2)
                        )
                    );
            }
            return lista;
        }

    }
}