
import { useParams } from "react-router";
import { Card, DetailsCard } from "../../ui";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";

import { useSessionDetails } from "../../hooks/useSessions";
import FileLink from "../components/FileLink";

export default function SessionLogDetails() {
    const { sessionLogId } = useParams();
    const { session, isLoading, error } = useSessionDetails(sessionLogId);

    if (!sessionLogId) {
        return (
            <Card className="p-6 text-red-600 dark:text-red-400">
                Invalid session identifier supplied.
            </Card>
        );
    }

    if (isLoading && !session) {
        return <LoadingSpinner />;
    }

    if (error) {
        return <ErrorDisplay error={error} />;
    }

    if (!session) {
        return (
            <Card className="p-6 text-gray-600 dark:text-gray-300">
                We could not load details for session ID {sessionLogId}.
            </Card>
        );
    }

    return (
        <div>


            <DetailsCard
                items={[
                    { label: "Group ID", value: session.groupId || "Unknown" },
                    { label: "Owner ID", value: session.ownerId },]}
                // description={{
                //     title: "Description",
                //     content: session. || "No description provided.",
                // }}
                className="max-w-3xl"
            />

            {session.logFile &&
                <>
                    <div className="mt-6">Log File</div>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mt-5">

                        {<FileLink file={session.logFile} />}
                    </div>
                </>
            }
        </div>
    );
}
