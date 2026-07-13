
CREATE TABLE endereco (
    id UUID PRIMARY KEY,
    logradouro VARCHAR(80) NOT NULL,
    numero VARCHAR(15) NOT NULL,
    cidade VARCHAR(35) NOT NULL,
    sigla_uf CHAR(2) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE cliente (
    id UUID PRIMARY KEY,
    id_endereco UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    telefone VARCHAR(20) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    tipo_cliente VARCHAR(25) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_endereco_em_cliente FOREIGN KEY (id_endereco)
        REFERENCES endereco(id)
);

CREATE TABLE cliente_fisico (
    id UUID PRIMARY KEY,
    nome VARCHAR(70) NOT NULL,
    cpf CHAR(11) UNIQUE NOT NULL,
    CONSTRAINT fk_id_cliente_em_cliente_fisico FOREIGN KEY (id)
        REFERENCES cliente(id)
);

CREATE TABLE cliente_juridico (
    id UUID PRIMARY KEY,
    razao_social VARCHAR(120) NOT NULL,
    nome_fantasia VARCHAR(30),
    inscricao_estadual VARCHAR(15),
    cnpj CHAR(14) UNIQUE NOT NULL,
    CONSTRAINT fk_id_cliente_em_cliente_juridico FOREIGN KEY (id)
        REFERENCES cliente(id)
);

CREATE TABLE produto (
    id UUID PRIMARY KEY,
    codigo_de_barra VARCHAR(128) UNIQUE NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    nome_produto VARCHAR(50) NOT NULL,
    descricao VARCHAR(150),
    preco_unitario DECIMAL(10,2) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE estoque (
    id UUID PRIMARY KEY,
    id_produto UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    quantidade_em_estoque INT NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_id_produto_em_estoque FOREIGN KEY (id_produto)
        REFERENCES produto(id)
);

CREATE TABLE servico (
    id UUID PRIMARY KEY,
    codigo_do_servico VARCHAR(128) UNIQUE NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    nome_servico VARCHAR(50) NOT NULL,
    descricao VARCHAR(150),
    preco_servico DECIMAL(10,2) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE vendedor (
    id UUID PRIMARY KEY,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    telefone VARCHAR(20) NOT NULL,
    email VARCHAR(200) UNIQUE NOT NULL,
    nome_completo VARCHAR(100) NOT NULL,
    cpf CHAR(11) UNIQUE NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE usuario (
    id UUID PRIMARY KEY,
    codigo_usuario CHAR(4) NOT NULL,
    nome VARCHAR(70) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    senha VARCHAR(300) NOT NULL,
    perfil_usuario VARCHAR(30) NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE fornecedor (
    id UUID PRIMARY KEY,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    telefone VARCHAR(20) NOT NULL,
    email VARCHAR(200) UNIQUE NOT NULL,
    razao_social VARCHAR(100) NOT NULL,
    nome_fantasia VARCHAR(100),
    cnpj CHAR(14) UNIQUE NOT NULL,
    inscricao_estadual VARCHAR(15),
    ativo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE venda (
    id UUID PRIMARY KEY,
    codigo_venda CHAR(6) NOT NULL UNIQUE,
    id_cliente UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    desconto_total DECIMAL(10,2),
    valor_total_com_desconto DECIMAL(10,2) NOT NULL,
    valor_total_sem_desconto DECIMAL(10,2) NOT NULL,
    id_vendedor UUID NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_cliente_em_venda FOREIGN KEY (id_cliente)
        REFERENCES cliente(id),
    CONSTRAINT fk_venda_id_vendedor FOREIGN KEY (id_vendedor)
        REFERENCES vendedor(id)
);

CREATE TABLE transacao (
    id UUID PRIMARY KEY,
    id_venda UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    tipo_pagamento VARCHAR(50) NOT NULL,
    meio_pagamento VARCHAR(50) NOT NULL,
    transacao_em_curso BOOLEAN NOT NULL DEFAULT FALSE,
    pago BOOLEAN NOT NULL DEFAULT FALSE,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_venda_em_transacao FOREIGN KEY (id_venda)
        REFERENCES venda(id)
);

CREATE TABLE parcela (
    id UUID PRIMARY KEY,
    id_transacao UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    numero_da_parcela_da_venda INT NOT NULL,
    valor_parcela DECIMAL(10,2) NOT NULL,
    pago BOOLEAN NOT NULL DEFAULT FALSE,
    data_vencimento DATE NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    atual BOOLEAN NOT NULL DEFAULT TRUE, 
    CONSTRAINT fk_transacao_em_parcela FOREIGN KEY (id_transacao)
        REFERENCES transacao(id)
);

CREATE TABLE item_venda (
    id UUID PRIMARY KEY,
    id_venda UUID NOT NULL,
    id_produto UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    quantidade INT NOT NULL,
    desconto_unitario DECIMAL(10,2),
    preco_unitario_do_produto_na_venda_sem_desconto DECIMAL(10,2) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    atual BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_venda_em_item_venda FOREIGN KEY (id_venda)
        REFERENCES venda(id),
    CONSTRAINT fk_produto_em_item_venda FOREIGN KEY (id_produto)
        REFERENCES produto(id)
);

CREATE TABLE servico_venda (
    id UUID PRIMARY KEY,
    id_venda UUID NOT NULL,
    id_servico UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    desconto_servico DECIMAL(10,2),
    preco_servico_na_venda_sem_desconto DECIMAL(10,2) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    atual BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_venda_em_servico_venda FOREIGN KEY (id_venda)
        REFERENCES venda(id),
    CONSTRAINT fk_servico_em_servico_venda FOREIGN KEY (id_servico)
        REFERENCES servico(id)
);

CREATE TABLE entrada_estoque (
    id UUID PRIMARY KEY,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_fornecedor UUID NOT NULL,
    codigo_entrada CHAR(6) NOT NULL UNIQUE,
    status VARCHAR(50) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_entrada_estoque_id_fornecedor FOREIGN KEY (id_fornecedor)
        REFERENCES fornecedor(id)
);

CREATE TABLE item_entrada_estoque (
    id UUID PRIMARY KEY,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_entrada_estoque UUID NOT NULL,
    id_produto UUID NOT NULL,
    quantidade INT NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    atual BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_item_entrada_estoque_id_entrada_estoque FOREIGN KEY (id_entrada_estoque)
        REFERENCES entrada_estoque(id),
    CONSTRAINT fk_item_entrada_estoque_id_produto FOREIGN KEY (id_produto)
        REFERENCES produto(id)
);

CREATE TABLE log_vendedor (
    id UUID PRIMARY KEY,
    id_vendedor UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_vendedor_id_vendedor FOREIGN KEY (id_vendedor)
        REFERENCES vendedor(id),
    CONSTRAINT fk_log_vendedor_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_produto (
    id UUID PRIMARY KEY,
    id_produto UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_produto_id_produto FOREIGN KEY (id_produto)
        REFERENCES produto(id),
    CONSTRAINT fk_log_produto_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_usuario (
    id UUID PRIMARY KEY,
    id_usuario UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_usuario_id_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuario(id),
    CONSTRAINT fk_log_usuario_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_cliente (
    id UUID PRIMARY KEY,
    id_cliente UUID NOT NULL,
    tipo_cliente VARCHAR(30) NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_cliente_id_cliente FOREIGN KEY (id_cliente)
        REFERENCES cliente(id),
    CONSTRAINT fk_log_cliente_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_servico (
    id UUID PRIMARY KEY,
    id_servico UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_servico_id_servico FOREIGN KEY (id_servico)
        REFERENCES servico(id),
    CONSTRAINT fk_log_servico_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_fornecedor (
    id UUID PRIMARY KEY,
    id_fornecedor UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_fornecedor_id_fornecedor FOREIGN KEY (id_fornecedor)
        REFERENCES fornecedor(id),
    CONSTRAINT fk_log_fornecedor_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_endereco (
    id UUID PRIMARY KEY,
    id_endereco UUID NOT NULL,
    id_cliente UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_endereco_id_endereco FOREIGN KEY (id_endereco)
        REFERENCES endereco(id),
    CONSTRAINT fk_log_endereco_id_cliente FOREIGN KEY (id_cliente)
        REFERENCES cliente(id),
    CONSTRAINT fk_log_endereco_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_estoque (
    id UUID PRIMARY KEY,
    id_estoque UUID NOT NULL,
    id_produto UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    acao_que_alterou_estoque VARCHAR(30) NOT NULL,
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_estoque_id_estoque FOREIGN KEY (id_estoque)
        REFERENCES estoque(id),
    CONSTRAINT fk_log_estoque_id_produto FOREIGN KEY (id_produto)
        REFERENCES produto(id),
    CONSTRAINT fk_log_estoque_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_entrada_estoque (
    id UUID PRIMARY KEY,
    id_entrada_estoque UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_entrada_estoque_id_entrada_estoque FOREIGN KEY (id_entrada_estoque)
        REFERENCES entrada_estoque(id),
    CONSTRAINT fk_log_entrada_estoque_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_item_entrada_estoque (
    id UUID PRIMARY KEY,
    id_item_entrada_estoque UUID NOT NULL,
    id_entrada_estoque UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_item_entrada_estoque_id_item_entrada_estoque FOREIGN KEY (id_item_entrada_estoque)
        REFERENCES item_entrada_estoque(id),
    CONSTRAINT fk_log_item_entrada_estoque_id_entrada_estoque FOREIGN KEY (id_entrada_estoque)
        REFERENCES entrada_estoque(id),
    CONSTRAINT fk_log_item_entrada_estoque_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_venda (
    id UUID PRIMARY KEY,
    id_venda UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_venda_id_venda FOREIGN KEY (id_venda)
        REFERENCES venda(id),
    CONSTRAINT fk_log_venda_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_item_venda (
    id UUID PRIMARY KEY,
    id_item_venda UUID NOT NULL,
    id_venda UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_item_venda_id_item_venda FOREIGN KEY (id_item_venda)
        REFERENCES item_venda(id),
    CONSTRAINT fk_log_item_venda_id_venda FOREIGN KEY (id_venda)
        REFERENCES venda(id),
    CONSTRAINT fk_log_item_venda_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_servico_venda (
    id UUID PRIMARY KEY,
    id_servico_venda UUID NOT NULL,
    id_venda UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_servico_venda_id_servico_venda FOREIGN KEY (id_servico_venda)
        REFERENCES servico_venda(id),
    CONSTRAINT fk_log_servico_venda_id_venda FOREIGN KEY (id_venda)
        REFERENCES venda(id),
    CONSTRAINT fk_log_servico_venda_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_transacao (
    id UUID PRIMARY KEY,
    id_transacao UUID NOT NULL,
    id_venda UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_transacao_id_transacao FOREIGN KEY (id_transacao)
        REFERENCES transacao(id),
    CONSTRAINT fk_log_transacao_id_venda FOREIGN KEY (id_venda)
        REFERENCES venda(id),
    CONSTRAINT fk_log_transacao_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);

CREATE TABLE log_parcela (
    id UUID PRIMARY KEY,
    id_parcela UUID NOT NULL,
    id_transacao UUID NOT NULL,
    acao VARCHAR(50) NOT NULL,
    campo_alterado VARCHAR(50) NOT NULL,
    valor_antigo VARCHAR(300),
    valor_novo VARCHAR(300),
    id_usuario_responsavel UUID NOT NULL,
    data_criacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_log_parcela_id_parcela FOREIGN KEY (id_parcela)
        REFERENCES parcela(id),
    CONSTRAINT fk_log_parcela_id_transacao FOREIGN KEY (id_transacao)
        REFERENCES transacao(id),
    CONSTRAINT fk_log_parcela_id_usuario_responsavel FOREIGN KEY (id_usuario_responsavel)
        REFERENCES usuario(id)
);