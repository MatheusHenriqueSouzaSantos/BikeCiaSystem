# BikeCiaSystem
O BikeCiaSystem é um sistema ERP desenvolvido para auxiliar bicicletarias no gerenciamento de suas operações diárias, como vendas e controle de estoque.

Este Projeto foi desenvolvido para a disciplina do Estágio Supervisionado Do curso de Análise e Desenvolvimento de Sistemas da Faculdade UMFG. 

Este repositório contém o backend(API) da aplicação, desenvolvido por mim utilizando C# e ASP.NET Core que.
No qual a API é responsável por garantir as regras de negócio, realizar a autenticação e autorização dos usuários, persistir os dados no PostgreSQL e disponibilizar os endpoints consumidos pelo frontend.

## ☁️ Demonstração  

O backend encontra-se publicado na plataforma Render.

API: https://projetoapidevendaestagio.onrender.com/api
Swagger: https://projetoapidevendaestagio.onrender.com/api/swagger

O frontend foi desenvolvido majoritariamente pelo meu parceiro de estágio, com minha colaboração durante o desenvolvimento.

Repositório: https://github.com/Marcos-ZF/FrontProjetoFinal
Aplicação (Vercel): https://front-projeto-final-snowy.vercel.app/

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
- Vendedores Com Maior Faturamento Por Periodo(Disponível somente para admins)
  
### Auditoria
- Logs em Todas as Operações(Disponível somente para admins)
  
### Alteração Das Informações Do Usuário Logado
- O usuário logado, pode alterar suas informações, exceto role
  
## Tecnologias
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


## Arquitetura do Sistema
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
