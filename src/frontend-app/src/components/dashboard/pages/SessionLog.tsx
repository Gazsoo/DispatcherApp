import { Card } from "../../ui";
import { Link } from "react-router-dom";
import { useSessionsLog } from "../../hooks/useSessions";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";

export default function SessionLog() {

    const { sessions, isLoading, error } = useSessionsLog();

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;
    return (
        <div className="space-y-4 p-4 text-gray-800 dark:text-gray-100">
            <h2 className="text-xl font-semibold text-gray-900 dark:text-gray-100">Sessions</h2>

            <div className="grid grid-cols-1 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3">
                {sessions.length === 0 && (
                    <div className="text-sm text-gray-500">No sessions received yet.</div>
                )}

                {sessions.map(s => {
                    return (
                        <Link
                            key={s.groupId}
                            to={`/dashboard/sessionlog/${s.groupId}`}
                            role="button"
                            tabIndex={s.participants?.length || 0}
                            className="cursor-pointer focus:outline-none focus:ring-2 focus:ring-accent rounded-xl"
                        >
                            <Card className="p-4 max-w-none hover:shadow-md transition-transform transform">
                                <div className="flex items-center justify-between gap-2">
                                    <div className="truncate">
                                        <div className="text-sm font-medium truncate">{s.groupId}</div>
                                        {/* <div className="text-xs text-gray-500 truncate">{s.assignmentId ?? '—'}</div> */}
                                    </div>

                                    <div className="flex items-center gap-2">
                                        <span
                                            className={`text-xs px-2 py-0.5 rounded-full font-semibold ${String(s.status).toLowerCase() === 'connected' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'}`}
                                        >
                                            {String(s.status)}
                                        </span>

                                    </div>
                                </div>

                                <div className="mt-2 text-sm text-gray-700 dark:text-gray-300 space-y-1">
                                    <div><span className="font-semibold">Assignment ID:</span> {s.assignmentId ?? '—'}</div>
                                </div>
                            </Card>
                        </Link>
                    );
                })}
            </div>
        </div>
    );
}