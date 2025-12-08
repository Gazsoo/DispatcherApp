
export interface NavigationItem {
    path: string;
    label: string;
    icon?: string; // Optional icon for UI
    requiredRoles?: string[]; // If set, only users with at least one of these roles can see the item
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
    // Admin-only navigation
    { path: "/dashboard/administration", label: "Administration", requiredRoles: ["Admin", "Administrator"] },
    { path: "/dashboard/profile", label: "Profile" },
];