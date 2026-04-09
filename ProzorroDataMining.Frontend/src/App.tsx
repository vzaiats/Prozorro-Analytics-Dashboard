import React, { useEffect, useState } from "react";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import DashboardContent from "./components/DashboardContent";
import "./css/Styles.css";
import { Procurer } from "./interfaces/Procurer";
import { Supplier } from "./interfaces/Supplier";
import { getTopProcurers, getTopSuppliers, getTotalSavings, importData } from "./services/api";

const App: React.FC = () => {
  const [savings, setSavings] = useState<number>(0);
  const [procurers, setProcurers] = useState<Procurer[]>([]);
  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [countdown, setCountdown] = useState<number>(60);
  const [timerActive, setTimerActive] = useState<boolean>(false);
  const [lastUpdated, setLastUpdated] = useState<string>("");

  const loadData = async () => {
    try {
      const savingsResponse = await getTotalSavings();
      setSavings(savingsResponse.totalSavings);

      setProcurers(await getTopProcurers());
      setSuppliers(await getTopSuppliers());

      const now = new Date();
      setLastUpdated(
        now.toLocaleTimeString("uk-UA", { hour: "2-digit", minute: "2-digit", second: "2-digit" })
      );

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

  const startTimer = () => {
    setCountdown(60);
    setTimerActive(true);
  };

  const handleRefresh = async () => {
    setLoading(true);
    try {
      await importData();
      await loadData();
      startTimer();
    } catch (error) {
      console.error("Error refreshing data", error);
      toast.error("❌ Error refreshing data", {
        toastId: "dataError",
        position: "top-left",
        autoClose: 5000,
        style: { backgroundColor: "#d32f2f", color: "#fff" },
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const init = async () => {
      setLoading(true);
      try {
        await importData();
        await loadData();
        startTimer();
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

  useEffect(() => {
    if (!timerActive) return;

    const timer = setInterval(() => {
      setCountdown(prev => {
        if (prev <= 1) {
          handleRefresh();
          return 60;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, [timerActive]);

  return (
    <div className="dashboard">
      <h1>Prozorro Analytics Dashboard</h1>
      <ToastContainer />
      {timerActive && !loading && (
        <div className="countdown-block">
          <div className="countdown">
            Next auto-refresh in: {Math.floor(countdown / 60)}:
            {(countdown % 60).toString().padStart(2, "0")}
          </div>
          {lastUpdated && <div className="last-updated">Last updated: {lastUpdated}</div>}
        </div>
      )}

      <button className="refresh-button" onClick={handleRefresh} disabled={loading}>
        {loading ? (
          <>
            Loading...
            <span className="btn-spinner"></span>
          </>
        ) : (
          "Refresh Data"
        )}
      </button>

      <DashboardContent
        loading={loading}
        savings={savings}
        procurers={procurers}
        suppliers={suppliers}
      />
    </div>
  );
};

export default App;
