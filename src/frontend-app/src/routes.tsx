import App from "./App";
import { AppProviders } from "./components/context/AppProvider";
import { DashboardLayout } from "./components/dashboard/DashboardLayout";
import Login from "./components/Login";
import { createBrowserRouter, Outlet } from "react-router";
import DashboardOverview from "./components/dashboard/pages/DashboardOverview";
import Tutorials from "./components/dashboard/pages/Tutorials";
import TutorialDetails from "./components/dashboard/pages/TutorialDetails";
import Files from "./components/dashboard/pages/Files";
import FileDetails from "./components/dashboard/pages/FileDetails";
import Assignments from "./components/dashboard/pages/Assignments";
import Settings from "./components/dashboard/pages/Settings";
import Administration from "./components/dashboard/pages/Administrations";
import Profile from "./components/dashboard/pages/Profile";

// Layout component that wraps all routes
function RootLayout() {
  return (
    <AppProviders>
      <Outlet />
    </AppProviders>
  );
}

export const router = createBrowserRouter([
  {
    element: <RootLayout />,
    children: [
      { path: "/", element: <App /> },
      { path: "/login", element: <Login /> },
      {
        path: "/dashboard",
        element: <DashboardLayout />,
        children: [
          // Index route - shows Overview by default
          { index: true, element: <DashboardOverview /> },

          // Tutorials with nested detail route
          {
            path: "tutorials",
            element: <Tutorials />,
          },
          {
            path: "tutorials/:tutorialId",
            element: <TutorialDetails />,
          },

          // Files with nested detail route
          {
            path: "files",
            element: <Files />,
          },
          {
            path: "files/:fileId",
            element: <FileDetails />,
          },

          // Simple routes without nesting
          { path: "assignments", element: <Assignments /> },
          { path: "settings", element: <Settings /> },
          { path: "administration", element: <Administration /> },
          { path: "profile", element: <Profile /> },
        ],
      },
    ],
  },
]);