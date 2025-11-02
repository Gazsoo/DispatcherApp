import App from "./App";
import { AppProviders } from "./components/context/AppProvider";
import { DashboardLayout } from "./components/dashboard/DashboardLayout";
import Login from "./components/Login";
import Register from "./components/Register";
import { createBrowserRouter, Outlet } from "react-router-dom";
import DashboardOverview from "./components/dashboard/pages/DashboardOverview";
import Tutorials from "./components/dashboard/pages/Tutorials";
import TutorialDetails from "./components/dashboard/pages/TutorialDetails";
import Files from "./components/dashboard/pages/Files";
import FileDetails from "./components/dashboard/pages/FileDetails";
import Assignments from "./components/dashboard/pages/Assignments";
import Settings from "./components/dashboard/pages/Settings";
import Administration from "./components/dashboard/pages/Administrations";
import Profile from "./components/dashboard/pages/Profile";
import RequireRole from "./components/auth/RequireRole";
import Unauthorized from "./components/auth/Unauthorized";
import Sessions from "./components/dashboard/pages/Sessions";
import SessionDetails from "./components/dashboard/pages/SessionDetails";

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
      { path: "/register", element: <Register /> },
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
          { path: "sessions", element: <Sessions /> },
          { path: "sessions/:sessionId", element: <SessionDetails /> },
          // { path: "settings", element: <Settings /> },
          { path: "administration", element: <RequireRole roles={["Administrator"]}><Administration /></RequireRole> },
          { path: "unauthorized", element: <Unauthorized /> },
          { path: "profile", element: <Profile /> },
        ],
      },
    ],
  },
]);