namespace ProzorroDataMining.Data.Constants
{
    public static class SqlQueriesConstants
    {
        // --- TenderRepository ---

        // Insert a new tender into Tenders table
        public const string InsertTender = @"
            INSERT INTO ""Tenders"" (""TenderId"", ""Status"", ""Budget"", ""ProcuringEntity"", ""TenderDate"", ""ImportedAt"", ""CpvCode"")
            VALUES (@TenderId, @Status, @Budget, @ProcuringEntity, @TenderDate, @ImportedAt, @CpvCode)
            ON CONFLICT (""TenderId"") DO NOTHING;";

        // Insert a new contract into Contracts table
        public const string InsertContract = @"
            INSERT INTO ""Contracts"" (""ContractId"", ""TenderId"", ""Amount"")
            VALUES (@ContractId, @TenderId, @Amount);";

        // Insert a new supplier into Suppliers table
        public const string InsertSupplier = @"
            INSERT INTO ""Suppliers"" (""SupplierId"", ""TenderId"", ""Name"")
            VALUES (@SupplierId, @TenderId, @Name);";

        // Get tender details by TenderId
        public const string GetTenderById = @"
            SELECT ""TenderId"", ""Status"", ""Budget"", ""ProcuringEntity"", ""TenderDate"", ""ImportedAt"", ""CpvCode""
            FROM ""Tenders""
            WHERE ""TenderId"" = @TenderId;";

        // Get all contracts for a given TenderId
        public const string GetContractsByTenderId = @"
            SELECT ""ContractId"", ""TenderId"", ""Amount""
            FROM ""Contracts""
            WHERE ""TenderId"" = @TenderId;";

        // Get all suppliers for a given TenderId
        public const string GetSuppliersByTenderId = @"
            SELECT ""SupplierId"", ""TenderId"", ""Name""
            FROM ""Suppliers""
            WHERE ""TenderId"" = @TenderId;";

        // Check if there is at least one record in any of the tables
        public const string CheckIfAnyDataExists = @"
            SELECT EXISTS (
                SELECT 1 FROM ""Tenders""
                UNION
                SELECT 1 FROM ""Contracts""
                UNION
                SELECT 1 FROM ""Suppliers""
            );";

        // Delete all contracts
        public const string DeleteContracts = @"DELETE FROM ""Contracts"";";

        // Delete all suppliers
        public const string DeleteSuppliers = @"DELETE FROM ""Suppliers"";";

        // Delete all tenders
        public const string DeleteTenders = @"DELETE FROM ""Tenders"";";

        // --- AnalyticsRepository ---

        // Calculate total savings (Budget - Amount)
        public const string GetTotalSavings = @"
            SELECT COALESCE(SUM(t.""Budget"" - c.""Amount""), 0) AS Savings
            FROM ""Tenders"" t
            JOIN ""Contracts"" c ON t.""TenderId"" = c.""TenderId"";";

        // Get top 5 procurers by total contract amount
        public const string GetTopProcurers = @"
            SELECT t.""ProcuringEntity"" AS ProcuringEntity, SUM(c.""Amount"") AS TotalAmount
            FROM ""Tenders"" t
            JOIN ""Contracts"" c ON t.""TenderId"" = c.""TenderId""
            GROUP BY t.""ProcuringEntity""
            ORDER BY TotalAmount DESC
            LIMIT 5;";

        // Get top 5 suppliers by total contract amount
        public const string GetTopSuppliers = @"
            SELECT s.""Name"" AS SupplierName, SUM(c.""Amount"") AS TotalAmount
            FROM ""Suppliers"" s
            JOIN ""Contracts"" c ON s.""TenderId"" = c.""TenderId""
            GROUP BY s.""Name""
            ORDER BY TotalAmount DESC
            LIMIT 5;";
    }
}
