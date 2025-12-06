// components/DashboardLayout.tsx
import { Outlet } from 'react-router-dom';
import ThemeToggle from '../ThemeToggle';
import Sidebar from './Sidebar';

export const DashboardLayout = () => {



    return (
        <>
            <div className="flex min-h-screen bg-surface-light dark:bg-surface-dark overflow-x-auto">
                <Sidebar />
                {/* Main content area */}
                <main className="flex-1 p-8 w-full min-h-screen">
                    <Outlet /> {/* Child routes render here */}
                </main>
                <ThemeToggle className="m-2" />
            </div>
        </>
    );
};