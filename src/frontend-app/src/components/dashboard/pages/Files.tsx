import { Link } from "react-router-dom";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { Button } from "../../ui";
import { useFiles } from "../../hooks/useFiles";

import React, { useRef } from "react";
import { useApiMutation } from "../../hooks/useApiClient";
import type { FileParameter, FileUploadResponse } from "../../../services/web-api-client";
import { apiClient } from "../../../api/client";
import downloadFileById from "../../../services/file-downloader";

export default function Files() {
    // Pass a lambda so `this` is preserved for the client method
    const { mutate, reset } = useApiMutation<FileParameter, FileUploadResponse>((input) => apiClient.files_PostFile(input));
    const { files, isLoading, error, refetch } = useFiles();
    const fileInputRef = useRef<HTMLInputElement>(null);

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;

    const handleUploadClick = () => {
        if (fileInputRef.current) {
            fileInputRef.current.click(); // open file picker
        }
    };

    const handleFileChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (!file) return;

        const fileParam: FileParameter = {
            fileName: file.name,
            data: file,
        };
        reset();

        try {
            const response = await mutate(fileParam);

            if (!response) throw new Error("Upload failed");

            alert("File uploaded successfully!");
            refetch(); // Refresh file list
        } catch (err) {
            alert(`Error uploading file: ${err instanceof Error ? err.message : String(err)}`);
        } finally {
            event.target.value = ""; // reset input
        }
    };

    const handleDownload = async (e: React.MouseEvent, fileId?: number, defaultName?: string) => {
        e.preventDefault();
        e.stopPropagation();

        if (fileId === undefined || fileId === null) {
            alert("Invalid file id");
            return;
        }

        try {
            const result = await downloadFileById(fileId, defaultName);
            if (!result.success) {
                alert(`Download failed: ${result.reason}`);
            }
        } catch (err) {
            alert(`Error downloading file: ${err instanceof Error ? err.message : String(err)}`);
        }
    };
    // Otherwise show the files list
    return (
        <>

            <div className="flex items-center justify-between mb-6">
                <h1 className="text-3xl font-bold">Files</h1>
                <div>
                    <Button variant="primary" className="w-auto" onClick={handleUploadClick}>
                        Upload File
                    </Button>
                </div>
                <input
                    type="file"
                    ref={fileInputRef}
                    className="hidden"
                    onChange={handleFileChange} />
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {files.map((file) => (
                    <Link
                        key={file.id}
                        to={`/dashboard/files/${file.id}`}
                        className="p-4 border border-gray-200 rounded-lg hover:border-blue-500 hover:shadow-md transition-all"
                    >
                        <div className="flex items-start justify-between">
                            <div className="flex-1 min-w-0">
                                <h3 className="text-lg font-semibold truncate">{file.fileName}</h3>
                                {/* <p className="text-sm text-gray-500 mt-1">{file.contentType}</p> */}
                            </div>
                            <div className="flex items-center ml-2 space-x-2">
                                <button
                                    className="p-2 rounded hover:bg-gray-100 text-gray-600"
                                    title="Download"
                                    onClick={(e) => handleDownload(e, file.id, file.fileName)}
                                    aria-label={`Download ${file.fileName}`}
                                >
                                    <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                                        <path fillRule="evenodd" d="M3 3a1 1 0 011-1h12a1 1 0 011 1v6a1 1 0 11-2 0V5H5v10h4a1 1 0 110 2H4a1 1 0 01-1-1V3zm9.293 6.293a1 1 0 011.414 1.414L11 15.414V9a1 1 0 112 0v6.414l2.707-4.707a1 1 0 111.586 1.172l-4 7a1 1 0 01-1.686 0l-4-7a1 1 0 011.586-1.172L12.293 9.293z" clipRule="evenodd" />
                                    </svg>
                                </button>
                            </div>
                        </div>
                    </Link>
                ))}
            </div></>
    );
}