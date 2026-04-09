-- Create database
CREATE DATABASE prozorro;

-- Create table for tenders
CREATE TABLE Tenders (
    TenderId TEXT PRIMARY KEY,
    Status TEXT NOT NULL,
    Budget NUMERIC NOT NULL,
    ProcuringEntity TEXT NOT NULL,
    TenderDate TIMESTAMP NOT NULL,
    ImportedAt TIMESTAMP DEFAULT NOW(),
    CpvCode TEXT
);

-- Create table for contracts
CREATE TABLE Contracts (
    ContractId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    TenderId TEXT NOT NULL REFERENCES Tenders(TenderId) ON DELETE CASCADE,
    Amount NUMERIC NOT NULL
);

-- Create table for suppliers
CREATE TABLE Suppliers (
    SupplierId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    TenderId TEXT NOT NULL REFERENCES Tenders(TenderId) ON DELETE CASCADE,
    Name TEXT NOT NULL
);

-- Add indexes
CREATE INDEX idx_tenders_date ON Tenders(TenderDate);

CREATE INDEX idx_contracts_tender_id ON Contracts(TenderId);

CREATE INDEX idx_suppliers_tender_id ON Suppliers(TenderId);

CREATE INDEX idx_tenders_procuringentity ON Tenders(ProcuringEntity);

CREATE INDEX idx_suppliers_name ON Suppliers(Name);

CREATE INDEX idx_tenders_cpvcode ON Tenders(CpvCode);