# BikeCiaSystem
 
O BikeCiaSystem é um sistema ERP desenvolvido para auxiliar bicicletarias no gerenciamento de suas operações diárias, como vendas e controle de estoque.
 
Este projeto foi desenvolvido para a disciplina de Estágio Supervisionado do curso de Análise e Desenvolvimento de Sistemas da Faculdade UMFG.
 
Este repositório contém o backend (API) da aplicação, desenvolvido por mim utilizando C# e ASP.NET Core, responsável por garantir as regras de negócio, realizar a autenticação e autorização dos usuários, persistir os dados no PostgreSQL e disponibilizar os endpoints consumidos pelo frontend.
 
## 📑 Sumário
- [Demonstração](#️-demonstração)
- [Funcionalidades](#-funcionalidades)
- [Tecnologias](#-tecnologias)
- [Arquitetura do Sistema](#️-arquitetura-do-sistema)
- [Diagramas do Sistema](#-diagramas-do-sistema)
- [Como Executar o Projeto](#-como-executar-o-projeto)
- [Testes](#-testes)
- [Estrutura de Pastas](#-estrutura-de-pastas)
- [Desafios e Aprendizados](#-desafios-e-aprendizados)
- [Autor](#-autor)
- [Licença](#-licença)

## ☁️ Demonstração  

O backend encontra-se publicado na plataforma Render.

API: https://projetoapidevendaestagio.onrender.com/api
Swagger: https://projetoapidevendaestagio.onrender.com/api/swagger

O frontend foi desenvolvido majoritariamente pelo meu parceiro de estágio, com minha colaboração durante o desenvolvimento.

Repositório: https://github.com/Marcos-ZF/FrontProjetoFinal
Aplicação (Vercel): https://front-projeto-final-snowy.vercel.app/

Para Acessar o sistema utilize o Usuário de demonstração:
- Email: demo@bikecia.com
- Senha: demo123
- * esse usuário possuí a role de user, e não é possível alterar nenhuma informação dele

## ✨ Funcionalidade
### 🔐 Autenticação
- JWT
- Roles(Admin/User)
  
### 📋 Cadastros
- Clientes(físico ou jurídico)
- Produtos
- Serviços
- Vendedores
- Fornecedores
- Usuarios(Disponível somente para admins)
  
### 💰 Movimentações
- Venda
- Entrada de Estoque
  
### 📊 Relatórios
- Produtos Mais Vendidos Por Período
- Produtos Em Falta
- Vendas Por Período
- Fornecedores Com Maior Volume De Entrada Estoque Por Período
- Vendedores Com Maior Faturamento Por Periodo(Disponível somente para admins)
  
### 📝 Auditoria
- Logs em Todas as Operações(Disponível somente para admins)
  
### 👤 Perfil do Usuário
- O usuário logado, pode alterar suas informações, exceto role
  
## 🚀 Tecnologias
- C#
- ASP.Net Core
- Entity Framework
- PostgreSQL
- XUnit(Testes Unitários)
- BCrypt.Net
- QuestPDF
- Swagger
- Git
- Docker
- Postman


## 🏗️ Arquitetura do Sistema
![Fluxo Do Sistema](resourcesImages/fluxoDoSistema.drawio.png)

## Diagramas Do Sistema
### Diagrama De Casos De Uso
![Diagrama De Casos De Uso](resourcesImages/diagramaDeCasoDeUso.png)

### Diagrama De Classes
#### Módulo De Entrada Estoque
![Diagrama De Classes Entrada Estoque](resourcesImages/DiagramaClasseEntrada.png)

#### Módulo De Venda
![Diagrama De Classes Venda](resourcesImages/DiagramaDeClasseVenda.png)

### Diagrama De Entidade Relacionamento
#### Módulo De Entrada Estoque
![Diagrama De Entidade Relacionamento De Entrada Estoque](resourcesImages/DerEntrada.png)

#### Módulo De Venda
![Diagrama De Entidade Relacionamento De Venda](resourcesImages/derVenda.png)
