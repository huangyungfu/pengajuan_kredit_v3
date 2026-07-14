--
-- PostgreSQL database dump
--

\restrict zgRSGcNr3hhMdwp5Tshdnq0slSdjEOXRFA15QIP0mdo1YBuGJ1bP4NkxfdhDaVt

-- Dumped from database version 18.4
-- Dumped by pg_dump version 18.4

-- Started on 2026-07-14 21:00:52

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
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
-- TOC entry 219 (class 1259 OID 24577)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 24584)
-- Name: pengajuan_kredit; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pengajuan_kredit (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    plafon numeric NOT NULL,
    bunga numeric(5,2) NOT NULL,
    tenor integer NOT NULL,
    angsuran numeric NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updated_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.pengajuan_kredit OWNER TO postgres;

--
-- TOC entry 5015 (class 0 OID 24577)
-- Dependencies: 219
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20260714113410_ModelFieldsNullableUpdate	8.0.28
\.


--
-- TOC entry 5016 (class 0 OID 24584)
-- Dependencies: 220
-- Data for Name: pengajuan_kredit; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.pengajuan_kredit (id, plafon, bunga, tenor, angsuran, created_at, updated_at) FROM stdin;
bed2f8bb-ed9e-4700-8585-d5411cfc6895	50000000	10.50	12	4383333	2026-07-14 19:52:09.600644+07	2026-07-14 19:52:09.600644+07
9a2d5506-fda8-4ead-9df2-3924ce07b20d	150000000	9.75	36	4822916	2026-07-14 19:52:09.600644+07	2026-07-14 19:52:09.600644+07
97484ff4-1d8f-455a-a4b5-58c577fab4c0	20000000	12.00	6	3533333	2026-07-14 19:52:09.600644+07	2026-07-14 19:52:09.600644+07
1ab9bdf5-fe9d-44ea-aad2-34133032cac9	300000000	8.50	60	6150000	2026-07-14 19:52:09.600644+07	2026-07-14 19:52:09.600644+07
7462aa60-74e1-4e09-b026-bf3eb0686ea1	80000000	11.00	72	1522222	2026-07-14 19:52:09.600644+07	2026-07-14 19:52:09.600644+07
\.


--
-- TOC entry 4863 (class 2606 OID 24583)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4865 (class 2606 OID 24599)
-- Name: pengajuan_kredit PK_pengajuan_kredit; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pengajuan_kredit
    ADD CONSTRAINT "PK_pengajuan_kredit" PRIMARY KEY (id);


--
-- TOC entry 4866 (class 1259 OID 24600)
-- Name: idx_plafon; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_plafon ON public.pengajuan_kredit USING btree (plafon);


--
-- TOC entry 4867 (class 1259 OID 24601)
-- Name: idx_tenor; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_tenor ON public.pengajuan_kredit USING btree (tenor);


-- Completed on 2026-07-14 21:00:52

--
-- PostgreSQL database dump complete
--

\unrestrict zgRSGcNr3hhMdwp5Tshdnq0slSdjEOXRFA15QIP0mdo1YBuGJ1bP4NkxfdhDaVt

