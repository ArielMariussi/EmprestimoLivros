using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLivros.Data
{
    public static class SeedData
    {
        public const string DemoEmail = "demo@demo.com";
        public const string DemoPassword = "Demo@123";

        public static async Task InicializarAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            
            var demoUser = await CriarContaDemoAsync(userManager);

            if (demoUser == null)
            {
                Console.WriteLine("⚠️ Não foi possível criar/encontrar a conta demo. Pulando seed de empréstimos.");
                return;
            }

            
            await CriarEmprestimosDemoAsync(context, demoUser.Id);
        }

        private static async Task<IdentityUser?> CriarContaDemoAsync(UserManager<IdentityUser> userManager)
        {
            var demoUser = await userManager.FindByEmailAsync(DemoEmail);

            if (demoUser == null)
            {
                demoUser = new IdentityUser
                {
                    UserName = DemoEmail,
                    Email = DemoEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(demoUser, DemoPassword);

                if (result.Succeeded)
                {
                    Console.WriteLine("✅ Conta demo criada com sucesso!");
                    return demoUser;
                }
                else
                {
                    Console.WriteLine("❌ Erro ao criar conta demo:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                    return null;
                }
            }

            Console.WriteLine("ℹ️ Conta demo já existe no banco.");
            return demoUser;
        }

        private static async Task CriarEmprestimosDemoAsync(ApplicationDbContext context, string demoUserId)
        {
           
            var jaTemEmprestimos = await context.Emprestimos
                .AnyAsync(e => e.UserId == demoUserId);

            if (jaTemEmprestimos)
            {
                Console.WriteLine("ℹ️ Conta demo já possui empréstimos cadastrados.");
                return;
            }

            
            var emprestimosDemo = new List<Emprestimo>
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

            await context.Emprestimos.AddRangeAsync(emprestimosDemo);
            await context.SaveChangesAsync();

            Console.WriteLine($"✅ {emprestimosDemo.Count} empréstimos demo criados com sucesso!");
        }
    }
}