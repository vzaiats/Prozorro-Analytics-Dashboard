import { Procurer } from "./Procurer";
import { Supplier } from "./Supplier";

export interface DashboardContentProps {
  loading: boolean;
  savings: number;
  procurers: Procurer[];
  suppliers: Supplier[];
}
