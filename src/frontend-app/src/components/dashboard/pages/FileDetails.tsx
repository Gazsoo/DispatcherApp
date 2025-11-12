import { useEffect, useMemo, useState } from "react";
import { useParams } from "react-router";
import { Card } from "../../ui";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import type { FileMetadataResponse } from "../../../services/web-api-client";
import { useApiCall } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";

export default function FileDetails() {
    const { fileId } = useParams();
    const numericFileId = useMemo(() => {
        const parsed = Number(fileId);
        return Number.isFinite(parsed) ? parsed : null;
    }, [fileId]);

    const { execute, isLoading, error } = useApiCall<FileMetadataResponse>();
    const [file, setFile] = useState<FileMetadataResponse | null>(null);

    useEffect(() => {
        if (numericFileId === null) {
            setFile(null);
            return;
        }

        const fetchFile = async () => {
            const data = await execute(() => apiClient.files_GetFileMetadata(numericFileId));
            if (data) {
                setFile(data);
            }
        };

        fetchFile();
    }, [execute, numericFileId]);

    const formattedSize = useMemo(() => {
        if (!file) {
            return "—";
        }
        const units = ["B", "KB", "MB", "GB", "TB"];
        let size = file.fileSize ?? 0;
        let unitIndex = 0;
        while (size >= 1024 && unitIndex < units.length - 1) {
            size /= 1024;
            unitIndex++;
        }
        return `${size.toFixed(1)} ${units[unitIndex]}`;
    }, [file]);

    if (!fileId || numericFileId === null) {
        return (
            <Card className="p-6 text-red-600 dark:text-red-400">
                Invalid file identifier supplied.
            </Card>
        );
    }

    if (isLoading && !file) {
        return <LoadingSpinner />;
    }

    if (error) {
        return <ErrorDisplay error={error} />;
    }

    if (!file) {
        return (
            <Card className="p-6 text-gray-600 dark:text-gray-300">
                We could not load details for file ID {fileId}.
            </Card>
        );
    }

    return (
        <div>
            <div className="flex items-center justify-between mb-6">
                <div>
                    <p className="text-sm text-gray-500">File #{file.id}</p>
                    <h1 className="text-3xl font-bold break-all">{file.fileName}</h1>
                </div>
                <div className="flex gap-3">
                    {file.downloadUrl && (
                        <a
                            href={file.downloadUrl}
                            target="_blank"
                            rel="noopener noreferrer"
                            className="inline-flex items-center justify-center rounded-lg bg-accent px-6 py-3 text-sm font-semibold text-white shadow-lg transition hover:bg-accent-dark"
                        >
                            Download
                        </a>
                    )}
                    <button
                        type="button"
                        className="inline-flex items-center justify-center rounded-lg border border-red-200/60 bg-red-50 px-6 py-3 text-sm font-semibold text-red-700 transition hover:bg-red-100"
                        disabled
                    >
                        Delete
                    </button>
                </div>
            </div>

            <Card className="p-6 space-y-6 max-w-3xl">
                <dl className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                    <div>
                        <dt className="text-sm text-gray-500">Content type</dt>
                        <dd className="text-base font-medium">{file.contentType || "Unknown"}</dd>
                    </div>
                    <div>
                        <dt className="text-sm text-gray-500">File size</dt>
                        <dd className="text-base font-medium">{formattedSize}</dd>
                    </div>
                    <div>
                        <dt className="text-sm text-gray-500">Uploaded by</dt>
                        <dd className="text-base font-medium">{file.createdByName || "—"}</dd>
                    </div>
                    <div>
                        <dt className="text-sm text-gray-500">Uploaded at</dt>
                        <dd className="text-base font-medium">
                            {file.uploadedAt ? new Date(file.uploadedAt).toLocaleString() : "—"}
                        </dd>
                    </div>
                </dl>

                <div>
                    <h2 className="text-sm text-gray-500 mb-2">Description</h2>
                    <p className="text-base text-gray-800 dark:text-gray-200 whitespace-pre-wrap">
                        {file.description || "No description provided."}
                    </p>
                </div>
            </Card>
        </div>
    );
}
