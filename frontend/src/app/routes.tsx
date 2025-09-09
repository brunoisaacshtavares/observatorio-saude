import { createBrowserRouter } from "react-router-dom";
import { Shell } from "../components/layout/Shell";
import Dashboard from "../pages/Dashboard";
import Estabelecimentos from "../pages/Estabelecimentos";
import HospitaisLeitos from "../pages/HospitaisLeitos";
import Analises from "../pages/Analises";
import Sobre from "../pages/Sobre";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Shell />,
    children: [
      { index: true, element: <Dashboard /> },
      { path: "estabelecimentos", element: <Estabelecimentos /> },
      { path: "hospitais-leitos", element: <HospitaisLeitos /> },
      { path: "analises", element: <Analises /> },
      { path: "sobre", element: <Sobre /> },
    ],
  },
]);
