import React, { useEffect, useMemo, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { apiClient } from "../../../api/client";
import { JWTManager } from "../../../auth/jwt/jwt-manager";

export default function DashboardOverview({
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

    // Build connection lazily
    const buildConnection = useMemo(() => {
        return () => {
            const url = import.meta.env.VITE_API_BASE_URL + hubUrl;
            const options = token ? { accessTokenFactory: () => token } : {};
            const builder = new signalR.HubConnectionBuilder()
                .withUrl(url, options)
                .withAutomaticReconnect();
            const conn = builder.build();

            // inbound events
            conn.on("SessionUpdated", (msg) => {
                append(`SessionUpdated: ${JSON.stringify(msg)}`);
            });
            conn.on("SessionJoined", (msg) => {
                append(`SessionJoined: ${JSON.stringify(msg)}`);
            });
            // lifecycle noise
            conn.onreconnecting((err) => {
                setStatus("reconnecting");
                append(`Reconnecting: ${err?.message ?? "unknown"}`);
            });
            conn.onreconnected((id) => {
                setStatus("connected");
                setConnectionId(id ?? conn.connectionId ?? null);
                append(`Reconnected. connectionId=${id ?? conn.connectionId}`);
            });
            conn.onclose((err) => {
                setStatus("disconnected");
                setConnectionId(null);
                append(`Closed: ${err?.message ?? "clean"}`);
            });

            return conn;
        };
    }, [hubUrl, token]);

    // Clean up on unmount
    useEffect(() => {
        return () => {
            if (connRef.current) {
                connRef.current.stop().catch(() => { });
            }
        };
    }, []);

    const connect = async () => {
        if (connRef.current?.state === signalR.HubConnectionState.Connected) return;
        connRef.current = buildConnection();
        try {
            await connRef.current.start();
            setStatus("connected");
            setConnectionId(connRef.current.connectionId ?? null);
            append("Connected.");
        } catch (e) {
            if (e instanceof Error) {
                setStatus("error");
                append(`Connect failed: ${e.message}`);
            }
        }
    };

    const disconnect = async () => {
        if (!connRef.current) return;
        await connRef.current.stop();
        setStatus("disconnected");
        setConnectionId(null);
        append("Disconnected.");
    };

    const invoke = async (method: string, ...args: any[]) => {
        if (!connRef.current || connRef.current.state !== signalR.HubConnectionState.Connected) {
            append("Not connected.");
            return;
        }
        try {
            const res = await connRef.current.invoke(method, ...args);
            append(`${method} → ${JSON.stringify(res)}`);
            return res;
        } catch (e) {
            if (e instanceof Error) {
                append(`${method} failed: ${e.message}`);
            }
        }
    };

    const joinSession = async () => {
        const id = await invoke("JoinSession", sessionId);
        setSessionId(id);
    };
    const leaveSession = async () => {
        const id = await invoke("LeaveSession", sessionId);
    };

    const ping = async () => {
        await invoke("Ping");
    };

    const whoAmI = async () => {
        // recommend hub method returns an object so you can see it clearly
        await invoke("WhoAmI");
    };

    // If your hub supports it: Update via WS (or do your HTTP PUT to trigger broadcast)
    const sendTestUpdate = async () => {
        // Example hub method signature: UpdateSession(string sessionId, long ifMatchVersion, string dataJson)
        // Adjust to your own. Here we just try if it exists:
        await invoke("UpdateSession", sessionId, 0, JSON.stringify({ quickTest: true, t: Date.now() }));
    };

    return (
        <div className="p-4 space-y-4">
            <h1 className="text-3xl font-bold">Dashboard Overview</h1>

            <div className="flex items-center gap-2">
                <span className="text-sm px-2 py-1 rounded bg-gray-100">
                    Status: <strong>{status}</strong>
                </span>
                {connectionId && (
                    <span className="text-xs px-2 py-1 rounded bg-gray-100">
                        ConnId: <code>{connectionId}</code>
                    </span>
                )}
            </div>

            <div className="flex flex-wrap gap-2">
                <button onClick={connect} className="px-3 py-2 rounded bg-black text-white">Connect</button>
                <button onClick={disconnect} className="px-3 py-2 rounded border">Disconnect</button>
                <button onClick={ping} className="px-3 py-2 rounded border">Ping</button>
                <button onClick={whoAmI} className="px-3 py-2 rounded border">WhoAmI</button>
                <button onClick={joinSession} className="px-3 py-2 rounded border">Join Session</button>
                <button onClick={leaveSession} className="px-3 py-2 rounded border">Leave Session</button>
                <button onClick={sendTestUpdate} className="px-3 py-2 rounded border">Send Test Update</button>
            </div>

            <div className="space-y-2">
                <label className="block text-sm font-medium">Session Id</label>
                <input
                    className="w-full border rounded px-3 py-2"
                    value={sessionId}
                    onChange={(e) => setSessionId(e.target.value)}
                    placeholder={sessionId}
                />
            </div>

            <div>
                <label className="block text-sm font-medium mb-1">Log</label>
                <textarea
                    readOnly
                    value={log.join("\n")}
                    className="w-full h-48 border rounded p-2 font-mono text-xs"
                />
            </div>
        </div>
    );
}
