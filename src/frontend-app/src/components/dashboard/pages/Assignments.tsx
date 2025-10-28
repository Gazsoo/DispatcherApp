import React, { useEffect, useMemo, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { apiClient } from "../../../api/client";
import { JWTManager } from "../../../auth/jwt/jwt-manager";



export default function Assignments({
    hubUrl = "/ws/sessions",
    token,
    defaultSessionId = "S-123"
}: {
    hubUrl?: string;
    token?: string;
    defaultSessionId?: string;
}) {
    const [status, setStatus] = useState("disconnected");
    const [connectionId, setConnectionId] = useState<string | null>(null);
    const [log, setLog] = useState<string[]>([]);
    const [sessionId, setSessionId] = useState<string>(defaultSessionId);
    const connRef = useRef<signalR.HubConnection | null>(null);
    token = JWTManager.getAccessToken() ?? "";;

    const append = (line: string) => setLog((x) => [...x, `[${new Date().toISOString()}] ${line}`]);
    const [sessions, setSessions] = useState<
        { id: string; connection?: signalR.HubConnection; status: string; data?: any }[]
    >([]);

    useEffect(() => {
        const loadSessions = async () => {
            try {
                const res = await apiClient.session_GetActive(); // replace with your real endpoint
                const sessionList = res ?? [];
                setSessions(sessionList.map(sess => ({ id: sess.groupId ?? "", status: "disconnected" })));
            } catch (err) {
                append(`Failed to fetch sessions: ${(err as Error).message}`);
            }
        };
        loadSessions();
    }, []);

    // connect to each session on load
    useEffect(() => {
        const connectAll = async () => {
            for (const s of sessions) {
                const url = import.meta.env.VITE_API_BASE_URL + hubUrl;
                const conn = new signalR.HubConnectionBuilder()
                    .withUrl(url, { accessTokenFactory: () => token || "" })
                    .withAutomaticReconnect()
                    .build();

                conn.on("SessionUpdated", (msg) => {
                    append(`[${s.id}] SessionUpdated: ${JSON.stringify(msg)}`);
                    setSessions(prev =>
                        prev.map(x =>
                            x.id === s.id ? { ...x, data: msg, status: "connected" } : x
                        )
                    );
                });

                conn.onclose(() => {
                    append(`[${s.id}] Closed`);
                    setSessions(prev =>
                        prev.map(x => (x.id === s.id ? { ...x, status: "disconnected" } : x))
                    );
                });

                try {
                    await conn.start();
                    append(`[${s.id}] Connected`);
                    setSessions(prev =>
                        prev.map(x =>
                            x.id === s.id ? { ...x, connection: conn, status: "connected" } : x
                        )
                    );
                    await conn.invoke("JoinSession", s.id);
                } catch (err) {
                    append(`[${s.id}] Failed: ${(err as Error).message}`);
                }
            }
        };

        if (sessions.length > 0) connectAll();
    }, [sessions.length]);

    return (
        <><div className="space-y-2">
            {sessions.map(s => (
                <div key={s.id} className="border rounded p-2">
                    <div className="flex justify-between">
                        <span>Session: {s.id}</span>
                        <span>Status: {s.status}</span>
                    </div>
                    <pre className="text-xs mt-2 bg-gray-50 p-2 rounded">
                        {JSON.stringify(s.data ?? {}, null, 2)}
                    </pre>
                </div>
            ))}
        </div><div>
                <label className="block text-sm font-medium mb-1">Log</label>
                <textarea
                    readOnly
                    value={log.join("\n")}
                    className="w-full h-48 border rounded p-2 font-mono text-xs" />
            </div></>
    );
}