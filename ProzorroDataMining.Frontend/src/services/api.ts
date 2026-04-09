import { API_URL } from "../config";

// Fetch total savings from backend
export async function getTotalSavings() {
  const response = await fetch(`${API_URL}/api/analytics/savings`);
  if (!response.ok) {
    throw new Error("Error fetching Total Savings");
  }
  return response.json();
}

// Fetch top procurers from backend
export async function getTopProcurers() {
  const response = await fetch(`${API_URL}/api/analytics/top-procurers`);
  if (!response.ok) {
    throw new Error("Error fetching Top Procurers");
  }
  return response.json();
}

// Fetch top suppliers from backend
export async function getTopSuppliers() {
  const response = await fetch(`${API_URL}/api/analytics/top-suppliers`);
  if (!response.ok) {
    throw new Error("Error fetching Top Suppliers");
  }
  return response.json();
}

// Import process from backend
export async function importData() {
  const response = await fetch(`${API_URL}/api/etl/import`, {
    method: "POST"
  });
  if (!response.ok) {
    const text = await response.text();
    throw new Error(`Error triggering data import: ${response.status} ${text}`);
  }
  return response.json();
}
