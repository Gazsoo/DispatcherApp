import App from "./App";
import Dashboard from "./components/Dashboard";
import Login from "./components/Login";
import { createBrowserRouter } from "react-router";

export const router = createBrowserRouter([
  { path: "/", Component: App },
  { path: "/login", Component: Login },
  { path: "/dashboard", Component: Dashboard },
]);
