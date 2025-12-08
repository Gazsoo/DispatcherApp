import { Link } from "react-router-dom";
import { Card } from "../../ui";
import type { TutorialResponse } from "../../../services/web-api-client";
import { useState, useEffect } from "react";
import { apiClient } from "../../../api/client";
import { JWTManager } from "../../../auth/jwt/jwt-manager";
import { useApiCall } from "../../hooks/useApiClient";
import downloadFileById from "../../../services/file-downloader";
import { LoadingSpinner } from "../../ui/LoadingSpinner";

interface TutorialCardProps {
    tutorial: TutorialResponse;
}

export function TutorialCard({ tutorial }: TutorialCardProps) {
    // Check if tutorial has an image file
    const [isLoading, setIsLoading] = useState(true);
    const [imageSrc, setImageSrc] = useState<string | null>(null);

    useEffect(() => {
        const loadImage = async () => {
            try {
                if (tutorial.picture?.id) {

                    // Fetch the image file as a blob with authentication
                    const resp = await apiClient.files_DownloadFile(tutorial.picture.id);
                    const anyResp = resp as any;
                    // Generated client returns { data: Blob, fileName?, status?, headers? }
                    if (!anyResp || !anyResp.data) {
                        // try to provide diagnostics
                        const status = anyResp?.status ?? 'unknown';
                        const headers = anyResp?.headers ? JSON.stringify(anyResp.headers) : undefined;
                        return { success: false, reason: `no-data-status-${status}`, details: headers };
                    }

                    const blob: Blob = anyResp.data;
                    const url = URL.createObjectURL(blob);
                    setImageSrc(url);


                    // Cleanup blob URL on unmount
                    return () => URL.revokeObjectURL(url);
                };

            } catch (err) {
                console.error('Failed to load tutorial image:', err);
                setImageSrc(null);
            } finally {
                setIsLoading(false);
            }
        }

        const cleanup = loadImage();

        return () => {
            cleanup.then();
        };
    }, [tutorial.picture?.id]);

    if (isLoading) return <LoadingSpinner />;
    return (
        <Link to={`/dashboard/tutorials/${tutorial.id}`}>
            <Card className="overflow-hidden hover:shadow-lg transition-shadow cursor-pointer h-full">
                {/* Image preview or placeholder */}
                <div className="w-full h-48 bg-gradient-to-br from-blue-100 to-purple-100 dark:from-blue-900 dark:to-purple-900 flex items-center justify-center">
                    {imageSrc ? (
                        <img
                            src={imageSrc}
                            alt={tutorial.title}
                            className="w-full h-full object-cover"
                        />
                    ) : (
                        <svg className="w-16 h-16 text-gray-accent" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                        </svg>
                    )}
                </div>

                {/* Content */}
                <div className="p-1">
                    <h2 className="text-xl font-semibold mb-2">{tutorial.title}</h2>
                    <p className="text-gray-600 dark:text-gray-400 text-sm line-clamp-2">
                        {tutorial.description || 'No description available'}
                    </p>
                </div>
            </Card>
        </Link>
    );
}