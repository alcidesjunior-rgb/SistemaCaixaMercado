
using System;
using System.Collections.Generic;
using System.Globalization;

class Produto
{
    public int Codigo;
    public string Nome;
    public decimal Preco;
    public int Estoque;

    public Produto(int codigo, string nome, decimal preco, int estoque)
    {
        Codigo = codigo;
        Nome = nome;
        Preco = preco;
        Estoque = estoque;
    }
}

class ItemCarrinho
{
    public Produto Produto;
    public int Quantidade;

    public ItemCarrinho(Produto produto, int quantidade)
    {
        Produto = produto;
        Quantidade = quantidade;
    }
}

class Program
{
    static List<Produto> produtos = new List<Produto>();
    static List<ItemCarrinho> carrinho = new List<ItemCarrinho>();

    static void Main()
    {
        CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

        // Produtos iniciais do mercado
        produtos.Add(new Produto(101, "Arroz 5kg", 25.90m, 20));
        produtos.Add(new Produto(102, "Feijao 1kg", 8.50m, 30));
        produtos.Add(new Produto(103, "Leite 1L", 5.20m, 40));
        produtos.Add(new Produto(104, "Acucar 1kg", 4.80m, 25));
        produtos.Add(new Produto(105, "Cafe 500g", 18.90m, 15));

        int opcao = -1;

        while (opcao != 0)
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("       MERCADO - SISTEMA PDV");
            Console.WriteLine("================================");
            Console.WriteLine("1 - Listar produtos");
            Console.WriteLine("2 - Adicionar produto ao carrinho");
            Console.WriteLine("3 - Visualizar carrinho");
            Console.WriteLine("4 - Remover produto do carrinho");
            Console.WriteLine("5 - Finalizar venda");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("================================");
            Console.Write("Escolha uma opcao: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Opcao invalida!");
                Console.ReadKey();
                continue;
            }

            switch (opcao)
            {
                case 1:
                    ListarProdutos();
                    break;

                case 2:
                    AdicionarProduto();
                    break;

                case 3:
                    VisualizarCarrinho();
                    break;

                case 4:
                    RemoverProduto();
                    break;

                case 5:
                    FinalizarVenda();
                    break;

                case 0:
                    Console.WriteLine("Sistema encerrado.");
                    break;

                default:
                    Console.WriteLine("Opcao inexistente!");
                    break;
            }

            if (opcao != 0)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    static void ListarProdutos()
    {
        Console.Clear();
        Console.WriteLine("========== PRODUTOS ==========");

        foreach (Produto produto in produtos)
        {
            Console.WriteLine(
                $"Codigo: {produto.Codigo} | " +
                $"Nome: {produto.Nome} | " +
                $"Preco: {produto.Preco:C} | " +
                $"Estoque: {produto.Estoque}"
            );
        }
    }

    static void AdicionarProduto()
    {
        Console.Clear();

        Console.Write("Digite o codigo do produto: ");
        string codigoTexto = Console.ReadLine();

        if (!int.TryParse(codigoTexto, out int codigo))
        {
            Console.WriteLine("Codigo invalido!");
            return;
        }

        Produto produto = produtos.Find(p => p.Codigo == codigo);

        if (produto == null)
        {
            Console.WriteLine("Produto nao encontrado!");
            return;
        }

        Console.Write("Digite a quantidade: ");

        if (!int.TryParse(Console.ReadLine(), out int quantidade) ||
            quantidade <= 0)
        {
            Console.WriteLine("Quantidade invalida!");
            return;
        }

        ItemCarrinho item = carrinho.Find(
            i => i.Produto.Codigo == codigo
        );

        int quantidadeNoCarrinho = item == null ? 0 : item.Quantidade;

        if (quantidade + quantidadeNoCarrinho > produto.Estoque)
        {
            Console.WriteLine("Estoque insuficiente!");
            return;
        }

        if (item == null)
        {
            carrinho.Add(new ItemCarrinho(produto, quantidade));
        }
        else
        {
            item.Quantidade += quantidade;
        }

        Console.WriteLine("Produto adicionado com sucesso!");
    }

    static void VisualizarCarrinho()
    {
        Console.Clear();

        Console.WriteLine("========== CARRINHO ==========");

        if (carrinho.Count == 0)
        {
            Console.WriteLine("Carrinho vazio!");
            return;
        }

        decimal total = 0;

        foreach (ItemCarrinho item in carrinho)
        {
            decimal subtotal = item.Produto.Preco * item.Quantidade;

            Console.WriteLine(
                $"{item.Produto.Nome} | " +
                $"Quantidade: {item.Quantidade} | " +
                $"Subtotal: {subtotal:C}"
            );

            total += subtotal;
        }

        Console.WriteLine("------------------------------");
        Console.WriteLine($"TOTAL DA COMPRA: {total:C}");
    }

    static void RemoverProduto()
    {
        Console.Clear();

        Console.Write("Digite o codigo do produto que deseja remover: ");

        if (!int.TryParse(Console.ReadLine(), out int codigo))
        {
            Console.WriteLine("Codigo invalido!");
            return;
        }

        ItemCarrinho item = carrinho.Find(
            i => i.Produto.Codigo == codigo
        );

        if (item == null)
        {
            Console.WriteLine("Produto nao encontrado no carrinho!");
            return;
        }

        carrinho.Remove(item);

        Console.WriteLine("Produto removido com sucesso!");
    }

    static void FinalizarVenda()
    {
        Console.Clear();

        if (carrinho.Count == 0)
        {
            Console.WriteLine("Nao ha produtos no carrinho!");
            return;
        }

        decimal total = 0;

        foreach (ItemCarrinho item in carrinho)
        {
            total += item.Produto.Preco * item.Quantidade;
        }

        Console.WriteLine("========== FINALIZAR VENDA ==========");
        Console.WriteLine($"Total da compra: {total:C}");

        Console.WriteLine("\nFormas de pagamento:");
        Console.WriteLine("1 - Dinheiro");
        Console.WriteLine("2 - PIX");
        Console.WriteLine("3 - Cartao");

        Console.Write("\nEscolha a forma de pagamento: ");

        if (!int.TryParse(Console.ReadLine(), out int pagamento) ||
            pagamento < 1 || pagamento > 3)
        {
            Console.WriteLine("Forma de pagamento invalida!");
            return;
        }

        if (pagamento == 1)
        {
            Console.Write("Valor recebido: R$ ");

            if (!decimal.TryParse(
                Console.ReadLine(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal recebido) || recebido < total)
            {
                Console.WriteLine("Valor recebido invalido ou insuficiente!");
                return;
            }

            decimal troco = recebido - total;

            Console.WriteLine($"Troco: {troco:C}");
        }
        else if (pagamento == 2)
        {
            Console.WriteLine("Pagamento selecionado: PIX");
        }
        else
        {
            Console.WriteLine("Pagamento selecionado: Cartao");
        }

        // Atualiza o estoque apos a venda
        foreach (ItemCarrinho item in carrinho)
        {
            item.Produto.Estoque -= item.Quantidade;
        }

        Console.WriteLine("\n================================");
        Console.WriteLine("       VENDA FINALIZADA!");
        Console.WriteLine($"       TOTAL: {total:C}");
        Console.WriteLine("================================");

        carrinho.Clear();
    }
}
