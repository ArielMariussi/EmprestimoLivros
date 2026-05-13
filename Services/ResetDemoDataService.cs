using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLivros.Services
{
    public class ResetDemoDataService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ResetDemoDataService> _logger;

        // ⏰ De quanto em quanto tempo resetar (24 horas)
        private readonly TimeSpan _intervalo = TimeSpan.FromMinutes(30);

        public ResetDemoDataService(
            IServiceProvider serviceProvider,
            ILogger<ResetDemoDataService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🔄 ResetDemoDataService iniciado. Reset a cada {Horas}h.", _intervalo.TotalHours);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Espera o intervalo ANTES de resetar
                    // (na primeira execução, espera 24h pra não resetar logo após o startup)
                    await Task.Delay(_intervalo, stoppingToken);

                    await ResetarDadosDemoAsync();
                }
                catch (OperationCanceledException)
                {
                    // App está fechando, nada de errado
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erro ao resetar dados do demo.");
                }
            }

            _logger.LogInformation("🛑 ResetDemoDataService parado.");
        }

        private async Task ResetarDadosDemoAsync()
        {
            _logger.LogInformation("🔄 Iniciando reset dos dados do demo...");

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Encontrar usuário demo
            var demoUser = await userManager.FindByEmailAsync(SeedData.DemoEmail);

            if (demoUser == null)
            {
                _logger.LogWarning("⚠️ Conta demo não encontrada. Pulando reset.");
                return;
            }

            // 2. Apagar TODOS os empréstimos do demo
            var emprestimosDoDemo = await context.Emprestimos
                .Where(e => e.UserId == demoUser.Id)
                .ToListAsync();

            if (emprestimosDoDemo.Any())
            {
                context.Emprestimos.RemoveRange(emprestimosDoDemo);
                await context.SaveChangesAsync();
                _logger.LogInformation("🗑️ {Quantidade} empréstimos do demo apagados.", emprestimosDoDemo.Count);
            }

            // 3. Recriar empréstimos bonitos
            var emprestimosDemo = CriarListaEmprestimosDemo(demoUser.Id);
            await context.Emprestimos.AddRangeAsync(emprestimosDemo);
            await context.SaveChangesAsync();

            _logger.LogInformation("✅ {Quantidade} empréstimos demo recriados com sucesso!", emprestimosDemo.Count);
        }

        private static List<Emprestimo> CriarListaEmprestimosDemo(string demoUserId)
        {
            return new List<Emprestimo>
            {
                new Emprestimo
                {
                    Recebedor = "Maria Silva",
                    Fornecedor = "Biblioteca Central",
                    LivroEmprestado = "Dom Casmurro - Machado de Assis",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-2),
                    UserId = demoUserId
                },
                new Emprestimo
                {
                    Recebedor = "João Pedro",
                    Fornecedor = "Ana Costa",
                    LivroEmprestado = "1984 - George Orwell",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-5),
                    UserId = demoUserId
                },
                new Emprestimo
                {
                    Recebedor = "Carla Mendes",
                    Fornecedor = "Biblioteca Central",
                    LivroEmprestado = "O Hobbit - J.R.R. Tolkien",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-1),
                    UserId = demoUserId
                },
                new Emprestimo
                {
                    Recebedor = "Roberto Lima",
                    Fornecedor = "Patrícia Souza",
                    LivroEmprestado = "Sapiens - Yuval Noah Harari",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-7),
                    UserId = demoUserId
                },
                new Emprestimo
                {
                    Recebedor = "Fernanda Alves",
                    Fornecedor = "Biblioteca Central",
                    LivroEmprestado = "Cem Anos de Solidão - Gabriel García Márquez",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-3),
                    UserId = demoUserId
                },
                new Emprestimo
                {
                    Recebedor = "Lucas Oliveira",
                    Fornecedor = "Marina Reis",
                    LivroEmprestado = "O Pequeno Príncipe - Antoine de Saint-Exupéry",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-10),
                    UserId = demoUserId
                },
                new Emprestimo
                {
                    Recebedor = "Beatriz Santos",
                    Fornecedor = "Biblioteca Central",
                    LivroEmprestado = "Senhor dos Anéis - J.R.R. Tolkien",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-4),
                    UserId = demoUserId
                },
                new Emprestimo
                {
                    Recebedor = "Gabriel Rocha",
                    Fornecedor = "Camila Ferreira",
                    LivroEmprestado = "A Revolução dos Bichos - George Orwell",
                    DataUltimaAtualizacao = DateTime.UtcNow.AddDays(-6),
                    UserId = demoUserId
                }
            };
        }
    }
}