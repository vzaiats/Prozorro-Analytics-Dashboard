import React from "react";
import "../css/Styles.css";

const SavingsWidget: React.FC<{ savings: number }> = ({ savings }) => {
  const formatUAH = (value: number) =>
    new Intl.NumberFormat("uk-UA", { style: "currency", currency: "UAH" }).format(value);

  return (
    <div className="card savings">
      <h2>Total Savings</h2>
      <p className="amount">{formatUAH(savings)}</p>
    </div>
  );
};

export default SavingsWidget;
