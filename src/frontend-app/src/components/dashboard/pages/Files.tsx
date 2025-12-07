import { Link } from "react-router-dom";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { Button } from "../../ui";
import { useFiles } from "../../hooks/useFiles";

import React, { useRef, useEffect } from "react";
import { useApiMutation } from "../../hooks/useApiClient";
import type { FileParameter, FileUploadResponse } from "../../../services/web-api-client";
import { apiClient } from "../../../api/client";
import downloadFileById from "../../../services/file-downloader";
import FileLink from "../components/FileLink";

export default function Files() {
    // Pass a lambda so `this` is preserved for the client method
    const { mutate, reset } = useApiMutation<FileParameter, FileUploadResponse>((input) => apiClient.files_PostFile(input));
    const { files, isLoading, error, refetch } = useFiles();
    const fileInputRef = useRef<HTMLInputElement>(null);

    // Refetch files when navigating back to this page
    useEffect(() => {
        refetch();
    }, []);

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
                    <FileLink key={file.id} file={file} />
                    // <Link
                    //     key={file.id}
                    //     to={`/dashboard/files/${file.id}`}
                    //     className="p-4 border border-gray-200 rounded-lg hover:border-blue-500 hover:shadow-md transition-all"
                    // >
                    //     <div className="flex items-start justify-between">
                    //         <div className="flex-1 min-w-0">
                    //             <h3 className="text-lg font-semibold truncate">{file.fileName}</h3>
                    //             {/* <p className="text-sm text-gray-500 mt-1">{file.contentType}</p> */}
                    //         </div>
                    //         <div className="flex items-center ml-2 space-x-2">
                    //             <button
                    //                 className="p-2 rounded hover:bg-gray-100 text-gray-600"
                    //                 title="Download"
                    //                 onClick={(e) => handleDownload(e, file.id, file.fileName)}
                    //                 aria-label={`Download ${file.fileName}`}
                    //             >
                    //                 <svg className="w-6 h-6 text-gray-800 dark:text-gray-200" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 10 14">
                    //                     <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 1v12m0 0 4-4m-4 4L1 9" />
                    //                 </svg>
                    //             </button>
                    //         </div>
                    //     </div>
                    // </Link>
                ))}
            </div></>
    );
}