import { Link, useLocation } from "react-router-dom";
import { dashboardNavigation } from "../../config/navigation";
import { useUser } from "../context/userContext";
import { useAuth } from "../hooks/useAuth";

export default function Sidebar() {
    const location = useLocation();
    const { userRoles } = useUser();
    const { logout } = useAuth();
    const isActive = (path: string) => location.pathname === path;

    const rolesList = Array.isArray(userRoles)
        ? userRoles
        : (userRoles ? [userRoles] : []);

    return (
        <aside className="w-64 bg-surface-light-border dark:bg-surface-dark-border border-r border-accent/20 flex flex-col">
            <div className="p-6">
                <h2 className="text-xl font-bold text-accent">Dispatcher</h2>
            </div>

            <nav className="px-4 space-y-2">
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

            <div className="mt-auto p-4">
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