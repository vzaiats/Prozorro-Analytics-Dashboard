import React from "react";
import { Cell, Legend, Pie, PieChart, Tooltip } from "recharts";
import "../css/Styles.css";

const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#AA46BE"];

const TopProcurersChart: React.FC<{ procurers: any[] }> = ({ procurers }) => {
  const formatUAH = (value: number) =>
    new Intl.NumberFormat("uk-UA", { style: "currency", currency: "UAH" }).format(value);

  return (
    <div className="card">
      <h2>Top 5 Procurers</h2>
      <div className="table-container">
        <table>
          <tbody>
            {procurers.map((p, index) => (
              <tr key={index}>
                <td>{p.procuringEntity}</td>
                <td className="amount">{formatUAH(p.totalAmount)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <div className="chart-container">
        <PieChart width={400} height={300}>
          <Pie
            data={procurers}
            dataKey="totalAmount"
            nameKey="procuringEntity"
            cx="50%"
            cy="50%"
            outerRadius={100}
            label={false}
          >
            {procurers.map((entry, index) => (
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

export default TopProcurersChart;
