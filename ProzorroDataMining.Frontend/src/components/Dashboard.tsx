import React, { useEffect, useState } from "react";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Procurer } from "../interfaces/Procurer";
import { Supplier } from "../interfaces/Supplier";
import { getTopProcurers, getTopSuppliers, getTotalSavings, importData } from "../services/api";
import DashboardContent from "./DashboardContent";

const Dashboard: React.FC = () => {
  const [savings, setSavings] = useState<number>(0);
  const [procurers, setProcurers] = useState<Procurer[]>([]);
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  const loadData = async () => {
    try {
      const savingsResponse = await getTotalSavings();
      setSavings(savingsResponse.totalSavings);

      setProcurers(await getTopProcurers());
      setSuppliers(await getTopSuppliers());

      toast.success("✅ Data loaded", {
        toastId: "dataLoaded",
        position: "top-left",
        autoClose: 5000,
        style: { backgroundColor: "#2e7d32", color: "#fff" },
      });
    } catch (error) {
      console.error("Error loading data", error);
      toast.error("❌ Failed to load data", {
        toastId: "dataError",
        position: "top-left",
        autoClose: 5000,
        style: { backgroundColor: "#d32f2f", color: "#fff" },
      });
    }
  };

  useEffect(() => {
    const init = async () => {
      setLoading(true);
      try {
        await importData();
        await loadData();
      } catch (error) {
        console.error("Init error", error);
        toast.error("❌ Failed to load data", {
          toastId: "dataError",
          position: "top-left",
          autoClose: 5000,
          style: { backgroundColor: "#d32f2f", color: "#fff" },
        });
      } finally {
        setLoading(false);
      }
    };
    init();
  }, []);

  return (
    <>
      <ToastContainer />
      <DashboardContent
        loading={loading}
        savings={savings}
        procurers={procurers}
        suppliers={suppliers}
      />
    </>
  );
};

export default Dashboard;
