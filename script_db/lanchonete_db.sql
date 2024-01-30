DROP SCHEMA IF EXISTS lanchonete CASCADE;
CREATE SCHEMA IF NOT EXISTS lanchonete;
SET search_path TO lanchonete;

DROP TABLE IF EXISTS lanchonete.Categoria;

CREATE TABLE lanchonete.Categoria
(
  Id serial                                     NOT NULL,
  Nome  varchar(100)       NOT NULL,
  Descricao varchar(200)       NULL,
  CONSTRAINT pk_tb_Categoria PRIMARY KEY (Id)
);

DROP TABLE IF EXISTS lanchonete.Produto;
CREATE TABLE lanchonete.Produto
(
  Id serial                                     NOT NULL,
  Nome varchar(100)       NOT NULL,
  Descricao varchar(200)       NULL,
  CONSTRAINT pk_tb_Produto PRIMARY KEY (Id)
);

DROP TABLE IF EXISTS lanchonete.Cliente;

CREATE TABLE lanchonete.Cliente
(
  Id serial NOT NULL,
  Nome varchar(100)       NOT NULL,
  CPF varchar(200)       NULL,
  CONSTRAINT pk_tb_Cliente PRIMARY KEY (Id)
);
