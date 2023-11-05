-- Criação do banco de dados Noticias
CREATE DATABASE Noticias;

USE Noticias;

CREATE TABLE "Noticias" (
    "Id" uuid primary key,
    "Titulo" VARCHAR(255),
    "Descricao" TEXT,
    "Chapeu" VARCHAR(100),
    "Autor" VARCHAR(100),
    "DataPublicacao" timestamp ,
    "DataAtualizacao" timestamp
);


CREATE TABLE "Usuarios" (
    "Id" uuid primary key,
    "Nome" VARCHAR(100),
    "Login" VARCHAR(50),
    "Senha" VARCHAR(255),
    "Ativo" BOOLEAN,
    "DataPublicacao" timestamp,
    "DataAtualizacao" timestamp,
    "DataUltimoLogin" timestamp
);
-- por algum motivo mistico, quando crio no postgree tudo fica em minusculo e da erro no
-- entity, assim que subir os dados corrigir manualmente no postgree o nome das coisas.