--
-- PostgreSQL database dump
--

-- Dumped from database version 12.5 (Debian 12.5-1.pgdg100+1)
-- Dumped by pg_dump version 13.0

-- Started on 2020-12-04 12:05:37

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 203 (class 1259 OID 16391)
-- Name: BankAccount; Type: TABLE; Schema: public; Owner: pguser
--

CREATE TABLE public."BankAccount" (
    "BankAccountId" uuid NOT NULL,
    type text,
    agencycode text,
    code integer NOT NULL,
    accountcode text
);


ALTER TABLE public."BankAccount" OWNER TO pguser;

--
-- TOC entry 204 (class 1259 OID 16399)
-- Name: Transaction; Type: TABLE; Schema: public; Owner: pguser
--

CREATE TABLE public."Transaction" (
    "TransactionId" uuid NOT NULL,
    bankaccountid uuid NOT NULL,
    type text,
    "Date" timestamp without time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    transactionvalue double precision NOT NULL,
    "Description" character varying(4000) NOT NULL
);


ALTER TABLE public."Transaction" OWNER TO pguser;

--
-- TOC entry 202 (class 1259 OID 16386)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: pguser
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO pguser;

--
-- TOC entry 2787 (class 2606 OID 16390)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: pguser
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 2790 (class 2606 OID 16398)
-- Name: BankAccount pk_bankaccounts; Type: CONSTRAINT; Schema: public; Owner: pguser
--

ALTER TABLE ONLY public."BankAccount"
    ADD CONSTRAINT pk_bankaccounts PRIMARY KEY ("BankAccountId");


--
-- TOC entry 2794 (class 2606 OID 16407)
-- Name: Transaction pk_transactions; Type: CONSTRAINT; Schema: public; Owner: pguser
--

ALTER TABLE ONLY public."Transaction"
    ADD CONSTRAINT pk_transactions PRIMARY KEY ("TransactionId");


--
-- TOC entry 2788 (class 1259 OID 16413)
-- Name: IX_BankAccount_BankAccountId; Type: INDEX; Schema: public; Owner: pguser
--

CREATE UNIQUE INDEX "IX_BankAccount_BankAccountId" ON public."BankAccount" USING btree ("BankAccountId");


--
-- TOC entry 2791 (class 1259 OID 16415)
-- Name: IX_Transaction_TransactionId; Type: INDEX; Schema: public; Owner: pguser
--

CREATE UNIQUE INDEX "IX_Transaction_TransactionId" ON public."Transaction" USING btree ("TransactionId");


--
-- TOC entry 2792 (class 1259 OID 16414)
-- Name: ix_transactions_bankaccountid; Type: INDEX; Schema: public; Owner: pguser
--

CREATE INDEX ix_transactions_bankaccountid ON public."Transaction" USING btree (bankaccountid);


--
-- TOC entry 2795 (class 2606 OID 16408)
-- Name: Transaction FK_Transaction_BankAccount; Type: FK CONSTRAINT; Schema: public; Owner: pguser
--

ALTER TABLE ONLY public."Transaction"
    ADD CONSTRAINT "FK_Transaction_BankAccount" FOREIGN KEY (bankaccountid) REFERENCES public."BankAccount"("BankAccountId") ON DELETE CASCADE;


-- Completed on 2020-12-04 12:05:37

--
-- PostgreSQL database dump complete
--

