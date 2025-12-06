import { useEffect, useMemo, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { Button, Card, DetailsCard } from "../../ui";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import type { FileMetadataResponse } from "../../../services/web-api-client";
import { useApiCall, useApiMutation } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";
import { useFileDetails } from "../../hooks/useFileDetails";

export default function FileDetails() {
    const { fileId } = useParams();
    const { file, formattedSize, isLoading, error } = useFileDetails(fileId);
    const { mutate: deleteFile, isLoading: isDeleting } = useApiMutation((id: number) => apiClient.files_DeleteFile(id));
    const navigate = useNavigate();

    if (!fileId) {
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
                    <Button
                        variant="dangerSubtle"
                        size="sm"
                        type="button"
                        className="w-auto"
                        isLoading={isDeleting}
                        disabled={isDeleting}
                        onClick={async (e) => {
                            if (!file.id) return;

                            const confirmed = window.confirm(`Delete ${file.fileName}?`);
                            if (!confirmed) return;

                            const result = await deleteFile(file.id);
                            if (result !== null) {
                                navigate("/dashboard/files");
                            } else {
                                window.alert("Failed to delete file.");
                            }
                        }}
                    >
                        Delete
                    </Button>
                </div>
            </div>

            <DetailsCard
                items={[
                    { label: "Content type", value: file.contentType || "Unknown" },
                    { label: "File size", value: formattedSize },
                    { label: "Uploaded by", value: file.createdByName || "—" },
                    { label: "Uploaded at", value: file.uploadedAt ? new Date(file.uploadedAt).toLocaleString() : "—" },
                ]}
                description={{
                    title: "Description",
                    content: file.description || "No description provided.",
                }}
                className="max-w-3xl"
            />
        </div>
    );
}
