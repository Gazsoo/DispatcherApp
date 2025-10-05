import { Link, useLocation } from "react-router-dom";
import { dashboardNavigation } from "../../config/navigation";

export default function Sidebar() {
    const location = useLocation();
    const isActive = (path: string) => location.pathname === path;

    return (
        <aside className="w-64 bg-surface-light-border dark:bg-surface-dark-border border-r border-accent/20">
            <div className="p-6">
                <h2 className="text-xl font-bold text-accent">Dispatcher</h2>
            </div>

            <nav className="px-4 space-y-2">
                {dashboardNavigation.map((item) => (
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
        </aside>
    );

}