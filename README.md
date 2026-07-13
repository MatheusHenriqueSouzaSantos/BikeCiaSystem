# BikeCiaSystem
O BikeCiaSystem é um sistema ERP desenvolvido para auxiliar bicicletarias no gerenciamento de suas operações diárias, como vendas e controle de estoque.

Este repositório contém o backend(API) da aplicação, desenvolvido por mim utilizando C# e ASP.NET Core.
No qual a API é responsável por garantir as regras de negócio, realizar a autenticação e autorização dos usuários, persistir os dados no PostgreSQL e disponibilizar os endpoints consumidos pelo frontend.


Este Projeto foi desenvolvido para a disciplina do Estágio Supervisionado Do curso de Análise e Desenvolvimento de Sistemas da Faculdade UMFG. 
O Front End da aplicação foi desenvolvido majoriatariamento pelo meu parceiro de estágio, comigo auxiliando quando necessário e está disponível em: https://github.com/Marcos-ZF/FrontProjetoFinal

## Funcionalidade
### Autenticação
- JWT
- Roles(Admin/User)
  
### Cadastros
- Clientes(físico ou jurídico)
- Produtos
- Serviços
- Vendedores
- Fornecedores
- Usuarios(Disponível somente para admins)
  
### Movimentos
- Venda
- Entrada de Estoque
  
### Relatórios
- Produtos Mais Vendidos Por Período
- Produtos Em Falta
- Vendas Por Período
- Fornecedores Com Maior Volume De Entrada Estoque Por Período
- Vendedores Com Maior Faturamento Por Periodo
  
### Auditoria
- Logs em Todas as Operações(Disponível somente para admins)
  
### Alteração Das Informações Do Usuário Logado
- O usuário logado, pode alterar suas informações, exceto role
  
## Tecnologias
- C#
- ASP.Net Core
- Entity Framework
- PostgreSQL
- QuestPDF
- Swagger
- Git
- XUnit(Testes Unitários)
- Postman
  
