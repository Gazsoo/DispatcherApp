// components/DashboardLayout.tsx
import { Outlet } from 'react-router-dom';
import ThemeToggle from '../ThemeToggle';
import Sidebar from './Sidebar';

export const DashboardLayout = () => {



    return (
        <>
            <div className="flex min-h-screen bg-surface-light dark:bg-surface-dark">
                <Sidebar />

                {/* Main content area */}
                <main className="flex-1 p-8">
                    <Outlet /> {/* Child routes render here */}


                </main>
                <ThemeToggle />
            </div></>
    );
};