import { useMemo, useState } from "react";
import { useParams } from "react-router-dom";
import { useSessionHub } from "../../hooks/useSessionHub";
import { Button, Card, DetailsCard } from "../../ui";
import { useUser } from "../../context/userContext";
import { useSessions } from "../../hooks/useSessions";
import LogBlock from "../components/LogBlock";
import { useApiCall, useApiMutation } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";
import AssignmnetStatusSelect from "../components/SessionStatusSelect";
import { AssignmentResponse, AssignmentStatus, AssignmentStatusUpdateRequest, SessionResponse } from "../../../services/web-api-client";
import { useAssignment } from "../../hooks/useAssignment";

const SessionDetails = () => {
    const { sessionId } = useParams();
    const { user } = useUser();
    const { sessions } = useSessions();
    const { error: closeError, execute: closeSession, isLoading: isClosing } = useApiCall();
    const { connection, isConnected, error, log,
        sessionStatus, sessionParticipants, sessionData } = useSessionHub(sessionId, "/ws/sessions");
    const { assignment, setAssignment } = useAssignment(sessionData?.assignmentId);
    const { mutate: changeState, isLoading: isChangingState, error: stateError } = useApiMutation(
        (x: { id: number, status: AssignmentStatusUpdateRequest }) =>
            apiClient.assignment_UpdateAssignmentStatus(x.id, x.status)
    );
    const onSelectState = async (e: React.ChangeEvent<HTMLSelectElement>, assignmentId: number | undefined) => {
        const newState = e.target.value as AssignmentStatus;
        if (assignmentId !== undefined) {
            if (assignment) {
                assignment.status = newState;
                setAssignment({ ...assignment });
            }
            await changeState({ id: assignmentId, status: { status: newState } });
        }
    }
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
                    { label: "Assignment ID", value: sessionData?.assignmentId || 'Unknown' },
                    { label: "Session Status", value: sessionStatus || 'Unknown' },
                    {
                        label: "Session Participants",
                        value: sessionParticipants.length > 0
                            ? sessionParticipants.map(p => p.name).join(', ')
                            : 'Unknown'
                    },
                ]}

            >
                {canClose &&
                    <Button variant="dangerSubtle" onClick={x => closeSession(
                        () => apiClient.session_UpdateStatus(sessionId ?? "", "Finished"))}>
                        Finish Session
                    </Button>
                }
                {assignment && <AssignmnetStatusSelect assignment={assignment} onSelectState={onSelectState}></AssignmnetStatusSelect>}

            </DetailsCard>

            <LogBlock log={log} />
        </div>
    );
};

export default SessionDetails;
