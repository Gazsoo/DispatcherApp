import { Link } from "react-router-dom";
import { Button, Card } from "../../ui";
import { useApiCall } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";
import { useEffect, useState } from "react";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { AssignmentResponse } from "../../../services/web-api-client";

const Assignments = () => {
    const { execute, isLoading, error } = useApiCall<AssignmentResponse[]>();
    const [assignments, setAssignments] = useState<AssignmentResponse[]>([]);

    useEffect(() => {
        const fetchAssignments = async () => {
            const result = await execute(() => apiClient.assignment_GetAssignments());
            if (result) {
                setAssignments(result);
            }
        };
        fetchAssignments();
    }, [execute]);

    const formatDateTime = (value?: Date) => {
        if (!value) return '—';
        try {
            const d = value instanceof Date ? value : new Date(value);
            if (isNaN(d.getTime())) return '—';
            return d.toLocaleString();
        } catch {
            return '—';
        }
    };

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <h1 className="text-3xl font-bold">Assignments</h1>
                <Link to="/dashboard/assignments/create">
                    <Button variant="primary">Create Assignment</Button>
                </Link>
            </div>

            {assignments.length === 0 ? (
                <p className="text-sm text-gray-600">No assignments to display yet.</p>
            ) : (
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                    {assignments.map((a) => {
                        const names = (a.assignees ?? []).map(u => u.userName || u.email || 'Unknown').join(', ');
                        return (
                            <Card key={a.id} className="hover:shadow-lg transition-shadow cursor-pointer h-full">
                                <div className="flex flex-col h-full justify-between">

                                    <div className="p-1">
                                        <h2 className="text-xl font-semibold mb-1 truncate">{a.name}</h2>
                                        {a.type && (
                                            <p className="text-xs uppercase tracking-wide text-gray-500 mb-2">{a.type}</p>
                                        )}
                                        <p className="text-gray-700 dark:text-gray-300 text-sm line-clamp-3 mb-2">
                                            {a.description || 'No description'}
                                        </p>
                                        <div className="text-sm text-gray-700 dark:text-gray-300 truncate mb-1">
                                            <span className="font-medium">Assignees:</span> {names || '—'}
                                        </div>
                                        <div className="text-sm text-gray-600 dark:text-gray-400 mb-1">
                                            <span className="font-medium">Due:</span> {formatDateTime(a.plannedTime)}
                                        </div>
                                        {a.value && (
                                            <div className="text-sm text-gray-600 dark:text-gray-400">
                                                <span className="font-medium">Value:</span> {a.value}
                                            </div>
                                        )}

                                    </div>
                                    <div className="pt-3">
                                        <Button type="button" variant="secondary" className="w-full">Start Session</Button>
                                    </div>
                                </div>
                            </Card>
                        );
                    })}
                </div>
            )}
        </div>
    );
}

export default Assignments;