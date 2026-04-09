import React from "react";
import Skeleton from "react-loading-skeleton";
import "react-loading-skeleton/dist/skeleton.css";
import { DashboardContentProps } from "../interfaces/DashboardContentProps";
import SavingsWidget from "./SavingsWidget";
import TopProcurersChart from "./TopProcurersChart";
import TopSuppliersChart from "./TopSuppliersChart";

const DashboardContent: React.FC<DashboardContentProps> = ({ loading, savings, procurers, suppliers }) => {
  return (
    <div className="dashboard">
      {loading ? (
        <>
          <div className="card savings">
            <h2>Total Savings</h2>
            <Skeleton height={30} width={200} />
          </div>

          <div className="card">
            <h2>Top 5 Procurers</h2>
            <Skeleton count={5} height={20} style={{ marginBottom: 8 }} />
            <Skeleton height={200} />
          </div>

          <div className="card">
            <h2>Top 5 Suppliers</h2>
            <Skeleton count={5} height={20} style={{ marginBottom: 8 }} />
            <Skeleton height={200} />
          </div>
        </>
      ) : (
        <>
          <SavingsWidget savings={savings} />
          <TopProcurersChart procurers={procurers} />
          <TopSuppliersChart suppliers={suppliers} />
        </>
      )}
    </div>
  );
};

export default DashboardContent;
