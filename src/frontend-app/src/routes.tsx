import App from "./App";
import { AppProviders } from "./components/context/AppProvider";
import { DashboardLayout } from "./components/dashboard/DashboardLayout";
import Login from "./components/Login";
import { createBrowserRouter, Outlet } from "react-router";
import { dashboardNavigation } from "./config/navigation";

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
      { path: "/", Component: App },
      { path: "/login", Component: Login },
      {
        path: "/dashboard",
        element: <DashboardLayout />,
        children: dashboardNavigation.flatMap(({ path, component: Component, children }, i) => {
          const routePath = i === 0 ? undefined : path.replace('/dashboard/', '');
          const mainRoute = i === 0
            ? { index: true, Component }
            : { path: routePath, Component };

          // If this nav item has nested children, add them
          if (children && children.length > 0) {
            return [
              {
                ...mainRoute,
                children: children.map(child => ({
                  path: child.path,
                  Component: child.component
                }))
              }
            ];
          }

          return [mainRoute];
        }),
      },
    ],
  },
]);