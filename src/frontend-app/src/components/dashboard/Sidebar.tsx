import { Link, useLocation } from "react-router-dom";
import { dashboardNavigation } from "../../config/navigation";
import { useUser } from "../context/userContext";
import { useAuth } from "../hooks/useAuth";

export default function Sidebar() {
    const location = useLocation();
    const { user, userRoles } = useUser();
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

            <div className="mt-auto p-4 space-y-3">
                {/* User info */}
                <div className="flex items-center gap-3 px-3 py-3 rounded-lg bg-surface-light-card dark:bg-surface-dark-card border border-surface-light-border dark:border-surface-dark-border">
                    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-accent/20 text-accent font-semibold">
                        {(() => {
                            const fn = user?.firstName?.trim() || '';
                            const ln = user?.lastName?.trim() || '';
                            const initials = `${fn?.[0] ?? ''}${ln?.[0] ?? ''}` || (user?.email?.[0]?.toUpperCase() ?? '?');
                            return initials.toUpperCase();
                        })()}
                    </div>
                    <div className="min-w-0">
                        <div className="font-medium truncate">
                            {user?.firstName || user?.lastName ? `${user?.firstName ?? ''} ${user?.lastName ?? ''}`.trim() : (user?.email ?? 'Unknown user')}
                        </div>
                        <div className="text-xs text-gray-600 dark:text-gray-400 truncate">
                            {(rolesList.length > 0 ? rolesList.join(', ') : (user?.role ?? '')) || '—'}
                        </div>
                    </div>
                </div>

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