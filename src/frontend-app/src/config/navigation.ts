
export interface NavigationItem {
    path: string;
    label: string;
    requiredRoles?: string[]; // If set, only users with these roles can see the item
}

// Shared navigation configuration
export const dashboardNavigation: NavigationItem[] = [
    { path: "/dashboard", label: "Overview" },
    { path: "/dashboard/tutorials", label: "Tutorials" },
    { path: "/dashboard/assignments", label: "Assignments" },
    { path: "/dashboard/sessions", label: "Sessions" },
    { path: "/dashboard/files", label: "Files", requiredRoles: ["Dispatcher", "Administrator"] },
    { path: "/dashboard/sessionlog", label: "Session Log", requiredRoles: ["Dispatcher", "Administrator"] },
    // { path: "/dashboard/settings", label: "Settings" },
    { path: "/dashboard/administration", label: "Administration", requiredRoles: ["Administrator"] },
    { path: "/dashboard/profile", label: "Profile" },
];