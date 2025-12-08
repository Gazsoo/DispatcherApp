import { Link, useLocation } from "react-router-dom";
import { dashboardNavigation } from "../../config/navigation";

import { useAuth } from "../hooks/useAuth";
import { useUser } from "../context/userContext";
import UserBadge from "./components/UserBadge";

export default function Sidebar() {
    const location = useLocation();

    const { logout } = useAuth();
    const isActive = (path: string) => location.pathname === path;

    const { userRoles } = useUser();
    const rolesList = Array.isArray(userRoles)
        ? userRoles
        : (userRoles ? [userRoles] : []);


    return (
        <aside className="w-64 h-screen sticky top-0 bg-surface-light-border dark:bg-surface-dark-border border-r border-accent/20 flex flex-col flex-shrink-0">
            <div className="p-6">
                <h2 className="text-xl font-bold text-accent">Dispatcher</h2>
            </div>

            <nav className="px-4 space-y-2 flex-1 overflow-y-auto">
                {dashboardNavigation
                    .filter(item => !item.requiredRoles || item.requiredRoles.some(r => rolesList.includes(r)))
                    .map((item) => (
                        <Link
                            key={item.path}
                            to={item.path}
                            className={`block px-4 py-2 rounded-lg ${isActive(item.path)
                                ? 'bg-accent text-white'
                                : 'text-content-light dark:text-content-dark hover:bg-accent/10'
                                }`}
                        >
                            {item.label}
                        </Link>
                    ))}
            </nav>

            <div className="mt-auto p-4 space-y-3">
                <UserBadge />

                <button
                    onClick={logout}
                    className="w-full text-left px-4 py-2 rounded-lg border hover:bg-accent/10 text-content-light dark:text-content-dark"
                >
                    Log out
                </button>
            </div>
        </aside>
    );

}
