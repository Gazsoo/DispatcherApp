import { createBrowserRouter, RouterProvider, useNavigate } from "react-router";
import App from "../../App";
import Login from "../login";

const router = createBrowserRouter([
  { path: "/", Component: App },
  { path: "/login", Component: Login }
]);