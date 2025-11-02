import { useMemo } from 'react';
import { useUser } from '../../context/userContext';
import { Card } from '../../ui';

export default function DashboardOverview() {
    const { user } = useUser();
    const displayName = useMemo(() => {
        return user?.firstName?.trim() || user?.email || 'there';
    }, [user]);

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight text-content-light dark:text-content-dark">
                    Welcome, {displayName}
                </h1>
                <p className="mt-1 text-sm text-gray-600 dark:text-gray-400">
                    Use the sidebar to jump into tasks.
                </p>
            </div>

            <Card>
                <div className="text-sm text-gray-700 dark:text-gray-300">
                    You’re all set. Check Assignments for live updates.
                </div>
            </Card>
        </div>
    );

}