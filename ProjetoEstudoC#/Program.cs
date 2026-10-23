using System;
using System.Collections.Generic;

class Produto
{
    public int Codigo { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }

    public int Estoque { get; set; }

    public Produto(int codigo, string nome, decimal preco, int estoque)
    {
        Codigo = codigo;
        Nome = nome;
        Preco = preco;
        Estoque = estoque;
    }
}
class Program
{
    static void Main(string[] args)
    {

        List<Produto> produtos = new List<Produto>();

        Console.WriteLine("Deseja criar um novo produto? S / N ");
        String resposta = Console.ReadLine();

        while (resposta.ToUpper().Trim() == "S")
        {

            Console.WriteLine("Qual o codigo do Produto");
            int codigo = int.Parse(Console.ReadLine());

            Console.WriteLine("Qual o nome:");
            string nome = Console.ReadLine();

            Console.WriteLine("Qual o preço:");
            decimal preco = decimal.Parse(Console.ReadLine());

            if (preco <= 0)
            {
                Console.WriteLine("Vai entregar de graça?");
                return;
            }


            Console.WriteLine("Quanto tem em estoque? ");
            int estoque = int.Parse(Console.ReadLine());
            if (estoque < 0)
            {
                Console.WriteLine("Negativo nao doido");
                return;
            }
            else
            {
                Console.WriteLine("Criado");
            }

            Produto produto = new Produto(codigo, nome, preco, estoque);

            produtos.Add(produto);

            decimal valorEstoque = produto.Preco * produto.Estoque;


            Console.WriteLine("Dados do seu produto:");
            Console.WriteLine($"O código dele é {produto.Codigo}");
            Console.WriteLine($"Ele é um: {produto.Nome}");
            Console.WriteLine($"Preço: {produto.Preco:F2}");
            Console.WriteLine($"Quantidade em estoque: {produto.Estoque}");
            Console.WriteLine($"Em Reais o estoque é de {valorEstoque:F2}");

            if (produto.Estoque == 0)
            {
                Console.WriteLine("O estoque ta zerado");
            }
            else if (produto.Estoque < 5)
            {
                Console.WriteLine("O estoque ta baixo mano");
            }
            else
            {
                Console.WriteLine("Pode vender suave");
            }

            Console.WriteLine("Deseja criar mais um? S / N");
            resposta = Console.ReadLine();
        }

        Console.WriteLine($"O total de produtos cadastrados foi {produtos.Count}");

        foreach (Produto item in produtos)
        {
            Console.WriteLine($"Código: {item.Codigo}");
            Console.WriteLine($"Nome: {item.Nome}");
            Console.WriteLine($"Preço: {item.Preco:F2}");
            Console.WriteLine($"Estoque: {item.Estoque}");
        }
    }
}