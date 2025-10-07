
export interface NavigationItem {
    path: string;
    label: string;
    icon?: string; // Optional icon for UI
}

// Shared navigation configuration
export const dashboardNavigation: NavigationItem[] = [
    { path: "/dashboard", label: "Overview" },
    { path: "/dashboard/tutorials", label: "Tutorials" },
    { path: "/dashboard/assignments", label: "Assignments" },
    { path: "/dashboard/files", label: "Files" },
    { path: "/dashboard/settings", label: "Settings" },
    { path: "/dashboard/administration", label: "Administration" },
    { path: "/dashboard/profile", label: "Profile" },
];