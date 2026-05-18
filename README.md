# EmprestimoLivros

Sistema web para gerenciamento de empréstimos de livros, desenvolvido com ASP.NET Core 9 MVC.

🔗 **Demo ao vivo:** https://emprestimolivros-9csn.onrender.com

## Sobre o projeto

EmprestimoLivros é uma aplicação web que permite o controle completo de empréstimos de livros. Cada usuário pode cadastrar, editar, excluir e visualizar seus próprios empréstimos, com sistema de autenticação e isolamento de dados.

O projeto conta com um **modo demonstração** acessível por um botão na tela de login, permitindo testar a aplicação sem precisar criar conta. Os dados da conta demo são resetados automaticamente a cada 24 horas.

## Como testar

Acesse https://emprestimolivros-9csn.onrender.com e você pode:

1. **Entrar como Demo** — clique no botão "Entrar como Demo" na tela de login (sem precisar cadastrar)
2. **Criar uma conta** — use o cadastro para ter sua própria área isolada

> A aplicação está no plano gratuito do Render. A primeira visita pode levar até 30 segundos para carregar (cold start).

## Funcionalidades

- Cadastro e login de usuários (ASP.NET Identity)
- Modo demonstração com botão de acesso rápido
- CRUD completo de empréstimos (criar, listar, editar, excluir)
- Isolamento de dados: cada usuário vê apenas seus próprios empréstimos
- Reset automático dos dados de demonstração a cada 24 horas
- Mensagens de feedback ao usuário (sucesso/erro)
- Interface responsiva com Bootstrap 5

## Tecnologias

- **ASP.NET Core 9 MVC** — framework web
- **Entity Framework Core 9** — ORM
- **ASP.NET Identity** — autenticação e autorização
- **PostgreSQL (Neon)** — banco de dados na nuvem
- **Bootstrap 5** — interface
- **Docker** — containerização
- **Render** — hospedagem com CI/CD automático
- **GitHub** — versionamento



## Estrutura do projeto

```
EmprestimoLivros/
├── Controllers/      Lógica de controle (Account, Emprestimo, Home)
├── Data/             DbContext e Seed inicial
├── Models/           Entidades
├── ViewModels/       ViewModels para Login e Register
├── Services/         BackgroundService de reset diário
├── Views/            Views Razor
├── Migrations/       Migrations do EF Core
├── wwwroot/          Arquivos estáticos (css, js, imagens)
├── Dockerfile        Multi-stage build
└── Program.cs        Bootstrap da aplicação
```

## Deploy

O deploy é feito automaticamente no Render a cada push na branch `main`. O Dockerfile faz build multi-stage e as migrations são aplicadas automaticamente no startup da aplicação.

## Autor

**Ariel Mariussi**

- Portfolio : https://portfolio-ariel-cyan.vercel.app/
- GitHub: [@ArielMariussi](https://github.com/ArielMariussi)
- Email: arielmariussi@gmail.com
