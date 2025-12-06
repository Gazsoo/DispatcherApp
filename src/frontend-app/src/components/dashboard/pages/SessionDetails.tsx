import { useMemo } from "react";
import { useParams } from "react-router-dom";
import { useSessionHub } from "../../hooks/useSessionHub";
import { Button, Card, DetailsCard } from "../../ui";
import { useUser } from "../../context/userContext";
import { useSessions } from "../../hooks/useSessions";
import LogBlock from "../components/LogBlock";

const SessionDetails = () => {
    const { sessionId } = useParams();
    const { user } = useUser();
    const { sessions } = useSessions();
    const { connection, isConnected, error, log,
        sessionStatus, sessionParticipants } = useSessionHub(sessionId, "/ws/sessions");

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
                        Connection Status: <span className="font-medium">{isConnected ? "Connected" : "Disconnected"}</span>
                    </p>

                </div>


            </div>

            {error && <Card className="p-4 text-red-600">Error: {error}</Card>}

            <DetailsCard
                items={[
                    { label: "Session Status", value: sessionStatus || 'Unknown' },
                    {
                        label: "Session Participants",
                        value: sessionParticipants.length > 0
                            ? sessionParticipants.map(p => p.name).join(', ')
                            : 'Unknown'
                    },
                ]}
            />
            <LogBlock log={log} />
        </div>
    );
};

export default SessionDetails;
