import { useParams } from "react-router-dom";
import { useSessionHub } from "../../hooks/useSessionHub";

const SessionDetails = () => {

    const { sessionId } = useParams();
    const { connection, isConnected, error, log } = useSessionHub(sessionId, "/ws/sessions");
    return (<>
        <div>Status: {isConnected ? 'Connected' : 'Disconnected'}</div>
        {error && <div className="text-red-600">Error: {error}</div>}
        <div className="mt-4">
            <h2>Session Log</h2>
            <pre>{log.join('\n')}</pre>
        </div>
        <div>Session Details Page for {sessionId}</div>
    </>
    );
}

export default SessionDetails;
