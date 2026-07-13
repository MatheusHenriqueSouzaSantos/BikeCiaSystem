# BikeCiaSystem
 
O BikeCiaSystem é um sistema ERP desenvolvido para auxiliar bicicletarias no gerenciamento de vendas e controle de estoque.
 
Este projeto foi desenvolvido para a disciplina de Estágio Supervisionado do curso de Análise e Desenvolvimento de Sistemas da Faculdade UMFG.
 
Este repositório contém o backend (API) da aplicação, desenvolvido por mim utilizando C# e ASP.NET Core, responsável por garantir as regras de negócio, realizar a autenticação e autorização dos usuários, persistir os dados no PostgreSQL e disponibilizar os endpoints consumidos pelo frontend. O frontend foi desenvolvido majoritariamento pelo me parceiro, com meu auxilio e está disponível em: https://github.com/Marcos-ZF/FrontProjetoFinal
 
## 📑 Sumário
- ☁️ [Demonstração](#️-demonstração)
- ✨ [Funcionalidades](#-funcionalidades)
- 🚀 [Tecnologias](#-tecnologias)
- 🏗️ [Arquitetura do Sistema](#️-arquitetura-do-sistema)
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

O FrontEnd se encontra publicado no vercel:

Aplicação (Vercel): https://front-projeto-final-snowy.vercel.app/

Para Acessar o sistema utilize o Usuário de demonstração:
- Email: demo@bikecia.com
- Senha: demo123
- * esse usuário possuí a role de user, e não é possível alterar nenhuma informação dele

## ✨ Funcionalidades
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

## 📐 Diagramas Do Sistema
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


## 🔧 Como Executar o Projeto Localmente

### Pré requisitos
- .NET SDK 8.0
- PostgreSQL 13 ou superior
- Git
- 
#### 1. Clone o Repositório (execute os comandos abaixo)
- git clone https://github.com/MatheusHenriqueSouzaSantos/BikeCiaSystem
- cd BikeCiaSystem
  
#### 2. Configure o Banco de Dados Postgres
- crie um banco de dados no postgreSQL chamado projeto_estagio_bicicletaria
- execute o script de criação nas tabelas que esta disponível na pasta databse desse repositório

#### 3. Defina as Variavéis de Ambiente (preencha as informações do banco de dados com os seus dados e execute os comandos)

##### Windows(Power Shell)
```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=projeto_estagio_bicicletaria;Username=SEU_USERNAME_POSTGRES;Password=SEU_PASSWORD_POSTGRES;"
$env:Jwt__Key="H8rQ2mLp5ZwX7NcDv1FsKa9YtGu4BeJx0MiCn3RhPk6LsVq8AfTdWy5UzEnSb4Oo"
$env:User__CodigoUser="abcd"
$env:User__Email="admin2026@gmail.com"
$env:User__Nome="admin"
$env:User__Senha="admin"
```

##### Linux/Mac
```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=projeto_estagio_bicicletaria;Username=SEU_USERNAME_POSTGRES;Password=SEU_PASSWORD_POSTGRES;"
export Jwt__Key="H8rQ2mLp5ZwX7NcDv1FsKa9YtGu4BeJx0MiCn3RhPk6LsVq8AfTdWy5UzEnSb4Oo"
export User__CodigoUser="abcd"
export User__Email="admin2026@gmail.com"
export User__Nome="admin"
export User__Senha="admin"
```

#### 4. Executar o Projeto
- dotnet run

#### Pronto, agora o projeto está disponível na rota: http://localhost:10000/api, e o swagger: http://localhost:10000/api/swagger, para fazer login pode usar o usuário:
##### email: admin2026@gmail.com
##### senha: admin

### 🧪 Testes
#### O projeto possuí testes unitários e para executalos basta executar:
- dotnet test


## 📄 Licença

Este projeto está licenciado sob a **PolyForm Noncommercial License 1.0.0**.

Isso significa que você pode usar, estudar, copiar e modificar o código livremente, 
inclusive para fins educacionais, desde que não seja para fins comerciais.

Texto completo da licença: https://polyformproject.org/licenses/noncommercial/1.0.0/
