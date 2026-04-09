import React from "react";
import { Cell, Legend, Pie, PieChart, Tooltip } from "recharts";
import "../css/Styles.css";

const COLORS = ["#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF"];

const TopSuppliersChart: React.FC<{ suppliers: any[] }> = ({ suppliers }) => {
  const formatUAH = (value: number) =>
    new Intl.NumberFormat("uk-UA", { style: "currency", currency: "UAH" }).format(value);

  return (
    <div className="card">
      <h2>Top 5 Suppliers</h2>
      <div className="table-container">
        <table>
          <tbody>
            {suppliers.map((s, index) => (
              <tr key={index}>
                <td>{s.supplierName}</td>
                <td className="amount">{formatUAH(s.totalAmount)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <div className="chart-container">
        <PieChart width={400} height={300}>
          <Pie
            data={suppliers}
            dataKey="totalAmount"
            nameKey="supplierName"
            cx="50%"
            cy="50%"
            outerRadius={100}
            label={false}
          >
            {suppliers.map((entry, index) => (
              <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
            ))}
          </Pie>
          <Tooltip formatter={(value: any) => formatUAH(Number(value))} />
          <Legend layout="horizontal" verticalAlign="bottom" align="center" />
        </PieChart>
      </div>
    </div>
  );
};

export default TopSuppliersChart;
