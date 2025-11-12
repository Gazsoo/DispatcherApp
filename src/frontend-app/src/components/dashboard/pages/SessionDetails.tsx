import { useMemo } from "react";
import { useParams } from "react-router-dom";
import { useSessionHub } from "../../hooks/useSessionHub";
import { Button, Card } from "../../ui";
import { useUser } from "../../context/userContext";
import { useSessions } from "../../hooks/useSessions";

const SessionDetails = () => {
    const { sessionId } = useParams();
    const { user } = useUser();
    const { sessions } = useSessions();
    const { connection, isConnected, error, log } = useSessionHub(sessionId, "/ws/sessions");

    const currentSession = useMemo(
        () => sessions.find((s) => s.groupId === sessionId),
        [sessions, sessionId]
    );

    const canClose = useMemo(() => {
        if (!currentSession || !user) return false;
        const isOwner = currentSession.ownerId === user.id;
        const isAdmin = user.role?.includes("Administrator");
        return isOwner || isAdmin;
    }, [currentSession, user]);

    return (
        <div className="space-y-4 p-4">
            <div className="flex items-center justify-between flex-wrap gap-3">
                <div>
                    <p className="text-sm text-gray-500">Session #{sessionId}</p>
                    <h1 className="text-2xl font-semibold">Session Details</h1>
                    <p className="text-sm text-gray-600">
                        Status: <span className="font-medium">{isConnected ? "Connected" : "Disconnected"}</span>
                    </p>
                </div>
                {canClose && (
                    <Button type="button" variant="danger" disabled>
                        Close Session
                    </Button>
                )}
            </div>

            {error && <Card className="p-4 text-red-600">Error: {error}</Card>}

            <Card className="p-4">
                <h2 className="text-lg font-semibold mb-2">Session Log</h2>
                <pre className="text-sm bg-neutral-900 text-neutral-100 rounded-lg p-4 max-h-80 overflow-auto">
                    {log.length ? log.join("\n") : "No activity recorded yet."}
                </pre>
            </Card>

            <Card className="p-4 text-sm text-gray-600 dark:text-gray-300">
                Additional session metadata will appear here.
            </Card>
        </div>
    );
};

export default SessionDetails;
